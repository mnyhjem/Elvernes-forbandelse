module ElvenCurse {
    export class Bunny extends NpcBase {
        constructor(game: Phaser.Game, npc: IPlayer) {
            super(game, npc);

            this.createSpriteAndAnimations();

            this.nameplate = new Nameplate(this.game, npc.name, npc);

            this.group = this.game.add.group();
            this.group.add(this.npcSprite);
            this.group.add(this.nameplate.group);
        }

        private createSpriteAndAnimations() {
            this.npcSprite = this.game.add.sprite(this.creature.location.x, this.creature.location.y, "bunny");
            this.npcSprite.anchor.setTo(0.5, 0.5);

            var imagesPerRow = 18;
            var offset = 6;
            // walk
            this.npcSprite.animations.add("walkFront", Phaser.ArrayUtils.numberArray(4 * imagesPerRow + offset, 4 * imagesPerRow + 2 + offset));
            this.npcSprite.animations.add("walkBack", Phaser.ArrayUtils.numberArray(7 * imagesPerRow + offset, 7 * imagesPerRow + offset + 2));
            this.npcSprite.animations.add("walkRight", Phaser.ArrayUtils.numberArray(6 * imagesPerRow + offset, 6 * imagesPerRow + offset + 2));
            this.npcSprite.animations.add("walkLeft", Phaser.ArrayUtils.numberArray(5 * imagesPerRow + offset, 5 * imagesPerRow + offset + 2));
            
            
            
        }
    }
}