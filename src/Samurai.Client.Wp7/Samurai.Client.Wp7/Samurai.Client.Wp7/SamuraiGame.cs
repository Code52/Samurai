using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Samurai.Client.Wp7.Screens;

namespace Samurai.Client.Wp7
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SamuraiGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager _screens;

        public SamuraiGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

#if WINDOWS_PHONE
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

#if !MONO
            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
#endif

            graphics.IsFullScreen = true;
#endif
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            _screens.Tombstone();
            base.OnExiting(sender, args);
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            _screens.Tombstone();
            base.OnDeactivated(sender, args);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _screens = new ScreenManager(this);
            Components.Add(_screens);
            _screens.LoadingScreen = new LoadingScreen();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Do this here so that the Graphics device is ready
            _screens.GetOrCreateScreen<MainMenuScreen>();
            _screens.TransitionTo<MainMenuScreen>();
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // DrawableGameComponent automatically does this for our screens
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // DrawableGameComponent automatically does this for our screens
            base.Draw(gameTime);
        }
    }
}
