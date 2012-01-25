using Microsoft.Xna.Framework.Graphics;

namespace Samurai.Client.Wp7.Screens
{
    public class BaseScreen
    {
        public ScreenManager Manager { get; set; }

        public bool IsReady { get; private set; }

        public BaseScreen() { }

        public virtual void Update(double elapsedSeconds) { }

        public virtual void Draw(double elapsedSeconds, GraphicsDevice device) { }

        public virtual void OnNavigatedTo() { }

        public virtual void OnNavigatedFrom() { }
    }
}
