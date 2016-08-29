var ElvenCurse;
(function (ElvenCurse) {
    var EntityPortraitplate = (function () {
        function EntityPortraitplate(game, entity) {
            this.game = game;
            this.entity = entity;
            this.createPlatesprite();
            this.group = this.game.add.group();
            this.group.add(this.plateSprite);
        }
        EntityPortraitplate.prototype.createPlatesprite = function () {
            this.plateSprite = this.game.add.sprite(10, 10, "EntityPortraitplate");
        };
        EntityPortraitplate.prototype.destroy = function () {
            //this.playerGroup.removeAll(true);
            this.group.destroy(true);
        };
        return EntityPortraitplate;
    }());
    ElvenCurse.EntityPortraitplate = EntityPortraitplate;
})(ElvenCurse || (ElvenCurse = {}));
