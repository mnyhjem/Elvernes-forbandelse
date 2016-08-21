module ElvenCurse {
    export class StatePreloader extends Phaser.State {
        game: Phaser.Game;
        spinner: Phaser.Sprite;
        text: Phaser.Text;

        constructor() {
            super();
        }

        init() {
            this.createLoadingbox();

            //this.game.add.plugin(Fabrique.Plugins.InputField);

            this.input.maxPointers = 1;
            this.scale.pageAlignHorizontally = true;
            this.game.renderer.renderSession.roundPixels = true;
        }

        createLoadingbox() {
            var box = this.make.graphics(0, 0);
            box.lineStyle(8, 0x88E2FE, 0.6);
            box.beginFill(0x88E2FE, 1);
            box.drawRect(-50, -50, 100, 100);
            box.endFill();

            this.spinner = this.add.sprite(this.world.centerX, this.world.centerY, box.generateTexture());
            this.spinner.anchor.set(0.5);

            this.text = this.add.text(50, 75, "Indlæser 0%", { font: "32px Arial", fill: "#ffffff", align: "center" });
        }

        preload() {
            //this.load.path = "/content/assets/graphics/";

            //this.load.audio("Bang", "Bang.mp3");
            //this.load.images(["graphics-tiles-waterflow", "ground_tiles"]);

            this.game.load.tilemap("world", "/content/assets/01.json", null, Phaser.Tilemap.TILED_JSON);
            this.game.load.image("water", "/content/assets/graphics/graphics-tiles-waterflow.png");
            this.game.load.image("ground", "/content/assets/graphics/ground_tiles.png");
            this.game.load.image("player", "/content/assets/graphics/player.png");

            this.load.onFileComplete.add(this.fileLoaded, this);
        }

        fileLoaded(progress: number) {
            this.text.text = "Indlæser " + progress + "%";
        }

        loadUpdate() {
            this.spinner.rotation += 0.05;
        }

        create() {
            this.game.state.start("Gameplay");
            //this.add.tween(this.spinner.scale).to({ x: 0, y: 0 }, 1000, "Elastic.easeIn", true, 250);
            //var t = this.add.tween(this.text).to({ alpha: 0 }, 1000, "Linear", true);
            //var self = this;
            //t.onComplete.add(function () {
            //    self.game.state.start("Gameplay");
            //    //self.game.state.start("Result", true, false, 167, 1500,6000);
            //    //self.game.state.start("Gameover");
            //    //self.game.state.start("Highscore");
            //});

        }
    }
}