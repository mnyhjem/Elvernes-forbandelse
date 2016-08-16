var ElvenCurse;
(function (ElvenCurse) {
    var Game = (function () {
        function Game(characterid) {
            var width = document.body.offsetWidth;
            var height = window.innerHeight - 50; //document.body.offsetHeight;
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
    var id = $("#game").attr("data-characterid");
    var g = new ElvenCurse.Game(id);
};
