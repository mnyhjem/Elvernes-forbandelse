module ElvenCurse {
    export class Game {
        game: Phaser.Game;

        constructor() {
            var width = document.body.offsetWidth;
            var height = window.innerHeight - 50;//document.body.offsetHeight;

            this.game = new Phaser.Game(width, height, Phaser.CANVAS, "game");

            this.game.state.add("Preloader", StatePreloader);
            this.game.state.add("Gameplay", StateGameplay);

            this.game.state.start("Preloader");
        }
    }
}

window.onload = function () {
    var g = new ElvenCurse.Game();
}