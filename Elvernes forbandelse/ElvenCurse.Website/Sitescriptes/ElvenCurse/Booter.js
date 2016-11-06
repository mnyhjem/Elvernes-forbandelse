var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ElvenCurse;
(function (ElvenCurse) {
    var Booter = (function (_super) {
        __extends(Booter, _super);
        function Booter() {
            _super.call(this);
        }
        Booter.prototype.preload = function () {
            // loading background
            this.game.load.image("loadingbackground", "/content/assets/graphics/backgrounds/4.jpg");
        };
        Booter.prototype.create = function () {
            this.game.state.start("Preloader");
        };
        return Booter;
    }(Phaser.State));
    ElvenCurse.Booter = Booter;
})(ElvenCurse || (ElvenCurse = {}));
//# sourceMappingURL=Booter.js.map