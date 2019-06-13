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
    public class Basic2d
    {
        protected Texture2D BasicModel;
        public Rectangle BoundingBox;
        protected float TimesBigger;

        public Basic2d(string path, int X, int Y, float TimesBigger)
        {
            BasicModel = Globals.content.Load<Texture2D>(path);
            BoundingBox = new Rectangle(X, Y, 
                                        (int)(BasicModel.Width * TimesBigger), 
                                        (int)(BasicModel.Height * TimesBigger)
                                        );
            this.TimesBigger = TimesBigger;
        }

        public virtual void Draw()
        {
            if (BasicModel != null)
            {
                Globals.spriteBatch.Draw(
                    BasicModel,
                    BoundingBox,
                    null,
                    Color.White,
                    0f,
                    new Vector2(0, 0),
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}