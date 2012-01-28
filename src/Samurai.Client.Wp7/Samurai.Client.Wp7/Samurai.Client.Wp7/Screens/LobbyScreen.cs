using Samurai.Client.Wp7.Api;

namespace Samurai.Client.Wp7.Screens
{
    public class LobbyScreen : BaseScreen
    {
        public override void LoadContent()
        {
            IsReady = true;
            base.LoadContent();
        }

        public void SetApi(ServerApi api)
        {

        }
    }
}
