module ElvenCurse {
    export class Game {
        game: Phaser.Game;

        constructor() {
            this.game = new Phaser.Game(1024, 768, Phaser.CANVAS, "game");

            this.game.state.add("Preloader", StatePreloader);
            this.game.state.add("Gameplay", StateGameplay);

            this.game.state.start("Preloader");
        }
    }
}

window.onload = function() {
    var g = new ElvenCurse.Game();
}