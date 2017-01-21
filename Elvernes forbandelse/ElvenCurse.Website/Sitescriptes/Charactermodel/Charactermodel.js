var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Charactermodel;
(function (Charactermodel) {
    var ShowCharacter = (function (_super) {
        __extends(ShowCharacter, _super);
        function ShowCharacter() {
            _super.call(this);
        }
        ShowCharacter.prototype.preload = function () {
            var url = $("#characterpreview").attr("data-url");
            this.game.load.spritesheet("sprite", url, 64, 64);
        };
        ShowCharacter.prototype.create = function () {
            this.game.stage.backgroundColor = "#FFFFFF";
            this.sprite = this.game.add.sprite(0, 0, "sprite");
            this.sprite.scale.setTo(4.5, 4.5);
            this.sprite.animations.add("turnaround", [26, 39, 0, 13]);
            this.sprite.animations.play("turnaround", 1, true);
        };
        return ShowCharacter;
    }(Phaser.State));
    Charactermodel.ShowCharacter = ShowCharacter;
})(Charactermodel || (Charactermodel = {}));
//# sourceMappingURL=Charactermodel.js.map