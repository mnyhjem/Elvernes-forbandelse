var ElvenCurse;
(function (ElvenCurse) {
    var OtherPlayer = (function () {
        function OtherPlayer(game, player) {
            this.game = game;
            this.player = player;
            this.playerSprite = this.game.add.sprite(player.location.x, player.location.y, "player");
            this.playerSprite.anchor.setTo(0.5, 0.5);
            this.nameplate = new ElvenCurse.Nameplate(this.game, player.name);
            this.playerGroup = this.game.add.group();
            this.playerGroup.add(this.playerSprite);
            this.playerGroup.add(this.nameplate.group);
        }
        OtherPlayer.prototype.bringToTop = function () {
            //this.playerSprite.bringToTop();
            this.game.world.bringToTop(this.playerGroup);
        };
        OtherPlayer.prototype.updatePosition = function (player) {
            this.player = player;
        };
        OtherPlayer.prototype.placeGroup = function () {
            var x = this.player.location.x * 32;
            var y = this.player.location.y * 32;
            this.playerSprite.x = x;
            this.playerSprite.y = y;
            this.nameplate.setPosition(x, y);
        };
        OtherPlayer.prototype.destroy = function () {
            //this.playerGroup.removeAll(true);
            this.playerGroup.destroy(true);
        };
        return OtherPlayer;
    }());
    ElvenCurse.OtherPlayer = OtherPlayer;
})(ElvenCurse || (ElvenCurse = {}));
