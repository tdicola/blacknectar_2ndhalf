using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework;

namespace BlackNectar2ndHalf
{
    public class EnergyPellet
    {
        public Vector2 Position { get; set; }
        public float YVelocity { get; set; }
    }

    public class Energy
    {
        #region Constants
        public const int PelletSize = 25;
        public const int MaxPellets = 5;
        public const float MinVelocity = 40.0f;
        public const float MaxVelocity = MinVelocity * 4;
        public const float PelletTimeIncrement = 1f;
        public const float InitialTimeToNextPellet = 8f;
        public const float EnergyColorFrequency = 0.2f;
        #endregion

        #region Private Members
        private float nextPellet;
        private float timeToNextPellet;
        private Random rand;
        private Color EnergyColor { get; set; }
        private float colorTime;
        private IGame game;
        #endregion

        #region Public Members
        public List<EnergyPellet> Pellets { get; private set; }
        #endregion

        #region Public Functions
        public void Initialize(IGame game)
        {
            this.game = game;
            rand = new Random();
            Pellets = new List<EnergyPellet>();
            timeToNextPellet = InitialTimeToNextPellet;
            nextPellet = timeToNextPellet;
            EnergyColor = Palette.Energy1;
            colorTime = 0f;
        }

        public void Update(TimeSpan t)
        {
            // Add new pellets
            nextPellet -= (float)t.TotalSeconds;
            if (nextPellet <= 0)
            {
                addPellet();
                nextPellet = timeToNextPellet;
                timeToNextPellet += 0.3f;
            }

            // Move pellets
            List<EnergyPellet> deadPellets = new List<EnergyPellet>();
            foreach (var pellet in Pellets)
            {
                Vector2 newPosition = new Vector2(pellet.Position.X, pellet.Position.Y + pellet.YVelocity * (float)t.TotalSeconds);
                pellet.Position = newPosition;
                if (newPosition.Y > (game.Height - 1))
                {
                    deadPellets.Add(pellet);
                }
            }

            // Remove dead pellets
            foreach (var pellet in deadPellets)
            {
                Pellets.Remove(pellet);
            }

            // Animate energy color
            colorTime += (float)t.TotalSeconds;
            if (colorTime > EnergyColorFrequency)
            {
                colorTime = 0;
                EnergyColor = (EnergyColor == Palette.Energy1) ? Palette.Energy2 : Palette.Energy1;
            }
        }

        private void addPellet()
        {
            if (Pellets.Count <= MaxPellets)
            {
                EnergyPellet newPellet = new EnergyPellet();
                newPellet.Position = new Vector2(rand.Next(0, game.Width - PelletSize), -PelletSize);
                newPellet.YVelocity = MinVelocity + ((MaxVelocity - MinVelocity) * (float)rand.NextDouble());
                Pellets.Add(newPellet);
            }
        }

        public void Render(WriteableBitmap screen)
        {
            foreach (var pellet in Pellets)
            {
                int left = (int)pellet.Position.X;
                int right = (int)(pellet.Position.X + (PelletSize-1));
                int top = (int)pellet.Position.Y;
                int bottom = (int)(pellet.Position.Y + (PelletSize-1));
                screen.FillRectangleFixed(left, top, right, bottom, EnergyColor);
            }
        }
        #endregion
    }
}
