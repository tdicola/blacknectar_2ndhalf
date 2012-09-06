using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace BlackNectar2ndHalf
{
    public class Player
    {
        #region Private Members
        private float X { get; set; }
        private float XVelocity { get; set; }
        private int Y { get; set; }
        private int EnergyBarHeight { get; set; }
        private int EnergyBarLeft { get; set; }
        private int EnergyBarBottom { get; set; }
        private int EnergyBarRight { get; set; }
        private BlackNectarGame game;
        private GameScreen gameScreen;
        private PeriodicTimer barBlinkTimer;
        private bool barBlink;
        #endregion

        #region Public Members
        public float Energy { get; private set; }
        #endregion

        #region Constants
        public const int PlayerHeight = 40;
        public const int PlayerWidth = 20;
        public const int PlayerMargin = 10;
        public const float PlayerSpeed = 300f;
        public const int EnergyBarWidth = 20;
        public const int EnergyBarMargin = 10;
        public const float EnergyMax = 100.0f;
        public const float EnergyDecay = -3.0f;
        #endregion

        #region Public Functions
        public void Initialize(BlackNectarGame game, GameScreen gameScreen)
        {
            this.game = game;
            this.gameScreen = gameScreen;

            Y = game.Height - 1 - PlayerHeight - PlayerMargin;
            X = (game.Width / 2.0f) - (PlayerWidth / 2.0f);
            XVelocity = 0;

            Energy = EnergyMax;
            EnergyBarHeight = game.Height - (2 * EnergyBarMargin) - PlayerHeight - PlayerMargin;
            EnergyBarLeft = game.Width - 1 - EnergyBarWidth - EnergyBarMargin;
            EnergyBarRight = EnergyBarLeft + EnergyBarWidth;
            EnergyBarBottom = game.Height - 1 - PlayerMargin - PlayerHeight - EnergyBarMargin;

            barBlink = false;
            barBlinkTimer = new PeriodicTimer();
            barBlinkTimer.Period = 0.4f;
            barBlinkTimer.OnChange += () =>
                {
                    barBlink = !barBlink;
                };
        }

        public void Update(TimeSpan t)
        {
            // Update position from input
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Left) && keys.IsKeyUp(Keys.Right))
            {
                XVelocity = -PlayerSpeed;
            }
            else if (keys.IsKeyDown(Keys.Right) && keys.IsKeyUp(Keys.Left))
            {
                XVelocity = PlayerSpeed;
            }
            else
            {
                XVelocity = 0;
            }
            X += XVelocity * (float)t.TotalSeconds;

            // Warp around edges
            if (X < -PlayerWidth)
            {
                X = game.Width - 1;
            }
            else if (X > game.Width - 1)
            {
                X = -PlayerWidth + 1;
            }

            // Update energy
            Energy += EnergyDecay * (float)t.TotalSeconds;
            if (Energy <= 0)
            {
                Energy = 0;
            }
            
            // Update timers
            barBlinkTimer.Update(t);
        }

        public void HandleCollisions(TimeSpan t)
        {
            Rectangle playerBound = new Rectangle((int)X, Y, PlayerWidth, PlayerHeight);

            // Check for energy collisions
            List<EnergyPellet> deadPellets = new List<EnergyPellet>();
            foreach (var pellet in gameScreen.Energy.Pellets)
            {
                Rectangle pelletBound = new Rectangle((int)pellet.Position.X, (int)pellet.Position.Y, BlackNectar2ndHalf.Energy.PelletSize, BlackNectar2ndHalf.Energy.PelletSize);
                if (playerBound.Intersects(pelletBound))
                {
                    deadPellets.Add(pellet);
                    Energy = EnergyMax;
                }
            }
            foreach (var pellet in deadPellets)
            {
                gameScreen.Energy.Pellets.Remove(pellet);
            }

            // Check for asteroid collisions
            foreach (var asteroid in gameScreen.Asteroids.asteroids)
            {
                Rectangle bound = new Rectangle((int)asteroid.Position.X, (int)asteroid.Position.Y, asteroid.Size, asteroid.Size);
                if (playerBound.Intersects(bound))
                {
                    Energy = 0;
                }
            }
        }

        public void RenderPlayer(WriteableBitmap screen)
        {
            screen.FillRectangleFixed((int)X, Y, (int)X + (PlayerWidth-1), Y + (PlayerHeight-1), Palette.Player);
        }

        public void RenderEnergyBar(WriteableBitmap screen)
        {
            if (Energy > 0)
            {
                int y1 = EnergyBarBottom - (int)((Energy / EnergyMax) * (float)EnergyBarHeight);
                if (Energy > 50.0f || barBlink)
                {
                    screen.FillRectangleFixed(EnergyBarLeft, y1, EnergyBarRight, EnergyBarBottom, Palette.Energy1);
                }
            }
        }
        #endregion
    }
}
