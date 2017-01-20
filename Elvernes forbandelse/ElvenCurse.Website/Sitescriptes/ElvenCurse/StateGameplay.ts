module ElvenCurse {
    export class StateGameplay extends Phaser.State {
        // debug
        onlineCount:number;
        // debug

        backgroundimage:Phaser.Image;

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

        cursors: Phaser.CursorKeys;
        mapMovedInThisPosition:string = "";
        currentMap: IWorldsection;

        // ui stuff
        playerPortraitplate: EntityPortraitplate;
        actionbar: Actionbar;
        worldsectionnameplate: Worldsectionnameplate;

        // music
        backgroundMusic: Phaser.Sound;
        
        initializing: boolean = true;
        signalRInitializing: boolean = true;

        // weather
        shadowTexture: Phaser.BitmapData;

        constructor() {
            super();
        }

        create() {
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
            this.actionbar = new Actionbar(this.game, this.player.creature);
            this.uiGroup.add(this.actionbar.group);
            this.worldsectionnameplate = new Worldsectionnameplate(this.game, this.currentMap);
            this.uiGroup.add(this.worldsectionnameplate.group);


            // signalr
            this.wireupSignalR();

            // physics
            this.game.physics.arcade.enable(this.player.playerSprite);
            this.game.camera.follow(this.player.playerSprite);

            // inputs
            this.cursors = this.game.input.keyboard.createCursorKeys();
        }

        update() {
            this.player.checkCollisions(this.collisionLayer);

            if (this.background == undefined) {
                return;
            }

            // move
            if (this.signalRInitializing) {
                return;
            }
            
            var oldX = this.player.creature.location.x, oldY = this.player.creature.location.y;
            this.player.move(this.cursors);
            this.player.creature.location.x = this.background.getTileX(this.player.playerSprite.x);
            this.player.creature.location.y = this.background.getTileX(this.player.playerSprite.y);
            if (this.player.creature.location.x !== oldX || this.player.creature.location.y !== oldY) {
                this.gameHub.server.movePlayer(this.player.creature.location.worldsectionId, this.player.creature.location.x, this.player.creature.location.y);
                
            }

            //this.placeOtherPlayersAndObjects();

            this.worldsectionnameplate.setPlayerPosition(this.player);

            //this.updateNightshadow();
        }

        private updateNightshadow() {
            if (this.initializing) {
                return;
            }

            // sæt vejr.. test
            if (this.shadowTexture === undefined) {
                this.shadowTexture = this.game.add.bitmapData(this.game.width, this.game.height);
                var lightSprite = this.game.add.image(0, 0, this.shadowTexture);
                lightSprite.blendMode = PIXI.blendModes.MULTIPLY;
                lightSprite.smoothed = true;

                this.uiGroup.add(lightSprite);
            }
            
            // Draw shadow
            this.shadowTexture.context.fillStyle = 'rgb(50, 50, 50)';
            this.shadowTexture.context.fillRect(0, 0, this.game.width, this.game.height);
            
            // Draw circle of light
            this.shadowTexture.context.beginPath();
            this.shadowTexture.context.fillStyle = 'rgb(255, 255, 255)';
            this.shadowTexture.context.arc(this.player.playerSprite.worldTransform.tx, this.player.playerSprite.worldTransform.ty, 100, 0, Math.PI * 2);
            this.shadowTexture.context.fill();

            // This just tells the engine it should update the texture cache
            this.shadowTexture.dirty = true;
        }

        render() {
            if (this.initializing) {
                return;
            }
            
            //this.game.debug.text(this.currentMap.name, 32, 32+50, "rgb(0,0,0)");
            //this.game.debug.text("Tile X: " + this.background.getTileX(this.player.playerSprite.x) + " position.x: " + this.player.playerSprite.position.x, 32, 48 + 50, "rgb(0,0,0)");
            //this.game.debug.text("Tile Y: " + this.background.getTileY(this.player.playerSprite.y) + " position.y: " + this.player.playerSprite.position.y, 32, 64 + 50, "rgb(0,0,0)");

            this.game.debug.text("Online: " + this.onlineCount, 32, 80, "rgb(0,0,0)");
        }

        private createMap() {
            this.log("CreateMap");
            this.game.load.onLoadComplete.remove(this.createMap, this);

            this.backgroundMusic = this.game.add.audio("medieval");
            //this.backgroundMusic.play();

            this.map = this.game.add.tilemap("world");

            var collisionTileId = -1;
            var i: number;
            for (i = 0; i < this.map.tilesets.length; i++) {
                var tileset = this.map.tilesets[i];
                this.map.addTilesetImage(tileset.name, tileset.name, tileset.tileWidth, tileset.tileHeight);
                
                if (tileset.name.toLowerCase() === "collision") {
                    collisionTileId = tileset.firstgid;
                }
            }
            
            for (i = 0; i < this.map.layers.length; i++) {
                var layer = this.map.layers[i];
                
                var l = this.map.createLayer(layer.name);
                
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
                }
                else if (layer.name.toLowerCase() === "collision" || layer.name.toLowerCase() === "collisionlayer") {
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

        private wireupSignalR() {
            var self = this;

            $.connection.hub.url = $("#serverpath").text() + "/signalr";
            this.gameHub = $.connection.gameHub;

            if (this.gameHub == undefined) {
                location.href = '/world/gameserverdown';
                return;
            }

            $.connection.hub.stateChanged(function (change) {
                console.log(change);
                // vi understøtter ikke reconnect.. så bare send folk til startsiden hvis vi ryger af..
                if (change.newState === $.connection.connectionState.disconnected ||
                    change.newState === $.connection.connectionState.reconnecting) {
                    location.href = '/Character';
                }
            });

            //this.gameHub.client.hello = function (text) {
            //    var t = 0;
            //}

            this.gameHub.client.onlinecount = function (cnt) {
                self.log("onlinecount callback");

                self.onlineCount = cnt;
            };

            this.gameHub.client.updatePlayer = function (player: IPlayer) {
                for (var i = 0; i < self.players.length; i++) {
                    if (self.players[i].player.id === player.id) {
                        //self.players[i].location.x = player.location.x;
                        //self.players[i].location.y = player.location.y;
                        //self.players[i].location.worldsectionId = player.location.worldsectionId;

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

            this.gameHub.client.updateNpc = function (npc: IPlayer) {
                for (var i = 0; i < self.npcs.length; i++) {
                    if (self.npcs[i].npc.id === npc.id) {
                        //self.players[i].location.x = player.location.x;
                        //self.players[i].location.y = player.location.y;
                        //self.players[i].location.worldsectionId = player.location.worldsectionId;

                        if (npc.connectionstatus === 0) {
                            self.npcs[i].destroy();
                            self.npcs.splice(i, 1);
                            self.placeOtherPlayersAndObjects();
                            return;
                        } else if (npc.location.worldsectionId !== self.player.creature.location.worldsectionId) {
                            self.npcs[i].destroy();
                            self.npcs.splice(i, 1);
                            self.placeOtherPlayersAndObjects();
                            return;
                        }
                        self.npcs[i].updatePlayer(npc);
                        self.placeOtherPlayersAndObjects();
                        return;
                    }
                }

                var newnpc;
                if (npc.type === 1) {
                    newnpc = new Wolf(self.game, npc);
                } else if (npc.type === 2) {
                    newnpc = new Bunny(self.game, npc);
                } else {
                    newnpc = new ElfHunter(self.game, npc);
                }
                
                self.middelgroundGroup.add(newnpc.group);
                self.npcs.push(newnpc);
                self.placeOtherPlayersAndObjects();
            };

            this.gameHub.client.updateInteractiveObjects = function (ios: IInteractiveObject[]) {
                for (var i = 0; i < ios.length; i++) {
                    var exists = false;
                    for (var j = 0; j < self.interactiveObjects.length; j++) {
                        if (self.interactiveObjects[j].interactiveObject.id === ios[i].id) {
                            exists = true;
                            self.interactiveObjects[j].interactiveObject = ios[i];
                            break;
                        }
                    }

                    if (!exists) {
                        var newio = new InteractiveObject(self.game, ios[i], self.gameHub);
                        self.middelgroundGroup.add(newio.group);
                        self.interactiveObjects.push(newio);
                    }
                }

                self.placeOtherPlayersAndObjects();
            };

            this.gameHub.client.updateOwnPlayer = function (player: IPlayer) {
                self.log("updateOwnPlayer callback");

                var x = player.location.x;
                var y = player.location.y;

                if (!y || y < 0) {
                    y = self.background == undefined ? self.player.playerSprite.position.y / 32 : self.background.getTileY(self.player.playerSprite.position.y);
                }
                if (!x || x < 0) {
                    x = self.background == undefined ? self.player.playerSprite.position.x / 32 : self.background.getTileX(self.player.playerSprite.position.x);
                }

                var height = self.map == undefined ? 32 : self.map.tileHeight;
                var width = self.map == undefined ? 32 : self.map.tileWidth;

                self.player.playerSprite.position.x = x * width;
                self.player.playerSprite.position.y = y * height;

                self.player.updatePlayer(player);
            }

            this.gameHub.client.changeMap = function (mapToLoad: IWorldsection) {
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
                
                // Load json
                self.game.load.tilemap("world", "/api/map/getmap/" + mapToLoad.id, null, Phaser.Tilemap.TILED_JSON);

                // Load images
                for (var i = 0; i < mapToLoad.tilemap.tilesets.length; i++) {
                    if (!self.game.cache.checkImageKey(mapToLoad.tilemap.tilesets[i].name)) {
                        self.game.load.image(mapToLoad.tilemap.tilesets[i].name, "/content/assets/graphics/" + mapToLoad.tilemap.tilesets[i].image.source);
                    }
                }
                
                self.game.load.onLoadComplete.add(self.createMap, self);
                self.game.load.start();
            }

            
            $.connection.hub.start()
                .done(function() {
                    // map sende events up
                    //self.characterHub.server.enterWorldsection(self.player.location.worldsectionId, self.player.location.x, self.player.location.y);
                    self.gameHub.server.test();
                    self.gameHub.server.enterWorldsection(self.player.creature.location.worldsectionId, self.player.creature.location.x, self.player.creature.location.y);
                    
                    self.signalRInitializing = false;

                    self.gameHub.server.changeMap("playerposition");
                });
        }

     
        private setBackgroundimage() {
            this.backgroundimage = this.add.image(0, 0, "loadingbackground");
            //this.backgroundimage.height = this.game.height;
            //this.backgroundimage.width = this.game.width;
            this.backgroundimage.smoothed = true;
            
        }

        private destroyMap() {
            if (!this.map) {
                return;
            }
            
            for (var i = 0; i < this.map.tilesets.length; i++) {
                this.map.tilesets[i] = null;
            }

            for (var layerindex = 0; layerindex < this.map.layers.length; layerindex++) {
                for (var tileindex = 0; tileindex < this.map.layers[layerindex].data.length; tileindex++) {
                    for (var j = 0; j < this.map.layers[layerindex].data[tileindex].length; j++) {
                        this.map.layers[layerindex].data[tileindex][j].destroy();
                        this.map.layers[layerindex].data[tileindex][j] = null;
                    }
                    
                }
            }

            if (this.collisionLayer !== undefined) {
                this.collisionLayer.destroy();
            }
            
            this.background.destroy();
            this.map.destroy();

            //this.backgroundGroup.destroy(true);
            //this.middelgroundGroup.destroy(true);
            //this.aboveMiddelgroup.destroy(true);
            //this.uiGroup.destroy(true);
        }

        private destroySounds() {
            if (this.backgroundMusic) {
                this.backgroundMusic.stop();
                this.backgroundMusic.destroy(true);
                this.backgroundMusic = null; 
            }
        }

        private destroyAllPlayersAndObjects() {
            var i: number;
            for (i = 0; i < this.players.length; i++) {
                var p = this.players[i];
                p.destroy();
            }
            this.players = new Array<OtherPlayer>();

            for (i = 0; i < this.npcs.length; i++) {
                var npc = this.npcs[i];
                npc.destroy();
            }
            this.npcs = new Array<NpcBase>();

            for (i = 0; i < this.interactiveObjects.length; i++) {
                var io = this.interactiveObjects[i];
                io.destroy();
            }
            this.interactiveObjects = new Array<InteractiveObject>();
        }

        private placeOtherPlayersAndObjects() {
            // players
            var i: number;
            for (i = 0; i < this.players.length; i++) {
                var p = this.players[i];
                if (p.player.location.worldsectionId !== this.player.creature.location.worldsectionId) {
                    continue;
                }

                p.placeGroup();
            }

            // npcs
            for (i = 0; i < this.npcs.length; i++) {
                var npc = this.npcs[i];
                if (npc.npc.location.worldsectionId !== this.player.creature.location.worldsectionId) {
                    continue;
                }

                npc.placeGroup();
            }

            // objects
            for (i = 0; i < this.interactiveObjects.length; i++) {
                var io = this.interactiveObjects[i];
                if (io.interactiveObject.location.worldsectionId !== this.player.creature.location.worldsectionId) {
                    continue;
                }

                io.placeGroup();
            }
        }

        private log(msg:string) {
            console.log(msg);
        }


    }
}