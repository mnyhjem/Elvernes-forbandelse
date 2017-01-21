namespace ElvenCurse {
    export class Actionbar {
        game: Phaser.Game;
        group: Phaser.Group;
        entity: IPlayer;
        actionbarSprite: Phaser.Sprite;

        abilitySprites: Phaser.Sprite[];

        activatedAbility:number;
        
        constructor(game: Phaser.Game, entity: IPlayer) {
            this.game = game;
            this.entity = entity;

            this.activatedAbility = -1;
            this.abilitySprites = new Array<Phaser.Sprite>();

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
            for (var i = 0; i < this.abilitySprites.length; i++) {
                var p = this.abilitySprites[i];
                p.events.onInputDown.removeAll();
                p.destroy();
            }

            this.group.destroy(true);
        }

        public addAbilities() {
            var x = this.actionbarSprite.x + 15;
            var y = this.actionbarSprite.y+8;

            var padding = 0;
            for (var i = 0; i < this.entity.abilities.length; i++) {
                var ability = this.entity.abilities[i];
                if (ability.passive) {
                    continue;
                }

                this.addAbility(x + (padding * 36), y, ability.abilityIcon, i);
                padding++;
            }
        }

        public getActivatedAbility() : number {
            var a = this.activatedAbility;

            this.activatedAbility = -1;
            return a;
        }

        private addAbility(x: number, y: number, abilityIcon: number, abilityIndex: number) {
            var s = this.game.add.sprite(x, y, "abilities", abilityIcon);
            s.scale.x = 0.7;
            s.scale.y = 0.7;
            this.abilitySprites.push(s);
            this.group.add(s);

            s.abilityIndex = abilityIndex;
            s.inputEnabled = true;
            s.events.onInputDown.add(function (sprite, pointer) {
                this.activatedAbility = sprite.abilityIndex;
            }, this);
        }
    }
}