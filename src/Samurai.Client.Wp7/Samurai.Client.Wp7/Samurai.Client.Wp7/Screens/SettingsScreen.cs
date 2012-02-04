using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNInterface.Controls;
using Microsoft.Xna.Framework.Content;
using XNInterface.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Samurai.Client.Wp7.Screens
{
    class SettingsScreen : BaseScreen
    {
        private ContentManager content;
        private SpriteBatch sb;
        private Window gui;
        private WP7Touch touch;

        private Button btnSound;

        private const string soundOn = "Sound: On";
        private const string soundOff = "Sound: Off";

        public override void LoadContent()
        {
            Manager.Jobs.CreateJob(
                () =>
                {
                    content = new ContentManager(Manager.Game.Services, "Content");
                    sb = new SpriteBatch(Manager.GraphicsDevice);
                    gui = content.Load<Window>("GUI\\Settings");
                    gui.Initialise(null);
                    gui.LoadGraphics(Manager.GraphicsDevice, content);
                    touch = new WP7Touch(gui);
                    touch.EnableTap();
                    BindInput();
                    LoadSettings();

                    IsReady = true;
                });
            base.LoadContent();
        }

        private void BindInput()
        {
            btnSound = gui.GetChild<Button>("btnSound");

            btnSound.Triggered +=
                (b) =>
                {
                    Settings.Instance.IsMute = !Settings.Instance.IsMute;
                    UpdateSoundLabel();
                };
        }

        private void LoadSettings()
        {
            UpdateSoundLabel();
        }

        private void UpdateSoundLabel()
        {
            if (!Settings.Instance.IsMute)
                btnSound.Text = soundOn;
            else
                btnSound.Text = soundOff;
        }

        public override void Update(double elapsedSeconds)
        {
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
            {
                Manager.TransitionTo<MainMenuScreen>();
            }

            touch.HandleGestures();
            gui.PerformLayout(Manager.GraphicsDevice.Viewport.Width, Manager.GraphicsDevice.Viewport.Height);
            gui.Update(elapsedSeconds);

            base.Update(elapsedSeconds);
        }

        public override void Draw(double elapsedSeconds, GraphicsDevice device)
        {
            sb.Begin();
            gui.Draw(device, sb, elapsedSeconds);
            sb.End();

            base.Draw(elapsedSeconds, device);
        }
    }
}
