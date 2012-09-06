using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Reflection;


namespace BlackNectar2ndHalf
{
    public class BlackNectarGame: IGame
    {
        #region Public Members
        public int Width
        {
            get { return 640; }
        }
        public int Height
        {
            get { return 480; }
        }
        public WriteableBitmap Screen { get; private set; }
        public TitleScreen TitleScreen;
        public GameScreen GameScreen;
        public InstructionScreen InstructionScreen;
        public GameOverScreen GameOverScreen;
        #endregion

        #region Private Members
        private IScreen currentScreen;
        public static MediaElement Media { get; set; }
        private bool mediaInitialized;
        #endregion

        #region Public Functions
        public void Initialize()
        {
            mediaInitialized = false;

            Screen = new WriteableBitmap(Width, Height);
            TitleScreen = new TitleScreen(this);
            GameScreen = new GameScreen(this);
            InstructionScreen = new InstructionScreen(this);
            GameOverScreen = new GameOverScreen(this);

            ChangeToTitleScreen();
        }

        public void Update(TimeSpan t)
        {
            if (!mediaInitialized)
            {
                //Media.SetSource(Application.GetResourceStream(new Uri("/BlackNectar2ndHalf;component/BlackNectar2ndHalf128Avg.mp3", UriKind.Relative)).Stream);
                Media.Source = new Uri("http://tonydicola.com/blacknectar/BlackNectar2ndHalf128Avg.mp3", UriKind.Absolute);
                // Loop audio
                Media.MediaEnded += (sender, e) =>
                    {
                        MediaElement me = (MediaElement)e.OriginalSource;
                        me.Stop();
                        me.Play();
                    };
                mediaInitialized = true;
            }

            currentScreen.Update(t);
        }

        public void ChangeToTitleScreen()
        {
            if (mediaInitialized)
            {
                Media.Stop();
            }
            TitleScreen.Reset();
            currentScreen = TitleScreen;
        }

        public void ChangeToGameScreen()
        {
            Media.Play();
            GameScreen.Reset();
            currentScreen = GameScreen;
        }

        public void ChangeToInstructionScreen()
        {
            InstructionScreen.Reset();
            currentScreen = InstructionScreen;
        }

        public void ChangeToGameOverScreen(TimeSpan survival)
        {
            GameOverScreen.Reset(survival);
            currentScreen = GameOverScreen;
        }



        #endregion
    }
}
