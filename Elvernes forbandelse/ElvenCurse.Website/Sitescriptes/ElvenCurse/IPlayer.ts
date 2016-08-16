module ElvenCurse {

    export interface ILocation {
        jsonname: string;
        worldsectionId: number;
        y: number;
        x: number;
    }

    export interface IPlayer {
        id: number;
        name: string;
        location: ILocation;
    }
}