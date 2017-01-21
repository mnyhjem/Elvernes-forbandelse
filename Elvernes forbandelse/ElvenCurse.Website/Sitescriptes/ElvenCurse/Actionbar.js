var ElvenCurse;
(function (ElvenCurse) {
    var Actionbar = (function () {
        function Actionbar(game, entity) {
            this.game = game;
            this.entity = entity;
            this.activatedAbility = -1;
            this.abilitySprites = new Array();
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
            for (var i = 0; i < this.abilitySprites.length; i++) {
                var p = this.abilitySprites[i];
                p.events.onInputDown.removeAll();
                p.destroy();
            }
            this.group.destroy(true);
        };
        Actionbar.prototype.addAbilities = function () {
            var x = this.actionbarSprite.x + 15;
            var y = this.actionbarSprite.y + 8;
            var padding = 0;
            for (var i = 0; i < this.entity.abilities.length; i++) {
                var ability = this.entity.abilities[i];
                if (ability.passive) {
                    continue;
                }
                this.addAbility(x + (padding * 36), y, ability.abilityIcon, i);
                padding++;
            }
        };
        Actionbar.prototype.getActivatedAbility = function () {
            var a = this.activatedAbility;
            this.activatedAbility = -1;
            return a;
        };
        Actionbar.prototype.addAbility = function (x, y, abilityIcon, abilityIndex) {
            var s = this.game.add.sprite(x, y, "abilities", abilityIcon);
            s.scale.x = 0.7;
            s.scale.y = 0.7;
            this.abilitySprites.push(s);
            this.group.add(s);
            s.abilityIndex = abilityIndex;
            s.inputEnabled = true;
            s.events.onInputDown.add(function (sprite, pointer) {
                this.activatedAbility = sprite.abilityIndex;
            }, this);
        };
        return Actionbar;
    }());
    ElvenCurse.Actionbar = Actionbar;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=Actionbar.js.map