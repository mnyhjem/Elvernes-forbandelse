var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ElvenCurse;
(function (ElvenCurse) {
    var ElfHunter = (function (_super) {
        __extends(ElfHunter, _super);
        //game: Phaser.Game;
        //player:IPlayer;
        //nameplate: Nameplate;
        //group: Phaser.Group;
        //playerSprite:Phaser.Sprite;
        function ElfHunter(game, npc) {
            //this.game = game;
            //this.player = player;
            _super.call(this, game, npc);
            this.createPlayerspriteAndAnimations();
            this.nameplate = new ElvenCurse.Nameplate(this.game, npc.name);
            this.group = this.game.add.group();
            this.group.add(this.npcSprite);
            this.group.add(this.nameplate.group);
        }
        //public bringToTop() {
        //    //this.playerSprite.bringToTop();
        //    this.game.world.bringToTop(this.playerGroup);
        //}
        //public updatePosition(player: IPlayer) {
        //    this.player = player;
        //}
        //public placeGroup() {
        //    var x = this.player.location.x * 32;
        //    var y = this.player.location.y * 32;
        //    if (this.playerSprite.x < x) {
        //        this.playAnimation("walkRight");
        //    }
        //    if (this.playerSprite.x > x) {
        //        this.playAnimation("walkLeft");
        //    }
        //    if (this.playerSprite.y > y) {
        //        this.playAnimation("walkBack");
        //    }
        //    if (this.playerSprite.y < y) {
        //        this.playAnimation("walkFront");
        //    }
        //    this.playerSprite.x = x;
        //    this.playerSprite.y = y;
        //    this.nameplate.setPosition(x, y);
        //}
        //public destroy() {
        //    //this.playerGroup.removeAll(true);
        //    this.playerSprite.animations.destroy();
        //    this.group.destroy(true);
        //}
        ElfHunter.prototype.createPlayerspriteAndAnimations = function () {
            this.npcSprite = this.game.add.sprite(this.npc.location.x, this.npc.location.y, "playertest");
            this.npcSprite.anchor.setTo(0.5, 0.5);
            var imagesPerRow = 13;
            // spellcast
            this.npcSprite.animations.add("spellcastBack", Phaser.ArrayUtils.numberArray(0 * imagesPerRow, 0 * imagesPerRow + 6)); //0,6
            this.npcSprite.animations.add("spellcastLeft", Phaser.ArrayUtils.numberArray(1 * imagesPerRow, 1 * imagesPerRow + 6)); //13,19
            this.npcSprite.animations.add("spellcastFront", Phaser.ArrayUtils.numberArray(2 * imagesPerRow, 2 * imagesPerRow + 6)); //26,32
            this.npcSprite.animations.add("spellcastRight", Phaser.ArrayUtils.numberArray(3 * imagesPerRow, 3 * imagesPerRow + 6)); //39,45
            // thrust
            this.npcSprite.animations.add("thrustBack", Phaser.ArrayUtils.numberArray(4 * imagesPerRow, 4 * imagesPerRow + 6));
            this.npcSprite.animations.add("thrustLeft", Phaser.ArrayUtils.numberArray(5 * imagesPerRow, 5 * imagesPerRow + 6));
            this.npcSprite.animations.add("thrustFront", Phaser.ArrayUtils.numberArray(6 * imagesPerRow, 6 * imagesPerRow + 6));
            this.npcSprite.animations.add("thrustRight", Phaser.ArrayUtils.numberArray(7 * imagesPerRow, 7 * imagesPerRow + 6));
            // walk
            this.npcSprite.animations.add("walkBack", Phaser.ArrayUtils.numberArray(8 * imagesPerRow, 8 * imagesPerRow + 6));
            this.npcSprite.animations.add("walkLeft", Phaser.ArrayUtils.numberArray(9 * imagesPerRow, 9 * imagesPerRow + 6));
            this.npcSprite.animations.add("walkFront", Phaser.ArrayUtils.numberArray(10 * imagesPerRow, 10 * imagesPerRow + 6));
            this.npcSprite.animations.add("walkRight", Phaser.ArrayUtils.numberArray(11 * imagesPerRow, 11 * imagesPerRow + 6));
            // slash
            this.npcSprite.animations.add("slashBack", Phaser.ArrayUtils.numberArray(12 * imagesPerRow, 12 * imagesPerRow + 6));
            this.npcSprite.animations.add("slashLeft", Phaser.ArrayUtils.numberArray(13 * imagesPerRow, 13 * imagesPerRow + 6));
            this.npcSprite.animations.add("slashFront", Phaser.ArrayUtils.numberArray(14 * imagesPerRow, 14 * imagesPerRow + 6));
            this.npcSprite.animations.add("slashRight", Phaser.ArrayUtils.numberArray(15 * imagesPerRow, 15 * imagesPerRow + 6));
            // shoot
            this.npcSprite.animations.add("shootBack", Phaser.ArrayUtils.numberArray(16 * imagesPerRow, 16 * imagesPerRow + 6));
            this.npcSprite.animations.add("shootLeft", Phaser.ArrayUtils.numberArray(17 * imagesPerRow, 17 * imagesPerRow + 6));
            this.npcSprite.animations.add("shootFront", Phaser.ArrayUtils.numberArray(18 * imagesPerRow, 18 * imagesPerRow + 6));
            this.npcSprite.animations.add("shootRight", Phaser.ArrayUtils.numberArray(19 * imagesPerRow, 19 * imagesPerRow + 6));
            // hurt
            this.npcSprite.animations.add("hurtBack", Phaser.ArrayUtils.numberArray(20 * imagesPerRow, 20 * imagesPerRow + 6));
            this.npcSprite.animations.add("hurtLeft", Phaser.ArrayUtils.numberArray(21 * imagesPerRow, 21 * imagesPerRow + 6));
            this.npcSprite.animations.add("hurtFront", Phaser.ArrayUtils.numberArray(22 * imagesPerRow, 22 * imagesPerRow + 6));
            this.npcSprite.animations.add("hurtRight", Phaser.ArrayUtils.numberArray(23 * imagesPerRow, 23 * imagesPerRow + 6));
            //this.playerSprite.animations.play("shootRight", 10, false);
        };
        return ElfHunter;
    }(ElvenCurse.NpcBase));
    ElvenCurse.ElfHunter = ElfHunter;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=ElfHunter.js.map