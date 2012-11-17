/**
 *
 */
function TileType() {
  this.name = '';
  this.canMoveOn = false;
  this.canShootOver = false;
}

TileType.prototype.toString = function () {
  //TODO: String representation of this base obj?
};

//public class Grass : TileType
function Grass() {
    this.name = 'Grass';
    this.canMoveOn = true;
    this.canShootOver = true;
}

Grass.prototype = new TileType();
Grass.prototype.toString = function () {
  return '.';
};

//public class Water : TileType
function Water() {
  this.name = 'Water';
  this.canMoveOn = false;
  this.canShootOver = true;
}

Water.prototype = new TileType();
Water.prototype.toString = function () {
  return '~';
};

function Rock() {
  this.name = 'Rock';
  this.canMoveOn = false;
  this.canShootOver = true;
}

Rock.prototype = new TileType();
Rock.prototype.toString = function () {
  return '@';
};

function Tree() {
  this.name = 'Tree';
  this.canMoveOn = false;
  this.canShootOver = false;
}

Tree.prototype = new TileType();
Tree.prototype.toString = function () {
  return 'T';
};