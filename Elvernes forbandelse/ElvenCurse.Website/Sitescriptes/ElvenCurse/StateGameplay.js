var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ElvenCurse;
(function (ElvenCurse) {
    var StateGameplay = (function (_super) {
        __extends(StateGameplay, _super);
        function StateGameplay() {
            _super.call(this);
            this.mapMovedInThisPosition = "";
            //waitingForServer: boolean = true;
            this.mapPath = "/content/assets/";
            this.initializing = true;
            this.signalRInitializing = true;
        }
        StateGameplay.prototype.create = function () {
            this.game.stage.disableVisibilityChange = true;
            this.backgroundGroup = this.game.add.group();
            this.middelgroundGroup = this.game.add.group();
            this.game.world.bringToTop(this.middelgroundGroup);
            this.uiGroup = this.game.add.group();
            //this.game.world.bringToTop(this.uiGroup);
            this.players = new Array();
            this.game.physics.startSystem(Phaser.Physics.ARCADE);
            this.player = new ElvenCurse.Player(this.game.add.sprite(450, 80, "player"), this.game);
            this.middelgroundGroup.add(this.player.playerGroup);
            this.wireupSignalR();
            this.game.physics.arcade.enable(this.player.playerSprite);
            this.game.camera.follow(this.player.playerSprite);
            this.cursors = this.game.input.keyboard.createCursorKeys();
        };
        StateGameplay.prototype.update = function () {
            this.player.checkCollisions(this.blocking);
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
            this.placeOtherPlayers();
        };
        StateGameplay.prototype.render = function () {
            if (this.initializing) {
                return;
            }
            this.game.debug.text(this.background.layer.properties.name, 32, 32, "rgb(0,0,0)");
            this.game.debug.text("Tile X: " + this.background.getTileX(this.player.playerSprite.x) + " position.x: " + this.player.playerSprite.position.x, 32, 48, "rgb(0,0,0)");
            this.game.debug.text("Tile Y: " + this.background.getTileY(this.player.playerSprite.y) + " position.y: " + this.player.playerSprite.position.y, 32, 64, "rgb(0,0,0)");
            this.game.debug.text("Online: " + this.onlineCount, 32, 80, "rgb(0,0,0)");
        };
        StateGameplay.prototype.placeplayer = function (x, y) {
            this.log("placeplayer");
            if (!y || y < 0) {
                y = this.background == undefined ? this.player.playerSprite.position.y / 32 : this.background.getTileY(this.player.playerSprite.position.y);
            }
            if (!x || x < 0) {
                x = this.background == undefined ? this.player.playerSprite.position.x / 32 : this.background.getTileX(this.player.playerSprite.position.x);
            }
            var height = this.map == undefined ? 32 : this.map.tileHeight;
            var width = this.map == undefined ? 32 : this.map.tileWidth;
            this.player.playerSprite.position.x = x * width;
            this.player.playerSprite.position.y = y * height;
            //this.player.location
        };
        StateGameplay.prototype.createMap = function () {
            this.log("CreateMap");
            this.map = this.game.add.tilemap("world");
            this.map.addTilesetImage("water", "water");
            this.map.addTilesetImage("ground", "ground");
            this.blocking = this.map.createLayer("blocking");
            this.background = this.map.createLayer("background");
            this.backgroundGroup.add(this.background);
            this.backgroundGroup.add(this.blocking);
            this.background.resizeWorld();
            this.map.setCollisionBetween(1, 100, true, this.blocking);
            //map.setCollision(23, true, background)
            if (this.player) {
                //this.player.bringToTop();
                //this.game.world.bringToTop(this.middelgroundGroup);
                this.placeOtherPlayers();
            }
            this.initializing = false;
        };
        StateGameplay.prototype.wireupSignalR = function () {
            var self = this;
            this.gameHub = $.connection.gameHub;
            //this.characterHub.client.methodehalløj = function ()
            this.gameHub.client.hello = function (text) {
                var t = 0;
            };
            this.gameHub.client.onlinecount = function (cnt) {
                self.log("onlinecount callback");
                self.onlineCount = cnt;
            };
            this.gameHub.client.updatePlayer = function (player) {
                self.log("updatePlayer callback");
                for (var i = 0; i < self.players.length; i++) {
                    if (self.players[i].player.id === player.id) {
                        //self.players[i].location.x = player.location.x;
                        //self.players[i].location.y = player.location.y;
                        //self.players[i].location.worldsectionId = player.location.worldsectionId;
                        if (player.connectionstatus === 0) {
                            self.log(player.name + " went offline");
                            self.players[i].destroy();
                            self.players.splice(i, 1);
                            return;
                        }
                        else if (player.location.worldsectionId !== self.player.location.worldsectionId) {
                            self.log(player.name + " left worldsection");
                            self.players[i].destroy();
                            self.players.splice(i, 1);
                            return;
                        }
                        self.players[i].updatePosition(player);
                        self.log(player.name + " moved");
                        return;
                    }
                }
                var newplayer = new ElvenCurse.OtherPlayer(self.game, player);
                self.middelgroundGroup.add(newplayer.playerGroup);
                self.players.push(newplayer);
                console.log(player.name + " added");
            };
            this.gameHub.client.updateOwnPlayer = function (player) {
                self.log("updateOwnPlayer callback");
                self.player.location.x = player.location.x;
                self.player.location.y = player.location.y;
                self.player.location.worldsectionId = player.location.worldsectionId;
                self.placeplayer(player.location.x, player.location.y);
            };
            this.gameHub.client.changeMap = function (mapToLoad) {
                self.log("Changemap callback");
                if (mapToLoad === null || mapToLoad === undefined) {
                    // end of world
                    return;
                }
                self.destroyAllPlayers();
                if (self.map) {
                    self.map.destroy();
                }
                // Load json
                //self.game.load.tilemap("world", null, mapToLoad.json, Phaser.Tilemap.TILED_JSON);
                self.game.load.tilemap("world", "/api/map/getmap/" + mapToLoad.id, null, Phaser.Tilemap.TILED_JSON);
                // Load images
                for (var i = 0; i < mapToLoad.tilemap.tilesets.length; i++) {
                    if (!self.game.cache.checkImageKey(mapToLoad.tilemap.tilesets[i].name)) {
                        self.game.load.image(mapToLoad.tilemap.tilesets[i].name, "/content/assets/graphics/" + mapToLoad.tilemap.tilesets[i].image);
                    }
                }
                //self.game.load.tilemap("world", self.mapPath + mapToLoad, null, Phaser.Tilemap.TILED_JSON);
                self.game.load.onLoadComplete.add(self.createMap, self);
                self.game.load.start();
            };
            $.connection.hub.start()
                .done(function () {
                // map sende events up
                //self.characterHub.server.enterWorldsection(self.player.location.worldsectionId, self.player.location.x, self.player.location.y);
                self.gameHub.server.test();
                self.gameHub.server.enterWorldsection(self.player.location.worldsectionId, self.player.location.x, self.player.location.y);
                self.signalRInitializing = false;
                self.gameHub.server.changeMap("playerposition");
            });
        };
        StateGameplay.prototype.destroyAllPlayers = function () {
            for (var i = 0; i < this.players.length; i++) {
                var p = this.players[i];
                p.destroy();
            }
            this.players = new Array();
        };
        StateGameplay.prototype.placeOtherPlayers = function () {
            for (var i = 0; i < this.players.length; i++) {
                var p = this.players[i];
                if (p.player.location.worldsectionId !== this.player.location.worldsectionId) {
                    continue;
                }
                p.placeGroup();
            }
        };
        StateGameplay.prototype.log = function (msg) {
            console.log(msg);
        };
        return StateGameplay;
    }(Phaser.State));
    ElvenCurse.StateGameplay = StateGameplay;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=StateGameplay.js.map