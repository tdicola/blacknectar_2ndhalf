using System;
using System.Windows.Media.Imaging;

namespace BlackNectar2ndHalf
{
    public interface IGame
    {
        void Initialize();
        void Update(TimeSpan t);
        int Width { get; }
        int Height { get; }
        WriteableBitmap Screen { get; }
    }
}
