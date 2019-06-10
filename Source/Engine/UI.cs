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
        private int PositionX;
        private int WinX;

        public UI()
        {
            font = Globals.content.Load<SpriteFont>("Fonts\\Arial16");
            WinX = Globals.spriteBatch.GraphicsDevice.Viewport.Width;
            PositionX = Globals.Hero.BoundingBox.Left - WinX / 2;

            Globals.TopBar = new List<Basic2d>();
            int i = 0;
            int x = PositionX;
            while (i < 10)
            {
                Globals.TopBar.Add(new Basic2d("2D\\top_bar", x, 0, 2.00f));
                x += 75 * 2;
                i++;
            }
        }

        public void Update()
        {
            PositionX = Globals.Hero.BoundingBox.Left - Globals.spriteBatch.GraphicsDevice.Viewport.Width / 2;

            int x = PositionX;
            foreach (Basic2d bar in Globals.TopBar)
            {
                bar.BoundingBox.X = x;
                x += 75 * 2;
            }
        }

        public void Draw()
        {
            string Health = "Health: " + Globals.Hero.Health;
            Vector2 HealthDims = font.MeasureString(Health);
            float HealthX = PositionX + HealthDims.X / 2;
            float HealthY = HealthDims.Y / 2;

            string Points = "Points: " + Globals.Hero.Points;
            Vector2 PointshDims = font.MeasureString(Points);
            float PointsX = HealthX + HealthDims.X / 2 + HealthDims.X;
            float PointsY = HealthDims.Y / 2;

            string Title = "CHYBA BANGLA";
            Vector2 TitleDims = font.MeasureString(Title);
            float TitleX = PositionX + WinX - HealthDims.X * 2f - 30;
            float TitleY = HealthDims.Y / 2;

            foreach (Basic2d bar in Globals.TopBar)
                bar.Draw();

            Globals.spriteBatch.DrawString(font, Title, new Vector2(TitleX, TitleY), Color.White);
            Globals.spriteBatch.DrawString(font, Health, new Vector2(HealthX, HealthY), Color.White);
            Globals.spriteBatch.DrawString(font, Points, new Vector2(PointsX, PointsY), Color.White);
        }
    }
}
