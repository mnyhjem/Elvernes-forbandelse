module ElvenCurse {
    export class Player implements IPlayer {
        //rotationSpeed = 50;
        moveSpeed = 80;

        game: Phaser.Game;
        name: string;
        location: ILocation;
        id: number;

        playerSprite: Phaser.Sprite;
        nameplate: Nameplate;

        playerGroup:Phaser.Group;

        
        constructor(playersprite: Phaser.Sprite, game:Phaser.Game) {
            this.game = game;

            this.loadPlayer();

            this.playerSprite = playersprite;
            this.playerSprite.anchor.setTo(0.5, 0.5);

            this.nameplate = new Nameplate(this.game, this.name);

            this.playerGroup = this.game.add.group();
            this.playerGroup.add(this.playerSprite);
            this.playerGroup.add(this.nameplate.group);
        }

        public bringToTop() {
            //this.playerSprite.bringToTop();

            this.game.world.bringToTop(this.playerGroup);
        }

        public move(cursors: Phaser.CursorKeys) {
            this.playerSprite.body.velocity.x = 0;
            this.playerSprite.body.velocity.y = 0;
            this.playerSprite.body.angularVelocity = 0;

            if (cursors.left.isDown) {
                //this.playerSprite.body.angularVelocity = -this.rotationSpeed;
                this.playerSprite.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(180, this.moveSpeed));
            }
            else if (cursors.right.isDown) {
                //this.playerSprite.body.angularVelocity = this.rotationSpeed;
                this.playerSprite.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(0, this.moveSpeed));
            }

            if (cursors.up.isDown) {
                //this.playerSprite.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(this.playerSprite.angle, this.moveSpeed));
                this.playerSprite.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(270, this.moveSpeed));
            }

            if (cursors.down.isDown) {
                //this.playerSprite.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(this.playerSprite.angle, -this.moveSpeed));
                this.playerSprite.body.velocity.copyFrom(this.game.physics.arcade.velocityFromAngle(90, this.moveSpeed));
            }

            this.nameplate.setPosition(this.playerSprite.x, this.playerSprite.y);
        }

        public checkCollisions(layer: Phaser.TilemapLayer) {
            this.game.physics.arcade.collide(this.playerSprite, layer);
        }

        private loadPlayer() {
            var self = this;
            $.ajax({
                url: "/api/character/getactive",
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