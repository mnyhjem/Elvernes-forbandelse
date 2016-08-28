var ElvenCurse;
(function (ElvenCurse) {
    var InteractiveObject = (function () {
        function InteractiveObject(game, io) {
            this.interactiveObject = io;
            this.game = game;
            this.createSpriteAndAnimations();
            this.group = this.game.add.group();
            this.group.add(this.sprite);
        }
        InteractiveObject.prototype.createSpriteAndAnimations = function () {
            this.sprite = this.game.add.sprite(this.interactiveObject.location.x, this.interactiveObject.location.y, "portal");
            this.sprite.anchor.setTo(0.5, 0.5);
        };
        InteractiveObject.prototype.placeGroup = function () {
            var x = this.interactiveObject.location.x * 32;
            var y = this.interactiveObject.location.y * 32;
            this.sprite.x = x;
            this.sprite.y = y;
        };
        InteractiveObject.prototype.destroy = function () {
            //this.playerGroup.removeAll(true);
            this.group.destroy(true);
        };
        return InteractiveObject;
    }());
    ElvenCurse.InteractiveObject = InteractiveObject;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=InteractiveObject.js.map