var ElvenCurse;
(function (ElvenCurse) {
    var Game = (function () {
        function Game() {
            this.game = new Phaser.Game(1024, 768, Phaser.CANVAS, "game");
            this.game.state.add("Preloader", ElvenCurse.StatePreloader);
            this.game.state.add("Gameplay", ElvenCurse.StateGameplay);
            this.game.state.start("Preloader");
        }
        return Game;
    }());
    ElvenCurse.Game = Game;
})(ElvenCurse || (ElvenCurse = {}));
window.onload = function () {
    var g = new ElvenCurse.Game();
};
//# sourceMappingURL=Game.js.map