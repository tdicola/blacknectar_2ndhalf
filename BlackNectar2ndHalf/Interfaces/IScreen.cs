using System;

namespace BlackNectar2ndHalf
{
    public interface IScreen
    {
        void Reset();
        void Update(TimeSpan t);
    }
}
