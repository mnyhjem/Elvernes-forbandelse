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
            this.changingMap = false;
            this.mapPath = "/content/assets/";
            this.initializing = true;
        }
        StateGameplay.prototype.create = function () {
            this.game.physics.startSystem(Phaser.Physics.ARCADE);
            this.player = new ElvenCurse.Player(this.game.add.sprite(450, 80, "car"), this.game);
            //this.createMap();
            this.changeMap("playerposition");
            this.wireupSignalR();
            //this.player = this.game.add.sprite(450, 80, 'car');
            //this.player.anchor.setTo(0.5, 0.5);
            //this.placeplayer(this.player.location.x, this.player.location.y);
            this.game.physics.arcade.enable(this.player.playerSprite);
            this.game.camera.follow(this.player.playerSprite);
            this.cursors = this.game.input.keyboard.createCursorKeys();
        };
        StateGameplay.prototype.update = function () {
            this.player.checkCollisions(this.blocking);
            this.player.move(this.cursors);
            // Change map stuff...
            if (this.changingMap) {
                return;
            }
            if (this.background.getTileX(this.player.playerSprite.x) < 1) {
                this.changeMap("left");
            }
            else if (this.background.getTileX(this.player.playerSprite.x) >= this.map.width) {
                this.changeMap("right");
            }
            else if (this.background.getTileY(this.player.playerSprite.y) > this.map.height) {
                this.changeMap("down");
            }
            else if (this.background.getTileY(this.player.playerSprite.y) < 1) {
                this.changeMap("up");
            }
        };
        StateGameplay.prototype.render = function () {
            if (this.initializing) {
                return;
            }
            this.game.debug.text(this.background.layer.properties.name, 32, 32, "rgb(0,0,0)");
            this.game.debug.text("Tile X: " + this.background.getTileX(this.player.playerSprite.x) + " position.x: " + this.player.playerSprite.position.x, 32, 48, "rgb(0,0,0)");
            this.game.debug.text("Tile Y: " + this.background.getTileY(this.player.playerSprite.y) + " position.y: " + this.player.playerSprite.position.y, 32, 64, "rgb(0,0,0)");
        };
        StateGameplay.prototype.changeMap = function (direction) {
            this.changingMap = true;
            var mapToLoad = "";
            switch (direction) {
                case "left":
                    mapToLoad = this.background.layer.properties.mapchange_left + ".json";
                    this.mapMovedInThisPosition = "left";
                    break;
                case "right":
                    mapToLoad = this.background.layer.properties.mapchange_right + ".json";
                    this.mapMovedInThisPosition = "right";
                    break;
                case "up":
                    mapToLoad = this.background.layer.properties.mapchange_up + ".json";
                    this.mapMovedInThisPosition = "up";
                    break;
                case "down":
                    mapToLoad = this.background.layer.properties.mapchange_down + ".json";
                    this.mapMovedInThisPosition = "down";
                    break;
                case "playerposition":
                    mapToLoad = this.player.location.jsonname + ".json";
                    this.mapMovedInThisPosition = "playerposition";
                    break;
            }
            if (mapToLoad === "undefined.json") {
                console.log("End of world");
                return;
            }
            if (this.map) {
                this.map.destroy();
            }
            this.game.load.tilemap("world", this.mapPath + mapToLoad, null, Phaser.Tilemap.TILED_JSON);
            this.game.load.onLoadComplete.add(this.createMap, this);
            this.game.load.start();
        };
        StateGameplay.prototype.placeplayer = function (x, y) {
            if (!y || y < 0) {
                y = this.background.getTileY(this.player.playerSprite.position.y);
            }
            if (!x || x < 0) {
                x = this.background.getTileX(this.player.playerSprite.position.x);
            }
            this.player.playerSprite.position.x = x * this.map.tileWidth;
            this.player.playerSprite.position.y = y * this.map.tileHeight;
        };
        StateGameplay.prototype.createMap = function () {
            this.map = this.game.add.tilemap("world");
            this.map.addTilesetImage("water", "water");
            this.map.addTilesetImage("ground", "ground");
            this.blocking = this.map.createLayer("blocking");
            this.background = this.map.createLayer("background");
            this.background.resizeWorld();
            this.map.setCollisionBetween(1, 100, true, this.blocking);
            //map.setCollision(23, true, background)
            if (this.player) {
                this.player.playerSprite.bringToTop();
                switch (this.mapMovedInThisPosition) {
                    case "left":
                        this.placeplayer(99, -1);
                        break;
                    case "right":
                        this.placeplayer(1, -1);
                        break;
                    case "up":
                        this.placeplayer(-1, 99);
                        break;
                    case "down":
                        this.placeplayer(-1, 1);
                        break;
                    case "playerposition":
                        this.placeplayer(this.player.location.x, this.player.location.y);
                        break;
                }
            }
            this.changingMap = false;
            this.initializing = false;
        };
        StateGameplay.prototype.wireupSignalR = function () {
            this.characterHub = $.connection.characterHub;
            //this.characterHub.client.methodehalløj = function ()
            this.characterHub.client.hello = function (text) {
                var t = 0;
            };
            var self = this;
            $.connection.hub.start()
                .done(function () {
                // map sende events up
                //self.characterHub.server.enterWorldsection(self.player.location.worldsectionId, self.player.location.x, self.player.location.y);
                self.characterHub.server.test();
            });
        };
        return StateGameplay;
    }(Phaser.State));
    ElvenCurse.StateGameplay = StateGameplay;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=StateGameplay.js.map