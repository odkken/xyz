using System;

namespace XYZ
{
    public class Pencil
    {
        public bool Drawing { get; private set; }
        public event Action OnDrawBegin;
        public void EnableDraw(bool enable)
        {
            Drawing = enable;
            if (Drawing)
                OnDrawBegin?.Invoke();
        }
    }
}