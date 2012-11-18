/**
 *
 */
function TileType() {
  this.Name = '';
  this.CanMoveOn = false;
  this.CanShootOver = false;
}

TileType.prototype.toString = function () {
  //TODO: String representation of this base obj?
};

function Grass() {
    this.Name = 'Grass';
    this.CanMoveOn = true;
    this.CanShootOver = true;
}

Grass.prototype = new TileType();
Grass.prototype.toString = function () {
  return '.';
};

function Water() {
  this.Name = 'Water';
  this.CanMoveOn = false;
  this.CanShootOver = true;
}

Water.prototype = new TileType();
Water.prototype.toString = function () {
  return '~';
};

function Rock() {
  this.Name = 'Rock';
  this.CanMoveOn = false;
  this.CanShootOver = true;
}

Rock.prototype = new TileType();
Rock.prototype.toString = function () {
  return '@';
};

function Tree() {
  this.Name = 'Tree';
  this.CanMoveOn = false;
  this.CanShootOver = false;
}

Tree.prototype = new TileType();
Tree.prototype.toString = function () {
  return 'T';
};