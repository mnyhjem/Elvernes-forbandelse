var ElvenCurse;
(function (ElvenCurse) {
    var Player = (function () {
        function Player(playersprite, game) {
            //rotationSpeed = 50;
            this.moveSpeed = 80;
            this.game = game;
            this.loadPlayer();
            this.playerSprite = playersprite;
            this.playerSprite.anchor.setTo(0.5, 0.5);
            this.nameplate = new ElvenCurse.Nameplate(this.game, this.name);
            this.playerGroup = this.game.add.group();
            this.playerGroup.add(this.playerSprite);
            this.playerGroup.add(this.nameplate.group);
        }
        Player.prototype.bringToTop = function () {
            //this.playerSprite.bringToTop();
            this.game.world.bringToTop(this.playerGroup);
        };
        Player.prototype.move = function (cursors) {
            this.playerSprite.body.velocity.x = 0;
            this.playerSprite.body.velocity.y = 0;
            this.playerSprite.body.angularVelocity = 0;
            var angleToMove = -1;
            if (cursors.right.isDown && cursors.down.isDown) {
                angleToMove = 45;
            }
            else if (cursors.left.isDown && cursors.down.isDown) {
                angleToMove = 135;
            }
            else if (cursors.right.isDown && cursors.up.isDown) {
                angleToMove = 315;
            }
            else if (cursors.left.isDown && cursors.up.isDown) {
                angleToMove = 225;
            }
            else if (cursors.left.isDown) {
                angleToMove = 180;
            }
            else if (cursors.right.isDown) {
                angleToMove = 0;
            }
            else if (cursors.up.isDown) {
                angleToMove = 270;
            }
            else if (cursors.down.isDown) {
                angleToMove = 90;
            }
            if (angleToMove > -1) {
                this.playerSprite.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(angleToMove, this.moveSpeed));
                this.nameplate.setPosition(this.playerSprite.x, this.playerSprite.y);
            }
        };
        Player.prototype.checkCollisions = function (layer) {
            this.game.physics.arcade.collide(this.playerSprite, layer);
        };
        Player.prototype.loadPlayer = function () {
            var self = this;
            $.ajax({
                url: "/api/character/getactive",
                success: function (result) {
                    //var hej = "vi skal sætte vores data her";
                    self.name = result.name;
                    self.location = result.location;
                },
                async: false // <-- vi er ikke async.. er med vilje...
            });
        };
        return Player;
    }());
    ElvenCurse.Player = Player;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=Player.js.map