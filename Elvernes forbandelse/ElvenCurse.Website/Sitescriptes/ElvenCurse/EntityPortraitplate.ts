namespace ElvenCurse {
    export class EntityPortraitplate {
        game: Phaser.Game;
        group: Phaser.Group;
        entity: IPlayer;
        plateSprite:Phaser.Sprite;

        constructor(game: Phaser.Game, entity: IPlayer) {
            this.game = game;
            this.entity = entity;

            this.createPlatesprite();

            this.group = this.game.add.group();
            this.group.add(this.plateSprite);
            
        }

        createPlatesprite() {
            this.plateSprite = this.game.add.sprite(10, 10, "EntityPortraitplate");
        }
        
        public destroy() {
            //this.playerGroup.removeAll(true);
            this.group.destroy(true);
        }
    }
}