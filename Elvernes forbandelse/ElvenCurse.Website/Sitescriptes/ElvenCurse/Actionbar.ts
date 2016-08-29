namespace ElvenCurse {
    export class Actionbar {
        game: Phaser.Game;
        group: Phaser.Group;
        entity: IPlayer;
        actionbarSprite:Phaser.Sprite;

        constructor(game: Phaser.Game, entity: IPlayer) {
            this.game = game;
            this.entity = entity;

            this.createPlatesprite();

            this.group = this.game.add.group();
            this.group.add(this.actionbarSprite);
            
        }

        createPlatesprite() {
            var x = (this.game.width / 2) - 234;
            var y = this.game.height - 100;
            this.actionbarSprite = this.game.add.sprite(x, y, "Actionbar");
        }
        
        public destroy() {
            //this.playerGroup.removeAll(true);
            this.group.destroy(true);
        }
    }
}