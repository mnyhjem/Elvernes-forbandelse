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
        }
        StateGameplay.prototype.create = function () {
            this.game.physics.startSystem(Phaser.Physics.ARCADE);
            this.createMap();
            this.player = this.game.add.sprite(450, 80, 'car');
            this.player.anchor.setTo(0.5, 0.5);
            this.placeplayer(80, 30);
            this.game.physics.arcade.enable(this.player);
            this.game.camera.follow(this.player);
            this.cursors = this.game.input.keyboard.createCursorKeys();
        };
        StateGameplay.prototype.update = function () {
            this.game.physics.arcade.collide(this.player, this.blocking);
            this.player.body.velocity.x = 0;
            this.player.body.velocity.y = 0;
            this.player.body.angularVelocity = 0;
            if (this.cursors.left.isDown) {
                this.player.body.angularVelocity = -200;
            }
            else if (this.cursors.right.isDown) {
                this.player.body.angularVelocity = 200;
            }
            if (this.cursors.up.isDown) {
                this.player.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(this.player.angle, 300));
            }
            if (this.cursors.down.isDown) {
                this.player.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(this.player.angle, -300));
            }
            // Change map stuff...
            if (this.changingMap) {
                return;
            }
            if (this.background.getTileX(this.player.x) < 1) {
                this.changeMap("left");
            }
            else if (this.background.getTileX(this.player.x) >= this.map.width) {
                this.changeMap("right");
            }
            else if (this.background.getTileY(this.player.y) > this.map.height) {
                this.changeMap("down");
            }
            else if (this.background.getTileY(this.player.y) < 1) {
                this.changeMap("up");
            }
        };
        StateGameplay.prototype.render = function () {
            this.game.debug.text(this.background.layer.properties.name, 32, 32, "rgb(0,0,0)");
            this.game.debug.text("Tile X: " + this.background.getTileX(this.player.x) + " position.x: " + this.player.position.x, 32, 48, "rgb(0,0,0)");
            this.game.debug.text("Tile Y: " + this.background.getTileY(this.player.y) + " position.y: " + this.player.position.y, 32, 64, "rgb(0,0,0)");
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
            }
            if (mapToLoad === "undefined.json") {
                console.log("End of world");
                return;
            }
            this.map.destroy();
            this.game.load.tilemap("world", this.mapPath + mapToLoad, null, Phaser.Tilemap.TILED_JSON);
            this.game.load.onLoadComplete.add(this.createMap, this);
            this.game.load.start();
        };
        StateGameplay.prototype.placeplayer = function (x, y) {
            if (!y || y < 0) {
                y = this.background.getTileY(this.player.position.y);
            }
            if (!x || x < 0) {
                x = this.background.getTileX(this.player.position.x);
            }
            this.player.position.x = x * this.map.tileWidth;
            this.player.position.y = y * this.map.tileHeight;
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
                this.player.bringToTop();
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
                }
            }
            this.changingMap = false;
        };
        return StateGameplay;
    }(Phaser.State));
    ElvenCurse.StateGameplay = StateGameplay;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=StateGameplay.js.map