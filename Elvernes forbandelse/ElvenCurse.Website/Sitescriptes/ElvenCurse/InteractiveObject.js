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
            this.sprite.inputEnabled = true;
            this.sprite.events.onInputDown.add(this.clickListener, this);
        };
        InteractiveObject.prototype.clickListener = function () {
            // send til serveren at der er klikket på os..
            this.gameHub.server.clickOnInteractiveObject(this.interactiveObject.id);
        };
        InteractiveObject.prototype.placeGroup = function () {
            var x = this.interactiveObject.location.x * 32;
            var y = this.interactiveObject.location.y * 32;
            this.sprite.x = x;
            this.sprite.y = y;
        };
        InteractiveObject.prototype.destroy = function () {
            this.group.destroy(true);
        };
        return InteractiveObject;
    }());
    ElvenCurse.InteractiveObject = InteractiveObject;
})(ElvenCurse || (ElvenCurse = {}));
