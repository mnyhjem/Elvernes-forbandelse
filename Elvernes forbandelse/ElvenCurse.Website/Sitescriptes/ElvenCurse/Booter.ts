﻿module ElvenCurse {
    export class Booter extends Phaser.State {
        game: Phaser.Game;

        constructor() {
            super();
        }

        preload() {
            // loading background
            this.game.load.image("loadingbackground", "/content/assets/graphics/backgrounds/4.jpg");
        }
        
        create() {
            this.game.state.start("Preloader");
        }
    }
}