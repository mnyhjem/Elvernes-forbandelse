module ElvenCurse {
    export class Wolf extends NpcBase {
        constructor(game: Phaser.Game, npc: IPlayer) {
            super(game, npc);

            this.createSpriteAndAnimations();

            this.nameplate = new Nameplate(this.game, npc.name, npc);

            this.group = this.game.add.group();
            this.group.add(this.npcSprite);
            this.group.add(this.nameplate.group);
        }

        private createSpriteAndAnimations() {
            this.npcSprite = this.game.add.sprite(this.creature.location.x, this.creature.location.y, "wolf1");
            this.npcSprite.anchor.setTo(0.5, 0.5);
            
            var imagesPerRow = 10;

            // walk
            this.npcSprite.animations.add("walkFront", Phaser.ArrayUtils.numberArray(0 * imagesPerRow, 0 * imagesPerRow + 1));
            this.npcSprite.animations.add("walkBack", Phaser.ArrayUtils.numberArray(1 * imagesPerRow, 1 * imagesPerRow + 1));
            this.npcSprite.animations.add("walkRight", Phaser.ArrayUtils.numberArray(2 * imagesPerRow, 2 * imagesPerRow + 4));
            this.npcSprite.animations.add("walkLeft", Phaser.ArrayUtils.numberArray(3 * imagesPerRow, 3 * imagesPerRow + 4));
            
            
            
        }
    }
}