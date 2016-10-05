module ElvenCurse {
    export interface IProperties {
        mapchange_down: string;
        mapchange_left: string;
        name: string;
    }

    export interface IPropertytypes {
        mapchange_down: string;
        mapchange_left: string;
        name: string;
    }

    export interface ILayer {
        data?: any;
        height: number;
        name: string;
        opacity: number;
        type: string;
        visible: boolean;
        width: number;
        x: number;
        y: number;
        properties: IProperties;
        propertytypes: IPropertytypes;
    }

    export interface ITileset {
        columns: number;
        firstgid: number;
        image: IImage;
        imageheight: number;
        imagewidth: number;
        margin: number;
        name: string;
        spacing: number;
        tilecount: number;
        tileheight: number;
        tilewidth: number;
    }

    export interface IImage {
        source: string;
        width: number;
        height: number;
        transparentcolor: string;
    }

    export interface ITilemap {
        height: number;
        layers: ILayer[];
        nextobjectid: number;
        orientation: string;
        renderorder: string;
        tileheight: number;
        tilesets: ITileset[];
        tilewidth: number;
        version: number;
        width: number;
    }

    export interface IWorldsection {
        id: number;
        mapchangeRight: number;
        mapchangeLeft: number;
        mapchangeUp: number;
        mapchangeDown: number;
        json: string;
        tilemap: ITilemap;
        name: string;
    }
}