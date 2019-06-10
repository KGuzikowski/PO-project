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
    public class Enemy : Character
    {
        //Random random;
        private int MoveRange;
        private int PixesMoved;

        // true means left, false means right
        private bool Direction;

        private bool ReadyForFight;

        public int Type;

        public Enemy(string path, int type, int X, int Y, float vel, int run, int attack, float TimesBigger, int range, int health, int strength)
        : base(path, X, Y, vel, run, attack, TimesBigger, health, strength)
        {
            MoveRange = range;
            PixesMoved = 0;
            Direction = false;
            ReadyForFight = false;
            Type = type;
        }

        public override void Update()
        {
            if (!Alive) return;
            if (ReadyForFight)
            {
                if (!HeroAllert(BoundingBox.Width))
                {
                    ReadyForFight = false;
                    AttackInProgress = false;
                }
                else if(HeroAllert(BoundingBox.Width/2))
                {
                    AttackInProgress = true;
                }
            }
            else
            {
                if (HeroAllert(BoundingBox.Width)) GetReady();
                else Move();
            }
            if (Health <= 0) AddPoints();
            base.Update();
        }

        protected override void HitEnemy()
        {
            if (HeroAllert(BoundingBox.Width / 2)) Globals.Hero.Health -= Strength;
        }

        private bool HeroAllert(int distance)
        {
            int spaceX = BoundingBox.Center.X - Globals.Hero.BoundingBox.Center.X;
            int spaceY = Math.Abs(BoundingBox.Center.Y - Globals.Hero.BoundingBox.Center.Y);

            return Globals.Hero.Alive && 
                   Math.Abs(spaceX) <= distance && spaceY <= BoundingBox.Height;
        }

        private void GetReady()
        {
            ReadyForFight = true;
            TempModel = BasicModel;

            int spaceX = BoundingBox.Center.X - Globals.Hero.BoundingBox.Center.X;
            int spaceY = Math.Abs(BoundingBox.Center.Y - Globals.Hero.BoundingBox.Center.Y);

            if (spaceX > 0)
             {
                SpriteDirection = 0;
                Direction = true;
            }
            else
            {
                SpriteDirection = 1;
                Direction = false;
            }
        }

        public void Move()
        {
            int start, finish;
            if(PixesMoved >= MoveRange)
            {
                Direction = !Direction;
                PixesMoved = 0;
            }

            if (Direction)
            {
                start = BoundingBox.Left;
                MoveLeft();
                finish = BoundingBox.Left;
                PixesMoved += start - finish;
            }
            else
            {
                start = BoundingBox.Right;
                MoveRight();
                finish = BoundingBox.Right;
                PixesMoved += finish - start;
            }
        }

        protected override bool Collides(Basic2d asset)
        {
            Rectangle box = BoundingBox;
            box.Y -= box.Height;
            if (box.Intersects(asset.BoundingBox))
            {
                Direction = !Direction;
                PixesMoved = 0;
                return true;
            }
            else return false;
        }

        private void AddPoints()
        {
            switch (Type)
            {
                case 1:
                    Globals.Hero.Points += 50;
                    break;
                case 2:
                    Globals.Hero.Points += 100;
                    break;
                case 3:
                    Globals.Hero.Points += 150;
                    break;
                case 4:
                    Globals.Hero.Points += 200;
                    break;
            }
        }
    }
}
