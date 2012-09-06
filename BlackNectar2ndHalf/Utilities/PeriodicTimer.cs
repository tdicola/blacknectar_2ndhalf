using System;

namespace BlackNectar2ndHalf
{
    public class PeriodicTimer
    {
        #region Public Members
        public float Period { get; set; }
        public event Action OnChange;
        #endregion

        #region Private Members
        private float time = 0;
        #endregion

        #region Public Functions
        public void Update(TimeSpan t)
        {
            time += (float)t.TotalSeconds;
            while (time >= Period)
            {
                time -= Period;
                if (OnChange != null)
                {
                    OnChange();
                }
            }
        }

        public void Reset()
        {
            time = 0;
        }
        #endregion
    }
}
