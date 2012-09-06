using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BlackNectar2ndHalf
{
    public partial class GameControl : UserControl
    {
        private IGame game;

        public GameControl()
        {
            InitializeComponent();
            this.MouseLeave += MouseFixed.MouseLeave;
            this.MouseLeftButtonDown += MouseFixed.LeftButtonDown;
            this.MouseLeftButtonUp += MouseFixed.LeftButtonUp;

            game = ServiceLocator.Resolve<IGame>();
            game.Initialize();

            this.Width = game.Width;
            this.Height = game.Height;
            screen.Source = game.Screen;
        }

        private void GameLoop_Update(object sender, SilverArcade.SilverSprite.SimpleEventArgs<TimeSpan> e)
        {
            // Update game
            game.Update(e.Result);
        }
    }
}
