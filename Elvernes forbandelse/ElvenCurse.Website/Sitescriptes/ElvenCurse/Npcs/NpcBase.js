var ElvenCurse;
(function (ElvenCurse) {
    var NpcBase = (function () {
        function NpcBase(game, npc) {
            this.game = game;
            this.npc = npc;
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
            this.npc = npc;
            var self = this;
            if (this.oldHealth > this.npc.health) {
                this.npcSprite.tint = 0xff0000;
                this.oldHealth = this.npc.health;
                this.game.time.events.add(Phaser.Timer.SECOND, function () {
                    self.npcSprite.tint = 0xffffff;
                }, this);
            }
        };
        NpcBase.prototype.placeGroup = function () {
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
            this.moveTo(x, y);
        };
        NpcBase.prototype.destroy = function () {
            //this.playerGroup.removeAll(true);
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