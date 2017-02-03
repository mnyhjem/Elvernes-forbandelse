module ElvenCurse {
    export class StateGameplay extends Phaser.State {
        // debug
        onlineCount: number;
        // debug

        backgroundimage: Phaser.Image;

        gameHub: IGameHub;

        game: Phaser.Game;
        map: Phaser.Tilemap;
        background: Phaser.TilemapLayer;
        collisionLayer: Phaser.TilemapLayer;

        // groups
        backgroundGroup: Phaser.Group;
        middelgroundGroup: Phaser.Group;
        aboveMiddelgroup: Phaser.Group;
        uiGroup: Phaser.Group;

        player: Player;
        players: OtherPlayer[];
        npcs: NpcBase[];
        interactiveObjects: InteractiveObject[];

        selectedCreature: NpcBase;

        cursors: Phaser.CursorKeys;
        mapMovedInThisPosition: string = "";
        currentMap: IWorldsection;

        // ui stuff
        playerPortraitplate: EntityPortraitplate;
        selectedCreaturePortraitplate: EntityPortraitplate;
        actionbar: Actionbar;
        worldsectionnameplate: Worldsectionnameplate;
        messageText: Phaser.Text;

        // music
        backgroundMusic: Phaser.Sound;

        initializing: boolean = true;
        signalRInitializing: boolean = true;

        // weather
        shadowTexture: Phaser.BitmapData;

        constructor() {
            super();
        }

        create(): void {
            this.game.stage.disableVisibilityChange = true;

            this.backgroundGroup = this.game.add.group();
            this.middelgroundGroup = this.game.add.group();
            this.aboveMiddelgroup = this.game.add.group();
            this.uiGroup = this.game.add.group();
            this.uiGroup.fixedToCamera = true;

            this.game.world.bringToTop(this.middelgroundGroup);
            this.game.world.bringToTop(this.aboveMiddelgroup);
            this.game.world.bringToTop(this.uiGroup);

            this.players = new Array<OtherPlayer>();
            this.npcs = new Array<NpcBase>();
            this.interactiveObjects = new Array<InteractiveObject>();

            this.game.physics.startSystem(Phaser.Physics.ARCADE);

            this.player = new Player(this.game);
            this.middelgroundGroup.add(this.player.playerGroup);

            // ui
            this.playerPortraitplate = new EntityPortraitplate(this.game, this.player.creature);
            this.uiGroup.add(this.playerPortraitplate.group);

            this.selectedCreaturePortraitplate = new EntityPortraitplate(this.game, null, 230);
            this.uiGroup.add(this.selectedCreaturePortraitplate.group);

            this.actionbar = new Actionbar(this.game, this.player.creature);
            this.uiGroup.add(this.actionbar.group);

            this.worldsectionnameplate = new Worldsectionnameplate(this.game, this.currentMap);
            this.uiGroup.add(this.worldsectionnameplate.group);

            this.messageText = this.game.add.text((this.game.width / 2) - 200, 200, "", { font: "32px Arial", fill: "#669966" }); // , backgroundColor: "#ffffff"
            this.uiGroup.add(this.messageText);

            // signalr
            this.wireupSignalR();

            // physics
            this.game.physics.arcade.enable(this.player.playerSprite);
            this.game.camera.follow(this.player.playerSprite);

            // inputs
            this.cursors = this.game.input.keyboard.createCursorKeys();
        }

        update(): void {
            if (this.background === undefined) {
                return;
            }

            // move
            if (this.signalRInitializing) {
                return;
            }

            this.player.checkCollisions(this.collisionLayer);

            var oldX: number = this.player.creature.location.x, oldY: number = this.player.creature.location.y;
            this.player.move(this.cursors);
            this.player.creature.location.x = this.background.getTileX(this.player.playerSprite.x);
            this.player.creature.location.y = this.background.getTileX(this.player.playerSprite.y);
            if (this.player.creature.location.x !== oldX || this.player.creature.location.y !== oldY) {
                this.gameHub.server.movePlayer(this.player.creature.location.worldsectionId, this.player.creature.location.x, this.player.creature.location.y);
            }

            var activatedAbility = this.actionbar.getActivatedAbility();
            if (activatedAbility > -1) {
                var creatureId = -1;
                if (this.selectedCreature !== undefined) {
                    creatureId = this.selectedCreature.creature.id;
                }
                this.gameHub.server.activateAbility(activatedAbility, creatureId);
            }

            if (this.game.input.keyboard.isDown(Phaser.KeyCode.ESC)) {
                this.selectedCreature = undefined;
                this.selectedCreaturePortraitplate.update(null);
            }

            // this.placeOtherPlayersAndObjects();

            this.worldsectionnameplate.setPlayerPosition(this.player);

            // this.updateNightshadow();
        }

        private updateNightshadow(): void {
            if (this.initializing) {
                return;
            }

            // sæt vejr.. test
            if (this.shadowTexture === undefined) {
                this.shadowTexture = this.game.add.bitmapData(this.game.width, this.game.height);
                var lightSprite: Phaser.Image = this.game.add.image(0, 0, this.shadowTexture);
                lightSprite.blendMode = PIXI.blendModes.MULTIPLY;
                lightSprite.smoothed = true;

                this.uiGroup.add(lightSprite);
            }

            // draw shadow
            this.shadowTexture.context.fillStyle = "rgb(50, 50, 50)";
            this.shadowTexture.context.fillRect(0, 0, this.game.width, this.game.height);

            // draw circle of light
            this.shadowTexture.context.beginPath();
            this.shadowTexture.context.fillStyle = "rgb(255, 255, 255)";
            this.shadowTexture.context.arc(this.player.playerSprite.worldTransform.tx, this.player.playerSprite.worldTransform.ty, 100, 0, Math.PI * 2);
            this.shadowTexture.context.fill();

            // this just tells the engine it should update the texture cache
            this.shadowTexture.dirty = true;
        }

        render(): void {
            if (this.initializing) {
                return;
            }
            

            // this.game.debug.text(this.currentMap.name, 32, 32+50, "rgb(0,0,0)");
            // this.game.debug.text("Tile X: " + this.background.getTileX(this.player.playerSprite.x) + " position.x: " + this.player.playerSprite.position.x, 32, 48 + 50, "rgb(0,0,0)");
            // this.game.debug.text("Tile Y: " + this.background.getTileY(this.player.playerSprite.y) + " position.y: " + this.player.playerSprite.position.y, 32, 64 + 50, "rgb(0,0,0)");

            //this.game.debug.text("Online: " + this.onlineCount, 32, 80, "rgb(0,0,0)");

            //if (this.selectedCreature !== undefined) {
            //    this.game.debug.text("Selected: " + this.selectedCreature.creature.name, 32, 95, "rgb(0,0,0)");
            //}

            //this.game.debug.text("Activated ability: " + this.actionbar.getActivatedAbility(), 32, 110, "rgb(0,0,0)");
        }

        private createMap(): void {
            this.log("CreateMap");
            this.game.load.onLoadComplete.remove(this.createMap, this);

            this.backgroundMusic = this.game.add.audio("medieval");
            // this.backgroundMusic.play();

            this.map = this.game.add.tilemap("world");
            //this.map.enableDebug = true; 
            this.log("aboveMiddelgroup" + this.aboveMiddelgroup.children.length);
            this.log("backgroundGroup" + this.backgroundGroup.children.length);


            var collisionTileId: number = -1;
            for (let i = 0; i < this.map.tilesets.length; i++) {
                var tileset: Phaser.Tileset = this.map.tilesets[i];
                this.map.addTilesetImage(tileset.name, tileset.name, tileset.tileWidth, tileset.tileHeight);

                if (tileset.name.toLowerCase() === "collision") {
                    collisionTileId = tileset.firstgid;
                }
            }

            for (let i = 0; i < this.map.layers.length; i++) {
                var layer: Phaser.TilemapLayer = this.map.layers[i];

                var l: Phaser.TilemapLayer = this.map.createLayer(layer.name);
                //l.renderSettings.enableScrollDelta = false;

                if (layer.properties.displayGroup !== undefined) {
                    switch (layer.properties.displayGroup.toLowerCase()) {
                        case "abovemiddelgroup":
                            this.aboveMiddelgroup.add(l);
                            break;
                    }
                } else {
                    this.backgroundGroup.add(l);
                }

                if (layer.name.toLowerCase() === "background") {
                    this.background = l;
                } else if (layer.name.toLowerCase() === "collision" || layer.name.toLowerCase() === "collisionlayer") {
                    l.visible = false;
                    this.collisionLayer = l;
                }
            }

            this.background.resizeWorld();
            if (this.collisionLayer !== undefined && collisionTileId > -1) {
                this.map.setCollision(collisionTileId, true, this.collisionLayer);
            }


            if (this.player) {
                this.placeOtherPlayersAndObjects();
            }
            this.initializing = false;

            if (this.backgroundimage != null) {
                this.backgroundimage.destroy(true);
                this.backgroundimage = null;
            }
        }

        private wireupSignalR(): void {
            var self: StateGameplay = this;

            $.connection.hub.url = $("#serverpath").text() + "/signalr";
            this.gameHub = $.connection.gameHub;

            if (this.gameHub === undefined) {
                location.href = "/world/gameserverdown";
                return;
            }

            $.connection.hub.stateChanged(function (change: any): void {
                console.log(change);
                // vi understøtter ikke reconnect.. så bare send folk til startsiden hvis vi ryger af..
                if (change.newState === $.connection.connectionState.disconnected ||
                    change.newState === $.connection.connectionState.reconnecting) {
                    location.href = "/Character";
                }
            });

            // this.gameHub.client.hello = function (text) {
            //    var t = 0;
            // }

            this.gameHub.client.onlinecount = function (cnt: number): void {
                self.log("onlinecount callback");

                self.onlineCount = cnt;
            };

            this.gameHub.client.playAnimation = function (creatureWithAnimation: IPlayer, targetCreature: IPlayer, animation: string): void {
                //self.player.playerSprite.rotation = self.game.physics.arcade.angleToXY(self.player.playerSprite, 100, 100);
                var direction = "Right";
                if (creatureWithAnimation.location.x > targetCreature.location.x) {
                    direction = "Left";
                }
                if (creatureWithAnimation.location.y > targetCreature.location.y) {
                    direction = "Back";
                }
                if (creatureWithAnimation.location.y < targetCreature.location.y) {
                    direction = "Front";
                }
                
                if (creatureWithAnimation.isPlayer) {
                    if (creatureWithAnimation.id === self.player.creature.id) {
                        self.player.playAnimation(animation + direction);
                        return;
                    }

                    for (let i = 0; i < self.players.length; i++) {
                        if (self.players[i].player.id === creatureWithAnimation.id) {
                            self.players[i].playAnimation(animation + direction);
                            return;
                        }
                    }
                }

                for (let i = 0; i < self.npcs.length; i++) {
                    if (self.npcs[i].creature.id === creatureWithAnimation.id) {
                        self.npcs[i].playAnimation(animation + direction);
                        return;
                    }
                }
            };

            this.gameHub.client.updatePlayer = function (player: IPlayer): void {
                for (let i = 0; i < self.players.length; i++) {
                    if (self.players[i].player.id === player.id) {
                        // self.players[i].location.x = player.location.x;
                        // self.players[i].location.y = player.location.y;
                        // self.players[i].location.worldsectionId = player.location.worldsectionId;

                        if (player.connectionstatus === 0) {
                            self.players[i].destroy();
                            self.players.splice(i, 1);
                            self.placeOtherPlayersAndObjects();
                            return;
                        } else if (player.location.worldsectionId !== self.player.creature.location.worldsectionId) {
                            self.players[i].destroy();
                            self.players.splice(i, 1);
                            self.placeOtherPlayersAndObjects();
                            return;
                        }
                        self.players[i].updatePlayer(player);
                        self.placeOtherPlayersAndObjects();
                        return;
                    }
                }

                var newplayer = new OtherPlayer(self.game, player);
                self.middelgroundGroup.add(newplayer.group);
                self.players.push(newplayer);
                self.placeOtherPlayersAndObjects();
            };

            this.gameHub.client.message = function (message: string): void {
                self.log(message);
                self.messageText.text = message;
                self.messageText.visible = true;
                setTimeout(function () {
                    self.messageText.visible = false;
                },
                    500);
            }

            this.gameHub.client.updateNpc = function (npc: IPlayer): void {
                for (let i = 0; i < self.npcs.length; i++) {
                    if (self.npcs[i].creature.id === npc.id) {
                        // self.players[i].location.x = player.location.x;
                        // self.players[i].location.y = player.location.y;
                        // self.players[i].location.worldsectionId = player.location.worldsectionId;

                        if (npc.connectionstatus === 0) {
                            self.npcs[i].destroy();
                            self.npcs.splice(i, 1);
                            //self.placeOtherPlayersAndObjects();
                            return;
                        } else if (npc.location.worldsectionId !== self.player.creature.location.worldsectionId) {
                            self.npcs[i].destroy();
                            self.npcs.splice(i, 1);
                            //self.placeOtherPlayersAndObjects();
                            return;
                        }
                        self.npcs[i].updatePlayer(npc);
                        if (self.selectedCreature != undefined && npc.id === self.selectedCreature.creature.id) {
                            self.selectedCreaturePortraitplate.update(npc);
                        }
                        self.npcs[i].placeGroup();
                        //self.placeOtherPlayersAndObjects();
                        return;
                    }
                }

                var newnpc: NpcBase;
                if (npc.type === 1) {
                    newnpc = new Wolf(self.game, npc);
                } else if (npc.type === 2) {
                    newnpc = new Bunny(self.game, npc);
                } else {
                    newnpc = new ElfHunter(self.game, npc);
                }

                newnpc.npcSprite.inputEnabled = true;
                newnpc.npcSprite.events.onInputDown.add(function (sprite, pointer) {
                    if (pointer.button === 0) {
// ReSharper disable once SuspiciousThisUsage
                        self.selectedCreature = this; // <-- det er den rigtige this..
                        self.selectedCreaturePortraitplate.update(self.selectedCreature.creature);
                    }
                },
                    newnpc);

                self.middelgroundGroup.add(newnpc.group);
                self.npcs.push(newnpc);
                newnpc.placeGroup();
                //self.placeOtherPlayersAndObjects();
            };

            this.gameHub.client.updateInteractiveObjects = function (ios: IInteractiveObject[]): void {
                for (let i = 0; i < ios.length; i++) {
                    var exists: boolean = false;
                    for (let j = 0; j < self.interactiveObjects.length; j++) {
                        if (self.interactiveObjects[j].interactiveObject.id === ios[i].id) {
                            exists = true;
                            self.interactiveObjects[j].interactiveObject = ios[i];
                            break;
                        }
                    }

                    if (!exists) {
                        var newio: InteractiveObject = new InteractiveObject(self.game, ios[i], self.gameHub);
                        self.middelgroundGroup.add(newio.group);
                        self.interactiveObjects.push(newio);
                    }
                }

                self.placeOtherPlayersAndObjects();
            };

            this.gameHub.client.updateOwnPlayer = function (player: IPlayer): void {
                self.log("updateOwnPlayer callback");

                var x: number = player.location.x;
                var y: number = player.location.y;

                if (!y || y < 0) {
                    y = self.background === undefined
                        ? self.player.playerSprite.position.y / 32
                        : self.background.getTileY(self.player.playerSprite.position.y);
                }
                if (!x || x < 0) {
                    x = self.background === undefined
                        ? self.player.playerSprite.position.x / 32
                        : self.background.getTileX(self.player.playerSprite.position.x);
                }

                var height: number = self.map === undefined ? 32 : self.map.tileHeight;
                var width: number = self.map === undefined ? 32 : self.map.tileWidth;

                self.player.playerSprite.position.x = x * width;
                self.player.playerSprite.position.y = y * height;

                self.player.updatePlayer(player);
                self.playerPortraitplate.update(player);
            };

            this.gameHub.client.updateOwnPlayerNoRepositioning = function (player: IPlayer): void {
                self.log("updateOwnPlayerNoRepositioning callback");

                self.player.updatePlayer(player);
                self.playerPortraitplate.update(player);
            };

            this.gameHub.client.changeMap = function (mapToLoad: IWorldsection): void {
                self.log("Changemap callback");
                self.setBackgroundimage();

                if (mapToLoad === null || mapToLoad === undefined) {
                    // end of world
                    return;
                }

                self.destroyAllPlayersAndObjects();

                self.destroyMap();

                self.destroySounds();

                self.currentMap = mapToLoad;

                self.worldsectionnameplate.updateMap(mapToLoad);

                // load json
                self.game.cache.removeTilemap("world");
                self.game.load.tilemap("world", "/api/map/getmap/" + mapToLoad.id, null, Phaser.Tilemap.TILED_JSON);

                // load images
                for (let i = 0; i < mapToLoad.tilemap.tilesets.length; i++) {
                    if (!self.game.cache.checkImageKey(mapToLoad.tilemap.tilesets[i].name)) {
                        //self.log("Henter " + mapToLoad.tilemap.tilesets[i].name);
                        self.game.load.image(mapToLoad.tilemap.tilesets[i].name, "/content/assets/graphics/" + mapToLoad.tilemap.tilesets[i].image.source);
                    }
                }

                self.game.load.onLoadComplete.add(self.createMap, self);
                self.game.load.start();
            };

            $.connection.hub.start()
                .done(function (): void {
                    // map sende events up
                    // self.characterHub.server.enterWorldsection(self.player.location.worldsectionId, self.player.location.x, self.player.location.y);
                    self.gameHub.server.test();
                    self.gameHub.server.enterWorldsection(self.player.creature.location.worldsectionId, self.player.creature.location.x, self.player.creature.location.y);

                    self.signalRInitializing = false;

                    self.gameHub.server.changeMap("playerposition");
                });
        }

        private setBackgroundimage(): void {
            this.backgroundimage = this.add.image(0, 0, "loadingbackground");
            // this.backgroundimage.height = this.game.height;
            // this.backgroundimage.width = this.game.width;
            this.backgroundimage.smoothed = true;
        }

        private destroyMap(): void {
            if (!this.map) {
                return;
            }

            for (let i = 0; i < this.map.tilesets.length; i++) {
                this.map.tilesets[i] = null;
            }

            for (let layerindex = 0; layerindex < this.map.layers.length; layerindex++) {
                for (let tileindex = 0; tileindex < this.map.layers[layerindex].data.length; tileindex++) {
                    for (let j = 0; j < this.map.layers[layerindex].data[tileindex].length; j++) {
                        this.map.layers[layerindex].data[tileindex][j].destroy();
                        this.map.layers[layerindex].data[tileindex][j] = null;
                    }
                }
            }

            if (this.collisionLayer !== undefined) {
                this.collisionLayer.destroy();
            }

            this.backgroundGroup.removeAll(true, false, true);
            //this.middelgroundGroup.removeAll(true, false, true);// <-- spilleren er i den der, så den sletter vi ikke..
            this.aboveMiddelgroup.removeAll(true, false, true);

            this.background.destroy();
            this.map.destroy();

            // this.backgroundGroup.destroy(true);
            // this.middelgroundGroup.destroy(true);
            // this.aboveMiddelgroup.destroy(true);
            // this.uiGroup.destroy(true);
        }

        private destroySounds(): void {
            if (this.backgroundMusic) {
                this.backgroundMusic.stop();
                this.backgroundMusic.destroy(true);
                this.backgroundMusic = null;
            }
        }

        private destroyAllPlayersAndObjects(): void {
            for (let i = 0; i < this.players.length; i++) {
                var p = this.players[i];
                p.destroy();
            }
            this.players = new Array<OtherPlayer>();

            for (let i = 0; i < this.npcs.length; i++) {
                var npc: NpcBase = this.npcs[i];
                npc.destroy();
            }
            this.npcs = new Array<NpcBase>();

            for (let i = 0; i < this.interactiveObjects.length; i++) {
                var io: InteractiveObject = this.interactiveObjects[i];
                io.destroy();
            }
            this.interactiveObjects = new Array<InteractiveObject>();
        }

        private placeOtherPlayersAndObjects(): void {
            // players
            for (let i = 0; i < this.players.length; i++) {
                var p = this.players[i];
                if (p.player.location.worldsectionId !== this.player.creature.location.worldsectionId) {
                    continue;
                }

                p.placeGroup();
            }

            // npcs
            for (let i = 0; i < this.npcs.length; i++) {
                var npc = this.npcs[i];
                if (npc.creature.location.worldsectionId !== this.player.creature.location.worldsectionId) {
                    continue;
                }

                npc.placeGroup();
            }

            // objects
            for (let i = 0; i < this.interactiveObjects.length; i++) {
                var io = this.interactiveObjects[i];
                if (io.interactiveObject.location.worldsectionId !== this.player.creature.location.worldsectionId) {
                    continue;
                }

                io.placeGroup();
            }
        }

        private log(msg: string): void {
            console.log(msg);
        }


    }
}