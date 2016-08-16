module ElvenCurse {
    export class Player implements IPlayer {
        game: Phaser.Game;
        id: number;
        name: string;
        location: ILocation;

        playerSprite: Phaser.Sprite;
        
        constructor(characterId: number, playersprite: Phaser.Sprite, game:Phaser.Game) {
            this.id = characterId;
            this.game = game;

            this.loadPlayer();

            this.playerSprite = playersprite;
            this.playerSprite.anchor.setTo(0.5, 0.5);
        }

        public move(cursors: Phaser.CursorKeys) {
            this.playerSprite.body.velocity.x = 0;
            this.playerSprite.body.velocity.y = 0;
            this.playerSprite.body.angularVelocity = 0;

            if (cursors.left.isDown) {
                this.playerSprite.body.angularVelocity = -200;
            }
            else if (cursors.right.isDown) {
                this.playerSprite.body.angularVelocity = 200;
            }

            if (cursors.up.isDown) {
                this.playerSprite.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(this.playerSprite.angle, 300));
            }

            if (cursors.down.isDown) {
                this.playerSprite.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(this.playerSprite.angle, -300));
            }
        }

        public checkCollisions(layer: Phaser.TilemapLayer) {
            this.game.physics.arcade.collide(this.playerSprite, layer);
        }

        private loadPlayer() {
            var self = this;
            $.ajax({
                url: "/api/character/get/" + this.id,
                success(result :IPlayer) {
                    //var hej = "vi skal sætte vores data her";
                    self.name = result.name;
                    self.location = result.location;
                },
                async: false // <-- vi er ikke async.. er med vilje...
            });
        }

        
    }
}