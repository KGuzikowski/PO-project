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
    public class MyKeyboard
    {
        KeyboardState newKeyboard, oldKeyboard;

        public void Update()
        {
            newKeyboard = Keyboard.GetState();
        }

        public void UpdateOld()
        {
            oldKeyboard = newKeyboard;
        }
    }
}
