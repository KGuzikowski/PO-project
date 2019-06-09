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
    public class MyTimer
    {
        private int mSec;
        private int delSec;
        private TimeSpan timer = new TimeSpan();
        private TimeSpan delay = new TimeSpan();

        public MyTimer(int m, int del)
        {
            mSec = m;
            delSec = del;
        }

        public void UpdateTimer(GameTime gameTime)
        {
            if(delay.TotalMilliseconds >= delSec)
                timer += gameTime.ElapsedGameTime;
            else delay += gameTime.ElapsedGameTime;
        }

        private void Reset()
        {
            timer = TimeSpan.Zero;
            delay = TimeSpan.Zero;
        }

        public bool ItsTime()
        {
            if (timer.TotalMilliseconds >= mSec)
            {
                Reset();
                return true;
            }
            else return false;
        }
    }
}
