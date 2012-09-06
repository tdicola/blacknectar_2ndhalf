using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework;

namespace BlackNectar2ndHalf
{
    public class Asteroid
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public int Size { get; set; }
    }

    public class Asteroids
    {
        private IGame game;
        public List<Asteroid> asteroids { get; set; }

        private const int MaxAsteroids = 15;
        private const int AsteroidSize = 30;
        private const int MinAsteroidSpeed = 30;
        private const int MaxAsteroidSpeed = 100;
        private const float InitialTimeToNext = 2;
        private const int MinAsteroidSize = 20;
        private const int MaxAsteroidSize = 40;

        private float timeToNext;
        private float delay;

        private Random rand;

        public void Initialize(IGame game)
        {
            this.game = game;
            rand = new Random();
            timeToNext = InitialTimeToNext;
            delay = timeToNext;

            asteroids = new List<Asteroid>();
        }

        public void Update(TimeSpan t)
        {
            // Move each asteroid
            List<Asteroid> delete = new List<Asteroid>();
            foreach (var asteroid in asteroids)
            {
                asteroid.Position = Vector2.Add(asteroid.Position, Vector2.Multiply(asteroid.Velocity, (float)t.TotalSeconds));

                // Delete asteroids outside bounds
                if (asteroid.Position.X < -asteroid.Size || asteroid.Position.X > game.Width - 1 ||
                    asteroid.Position.Y > game.Height - 1)
                {
                    delete.Add(asteroid);
                }
            }

            foreach (var asteroid in delete)
            {
                asteroids.Remove(asteroid);
            }

            // Add new asteroids after random amount of time
            timeToNext -= (float)t.TotalSeconds;
            if (timeToNext <= 0)
            {
                timeToNext = delay;
                delay -= (1.0f / 240.0f);
                if (delay < 1.0f)
                {
                    delay = 1.0f;
                }
                addRandomAsteroid();
            }
        }

        private void addRandomAsteroid()
        {
            if (asteroids.Count < MaxAsteroids)
            {
                // Pick a side to start from (left, top, right)
                // Pick a location to target (random pixel on bottom), consider for some sides (left, right) really close
                // spots aren't ideal

                int sxmin, sxmax, symin, symax;
                int txmin, txmax;

                int size = rand.Next(MinAsteroidSize, MaxAsteroidSize + 1);

                switch (rand.Next(3))
                {
                    case 0: // left
                        sxmin = -size;
                        sxmax = -size;
                        symin = 0;
                        symax = game.Height - 1 - (game.Height / 3);
                        txmin = game.Width / 6;
                        txmax = game.Width - 1;
                        break;
                    case 1: // top
                        sxmin = -size;
                        sxmax = game.Width - 1;
                        symin = 0;
                        symax = 0;
                        txmin = 0;
                        txmax = game.Width - 1;
                        break;
                    default: // right
                        sxmin = game.Width - 1;
                        sxmax = game.Width - 1;
                        symin = 0;
                        symax = game.Height - 1 - (game.Height / 6);
                        txmin = 0;
                        txmax = game.Width - 1 - (game.Width / 6);
                        break;
                }

                Asteroid asteroid = new Asteroid();
                asteroid.Size = size;
                asteroid.Position = new Vector2(rand.Next(sxmin, sxmax + 1), rand.Next(symin, symax + 1));
                asteroid.Velocity = Vector2.Normalize(Vector2.Subtract(new Vector2(rand.Next(txmin, txmax + 1), game.Height), asteroid.Position));

                float speed = MinAsteroidSpeed + ((float)rand.NextDouble() * (MaxAsteroidSpeed - MinAsteroidSpeed));

                asteroid.Velocity = Vector2.Multiply(asteroid.Velocity, speed);

                asteroids.Add(asteroid);
            }
        }

        public void Render(WriteableBitmap screen)
        {
            foreach (var asteroid in asteroids)
            {
                screen.FillRectangleFixed((int)asteroid.Position.X, (int)asteroid.Position.Y, (int)asteroid.Position.X + asteroid.Size, (int)asteroid.Position.Y + asteroid.Size, Palette.Asteroid);
            }
        }
    }
}
