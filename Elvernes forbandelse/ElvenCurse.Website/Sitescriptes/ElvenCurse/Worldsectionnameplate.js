var ElvenCurse;
(function (ElvenCurse) {
    var Worldsectionnameplate = (function () {
        function Worldsectionnameplate(game, map) {
            this.game = game;
            this.map = map;
            this.createPlatesprite();
            this.group = this.game.add.group();
            this.group.add(this.plateSprite);
        }
        Worldsectionnameplate.prototype.createPlatesprite = function () {
            var x = this.game.width - 281 - 10;
            this.plateSprite = this.game.add.sprite(x, 10, "Worldsectionnameplate");
        };
        Worldsectionnameplate.prototype.updateMap = function (map) {
            this.map = map;
            if (this.map !== undefined) {
                if (this.text) {
                    this.text.destroy(true);
                }
                var style = { font: "20px beyond_wonderlandregular", fill: "#fffec3" };
                this.text = this.game.add.text(this.plateSprite.x + 20, 22, this.map.name, style, this.group);
            }
        };
        Worldsectionnameplate.prototype.destroy = function () {
            //this.playerGroup.removeAll(true);
            this.group.destroy(true);
        };
        Worldsectionnameplate.prototype.setPlayerPosition = function (player) {
            if (this.playerpositiontext === undefined) {
                var style = { font: "16px verdana", fill: "#fffec3" };
                this.playerpositiontext = this.game.add.text(this.plateSprite.x + 20, 60, "", style, this.group);
            }
            this.playerpositiontext.text = "[" + player.creature.location.x + ", " + player.creature.location.y + "]";
        };
        return Worldsectionnameplate;
    }());
    ElvenCurse.Worldsectionnameplate = Worldsectionnameplate;
})(ElvenCurse || (ElvenCurse = {}));
