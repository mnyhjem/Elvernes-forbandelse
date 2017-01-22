module ElvenCurse {

    export interface ILocation {
        worldsectionId: number;
        y: number;
        x: number;
    }

    export interface IAbility {
        name: string;
        cooldown: number;
        baseDamage: number;
        baseHeal: number;
        passive: boolean;
        isHeal: boolean;
        abilityIcon: number;
    }

    export interface IPlayer {
        id: number;
        name: string;
        location: ILocation;
        playerSprite: Phaser.Sprite;
        connectionstatus: number;
        type: number;
        health: number;
        isAlive: boolean;
        maxHealth: number;
        abilities : Array<IAbility>;
        isPlayer: boolean;
    }
}