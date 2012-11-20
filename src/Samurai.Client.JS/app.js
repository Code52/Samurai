require.config({
  paths: {  //Configure library/module paths.
    /*'jquery' : 'https://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min',*/
    'jquery' : 'lib/jquery-1.8.2.min',
    'contentManager': 'contentManager',
    'coord': 'models/coord',
    'gameState' : 'models/gameState',
    'map' : 'models/map',
    'player' : 'models/player',
    'renderer' : 'renderer',
    'serverApi' : 'serverApi',
    'tiles' : 'models/tileType',
  }
});

require(['jquery', 'samurai'], function ($, Samurai) {
  $(function() {
      var samurai = new Samurai();

      samurai.run();
  });
});