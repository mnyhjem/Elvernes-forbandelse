var ElvenCurse;
(function (ElvenCurse) {
    var Player = (function () {
        function Player(playersprite, game) {
            this.game = game;
            this.loadPlayer();
            this.playerSprite = playersprite;
            this.playerSprite.anchor.setTo(0.5, 0.5);
        }
        Player.prototype.move = function (cursors) {
            this.playerSprite.body.velocity.x = 0;
            this.playerSprite.body.velocity.y = 0;
            this.playerSprite.body.angularVelocity = 0;
            if (cursors.left.isDown) {
                this.playerSprite.body.angularVelocity = -200;
            }
            else if (cursors.right.isDown) {
                this.playerSprite.body.angularVelocity = 200;
            }
            if (cursors.up.isDown) {
                this.playerSprite.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(this.playerSprite.angle, 300));
            }
            if (cursors.down.isDown) {
                this.playerSprite.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(this.playerSprite.angle, -300));
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