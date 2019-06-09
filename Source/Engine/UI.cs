#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
#endregion

namespace StudentSurvival
{
    public class UI
    {
        private SpriteFont font;

        public UI()
        {
            font = Globals.content.Load<SpriteFont>("Fonts\\Arial16");
        }

        public void Update()
        {

        }

        public void Draw()
        {
            string tempStr = "Hero: " + Globals.Hero.Health;
            Vector2 strDims = font.MeasureString(tempStr);
            float x = Globals.spriteBatch.GraphicsDevice.Viewport.Width / 2 - strDims.X / 2;
            float y = 50 + strDims.Y / 2;

            Globals.spriteBatch.DrawString(font, tempStr, new Vector2(x, y), Color.Black);
        }
    }
}
