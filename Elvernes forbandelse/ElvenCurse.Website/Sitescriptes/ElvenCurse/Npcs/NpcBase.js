var ElvenCurse;
(function (ElvenCurse) {
    var NpcBase = (function () {
        function NpcBase(game, npc) {
            this.game = game;
            this.creature = npc;
            this.oldHealth = this.creature.health;
        }
        NpcBase.prototype.playAnimation = function (animationName) {
            var animation = this.npcSprite.animations.getAnimation(animationName);
            //this.nameplate.nametext.text = animation.frameTotal.toString();
            if (animation === undefined || animation === null) {
                return;
            }
            if (!animation.isPlaying) {
                this.npcSprite.animations.play(animationName, 10, false);
            }
        };
        NpcBase.prototype.updatePlayer = function (npc) {
            var revieve = this.creature.isAlive === false && npc.isAlive === true;
            this.creature = npc;
            if (!this.creature.isAlive) {
                this.playAnimation("hurtBack"); // todo mangler grafik for når man er død der virker for alle npcer..
            }
            if (revieve) {
            }
            var self = this;
            if (this.oldHealth > this.creature.health) {
                this.npcSprite.tint = 0xff0000;
                this.oldHealth = this.creature.health;
                this.game.time.events.add(Phaser.Timer.SECOND, function () {
                    self.npcSprite.tint = 0xffffff;
                }, this);
            }
            this.nameplate.update(this.creature);
        };
        NpcBase.prototype.placeGroup = function () {
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
        };
        NpcBase.prototype.destroy = function () {
            //this.playerGroup.removeAll(true);
            this.npcSprite.events.onInputDown.removeAll();
            this.npcSprite.animations.destroy();
            this.group.destroy(true);
        };
        NpcBase.prototype.moveTo = function (x, y) {
            if (this.npcSprite.x === 0 || this.npcSprite.y === 0) {
                this.npcSprite.x = x;
                this.npcSprite.y = y;
            }
            else {
                this.game.add.tween(this.npcSprite).to({ x: x, y: y }, 50, Phaser.Easing.Linear.None, true).start();
            }
            this.nameplate.setPosition(x, y);
        };
        return NpcBase;
    }());
    ElvenCurse.NpcBase = NpcBase;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=NpcBase.js.map