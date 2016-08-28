module ElvenCurse {

    export class InteractiveObject {
        interactiveObject : IInteractiveObject;
        sprite: Phaser.Sprite;
        group: Phaser.Group;
        game: Phaser.Game;
        gameHub: IGameHub;

        constructor(game: Phaser.Game, io: IInteractiveObject, gameHub: IGameHub) {
            this.interactiveObject = io;
            this.game = game;
            this.gameHub = gameHub;

            this.createSpriteAndAnimations();

            this.group = this.game.add.group();
            this.group.add(this.sprite);
        }

        private createSpriteAndAnimations() {
            this.sprite = this.game.add.sprite(this.interactiveObject.location.x, this.interactiveObject.location.y, "portal");
            this.sprite.anchor.setTo(0.5, 0.5);

            this.sprite.inputEnabled = true;
            this.sprite.events.onInputDown.add(this.clickListener, this);
        }

        private clickListener() {
            // send til serveren at der er klikket på os..

            this.gameHub.server.clickOnInteractiveObject(this.interactiveObject.id);
        }

        public placeGroup() {
            var x = this.interactiveObject.location.x * 32;
            var y = this.interactiveObject.location.y * 32;

            this.sprite.x = x;
            this.sprite.y = y;
        }

        public destroy() {
            this.group.destroy(true);
        }
    }
}