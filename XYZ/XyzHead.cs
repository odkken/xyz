using SFML.Graphics;
using SFML.System;
using System;
using static XYZ.StaticUtil;

namespace XYZ
{
    public class XyzHead
    {
        public XyzHead(int width, int height, int depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }
        readonly float speed = 20f;
        private float xDest;
        private float yDest;
        private float zDest;
        private readonly int width;
        private readonly int height;
        private readonly int depth;

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
        object IsMovingLock = new object();
        private bool isMoving;

        public bool IsMoving
        {
            get
            {
                lock (IsMovingLock)
                    return isMoving;
            }

            private set
            {
                isMoving = value;
            }
        }
        public View View { get; }

        public event Action OnStep;

        public void Update(float dt)
        {
            lock (IsMovingLock)
            {
                IsMoving = false;
                if (xDest != X)
                {
                    X += speed * dt * (xDest > X ? 1 : -1);
                    IsMoving = true;
                }
                if (yDest != Y)
                {
                    Y += speed * dt * (yDest > Y ? 1 : -1);
                    IsMoving = true;
                }
                if (zDest != Z)
                {
                    Z += speed * dt * (zDest > Z ? 1 : -1);
                    IsMoving = true;
                }
            }
        }

        public void MoveX(float steps)
        {
            xDest += steps;
            xDest = xDest.Clamp(0, width - 1);
        }
        public void MoveY(float steps)
        {
            yDest += steps;
            yDest = xDest.Clamp(0, height - 1);
        }
        public void MoveZ(float steps)
        {
            zDest += steps;
            zDest = zDest.Clamp(0, depth - 1);
        }
    }
}