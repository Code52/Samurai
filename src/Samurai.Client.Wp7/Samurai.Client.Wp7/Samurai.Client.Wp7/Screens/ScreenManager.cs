using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Samurai.Client.Wp7.Screens
{
    public class ScreenManager : DrawableGameComponent
    {
        private readonly List<BaseScreen> screens = new List<BaseScreen>();

        public BaseScreen LoadingScreen = null;

        private BaseScreen curScreen = null;
        private BaseScreen nextScreen = null;

        public ScreenManager(Game game)
            : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            // See if we need to change screens
            if (nextScreen != null && nextScreen.IsReady)
            {
                if (curScreen != null)
                    curScreen.OnNavigatedFrom();
                curScreen = nextScreen;
                curScreen.OnNavigatedTo();
            }

            if (curScreen != null)
                curScreen.Update(gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (curScreen == null)
                return;
            curScreen.Draw(gameTime.ElapsedGameTime.TotalSeconds, Game.GraphicsDevice);
            base.Draw(gameTime);
        }

        public T GetOrCreateScreen<T>() where T : BaseScreen
        {
            for (int i = 0; i < screens.Count; i++)
            {
                if (screens[i] is T)
                    return (T)screens[i];
            }

            // No screen found, create a new one
            var screen = Activator.CreateInstance<T>();
            screen.Manager = this;
            screens.Add(screen);
            return screen;
        }

        public void TransitionTo<T>() where T : BaseScreen
        {
            var scr = GetOrCreateScreen<T>();
            nextScreen = scr;
            // Switch to loading screen if it exists
            if (LoadingScreen != null)
            {
                if (curScreen != null)
                    curScreen.OnNavigatedFrom();
                curScreen = LoadingScreen;
                curScreen.OnNavigatedTo();
            }
        }
    }
}
