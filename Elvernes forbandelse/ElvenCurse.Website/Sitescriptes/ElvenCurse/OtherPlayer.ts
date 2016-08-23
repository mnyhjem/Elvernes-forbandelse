module ElvenCurse {
    export class OtherPlayer {
        game: Phaser.Game;
        player:IPlayer;
        nameplate: Nameplate;
        playerGroup: Phaser.Group;
        playerSprite:Phaser.Sprite;

        constructor(game: Phaser.Game, player: IPlayer) {
            this.game = game;
            this.player = player;

            this.playerSprite = this.game.add.sprite(player.location.x, player.location.y, "player");
            this.playerSprite.anchor.setTo(0.5, 0.5);

            this.nameplate = new Nameplate(this.game, player.name);

            this.playerGroup = this.game.add.group();
            this.playerGroup.add(this.playerSprite);
            this.playerGroup.add(this.nameplate.group);
        }

        public bringToTop() {
            //this.playerSprite.bringToTop();

            this.game.world.bringToTop(this.playerGroup);
        }
        
        public updatePosition(player: IPlayer) {
            this.player = player;
        }

        public placeGroup() {
            var x = this.player.location.x * 32;
            var y = this.player.location.y * 32;

            this.playerSprite.x = x;
            this.playerSprite.y = y;
            this.nameplate.setPosition(x, y);
        }
    }
}