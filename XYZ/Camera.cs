using SFML.Graphics;
using SFML.System;

namespace XYZ
{
    class Camera : Drawable
    {
        private RectangleShape _border;
        public View View;

        public Camera(float width, float height)
        {
            _border = new RectangleShape(new Vector2f(width, height))
            {
                Origin = new Vector2f(width / 2f, height / 2f),
                FillColor = Color.Transparent,
                OutlineColor = Color.White,
                OutlineThickness = 1
            };
            View = new View(new Vector2f(), new Vector2f(width, height));
        }
        public void UpdateViewport(FloatRect viewport)
        {
            View.Viewport = viewport;
        }
        public void SetPosition(Vector2f pos)
        {
            View.Center = pos;
            _border.Position = pos;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _border.Draw(target, states);
        }
    }
}
