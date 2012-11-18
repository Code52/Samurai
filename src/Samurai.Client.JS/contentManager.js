/**
 *
 */
function ContentManager() {
  this.textures = {};
}

ContentManager.prototype.loadTexture2D = function (texture) {
  if(this.textures[texture])
    return this.textures[texture];
  else {
    var tmpImg = new Image();

    tmpImg.src = texture;
    this.textures[texture] = tmpImg;

    return tmpImg;
  }
};