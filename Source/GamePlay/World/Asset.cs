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
        public int Type;

        private int ActionStrength;
        private bool Done;
        private MyTimer FireTime;
        private bool TimeToHurt;
        private int TimeAgo;

        public Asset(string path, int X, int Y, float TimesBigger, int type, int actionStrength)
        : base(path, X, Y, TimesBigger)
        {
            Type = type;
            ActionStrength = actionStrength;
            Done = false;
            FireTime = new MyTimer(1000, 0);
            TimeToHurt = true;
            TimeAgo = 0;
        }

        public void Execute(GameTime gameTime)
        {
            if(Type == 1 && Done == false)
            {
                Heal();
            }
            else if(Type == 2)
            {
                Hurt(gameTime);
            }
        }

        private void Heal()
        {
            if (Globals.Hero.Health >= Globals.Hero.MaxHealth)
                return;

            Globals.Hero.Health += ActionStrength;
            if (Globals.Hero.Health > Globals.Hero.MaxHealth)
                Globals.Hero.Health = Globals.Hero.MaxHealth;

            Done = true;
            BasicModel = null;
        }

        private void Hurt(GameTime gameTime)
        {
            if (TimeAgo == 3)
            {
                TimeAgo = 0;
                TimeToHurt = true;
            }

            if (TimeToHurt)
            {
                Globals.Hero.Health -= ActionStrength;
                TimeToHurt = false;
            }
            else if (FireTime.ItsTime())
            {
                Globals.Hero.Health -= ActionStrength;
                TimeAgo++;
            }
            else FireTime.UpdateTimer(gameTime);
        }
    }
}
