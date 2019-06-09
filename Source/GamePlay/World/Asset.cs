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
    public class Asset : Basic2d
    {
        //Type = 1 : gives health
        //Type = 2 : takes health
        private int Type;

        private int ActionStrength;

        Asset(string path, int X, int Y, int TimesBigger, int type, int actionStrength)
        : base(path, X, Y, TimesBigger)
        {
            Type = type;
            ActionStrength = actionStrength;
        }

        public void Execute()
        {
            switch (Type)
            {
                case 1:
                    Globals.Hero.Health += ActionStrength;
                    break;
                case 2:
                    Globals.Hero.Health -= ActionStrength;
                    break;
            }
        }
    }
}
