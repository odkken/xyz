using SFML.Graphics;
using System;

namespace XYZ
{
    public class Picture
    {
        public Picture(int width, int height, XyzHead head, Pencil pencil)
        {
            _head = head;
            _pencil = pencil;
            Image = new Color[width, height];
        }

        public void Update()
        {
            if (_pencil.Drawing)
            {
                lock (Image)
                    Image[(int)Math.Round(_head.X), (int)Math.Round(_head.Y)] = Color.Black;
            }
        }

        public void Erase()
        {
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    Image[i, j] = Color.White;
                }
            }
        }

        private XyzHead _head;
        private Pencil _pencil;

        public Color[,] Image { get; }
    }
}