var ElvenCurse;
(function (ElvenCurse) {
    var Nameplate = (function () {
        function Nameplate(game, name) {
            this.game = game;
            this.group = this.game.add.group();
            this.nametext = this.game.add.text(128, 960, name, "");
            this.nametext.anchor.set(0.5);
            this.nametext.fontSize = 15;
            //this.nametext.addColor("#0066ff", 0);
            this.nametext.fill = "#0066ff";
            this.group.add(this.nametext);
            //this.nametext.x = 2555;
            //this.nametext.y = 960;
        }
        Nameplate.prototype.setPosition = function (x, y) {
            y -= 32;
            this.group.forEach(function (element) {
                element.x = x;
                element.y = y;
            }, this);
        };
        return Nameplate;
    }());
    ElvenCurse.Nameplate = Nameplate;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=Nameplate.js.map