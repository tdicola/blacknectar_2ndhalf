using System;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Input;

namespace BlackNectar2ndHalf
{
    public class TitleScreen: IScreen
    {
        #region Private Members
        private BlackNectarGame game;
        private StarField starField;
        private bool blink;
        private PeriodicTimer blinkTimer;
        private int oldInputState;
        #endregion

        #region Constants
        private float BlinkOnPeriod = 2.0f;
        private float BlinkOffPeriod = 0.5f;
        #endregion

        #region Public Functions
        public TitleScreen(BlackNectarGame game)
        {
            this.game = game;
            starField = new StarField();
            starField.Initialize(game, StarField.StarCount);
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
            blinkTimer.Reset();
            blinkTimer.Period = BlinkOnPeriod;
            blink = false;
            // Buffer old input to 'debounce' transitions between states (i.e.
            // only change when input goes from not pressed to pressed)
            oldInputState = -1;
        }

        public void Update(TimeSpan t)
        {
            // Update state
            blinkTimer.Update(t);
            starField.Pan(0, StarField.StarSpeed, t);

            // Render title
            game.Screen.Clear(Palette.ClearFill);
            starField.Render(game.Screen);
            Fonts.HeavyData.Render("BLACK NECTAR", 100, 100, 72, Palette.Energy1, game.Screen);
            Fonts.HeavyData.Render("SECOND HALF", 230, 170, 36, Palette.Energy1, game.Screen);
            if (!blink)
            {
                Fonts.HeavyData.Render("CLICK OR PRESS KEY TO CONTINUE", 100, 390, 30, Palette.Energy1, game.Screen);
            }
            game.Screen.Invalidate();

            // Check for key presses and change to game
            KeyboardState keys = Keyboard.GetState();
            if ((keys.GetPressedKeys().Length > 0 || MouseFixed.LeftButton) && oldInputState == 0)
            {
                game.ChangeToInstructionScreen();
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
