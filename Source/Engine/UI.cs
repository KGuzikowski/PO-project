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
        private SpriteFont EndFont;
        private int PositionX;
        private int WinX;
        GraphicsDevice GraphicsDevice;
        int WorldX;

        public UI(GraphicsDevice graphics, int WorldX)
        {
            this.WorldX = WorldX;
            font = Globals.content.Load<SpriteFont>("Fonts\\Arial16");
            EndFont = Globals.content.Load<SpriteFont>("Fonts\\GameOver");
            WinX = Globals.spriteBatch.GraphicsDevice.Viewport.Width;
            PositionX = Globals.Hero.BoundingBox.Left - WinX / 2;
            GraphicsDevice = graphics;

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

            int WinY = Globals.spriteBatch.GraphicsDevice.Viewport.Height;
            int RectW = 800;
            int RectH = 400;
            int RectX = PositionX + WinX / 2 - RectW / 2;
            int RectY = WinY / 2 - RectH / 2 + Globals.TopBar[0].BoundingBox.Height / 2;
            Rectangle Rect = new Rectangle(RectX, RectY, RectW, RectH);

            if(Globals.Hero.Win)
            {
                string GameOver = "YOU WON";

                DrawStringToRect(EndFont, GameOver, Rect);
                GraphicsDevice.Clear(Color.DarkGray);
            }
            if (!Globals.Hero.Alive && !Globals.Hero.Win)
            {
                string GameOver = "GAME OVER";

                DrawStringToRect(EndFont, GameOver, Rect);
                GraphicsDevice.Clear(Color.DarkGray);
            }
        }

        public void DrawStringToRect(SpriteFont font, string strToDraw, Rectangle boundaries)
        {
            Vector2 size = font.MeasureString(strToDraw);

            float xScale = (boundaries.Width / size.X);
            float yScale = (boundaries.Height / size.Y);
            
            float scale = Math.Min(xScale, yScale);
            
            int strWidth = (int)Math.Round(size.X * scale);
            int strHeight = (int)Math.Round(size.Y * scale);
            Vector2 position = new Vector2();
            position.X = (((boundaries.Width - strWidth) / 2) + boundaries.X);
            position.Y = (((boundaries.Height - strHeight) / 2) + boundaries.Y);
            
            float rotation = 0.0f;
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f;
            SpriteEffects spriteEffects = new SpriteEffects();
            Globals.spriteBatch.DrawString(font, strToDraw, position, Color.White, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }
    }
}

