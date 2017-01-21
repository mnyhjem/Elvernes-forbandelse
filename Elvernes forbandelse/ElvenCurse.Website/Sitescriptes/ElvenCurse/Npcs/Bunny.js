var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ElvenCurse;
(function (ElvenCurse) {
    var Bunny = (function (_super) {
        __extends(Bunny, _super);
        function Bunny(game, npc) {
            _super.call(this, game, npc);
            this.createSpriteAndAnimations();
            this.nameplate = new ElvenCurse.Nameplate(this.game, npc.name, npc);
            this.group = this.game.add.group();
            this.group.add(this.npcSprite);
            this.group.add(this.nameplate.group);
        }
        Bunny.prototype.createSpriteAndAnimations = function () {
            this.npcSprite = this.game.add.sprite(this.creature.location.x, this.creature.location.y, "bunny");
            this.npcSprite.anchor.setTo(0.5, 0.5);
            var imagesPerRow = 18;
            var offset = 6;
            // walk
            this.npcSprite.animations.add("walkFront", Phaser.ArrayUtils.numberArray(4 * imagesPerRow + offset, 4 * imagesPerRow + 2 + offset));
            this.npcSprite.animations.add("walkBack", Phaser.ArrayUtils.numberArray(7 * imagesPerRow + offset, 7 * imagesPerRow + offset + 2));
            this.npcSprite.animations.add("walkRight", Phaser.ArrayUtils.numberArray(6 * imagesPerRow + offset, 6 * imagesPerRow + offset + 2));
            this.npcSprite.animations.add("walkLeft", Phaser.ArrayUtils.numberArray(5 * imagesPerRow + offset, 5 * imagesPerRow + offset + 2));
        };
        return Bunny;
    }(ElvenCurse.NpcBase));
    ElvenCurse.Bunny = Bunny;
})(ElvenCurse || (ElvenCurse = {}));
