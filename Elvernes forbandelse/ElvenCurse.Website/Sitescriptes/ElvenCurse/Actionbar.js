var ElvenCurse;
(function (ElvenCurse) {
    var Actionbar = (function () {
        function Actionbar(game, entity) {
            this.game = game;
            this.entity = entity;
            this.createPlatesprite();
            this.group = this.game.add.group();
            this.addAbilities();
            this.group.add(this.actionbarSprite);
        }
        Actionbar.prototype.createPlatesprite = function () {
            var x = (this.game.width / 2) - 234;
            var y = this.game.height - 100;
            this.actionbarSprite = this.game.add.sprite(x, y, "Actionbar");
            this.actionbarSprite.scale.x = 1.2;
            this.actionbarSprite.scale.y = 1.2;
        };
        Actionbar.prototype.destroy = function () {
            // this.playerGroup.removeAll(true);
            this.group.destroy(true);
        };
        Actionbar.prototype.addAbilities = function () {
            var x = this.actionbarSprite.x + 15;
            var y = this.actionbarSprite.y + 8;
            // test
            for (var i = 0; i < 15; i++) {
                var abilityindex = i;
                if (abilityindex >= 11) {
                    abilityindex += 1;
                }
                this.addAbility(x + (i * 36), y, abilityindex);
            }
        };
        Actionbar.prototype.addAbility = function (x, y, abilityIndex) {
            var s = this.game.add.sprite(x, y, "abilities", abilityIndex);
            s.scale.x = 0.7;
            s.scale.y = 0.7;
            this.group.add(s);
        };
        return Actionbar;
    }());
    ElvenCurse.Actionbar = Actionbar;
})(ElvenCurse || (ElvenCurse = {}));
