using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Input;

namespace BlackNectar2ndHalf
{
    public class GameOverScreen : IScreen
    {
        #region Private Members
        private BlackNectarGame game;
        private bool blink;
        private PeriodicTimer blinkTimer;
        private int oldInputState;
        private TimeSpan survival;
        #endregion

        #region Constants
        private float BlinkOnPeriod = 2.0f;
        private float BlinkOffPeriod = 0.5f;
        #endregion

        #region Public Functions
        public GameOverScreen(BlackNectarGame game)
        {
            this.game = game;
            blinkTimer = new PeriodicTimer();
            blinkTimer.OnChange += () =>
            {
                // Every second blink for a half second
                blink = !blink;
                if (!blink)
                {
                    blinkTimer.Period = BlinkOnPeriod;
                }
                else
                {
                    blinkTimer.Period = BlinkOffPeriod;
                }
            };
        }

        public void Reset()
        {
            Reset(new TimeSpan());
        }

        public void Reset(TimeSpan survival)
        {
            this.survival = survival;
            blinkTimer.Reset();
            blinkTimer.Period = BlinkOnPeriod;
            blink = false;
            // Buffer old input to 'debounce' transitions between states (i.e.
            // only change when input goes from not pressed to pressed)
            oldInputState = -1;
        }

        public void Update(TimeSpan t)
        {
            // Update timers
            blinkTimer.Update(t);

            // Render instructions
            game.Screen.Clear(Palette.ClearFill);
            Fonts.HeavyData.Render("GAME OVER", 180, 100, 60, Palette.Energy1, game.Screen);
            Fonts.HeavyData.Render(String.Format("SURVIVAL: {0:D2}:{1:D2}", survival.Minutes, survival.Seconds), 210, 200, 30, Palette.Energy1, game.Screen);
            if (!blink)
            {
                Fonts.HeavyData.Render("CLICK OR PRESS KEY TO CONTINUE", 100, 390, 30, Palette.Energy1, game.Screen);
            }
            game.Screen.Invalidate();

            // Check for key presses and change to game
            KeyboardState keys = Keyboard.GetState();
            if ((keys.GetPressedKeys().Length > 0 || MouseFixed.LeftButton) && oldInputState == 0)
            {
                game.ChangeToTitleScreen();
            }
            else
            {
                oldInputState = keys.GetPressedKeys().Length > 0 ? 1 : 0;
                oldInputState += MouseFixed.LeftButton ? 1 : 0;
            }
        }
        #endregion
    }
}
