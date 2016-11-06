module ElvenCurse {

    export interface ILocation {
        worldsectionId: number;
        y: number;
        x: number;
    }

    export interface IPlayer {
        id: number;
        name: string;
        location: ILocation;
        playerSprite: Phaser.Sprite;
        connectionstatus: number;
        type: number;
    }
}