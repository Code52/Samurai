using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Samurai.Client.Wp7.Screens
{
    public class ScreenManager : DrawableGameComponent
    {
        private readonly List<BaseScreen> screens;

        private BaseScreen loadingScreen = null;
        public BaseScreen LoadingScreen
        {
            get { return loadingScreen; }
            set
            {
                loadingScreen = value;
                loadingScreen.Manager = this;
            }
        }

        private BaseScreen curScreen = null;
        private BaseScreen nextScreen = null;

        public readonly BackgroundJobs Jobs;

        public ScreenManager(Game game)
            : base(game)
        {
            screens = new List<BaseScreen>();
            Jobs = new BackgroundJobs();
            Jobs.SpinUp();
        }

        protected override void LoadContent()
        {
            for (int i = 0; i < screens.Count; i++)
                screens[i].LoadContent();

            if (loadingScreen != null)
                loadingScreen.LoadContent();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            for (int i = 0; i < screens.Count; i++)
                screens[i].UnloadContent();

            if (loadingScreen != null)
                loadingScreen.UnloadContent();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // See if we need to change screens
            if (nextScreen != null && nextScreen.IsReady)
            {
                if (curScreen != null)
                    curScreen.OnNavigatedFrom();
                curScreen = nextScreen;
                nextScreen = null;
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
            if (!nextScreen.IsReady)
                nextScreen.LoadContent();
            // Switch to loading screen if it exists
            if (loadingScreen != null)
            {
                if (curScreen != null)
                    curScreen.OnNavigatedFrom();
                curScreen = loadingScreen;
                curScreen.OnNavigatedTo();
            }
        }

        public void Tombstone()
        {
            if (curScreen != null)
                curScreen.OnNavigatedFrom();
        }

        public void ExitGame()
        {
            Game.Exit();
        }
    }
}
