var ElvenCurse;
(function (ElvenCurse) {
    var Game = (function () {
        function Game() {
            var width = document.body.offsetWidth;
            var height = window.innerHeight - 50; //document.body.offsetHeight;
            height = 600;
            width = 800;
            this.game = new Phaser.Game(width, height, Phaser.CANVAS, "game");
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
