using Emgu.CV;
using Emgu.CV.Structure;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace XYZ
{
    static class Program
    {
        static void Main()
        {
            var width = 800;
            var height = 600;
            var cameraWidth = 400;
            var cameraHeight = 300;
            var camViewWidth = cameraWidth * 1f / width;
            var camViewHeight = cameraHeight * 1f / height;
            var head = new XyzHead(width, height, 1);
            var cam = new Camera(cameraWidth, cameraHeight);

            //var pencil = new Pencil();
            //var picture = new Picture(width, height, head, pencil);

            using (var window = new RenderWindow(new VideoMode((uint)width, (uint)height), "xyzsim"))
            {
                window.Closed += (sender, eventArgs) => window.Close();
                window.KeyPressed += (a, b) =>
                {
                    if (b.Code == Keyboard.Key.Escape)
                        window.Close();
                };

                var chip = new RectangleShape(new Vector2f(20, 20))
                {
                    Position = new Vector2f(100, 100),
                    FillColor = Color.Red
                };

                //pencil.EnableDraw(true);

                head.MoveX(chip.Position.X);
                head.MoveY(chip.Position.Y);

                Task.Run(() =>
                {
                    var physicalClock = new Stopwatch();
                    physicalClock.Start();
                    var prevT = physicalClock.Elapsed.TotalSeconds;
                    while (window.IsOpen)
                    {
                        var newT = physicalClock.Elapsed.TotalSeconds;
                        var dt = newT - prevT;
                        prevT = newT;
                        head.Update((float)dt);
                    }
                });

                var cameraRenderTexture = new RenderTexture((uint)cameraWidth, (uint)cameraHeight);

                var camSprite = new Sprite
                {
                    Texture = cameraRenderTexture.Texture
                };

                var camBorder = new RectangleShape(new Vector2f(cameraWidth, cameraHeight))
                {
                    OutlineThickness = 1,
                    OutlineColor = Color.White,
                    FillColor = Color.Transparent
                };

                var processedSprite = new Sprite
                {
                    Texture = new Texture((uint)cameraWidth, (uint)cameraHeight)
                };

                var processedBorder = new RectangleShape(new Vector2f(cameraWidth, cameraHeight))
                {
                    OutlineThickness = 1,
                    OutlineColor = Color.White,
                    FillColor = Color.Transparent
                };

                processedSprite.Position = new Vector2f(cameraWidth, 0);
                processedBorder.Position = new Vector2f(cameraWidth, 0);


                cam.UpdateViewport(new FloatRect(0, 0, 1, 1));
                cameraRenderTexture.SetView(cam.View);

                var cvImVector = new byte[cameraHeight, cameraWidth, 4];
                var cvBufferImage = new Image<Rgba, byte>(new System.Drawing.Size(cameraWidth, cameraHeight));
                while (!head.IsMoving)
                    Thread.Sleep(1);

                while (window.IsOpen)
                {
                    window.DispatchEvents();

                    cam.SetPosition(new Vector2f(head.X, head.Y));

                    cameraRenderTexture.SetView(cam.View);
                    cameraRenderTexture.Clear();
                    cameraRenderTexture.Draw(chip);
                    cameraRenderTexture.Display();

                    var sfImg = cameraRenderTexture.Texture.CopyToImage();
                    cvBufferImage.Bytes = sfImg.Pixels;
                    var blurred = cvBufferImage.Canny(3, 3).Convert<Rgba, byte>();
                    var x = blurred.Bytes;
                    processedSprite.Texture.Update(x);
                    window.Clear();
                    window.Draw(camBorder);
                    window.Draw(camSprite);
                    window.Draw(processedSprite);
                    window.Draw(processedBorder);
                    window.Display();
                }
            }
        }
    }
}
