using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Input;

namespace BlackNectar2ndHalf
{
    public class InstructionScreen: IScreen
    {
        #region Private Members
        private BlackNectarGame game;
        private bool blink;
        private PeriodicTimer blinkTimer;
        private PeriodicTimer energyTimer;
        private int oldInputState;
        private Color energyColor;
        #endregion

        #region Constants
        private float BlinkOnPeriod = 2.0f;
        private float BlinkOffPeriod = 0.5f;
        #endregion

        #region Public Functions
        public InstructionScreen(BlackNectarGame game)
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
            energyTimer = new PeriodicTimer();
            energyTimer.OnChange += () =>
                {
                    energyColor = energyColor == Palette.Energy1 ? Palette.Energy2 : Palette.Energy1;
                };
        }

        public void Reset()
        {
            blinkTimer.Reset();
            blinkTimer.Period = BlinkOnPeriod;
            blink = false;
            energyTimer.Reset();
            energyTimer.Period = 0.2f;
            energyColor = Palette.Energy1;
            // Buffer old input to 'debounce' transitions between states (i.e.
            // only change when input goes from not pressed to pressed)
            oldInputState = -1;
        }

        public void Update(TimeSpan t)
        {
            // Update timers
            blinkTimer.Update(t);
            energyTimer.Update(t);

            // Render instructions
            game.Screen.Clear(Palette.ClearFill);
            Fonts.HeavyData.Render("INSTRUCTIONS", 50, 30, 48, Palette.Energy1, game.Screen);
            Fonts.HeavyData.Render("MOVE LEFT AND RIGHT WITH ARROW KEYS", 60, 120, 30, Palette.Energy1, game.Screen);
            Fonts.HeavyData.Render("AVOID ASTEROIDS", 60, 170, 30, Palette.Energy1, game.Screen);
            game.Screen.FillRectangleFixed(300, 172, 300 + 30, 172 + 30, Palette.Asteroid);
            Fonts.HeavyData.Render("COLLECT ENERGY TO SURVIVE", 60, 220, 30, Palette.Energy1, game.Screen);
            game.Screen.FillRectangleFixed(450, 226, 450 + Energy.PelletSize, 226 + Energy.PelletSize, energyColor);
            if (!blink)
            {
                Fonts.HeavyData.Render("CLICK OR PRESS KEY TO PLAY", 120, 390, 30, Palette.Energy1, game.Screen);
            }
            game.Screen.Invalidate();

            // Check for key presses and change to game
            KeyboardState keys = Keyboard.GetState();
            if ((keys.GetPressedKeys().Length > 0 || MouseFixed.LeftButton) && oldInputState == 0)
            {
                game.ChangeToGameScreen();
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
