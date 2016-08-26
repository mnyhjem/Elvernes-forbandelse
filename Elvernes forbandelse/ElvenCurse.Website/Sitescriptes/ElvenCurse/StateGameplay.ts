module ElvenCurse {
    export class StateGameplay extends Phaser.State {
        // debug
        onlineCount:number;
        // debug


        gameHub: IGameHub;

        game: Phaser.Game;
        map: Phaser.Tilemap;
        background: Phaser.TilemapLayer;
        collisionLayer: Phaser.TilemapLayer;
        tileLayers:Phaser.TilemapLayer[];

        // groups
        backgroundGroup: Phaser.Group;
        middelgroundGroup: Phaser.Group;
        uiGroup: Phaser.Group;

        player: Player;
        players: OtherPlayer[];
        npcs: OtherPlayer[];

        cursors: Phaser.CursorKeys;
        mapMovedInThisPosition:string = "";
        currentMap: IWorldsection;
        
        mapPath: string = "/content/assets/";
        initializing: boolean = true;
        signalRInitializing: boolean = true;

        constructor() {
            super();
        }

        create() {
            this.game.stage.disableVisibilityChange = true;

            this.backgroundGroup = this.game.add.group();
            this.middelgroundGroup = this.game.add.group();
            this.game.world.bringToTop(this.middelgroundGroup);
            this.uiGroup = this.game.add.group();

            this.players = new Array<OtherPlayer>();
            this.npcs = new Array<OtherPlayer>();

            this.game.physics.startSystem(Phaser.Physics.ARCADE);

            this.player = new Player(this.game);
            this.middelgroundGroup.add(this.player.playerGroup);
            
            this.wireupSignalR();

            this.game.physics.arcade.enable(this.player.playerSprite);
            this.game.camera.follow(this.player.playerSprite);

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

            var oldX = this.player.location.x, oldY = this.player.location.y;
            this.player.move(this.cursors);
            this.player.location.x = this.background.getTileX(this.player.playerSprite.x);
            this.player.location.y = this.background.getTileX(this.player.playerSprite.y);
            if (this.player.location.x !== oldX || this.player.location.y !== oldY) {
                this.gameHub.server.movePlayer(this.player.location.worldsectionId, this.player.location.x, this.player.location.y);
            }

            this.placeOtherPlayersAndNpcs();
        }

        render() {
            if (this.initializing) {
                return;
            }
            
            this.game.debug.text(this.currentMap.name, 32, 32, "rgb(0,0,0)");
            this.game.debug.text("Tile X: " + this.background.getTileX(this.player.playerSprite.x) + " position.x: " + this.player.playerSprite.position.x, 32, 48, "rgb(0,0,0)");
            this.game.debug.text("Tile Y: " + this.background.getTileY(this.player.playerSprite.y) + " position.y: " + this.player.playerSprite.position.y, 32, 64, "rgb(0,0,0)");

            this.game.debug.text("Online: " + this.onlineCount, 32, 80, "rgb(0,0,0)");
        }

        private placeplayer(x: number, y: number) {
            this.log("placeplayer");

            if (!y || y < 0) {
                y = this.background == undefined ? this.player.playerSprite.position.y / 32 : this.background.getTileY(this.player.playerSprite.position.y);
            }
            if (!x || x < 0) {
                x = this.background == undefined ? this.player.playerSprite.position.x / 32 :this.background.getTileX(this.player.playerSprite.position.x);
            }

            var height = this.map == undefined ? 32 : this.map.tileHeight;
            var width = this.map == undefined ? 32 : this.map.tileWidth;

            this.player.playerSprite.position.x = x * width;
            this.player.playerSprite.position.y = y * height;
            //this.player.location
        }

        private createMap() {
            this.log("CreateMap");

            this.map = this.game.add.tilemap("world");

            //this.map.addTilesetImage("water", "water");
            //this.map.addTilesetImage("ground", "ground");
            var collisionTileId = -1;
            for (var i = 0; i < this.map.tilesets.length; i++) {
                var tileset = this.map.tilesets[i];
                if (tileset.name == "treesv6_0") {
                    this.map.addTilesetImage(tileset.name, tileset.name, tileset.tileWidth, tileset.tileHeight);
                } else {
                    this.map.addTilesetImage(tileset.name, tileset.name, tileset.tileWidth, tileset.tileHeight);
                }
                
                if (tileset.name === "Collision") {
                    collisionTileId = tileset.firstgid;
                }
            }
            
            for (var i = 0; i < this.map.layers.length; i++) {
                var layer = this.map.layers[i];
                
                var l = this.map.createLayer(layer.name);
                this.backgroundGroup.add(l);

                if (layer.name === "background") {
                    this.background = l;
                }
                else if (layer.name === "collision" || layer.name === "collisionLayer") {
                    //l.visible = false;
                    this.collisionLayer = l;
                }
            }
            
            this.background.resizeWorld();
            
            //this.map.setCollisionBetween(1, 100, true, this.blocking);
            //this.map.setCollision([13, 133], true, this.blocking);
            this.map.setCollision(collisionTileId, true, this.collisionLayer);

            if (this.player) {
                //this.player.bringToTop();
                //this.game.world.bringToTop(this.middelgroundGroup);
                this.placeOtherPlayersAndNpcs();
            }
            this.initializing = false;
        }

        private wireupSignalR() {
            var self = this;

            this.gameHub = $.connection.gameHub;
            //this.characterHub.client.methodehalløj = function ()
            this.gameHub.client.hello = function(text) {
                var t = 0;
            }

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
                            return;
                        } else if (player.location.worldsectionId !== self.player.location.worldsectionId) {
                            self.players[i].destroy();
                            self.players.splice(i, 1);
                            return;
                        }
                        self.players[i].updatePosition(player);
                        return;
                    }
                }

                var newplayer = new OtherPlayer(self.game, player);
                self.middelgroundGroup.add(newplayer.playerGroup);
                self.players.push(newplayer);
            };

            this.gameHub.client.updateNpc = function (npc: IPlayer) {
                for (var i = 0; i < self.npcs.length; i++) {
                    if (self.npcs[i].player.id === npc.id) {
                        //self.players[i].location.x = player.location.x;
                        //self.players[i].location.y = player.location.y;
                        //self.players[i].location.worldsectionId = player.location.worldsectionId;

                        if (npc.connectionstatus === 0) {
                            self.npcs[i].destroy();
                            self.npcs.splice(i, 1);
                            return;
                        } else if (npc.location.worldsectionId !== self.player.location.worldsectionId) {
                            self.npcs[i].destroy();
                            self.npcs.splice(i, 1);
                            return;
                        }
                        self.npcs[i].updatePosition(npc);
                        return;
                    }
                }

                var newnpc = new OtherPlayer(self.game, npc);
                self.middelgroundGroup.add(newnpc.playerGroup);
                self.npcs.push(newnpc);
            };

            this.gameHub.client.updateOwnPlayer = function (player: IPlayer) {
                self.log("updateOwnPlayer callback");
                self.player.location.x = player.location.x;
                self.player.location.y = player.location.y;
                self.player.location.worldsectionId = player.location.worldsectionId;
                self.placeplayer(player.location.x, player.location.y);
            }

            this.gameHub.client.changeMap = function (mapToLoad: IWorldsection) {
                self.log("Changemap callback");

                if (mapToLoad === null || mapToLoad === undefined) {
                    // end of world
                    return;
                }

                self.destroyAllPlayersAndNpcs();

                if (self.map) {
                    self.map.destroy();
                }

                self.currentMap = mapToLoad;
                
                // Load json
                //self.game.load.tilemap("world", null, mapToLoad.json, Phaser.Tilemap.TILED_JSON);
                self.game.load.tilemap("world", "/api/map/getmap/" + mapToLoad.id, null, Phaser.Tilemap.TILED_JSON);

                // Load images
                for (var i = 0; i < mapToLoad.tilemap.tilesets.length; i++) {
                    if (!self.game.cache.checkImageKey(mapToLoad.tilemap.tilesets[i].name)) {
                        //self.game.load.image(mapToLoad.tilemap.tilesets[i].name, "/content/assets/graphics/" + mapToLoad.tilemap.tilesets[i].image);
                    }
                }

                //self.game.load.tilemap("world", self.mapPath + mapToLoad, null, Phaser.Tilemap.TILED_JSON);

                self.game.load.onLoadComplete.add(self.createMap, self);
                self.game.load.start();
            }

            
            $.connection.hub.start()
                .done(function() {
                    // map sende events up
                    //self.characterHub.server.enterWorldsection(self.player.location.worldsectionId, self.player.location.x, self.player.location.y);
                    self.gameHub.server.test();
                    self.gameHub.server.enterWorldsection(self.player.location.worldsectionId, self.player.location.x, self.player.location.y);
                    
                    self.signalRInitializing = false;

                    self.gameHub.server.changeMap("playerposition");
                });
        }

        private destroyAllPlayersAndNpcs() {
            for (var i = 0; i < this.players.length; i++) {
                var p = this.players[i];
                p.destroy();
            }
            this.players = new Array<OtherPlayer>();

            for (var i = 0; i < this.npcs.length; i++) {
                var p = this.npcs[i];
                p.destroy();
            }
            this.npcs = new Array<OtherPlayer>();
        }

        private placeOtherPlayersAndNpcs() {
            for (var i = 0; i < this.players.length; i++) {
                var p = this.players[i];
                if (p.player.location.worldsectionId !== this.player.location.worldsectionId) {
                    continue;
                }

                p.placeGroup();
            }

            for (var i = 0; i < this.npcs.length; i++) {
                var p = this.npcs[i];
                if (p.player.location.worldsectionId !== this.player.location.worldsectionId) {
                    continue;
                }

                p.placeGroup();
            }
        }

        private log(msg:string) {
            console.log(msg);
        }
    }
}