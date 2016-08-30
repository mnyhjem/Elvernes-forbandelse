namespace ElvenCurse {
    export class Worldsectionnameplate {
        game: Phaser.Game;
        group: Phaser.Group;
        map: IWorldsection;
        plateSprite: Phaser.Sprite;
        text:Phaser.Text;

        constructor(game: Phaser.Game, map: IWorldsection) {
            this.game = game;
            this.map = map;

            this.createPlatesprite();

            this.group = this.game.add.group();
            this.group.add(this.plateSprite);
        }

        createPlatesprite() {
            var x = this.game.width - 281 - 10;
            this.plateSprite = this.game.add.sprite(x, 10, "Worldsectionnameplate");
        }

        updateMap(map: IWorldsection) {
            this.map = map;

            if (this.map !== undefined) {
                if (this.text) {
                    this.text.destroy(true);
                }
                var style = { font: "20px beyond_wonderlandregular", fill: "#fffec3"};
                this.text = this.game.add.text(this.plateSprite.x + 20, 22, this.map.name, style, this.group);
            }
        }
        
        public destroy() {
            //this.playerGroup.removeAll(true);
            this.group.destroy(true);
        }
    }
}