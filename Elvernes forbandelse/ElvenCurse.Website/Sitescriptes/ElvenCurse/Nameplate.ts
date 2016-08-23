namespace ElvenCurse {
    export class Nameplate {
        game:Phaser.Game;
        group: Phaser.Group;
        nametext: Phaser.Text;
        
        constructor(game:Phaser.Game, name: string) {
            this.game = game;
            this.group = this.game.add.group();

            this.nametext = this.game.add.text(128, 960, name, "");
            this.nametext.anchor.set(0.5);
            this.nametext.fontSize = 15;
            //this.nametext.addColor("#0066ff", 0);
            this.nametext.fill = "#0066ff";
            this.group.add(this.nametext);

            //this.nametext.x = 2555;
            //this.nametext.y = 960;
        }

        public setPosition(x: number, y: number) {
            y -= 32;
            this.group.forEach(element => {
                element.x = x;
                element.y = y;
            }, this);
            
        }
    }
}