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
    public class Globals
    {
        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static KeyboardState NewState;
        public static KeyboardState OldState;
        public static List<Basic2d> GameAssets;
        public static List<Basic2d> BackgroundElems;
        public static List<Basic2d> BackgroundAssets;
        public static List<Asset> InteractiveAssets;
        public static List<Enemy> EnemiesAssets;
        public static List<Basic2d> TopBar;
        public static Hero Hero;
    }
}
