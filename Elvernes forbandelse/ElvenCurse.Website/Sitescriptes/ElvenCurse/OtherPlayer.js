var ElvenCurse;
(function (ElvenCurse) {
    var OtherPlayer = (function () {
        function OtherPlayer(game, player) {
            this.game = game;
            this.player = player;
            this.playerSprite = this.game.add.sprite(0, 0, "playersprite_" + this.player.id);
            this.playerSprite.anchor.setTo(0.5, 0.5);
            this.loadPlayersprite();
            this.nameplate = new ElvenCurse.Nameplate(this.game, player.name, this.player);
            this.group = this.game.add.group();
            this.group.add(this.playerSprite);
            this.group.add(this.nameplate.group);
        }
        OtherPlayer.prototype.loadPlayersprite = function () {
            if (!this.game.cache.checkImageKey("playersprite_" + this.player.id)) {
                this.game.load.spritesheet("playersprite_" + this.player.id, "/charactersprite/?id=" + this.player.id + "&isnpc=false&t=" + new Date().getTime(), 64, 64);
            }
            this.game.load.onLoadComplete.add(this.spriteLoaded, this);
            this.game.load.start();
        };
        OtherPlayer.prototype.spriteLoaded = function () {
            this.game.load.onLoadComplete.remove(this.spriteLoaded, this);
            this.playerSprite.loadTexture("playersprite_" + this.player.id);
            this.playerSprite.frame = 26; // <-- kig mod kameraet til at starte med
            this.createPlayerspriteAndAnimations();
        };
        OtherPlayer.prototype.updatePlayer = function (player) {
            if (this.shownAsDead && player.isAlive) {
                this.shownAsDead = false;
                this.playAnimation("spellcastFront");
            }
            this.player = player;
            this.nameplate.update(this.player);
            var self = this;
            if (this.oldHealth > this.player.health) {
                this.playerSprite.tint = 0xff0000;
                this.oldHealth = this.player.health;
                this.game.time.events.add(Phaser.Timer.SECOND, function () {
                    self.playerSprite.tint = 0xffffff;
                }, this);
            }
        };
        OtherPlayer.prototype.placeGroup = function () {
            var x = this.player.location.x * 32;
            var y = this.player.location.y * 32;
            if (this.playerSprite.x < x) {
                this.playAnimation("walkRight");
            }
            if (this.playerSprite.x > x) {
                this.playAnimation("walkLeft");
            }
            if (this.playerSprite.y > y) {
                this.playAnimation("walkBack");
            }
            if (this.playerSprite.y < y) {
                this.playAnimation("walkFront");
            }
            if (this.playerSprite.x === 0 || this.playerSprite.y === 0) {
                this.playerSprite.x = x;
                this.playerSprite.y = y;
            }
            else {
                this.game.add.tween(this.playerSprite).to({ x: x, y: y }, 50, Phaser.Easing.Linear.None, true).start();
            }
            this.nameplate.setPosition(x, y);
            this.checkDead();
        };
        OtherPlayer.prototype.checkDead = function () {
            if (!this.shownAsDead && !this.player.isAlive) {
                this.playAnimation("hurtBack");
                this.shownAsDead = true;
            }
        };
        OtherPlayer.prototype.destroy = function () {
            this.playerSprite.animations.destroy();
            this.group.destroy(true);
        };
        OtherPlayer.prototype.createPlayerspriteAndAnimations = function () {
            // this.playerSprite = this.game.add.sprite(this.player.location.x, this.player.location.y, "playertest");
            // this.playerSprite.anchor.setTo(0.5, 0.5);
            var imagesPerRow = 13;
            // spellcast
            this.playerSprite.animations.add("spellcastBack", Phaser.ArrayUtils.numberArray(0 * imagesPerRow, 0 * imagesPerRow + 6)); // 0,6
            this.playerSprite.animations.add("spellcastLeft", Phaser.ArrayUtils.numberArray(1 * imagesPerRow, 1 * imagesPerRow + 6)); // 13,19
            this.playerSprite.animations.add("spellcastFront", Phaser.ArrayUtils.numberArray(2 * imagesPerRow, 2 * imagesPerRow + 6)); // 26,32
            this.playerSprite.animations.add("spellcastRight", Phaser.ArrayUtils.numberArray(3 * imagesPerRow, 3 * imagesPerRow + 6)); // 39,45
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
            // this.playerSprite.animations.play("shootRight", 10, false);
        };
        OtherPlayer.prototype.playAnimation = function (animationName) {
            if (this.playerSprite.animations.getAnimation(animationName) == null) {
                return;
            }
            if (!this.playerSprite.animations.getAnimation(animationName).isPlaying) {
                this.playerSprite.animations.play(animationName, 10, false);
            }
        };
        return OtherPlayer;
    }());
    ElvenCurse.OtherPlayer = OtherPlayer;
})(ElvenCurse || (ElvenCurse = {}));
