module ElvenCurse {
    export class NpcBase {
        game: Phaser.Game;
        group: Phaser.Group;
        npcSprite: Phaser.Sprite;
        npc: IPlayer;
        nameplate: Nameplate;

        constructor(game: Phaser.Game, npc: IPlayer) {
            this.game = game;
            this.npc = npc;
        }

        private playAnimation(animationName: string) {
            var animation = this.npcSprite.animations.getAnimation(animationName);

            //this.nameplate.nametext.text = animation.frameTotal.toString();
            if (animation === undefined || animation === null) {
                return;
            }

            if (!animation.isPlaying) {
                this.npcSprite.animations.play(animationName, 10, false);
            }
        }

        public updatePosition(npc: IPlayer) {
            this.npc = npc;
        }

        public placeGroup() {
            //var x = this.npc.location.x * 32;
            //var y = this.npc.location.y * 32;
            //this.npcSprite.x = x;
            //this.npcSprite.y = y;
            //this.nameplate.setPosition(x, y);
            //this.playAnimation("walkRight");
            //return;

            var x = this.npc.location.x * 32;
            var y = this.npc.location.y * 32;

            if (this.npcSprite.x < x) {
                this.playAnimation("walkRight");
            }
            if (this.npcSprite.x > x) {
                this.playAnimation("walkLeft");
            }
            if (this.npcSprite.y > y) {
                this.playAnimation("walkBack");
            }
            if (this.npcSprite.y < y) {
                this.playAnimation("walkFront");
            }
            this.npcSprite.x = x;
            this.npcSprite.y = y;
            this.nameplate.setPosition(x, y);
        }

        public destroy() {
            //this.playerGroup.removeAll(true);
            this.npcSprite.animations.destroy();
            this.group.destroy(true);
        }
    }
}