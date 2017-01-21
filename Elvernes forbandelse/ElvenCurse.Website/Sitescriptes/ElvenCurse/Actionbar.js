var ElvenCurse;
(function (ElvenCurse) {
    var Actionbar = (function () {
        function Actionbar(game, entity) {
            this.game = game;
            this.entity = entity;
            this.createPlatesprite();
            this.group = this.game.add.group();
            this.group.add(this.actionbarSprite);
        }
        Actionbar.prototype.createPlatesprite = function () {
            var x = (this.game.width / 2) - 234;
            var y = this.game.height - 100;
            this.actionbarSprite = this.game.add.sprite(x, y, "Actionbar");
        };
        Actionbar.prototype.destroy = function () {
            // this.playerGroup.removeAll(true);
            this.group.destroy(true);
        };
        return Actionbar;
    }());
    ElvenCurse.Actionbar = Actionbar;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=Actionbar.js.map