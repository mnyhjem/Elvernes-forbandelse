module ElvenCurse {
    export class StatePreloader extends Phaser.State {
        game: Phaser.Game;
        spinner: Phaser.Sprite;
        text: Phaser.Text;

        constructor() {
            super();
        }

        init() {
            

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
            var bg = this.add.image(0, 0, "loadingbackground");
            bg.height = this.game.height;
            bg.width = this.game.width;
            bg.smoothed = true;

            this.createLoadingbox();

            this.game.load.spritesheet("playertest", "/content/assets/graphics/playertest.png", 64, 64);
            this.game.load.spritesheet("wolf1", "/content/assets/graphics/npcs/wolf/wolf.png", 64, 64);

            // ui
            this.game.load.image("EntityPortraitplate", "/content/assets/graphics/EntityPortraitplate.png");
            this.game.load.image("Actionbar", "/content/assets/graphics/Actionbar.png");
            this.game.load.image("Worldsectionnameplate", "/content/assets/graphics/Worldsectionnameplate.png");

            // tilesets
            this.game.load.image("water", "/content/assets/graphics/graphics-tiles-waterflow.png");
            this.game.load.image("ground", "/content/assets/graphics/ground_tiles.png");
            this.game.load.image("player", "/content/assets/graphics/player.png");
            this.game.load.image("Collision", "/content/assets/graphics/Collision.png");

            // interactive objects
            this.game.load.spritesheet("portal", "/content/assets/graphics/portal-animation.png", 32, 32);

            // music
            this.game.load.audio("medieval", "/content/assets/music/medieval.ogg");

            // fonts
            this.game.load.script("webfont", "//ajax.googleapis.com/ajax/libs/webfont/1.4.7/webfont.js");

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