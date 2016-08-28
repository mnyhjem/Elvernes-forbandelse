module ElvenCurse {

    export class InteractiveObject {
        interactiveObject : IInteractiveObject;
        sprite: Phaser.Sprite;
        group: Phaser.Group;
        game:Phaser.Game;

        constructor(game: Phaser.Game, io: IInteractiveObject) {
            this.interactiveObject = io;
            this.game = game;

            this.createSpriteAndAnimations();

            this.group = this.game.add.group();
            this.group.add(this.sprite);
        }

        private createSpriteAndAnimations() {
            this.sprite = this.game.add.sprite(this.interactiveObject.location.x, this.interactiveObject.location.y, "portal");
            this.sprite.anchor.setTo(0.5, 0.5);
        }

        public placeGroup() {
            var x = this.interactiveObject.location.x * 32;
            var y = this.interactiveObject.location.y * 32;

            this.sprite.x = x;
            this.sprite.y = y;
        }

        public destroy() {
            //this.playerGroup.removeAll(true);
            this.group.destroy(true);
        }
    }
}