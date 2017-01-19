var ElvenCurse;
(function (ElvenCurse) {
    var InteractiveObject = (function () {
        function InteractiveObject(game, io, gameHub) {
            this.interactiveObject = io;
            this.game = game;
            this.gameHub = gameHub;
            this.createSpriteAndAnimations();
            this.group = this.game.add.group();
            this.group.add(this.sprite);
        }
        InteractiveObject.prototype.createSpriteAndAnimations = function () {
            this.sprite = this.game.add.sprite(this.interactiveObject.location.x, this.interactiveObject.location.y, "portal");
            this.sprite.anchor.setTo(0.5, 0.5);
            this.sprite.animations.add("idle", Phaser.ArrayUtils.numberArray(0, 3));
            this.sprite.inputEnabled = true;
            this.sprite.events.onInputDown.add(this.clickListener, this);
            this.playAnimation("idle", true);
        };
        InteractiveObject.prototype.clickListener = function () {
            // send til serveren at der er klikket på os..
            this.gameHub.server.clickOnInteractiveObject(this.interactiveObject.id);
        };
        InteractiveObject.prototype.placeGroup = function () {
            var x = this.interactiveObject.location.x * 32;
            var y = this.interactiveObject.location.y * 32;
            if (this.sprite.x !== x) {
                this.sprite.x = x;
            }
            if (this.sprite.y !== y) {
                this.sprite.y = y;
            }
        };
        InteractiveObject.prototype.playAnimation = function (animationName, loop) {
            if (!this.sprite.animations.getAnimation(animationName).isPlaying) {
                this.sprite.animations.play(animationName, 10, loop);
            }
        };
        InteractiveObject.prototype.destroy = function () {
            this.sprite.animations.destroy();
            this.group.destroy(true);
        };
        return InteractiveObject;
    }());
    ElvenCurse.InteractiveObject = InteractiveObject;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=InteractiveObject.js.map