module Charactermodel {
    export class ShowCharacter extends Phaser.State {
        game: Phaser.Game;
        sprite:Phaser.Sprite;

        constructor() {
            super();
        }

        preload() {
            var url = $("#characterpreview").attr("data-url");
            this.game.load.spritesheet("sprite", url, 64, 64);
        }

        create() {
            this.game.stage.backgroundColor = "#FFFFFF";
            this.sprite = this.game.add.sprite(0, 0, "sprite");
            this.sprite.scale.setTo(4.5, 4.5);

            this.sprite.animations.add("turnaround", [26, 39, 0, 13]);

            this.sprite.animations.play("turnaround", 1, true);
        }
    }
}
