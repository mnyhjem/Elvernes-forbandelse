
var game = new Phaser.Game(1024, 768, Phaser.CANVAS, 'phaser-example', { preload: preload, create: create, update: update, render: render });

function preload() {

    game.load.tilemap('world', '/sitescripts/legends/01.json', null, Phaser.Tilemap.TILED_JSON);
    game.load.image('water', '/sitescripts/legends/graphics-tiles-waterflow.png');
    game.load.image('ground', '/sitescripts/legends/ground_tiles.png');

    game.load.image('car', 'assets/sprites/car90.png');

}

var map;
var background;
var blocking;

var cursors;
var sprite;

function create() {

    game.physics.startSystem(Phaser.Physics.ARCADE);

    createMap();

    sprite = game.add.sprite(450, 80, 'car');
    sprite.anchor.setTo(0.5, 0.5);
    placeplayer(80, 30);

    game.physics.arcade.enable(sprite);

    game.camera.follow(sprite);

    cursors = game.input.keyboard.createCursorKeys();

    //game.input.onDown.addOnce(replaceTiles, this);

}

function placeplayer(x, y) {
    if (!y) {
        y = background.getTileY(sprite.position.y);
    }
    if (!x || x < 0) {
        x = background.getTileX(sprite.position.x);
    }
    sprite.position.x = x * map.tileWidth;
    sprite.position.y = y * map.tileHeight;
}

//function replaceTiles() {

//    //  This will replace every instance of tile 31 (cactus plant) with tile 46 (the sign post).
//    //  It does this across the whole layer of the map unless a region is specified.

//    //  You can also pass in x, y, width, height values to control the area in which the replace happens

//    //map.replace(31, 46);
//    map.destroy();
//    game.load.tilemap('desert', 'assets/tilemaps/maps/bane2.json', null, Phaser.Tilemap.TILED_JSON);

//    game.load.onLoadComplete.add(createMap, this);
//    game.load.start();
    
//}

var mapMovedInThisPosition = "";
var changingMap = false;

function changeMap(direction) {
    changingMap = true;
    var mapToLoad = "";
    switch (direction) {
        case "left":
            mapToLoad = background.layer.properties.mapchange_left + ".json";
            mapMovedInThisPosition = "left";
            break;
        case "right":
            mapToLoad = background.layer.properties.mapchange_right + ".json";
            mapMovedInThisPosition = "right";
            break;
        case "up":
            mapToLoad = background.layer.properties.mapchange_up + ".json";
            mapMovedInThisPosition = "up";
            break;
        case "down":
            mapToLoad = background.layer.properties.mapchange_down + ".json";
            mapMovedInThisPosition = "down";
            break;
    }

    if (mapToLoad === "undefined.json") {
        console.log("End of world");
        return;
    }

    map.destroy();
    game.load.tilemap('world', '/sitescripts/legends/' + mapToLoad, null, Phaser.Tilemap.TILED_JSON);

    game.load.onLoadComplete.add(createMap, this);
    game.load.start();
}

function createMap() {
    map = game.add.tilemap('world');

    map.addTilesetImage('water', 'water');
    map.addTilesetImage('ground', 'ground');

    blocking = map.createLayer('blocking');
    background = map.createLayer('background');
    
    background.resizeWorld();

    map.setCollisionBetween(1, 100, true, blocking);
    //map.setCollision(23, true, background)

    if (sprite) {
        sprite.bringToTop();

        switch(mapMovedInThisPosition){
            case "left":
                placeplayer(99);
                break;
            case "right":
                placeplayer(1);
                break;
            case "up":
                placeplayer(-1, 99);
                break;
            case "down":
                placeplayer(-1, 1);
                break;
        }
    }
    changingMap = false;
}

function update() {
    game.physics.arcade.collide(sprite, blocking);


    sprite.body.velocity.x = 0;
    sprite.body.velocity.y = 0;
    sprite.body.angularVelocity = 0;

    if (cursors.left.isDown)
    {
        sprite.body.angularVelocity = -200;
    }
    else if (cursors.right.isDown)
    {
        sprite.body.angularVelocity = 200;
    }

    if (cursors.up.isDown)
    {
        sprite.body.velocity.copyFrom(game.physics.arcade.velocityFromAngle(sprite.angle, 300));
    }

    if (cursors.down.isDown) {
        sprite.body.velocity.copyFrom(game.physics.arcade.velocityFromAngle(sprite.angle, -300));
    }


    // Change map stuff...
    if (changingMap) {
        return;
    }

    if (background.getTileX(sprite.x) < 1)
    {
        changeMap("left");
    }
    else if (background.getTileX(sprite.x) >= map.width)
    {
        changeMap("right");
    }
    else if (background.getTileY(sprite.y) > map.height)
    {
        changeMap("down");
    }
    else if (background.getTileY(sprite.y) < 1)
    {
        changeMap("up");
    }
}

function render() {

    game.debug.text(background.layer.properties.name, 32, 32, 'rgb(0,0,0)');
    game.debug.text('Tile X: ' + background.getTileX(sprite.x) + ' position.x: ' + sprite.position.x, 32, 48, 'rgb(0,0,0)');
    game.debug.text('Tile Y: ' + background.getTileY(sprite.y) + ' position.y: ' + sprite.position.y, 32, 64, 'rgb(0,0,0)');

}
