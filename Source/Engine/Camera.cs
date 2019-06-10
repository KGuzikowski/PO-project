using Microsoft.Xna.Framework;

namespace StudentSurvival
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(Hero target)
        {
            Matrix position = position = Matrix.CreateTranslation(
                -target.BoundingBox.Left,
                0, 0);

            Matrix offset = Matrix.CreateTranslation(
                Globals.spriteBatch.GraphicsDevice.Viewport.Width / 2,
                0, 0);

            Transform = position * offset;
        }
    }
}
