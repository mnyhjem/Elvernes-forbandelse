var ElvenCurse;
(function (ElvenCurse) {
    var Game = (function () {
        function Game() {
            var width = document.body.offsetWidth - 100;
            var height = window.innerHeight - 100; //document.body.offsetHeight;
            //height = 600;
            //width = 800;
            if (width > 1000) {
                width = 1000;
            }
            if (height > 800) {
                height = 800;
            }
            this.game = new Phaser.Game(width, height, Phaser.CANVAS, "game");
            this.game.state.add("Booter", ElvenCurse.Booter);
            this.game.state.add("Preloader", ElvenCurse.StatePreloader);
            this.game.state.add("Gameplay", ElvenCurse.StateGameplay);
            this.game.state.start("Booter");
        }
        return Game;
    }());
    ElvenCurse.Game = Game;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=Game.js.map