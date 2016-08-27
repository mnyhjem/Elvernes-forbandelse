module ElvenCurse {
    export class Game {
        game: Phaser.Game;

        constructor() {
            var width = document.body.offsetWidth - 100;
            var height = window.innerHeight - 100;//document.body.offsetHeight;

            //height = 600;
            //width = 800;

            this.game = new Phaser.Game(width, height, Phaser.CANVAS, "game");

            this.game.state.add("Preloader", StatePreloader);
            this.game.state.add("Gameplay", StateGameplay);

            this.game.state.start("Preloader");
        }
    }
}