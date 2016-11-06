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

            this.sprite.animations.add("idle", Phaser.ArrayUtils.numberArray(0, 3));

            this.sprite.inputEnabled = true;
            this.sprite.events.onInputDown.add(this.clickListener, this);

            this.playAnimation("idle", true);
        }

        private clickListener() {
            // send til serveren at der er klikket på os..

            this.gameHub.server.clickOnInteractiveObject(this.interactiveObject.id);
        }

        public placeGroup() {
            var x = this.interactiveObject.location.x * 32;
            var y = this.interactiveObject.location.y * 32;

            if (this.sprite.x !== x) {
                this.sprite.x = x;
            }
            if (this.sprite.y !== y) {
                this.sprite.y = y;
            }
        }

        private playAnimation(animationName: string, loop:boolean) {
            if (!this.sprite.animations.getAnimation(animationName).isPlaying) {
                this.sprite.animations.play(animationName, 10, loop);
            }
        }

        public destroy() {
            this.sprite.animations.destroy();
            this.group.destroy(true);
        }
    }
}