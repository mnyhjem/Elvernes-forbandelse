module ElvenCurse {

    export interface IInteractiveObject {
        id: number;
        name: string;
        location: ILocation;
        playerSprite: Phaser.Sprite;
    }
}