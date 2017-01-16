var ElvenCurse;
(function (ElvenCurse) {
    var Nameplate = (function () {
        function Nameplate(game, name, creature) {
            this.game = game;
            this.group = this.game.add.group();
            this.creature = creature;
            this.nametext = this.game.add.text(0, 0, name, "");
            this.nametext.anchor.set(0.5);
            this.nametext.fontSize = 15;
            //this.nametext.addColor("#0066ff", 0);
            this.nametext.fill = "#0066ff";
            this.group.add(this.nametext);
            this.healthBarBackground = game.add.graphics(0, 0);
            this.healthBarBackground.beginFill(0x660000, 1);
            this.healthBarBackground.drawRect(0, 0, 100, 5);
            this.group.add(this.healthBarBackground);
            this.healthBar = this.game.add.graphics(0, 0);
            this.healthBar.beginFill(0x009900, 1);
            this.healthBar.drawRect(0, 0, 100, 5);
            this.healthBar.moveTo(0, 0);
            this.healthBar.lineTo(100, 5);
            this.healthBar.scale.x = 0;
            this.healthBar.endFill();
            this.group.add(this.healthBar);
            this.update(creature);
            //this.nametext.x = 2555;
            //this.nametext.y = 960;
            this.setPosition(this.creature.location.x * 32, this.creature.location.y * 32);
            this.update(this.creature);
        }
        ///
        /// x og y er i pixels.. ikke i tiles...
        ///
        Nameplate.prototype.setPosition = function (x, y) {
            y -= 32;
            //this.group.forEach(element => {
            //    element.x = x;
            //    element.y = y;
            //}, this);
            this.nametext.x = x;
            this.nametext.y = y - 10;
            this.healthBarBackground.x = x - 50;
            this.healthBarBackground.y = y;
            this.healthBar.x = x - 50;
            this.healthBar.y = y;
        };
        Nameplate.prototype.update = function (creature) {
            //this.healthBar.scale.set(0.25, 1);
            //this.healthBar.x = -95;
            //this.healthBar.width = 5;
            //this.healthBar.width = 50;
            //this.healthBar.x = -50;
            if (creature.health != undefined) {
                this.healthBar.scale.x = creature.health / creature.maxHealth;
            }
        };
        return Nameplate;
    }());
    ElvenCurse.Nameplate = Nameplate;
})(ElvenCurse || (ElvenCurse = {}));
