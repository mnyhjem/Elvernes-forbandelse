var ElvenCurse;
(function (ElvenCurse) {
    var Player = (function () {
        function Player(game) {
            //rotationSpeed = 50;
            this.moveSpeed = 80;
            this.game = game;
            this.loadPlayer();
            this.createPlayerspriteAndAnimations();
            this.nameplate = new ElvenCurse.Nameplate(this.game, this.name);
            this.playerGroup = this.game.add.group();
            this.playerGroup.add(this.playerSprite);
            this.playerGroup.add(this.nameplate.group);
        }
        Player.prototype.updatePlayer = function (player) {
            this.location.x = player.location.x;
            this.location.y = player.location.y;
            this.location.worldsectionId = player.location.worldsectionId;
            this.health = player.health;
            this.isAlive = player.isAlive;
            if (!this.isAlive) {
                this.playAnimation("hurtBack");
            }
        };
        //public bringToTop() {
        //    //this.playerSprite.bringToTop();
        //    this.game.world.bringToTop(this.playerGroup);
        //}
        Player.prototype.move = function (cursors) {
            this.playerSprite.body.velocity.x = 0;
            this.playerSprite.body.velocity.y = 0;
            this.playerSprite.body.angularVelocity = 0;
            if (this.isAlive === false) {
                return;
            }
            var angleToMove = -1;
            if ((cursors.right.isDown && cursors.down.isDown) || (this.game.input.keyboard.isDown(Phaser.KeyCode.D) && this.game.input.keyboard.isDown(Phaser.KeyCode.S))) {
                angleToMove = 45;
                this.playAnimation("walkRight");
            }
            else if (cursors.left.isDown && cursors.down.isDown || (this.game.input.keyboard.isDown(Phaser.KeyCode.A) && this.game.input.keyboard.isDown(Phaser.KeyCode.S))) {
                angleToMove = 135;
                this.playAnimation("walkLeft");
            }
            else if (cursors.right.isDown && cursors.up.isDown || (this.game.input.keyboard.isDown(Phaser.KeyCode.D) && this.game.input.keyboard.isDown(Phaser.KeyCode.W))) {
                angleToMove = 315;
                this.playAnimation("walkRight");
            }
            else if (cursors.left.isDown && cursors.up.isDown || (this.game.input.keyboard.isDown(Phaser.KeyCode.A) && this.game.input.keyboard.isDown(Phaser.KeyCode.W))) {
                angleToMove = 225;
                this.playAnimation("walkLeft");
            }
            else if (cursors.left.isDown || this.game.input.keyboard.isDown(Phaser.KeyCode.A)) {
                this.playAnimation("walkLeft");
                angleToMove = 180;
            }
            else if (cursors.right.isDown || this.game.input.keyboard.isDown(Phaser.KeyCode.D)) {
                angleToMove = 0;
                this.playAnimation("walkRight");
            }
            else if (cursors.up.isDown || this.game.input.keyboard.isDown(Phaser.KeyCode.W)) {
                angleToMove = 270;
                this.playAnimation("walkBack");
            }
            else if (cursors.down.isDown || this.game.input.keyboard.isDown(Phaser.KeyCode.S)) {
                angleToMove = 90;
                this.playAnimation("walkFront");
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
        Player.prototype.createPlayerspriteAndAnimations = function () {
            this.playerSprite = this.game.add.sprite(2560, 900, "playertest");
            this.playerSprite.anchor.setTo(0.5, 0.5);
            var imagesPerRow = 13;
            // spellcast
            this.playerSprite.animations.add("spellcastBack", Phaser.ArrayUtils.numberArray(0 * imagesPerRow, 0 * imagesPerRow + 6)); //0,6
            this.playerSprite.animations.add("spellcastLeft", Phaser.ArrayUtils.numberArray(1 * imagesPerRow, 1 * imagesPerRow + 6)); //13,19
            this.playerSprite.animations.add("spellcastFront", Phaser.ArrayUtils.numberArray(2 * imagesPerRow, 2 * imagesPerRow + 6)); //26,32
            this.playerSprite.animations.add("spellcastRight", Phaser.ArrayUtils.numberArray(3 * imagesPerRow, 3 * imagesPerRow + 6)); //39,45
            // thrust
            this.playerSprite.animations.add("thrustBack", Phaser.ArrayUtils.numberArray(4 * imagesPerRow, 4 * imagesPerRow + 6));
            this.playerSprite.animations.add("thrustLeft", Phaser.ArrayUtils.numberArray(5 * imagesPerRow, 5 * imagesPerRow + 6));
            this.playerSprite.animations.add("thrustFront", Phaser.ArrayUtils.numberArray(6 * imagesPerRow, 6 * imagesPerRow + 6));
            this.playerSprite.animations.add("thrustRight", Phaser.ArrayUtils.numberArray(7 * imagesPerRow, 7 * imagesPerRow + 6));
            // walk
            this.playerSprite.animations.add("walkBack", Phaser.ArrayUtils.numberArray(8 * imagesPerRow, 8 * imagesPerRow + 6));
            this.playerSprite.animations.add("walkLeft", Phaser.ArrayUtils.numberArray(9 * imagesPerRow, 9 * imagesPerRow + 6));
            this.playerSprite.animations.add("walkFront", Phaser.ArrayUtils.numberArray(10 * imagesPerRow, 10 * imagesPerRow + 6));
            this.playerSprite.animations.add("walkRight", Phaser.ArrayUtils.numberArray(11 * imagesPerRow, 11 * imagesPerRow + 6));
            // slash
            this.playerSprite.animations.add("slashBack", Phaser.ArrayUtils.numberArray(12 * imagesPerRow, 12 * imagesPerRow + 6));
            this.playerSprite.animations.add("slashLeft", Phaser.ArrayUtils.numberArray(13 * imagesPerRow, 13 * imagesPerRow + 6));
            this.playerSprite.animations.add("slashFront", Phaser.ArrayUtils.numberArray(14 * imagesPerRow, 14 * imagesPerRow + 6));
            this.playerSprite.animations.add("slashRight", Phaser.ArrayUtils.numberArray(15 * imagesPerRow, 15 * imagesPerRow + 6));
            // shoot
            this.playerSprite.animations.add("shootBack", Phaser.ArrayUtils.numberArray(16 * imagesPerRow, 16 * imagesPerRow + 6));
            this.playerSprite.animations.add("shootLeft", Phaser.ArrayUtils.numberArray(17 * imagesPerRow, 17 * imagesPerRow + 6));
            this.playerSprite.animations.add("shootFront", Phaser.ArrayUtils.numberArray(18 * imagesPerRow, 18 * imagesPerRow + 6));
            this.playerSprite.animations.add("shootRight", Phaser.ArrayUtils.numberArray(19 * imagesPerRow, 19 * imagesPerRow + 6));
            // hurt
            this.playerSprite.animations.add("hurtBack", Phaser.ArrayUtils.numberArray(20 * imagesPerRow, 20 * imagesPerRow + 5));
            this.playerSprite.animations.add("hurtLeft", Phaser.ArrayUtils.numberArray(21 * imagesPerRow, 21 * imagesPerRow + 5));
            this.playerSprite.animations.add("hurtFront", Phaser.ArrayUtils.numberArray(22 * imagesPerRow, 22 * imagesPerRow + 5));
            this.playerSprite.animations.add("hurtRight", Phaser.ArrayUtils.numberArray(23 * imagesPerRow, 23 * imagesPerRow + 5));
        };
        Player.prototype.playAnimation = function (animationName) {
            if (!this.playerSprite.animations.getAnimation(animationName).isPlaying) {
                this.playerSprite.animations.play(animationName, 10, false);
            }
        };
        return Player;
    }());
    ElvenCurse.Player = Player;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=Player.js.map