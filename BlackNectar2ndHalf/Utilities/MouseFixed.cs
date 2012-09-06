using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BlackNectar2ndHalf
{
    public static class MouseFixed
    {
        public static bool LeftButton { get; private set; }

        static MouseFixed()
        {
            LeftButton = false;
        }

        public static void LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LeftButton = true;
        }

        public static void LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LeftButton = false;
        }

        public static void MouseLeave(object sender, MouseEventArgs e)
        {
            // If mouse leaves the area we can't trust we'll get mouse click events, just
            // assume there are none.
            LeftButton = false;
        }
    }
}
