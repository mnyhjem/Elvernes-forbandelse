module ElvenCurse {
    export class NpcBase {
        game: Phaser.Game;
        group: Phaser.Group;
        npcSprite: Phaser.Sprite;
        creature: IPlayer;
        nameplate: Nameplate;
        oldHealth:number;

        constructor(game: Phaser.Game, npc: IPlayer) {
            this.game = game;
            this.creature = npc;
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

        public updatePlayer(npc: IPlayer) {
            this.creature = npc;

            var self = this;
            if (this.oldHealth > this.creature.health) {
                this.npcSprite.tint = 0xff0000;
                this.oldHealth = this.creature.health;
                this.game.time.events.add(Phaser.Timer.SECOND,
                    function () {
                        self.npcSprite.tint = 0xffffff;
                    },
                    this);
            }
        }

        public placeGroup() {
            //var x = this.npc.location.x * 32;
            //var y = this.npc.location.y * 32;
            //this.npcSprite.x = x;
            //this.npcSprite.y = y;
            //this.nameplate.setPosition(x, y);
            //this.playAnimation("walkRight");
            //return;

            var x = this.creature.location.x * 32;
            var y = this.creature.location.y * 32;

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

            this.moveTo(x, y);
        }

        public destroy() {
            //this.playerGroup.removeAll(true);
            this.npcSprite.events.onInputDown.removeAll();
            this.npcSprite.animations.destroy();
            this.group.destroy(true);
        }

        private moveTo(x: number, y: number) {
            if (this.npcSprite.x === 0 || this.npcSprite.y === 0) {
                this.npcSprite.x = x;
                this.npcSprite.y = y;
            } else {
                this.game.add.tween(this.npcSprite).to({ x, y }, 50, Phaser.Easing.Linear.None, true).start();
            }
            
            

            this.nameplate.setPosition(x, y);
        }
    }
}