using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace BlackNectar2ndHalf
{
    public class Font
    {
        #region Private Members
        private TextBlock textSource;
        private TranslateTransform textTranslate;
        #endregion

        #region Public Functions
        public Font(string path, string name)
        {
            textSource = new TextBlock();
            textSource.FontSource = new FontSource(Application.GetResourceStream(new Uri(path, UriKind.Relative)).Stream);
            textSource.FontFamily = new FontFamily(name);
            textSource.Foreground = new SolidColorBrush();
            textTranslate = new TranslateTransform();
        }

        public void Render(string text, int x, int y, int size, Color foreground, WriteableBitmap screen)
        {
            textSource.Text = text;
            textSource.FontSize = size;
            (textSource.Foreground as SolidColorBrush).Color = foreground;
            textTranslate.X = x;
            textTranslate.Y = y;
            screen.Render(textSource, textTranslate);
        }
        #endregion
    }
}
