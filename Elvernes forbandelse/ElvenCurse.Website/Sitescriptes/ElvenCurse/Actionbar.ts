namespace ElvenCurse {
    export class Actionbar {
        game: Phaser.Game;
        group: Phaser.Group;
        entity: IPlayer;
        actionbarSprite: Phaser.Sprite;
        
        constructor(game: Phaser.Game, entity: IPlayer) {
            this.game = game;
            this.entity = entity;

            this.createPlatesprite();

            this.group = this.game.add.group();
            this.addAbilities();
            this.group.add(this.actionbarSprite);
            
        }

        createPlatesprite():void {
            var x:number = (this.game.width / 2) - 234;
            var y:number = this.game.height - 100;
            this.actionbarSprite = this.game.add.sprite(x, y, "Actionbar");
            this.actionbarSprite.scale.x = 1.2;
            this.actionbarSprite.scale.y = 1.2;
        }

        public destroy():void {
            // this.playerGroup.removeAll(true);
            this.group.destroy(true);
        }

        public addAbilities() {
            var x = this.actionbarSprite.x + 15;
            var y = this.actionbarSprite.y+8;

            // test
            for (var i = 0; i < 15; i++) {
                var abilityindex = i;
                if (abilityindex >= 11) {
                    abilityindex += 1;
                }
                this.addAbility(x + (i * 36), y, abilityindex);
            }
        }

        private addAbility(x: number, y: number, abilityIndex: number) {
            var s = this.game.add.sprite(x, y, "abilities", abilityIndex);
            s.scale.x = 0.7;
            s.scale.y = 0.7;
            this.group.add(s);
        }
    }
}