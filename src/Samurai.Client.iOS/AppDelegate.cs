using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Samurai.Client.Wp7;
using Microsoft.Xna.Framework;

namespace Samurai.Client.iOS
{
	[Register ("AppDelegate")]
	class Program : UIApplicationDelegate 
	{
		private Samurai.Client.Wp7.SamuraiGame game;

		public override void FinishedLaunching (UIApplication app)
		{
			// Fun begins..
			game = new SamuraiGame();
			game.Run();
		}

		static void Main (string [] args)
		{
			UIApplication.Main (args,null,"AppDelegate");
		}
	}
}
