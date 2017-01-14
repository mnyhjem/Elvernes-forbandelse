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
        NpcBase.prototype.updatePosition = function (npc) {
            this.npc = npc;
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
            this.npcSprite.x = x;
            this.npcSprite.y = y;
            this.nameplate.setPosition(x, y);
        };
        NpcBase.prototype.destroy = function () {
            //this.playerGroup.removeAll(true);
            this.npcSprite.animations.destroy();
            this.group.destroy(true);
        };
        return NpcBase;
    }());
    ElvenCurse.NpcBase = NpcBase;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=NpcBase.js.map