using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework;


namespace BlackNectar2ndHalf
{
    public class StarField
    {
        #region Members
        private List<Vector2> stars1;
        private List<Vector2> stars2;
        private List<Vector2> stars3;
        private Random rand;
        private IGame game;
        #endregion

        #region Constants
        public const int StarCount = 150;
        public const float StarSpeed = 10f;
        private const int Star2Divisor = 2;
        private const int Star3Divisor = 10;
        private const float Star2Speed = 2f;
        private const float Star3Speed = 3f;
        private const int Star1Size = 1;
        private const int Star2Size = 2;
        private const int Star3Size = 3;
        #endregion

        public StarField()
        {
        }

        public void Initialize(IGame game, int starCount)
        {
            rand = new Random();
            this.game = game;

            int starCount1 = starCount;
            int starCount2 = starCount / Star2Divisor;
            int starCount3 = starCount / Star3Divisor;

            stars1 = new List<Vector2>(starCount1);
            stars2 = new List<Vector2>(starCount2);
            stars3 = new List<Vector2>(starCount3);

            for (int i = 0; i < starCount1; ++i)
            {
                int x = rand.Next(game.Width);
                int y = rand.Next(game.Height);
                stars1.Add(new Vector2(x, y));
            }

            for (int i = 0; i < starCount2; ++i)
            {
                int x = rand.Next(game.Width);
                int y = rand.Next(game.Height);
                stars2.Add(new Vector2(x, y));
            }

            for (int i = 0; i < starCount3; ++i)
            {
                int x = rand.Next(game.Width);
                int y = rand.Next(game.Height);
                stars3.Add(new Vector2(x, y));
            }
        }

        public void Pan(float xSpeed, float ySpeed, TimeSpan t)
        {
            Pan(stars1, xSpeed, ySpeed, t);
            Pan(stars2, xSpeed * Star2Speed, ySpeed * Star2Speed, t);
            Pan(stars3, xSpeed * Star3Speed, ySpeed * Star3Speed, t);
        }

        public void Pan(List<Vector2> stars, float xSpeed, float ySpeed, TimeSpan t)
        {
            float xDelta = xSpeed * (float)t.TotalSeconds;
            float yDelta = ySpeed * (float)t.TotalSeconds;

            for (int i = 0; i < stars.Count; ++i)
            {
                Vector2 star = stars[i];
                star.X = star.X + xDelta;
                star.Y = star.Y + yDelta;

                if (star.X > (game.Width-1))
                {
                    star.X = star.X % (game.Width-1);
                }
                if (star.Y > (game.Height-1))
                {
                    star.Y = star.Y % (game.Height-1);
                }

                stars[i] = star;
            }
        }

        public void Render(WriteableBitmap screen)
        {
            Render(stars1, Palette.Star1, Star1Size, screen);
            Render(stars2, Palette.Star2, Star2Size, screen);
            Render(stars3, Palette.Star3, Star3Size, screen);
        }

        public void Render(List<Vector2> stars, Color color, int size, WriteableBitmap screen)
        {
            if (size <= 1)
            {
                foreach (var star in stars)
                {
                    // TODO: Outside bounds of array error can occur here! Float accuracy adding up?
                    screen.SetPixel((int)star.X, (int)star.Y, color);
                }
            }
            else
            {
                foreach (var star in stars)
                {
                    screen.FillRectangleFixed((int)star.X, (int)star.Y, (int)star.X + size, (int)star.Y + size, color);
                }
            }
        }
    }
}
