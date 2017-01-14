var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ElvenCurse;
(function (ElvenCurse) {
    var Wolf = (function (_super) {
        __extends(Wolf, _super);
        function Wolf(game, npc) {
            _super.call(this, game, npc);
            this.createSpriteAndAnimations();
            this.nameplate = new ElvenCurse.Nameplate(this.game, npc.name);
            this.group = this.game.add.group();
            this.group.add(this.npcSprite);
            this.group.add(this.nameplate.group);
        }
        Wolf.prototype.createSpriteAndAnimations = function () {
            this.npcSprite = this.game.add.sprite(this.npc.location.x, this.npc.location.y, "wolf1");
            this.npcSprite.anchor.setTo(0.5, 0.5);
            var imagesPerRow = 10;
            // walk
            this.npcSprite.animations.add("walkFront", Phaser.ArrayUtils.numberArray(0 * imagesPerRow, 0 * imagesPerRow + 1));
            this.npcSprite.animations.add("walkBack", Phaser.ArrayUtils.numberArray(1 * imagesPerRow, 1 * imagesPerRow + 1));
            this.npcSprite.animations.add("walkRight", Phaser.ArrayUtils.numberArray(2 * imagesPerRow, 2 * imagesPerRow + 4));
            this.npcSprite.animations.add("walkLeft", Phaser.ArrayUtils.numberArray(3 * imagesPerRow, 3 * imagesPerRow + 4));
        };
        return Wolf;
    }(ElvenCurse.NpcBase));
    ElvenCurse.Wolf = Wolf;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=Wolf.js.map