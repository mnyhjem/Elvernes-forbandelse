var ElvenCurse;
(function (ElvenCurse) {
    var EntityPortraitplate = (function () {
        function EntityPortraitplate(game, creature, leftMargin) {
            if (leftMargin === void 0) { leftMargin = 0; }
            this.game = game;
            this.creature = creature;
            this.group = this.game.add.group();
            this.healthBarBackground = game.add.graphics(0, 0);
            this.healthBarBackground.beginFill(0x660000, 1);
            this.healthBarBackground.drawRect(0, 0, 128, 24);
            this.group.add(this.healthBarBackground);
            this.healthBar = this.game.add.graphics(0, 0);
            this.healthBar.beginFill(0x009900, 1);
            this.healthBar.drawRect(0, 0, 128, 24);
            this.healthBar.moveTo(0, 0);
            this.healthBar.lineTo(100, 5);
            this.healthBar.scale.x = 0;
            this.healthBar.endFill();
            this.group.add(this.healthBar);
            this.healthBarBackground.x = 58 + leftMargin;
            this.healthBarBackground.y = 20;
            this.healthBar.x = 58 + leftMargin;
            this.healthBar.y = 20;
            this.createPlatesprite(leftMargin);
            this.group.add(this.plateSprite);
            this.update(this.creature);
        }
        EntityPortraitplate.prototype.createPlatesprite = function (leftMargin) {
            this.plateSprite = this.game.add.sprite(10 + leftMargin, 10, "EntityPortraitplate");
        };
        EntityPortraitplate.prototype.destroy = function () {
            //this.playerGroup.removeAll(true);
            this.group.destroy(true);
        };
        EntityPortraitplate.prototype.update = function (creature) {
            if (creature == null) {
                return;
            }
            this.creature = creature;
            //this.healthBar.scale.set(0.25, 1);
            //this.healthBar.x = -95;
            //this.healthBar.width = 5;
            //this.healthBar.width = 50;
            //this.healthBar.x = -50;
            if (this.creature.health != undefined) {
                this.healthBar.scale.x = this.creature.health / this.creature.maxHealth;
            }
        };
        return EntityPortraitplate;
    }());
    ElvenCurse.EntityPortraitplate = EntityPortraitplate;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=EntityPortraitplate.js.map