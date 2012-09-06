using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BlackNectar2ndHalf
{
    public class GameScreen: IScreen
    {
        #region Private Members
        private BlackNectarGame game;
        private StarField starField;
        private TimeSpan survival;
        private TimeSpan dying;
        private Rectangle fadeRect;
        private TranslateTransform fadeTransform;
        #endregion

        #region Public Members
        public GameStateType State;
        public enum GameStateType { Playing, Dying };
        public Player Player { get; private set; }
        public Energy Energy { get; private set; }
        public Asteroids Asteroids { get; private set; }
        #endregion

        #region Constants
        public float DyingDuration = 3.0f;
        #endregion

        #region Public Functions
        public GameScreen(BlackNectarGame game)
        {
            this.game = game;
            starField = new StarField();
            starField.Initialize(game, StarField.StarCount);
            fadeRect = new Rectangle();
            fadeRect.Fill = new SolidColorBrush(Colors.Black);
            fadeRect.Width = game.Width;
            fadeRect.Height = game.Height;
            fadeRect.Opacity = 0.0f;
            fadeTransform = new TranslateTransform();
            fadeTransform.X = 0;
            fadeTransform.Y = 0;
        }

        public void Reset()
        {
            survival = new TimeSpan();
            dying = new TimeSpan();
            Player = new Player();
            Player.Initialize(game, this);
            Energy = new Energy();
            Energy.Initialize(game);
            Asteroids = new Asteroids();
            Asteroids.Initialize(game);
            State = GameStateType.Playing;
        }

        public void Update(TimeSpan t)
        {
            // Update world
            starField.Pan(0, StarField.StarSpeed, t);
            if (State == GameStateType.Playing)
            {
                Player.Update(t);
            }
            Energy.Update(t);
            Asteroids.Update(t);

            if (State == GameStateType.Playing)
            {
                Player.HandleCollisions(t);
                survival += t;
            }
            else
            {
                dying += t;
                if (dying.TotalSeconds > DyingDuration)
                {
                    game.ChangeToGameOverScreen(survival);
                }
            }

            if (Player.Energy <= 0.0f)
            {
                State = GameStateType.Dying;
            }

            // Render world
            game.Screen.Clear(Palette.ClearFill);
            starField.Render(game.Screen);
            Energy.Render(game.Screen);
            Player.RenderPlayer(game.Screen);
            Asteroids.Render(game.Screen);
            Player.RenderEnergyBar(game.Screen);
            Fonts.HeavyData.Render(String.Format("SURVIVAL: {0:D2}:{1:D2}", survival.Minutes, survival.Seconds), 10, 5, 22, Palette.Energy1, game.Screen);

            // Fade out while dying
            if (State == GameStateType.Dying)
            {
                fadeRect.Opacity = dying.TotalSeconds / (double)DyingDuration;
                game.Screen.Render(fadeRect, fadeTransform);
            }

            game.Screen.Invalidate();
        }
        #endregion
    }
}
