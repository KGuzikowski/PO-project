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
    public class Character : Basic2d
    {
        protected float velocity;
        protected Texture2D TempModel;

        protected int RunSpriteNum;

        // 1 and looking right
        // 0 and looking left
        protected int SpriteDirection;

        // This gets reset when sprites change
        protected int StepsMovedSinceChange;
        public List<Texture2D> RunImgs;
        private int RunChange;

        public List<Texture2D> AttackImgs;
        protected bool AttackInProgress;
        protected int AttackSpriteNum;
        protected int AttackStepsSinceChange;
        private int AttackChange;

        public int Health;
        public int Strength;

        public bool Alive;
        public List<Texture2D> DieImgs;
        protected int DieChange;
        protected int DieSpriteNum;

        public Character(string path, int X, int Y, float vel, int run, int attack, int die, float TimesBigger, int health, int strength)
        : base(path, X, Y, TimesBigger)
        {
            Alive = true;
            velocity = vel;
            TempModel = BasicModel;
            RunSpriteNum = 0;
            SpriteDirection = 1;
            StepsMovedSinceChange = 0;
            RunImgs = new List<Texture2D>();
            AttackImgs = new List<Texture2D>();
            DieImgs = new List<Texture2D>();
            AttackInProgress = false;
            AttackSpriteNum = 0;
            AttackStepsSinceChange = 0;
            RunChange = run;
            AttackChange = attack;
            DieChange = die;
            DieSpriteNum = 0;
            Health = health;
            Strength = strength;
        }

        public virtual void Update()
        {
            if (!Alive) return;
            if (Health <= 0)
            {
                Health = 0;
                Die();
                return;
            }
            
            if (AttackInProgress)
            {
                Attack();
            }
        }

        public new virtual void Draw()
        {
            if (!Alive) return;

            if (TempModel != null)
            {
                if(SpriteDirection == 1)
                    Globals.spriteBatch.Draw(
                        TempModel,
                        BoundingBox,
                        null,
                        Color.White,
                        0f,
                        new Vector2(0, TempModel.Height),
                        SpriteEffects.None,
                        0f
                    );
                else
                    Globals.spriteBatch.Draw(
                        TempModel,
                        BoundingBox,
                        null,
                        Color.White,
                        0f,
                        new Vector2(0, TempModel.Height),
                        SpriteEffects.FlipHorizontally,
                        0f
                    );

            }
            else if (BasicModel != null)
            {
                if (SpriteDirection == 1)
                    Globals.spriteBatch.Draw(
                        BasicModel,
                        BoundingBox,
                        null,
                        Color.White,
                        0f,
                        new Vector2(0, BasicModel.Height),
                        SpriteEffects.None,
                        0f
                    );
                else
                    Globals.spriteBatch.Draw(
                        BasicModel,
                        BoundingBox,
                        null,
                        Color.White,
                        0f,
                        new Vector2(0, BasicModel.Height),
                        SpriteEffects.FlipHorizontally,
                        0f
                    );
            }
        }

        protected virtual void Die()
        {
            if (StepsMovedSinceChange % DieChange == 0)
            {
                if (DieSpriteNum != DieImgs.Count)
                {
                    TempModel = DieImgs[DieSpriteNum];
                    BoundingBox.Width = (int)(TempModel.Width * TimesBigger);
                    BoundingBox.Height = (int)(TempModel.Height * TimesBigger);
                    DieSpriteNum++;
                }
                else Alive = false;
            }
            StepsMovedSinceChange++;
        }

        protected void Attack()
        {
            if (AttackSpriteNum == AttackImgs.Count)
            {
                AttackSpriteNum = 0;
                AttackInProgress = false;
                TempModel = BasicModel;
                BoundingBox.Width = (int)(TempModel.Width * TimesBigger);
                BoundingBox.Height = (int)(TempModel.Height * TimesBigger);
                HitEnemy();
            }
            else
            {
                if (AttackStepsSinceChange % AttackChange == 0)
                {
                    TempModel = AttackImgs[AttackSpriteNum];
                    BoundingBox.Width = (int)(TempModel.Width * TimesBigger);
                    BoundingBox.Height = (int)(TempModel.Height * TimesBigger);
                    AttackSpriteNum++;
                    StepsMovedSinceChange = 0;
                }
                AttackStepsSinceChange++;
            }
        }

        protected virtual void HitEnemy()
        {

        }

        protected void RunSpritesManager()
        {
            if (StepsMovedSinceChange % RunChange == 0)
            {
                if (RunSpriteNum == RunImgs.Count) RunSpriteNum = 0;
                TempModel = RunImgs[RunSpriteNum];
                BoundingBox.Width = (int)(TempModel.Width * TimesBigger);
                BoundingBox.Height = (int)(TempModel.Height * TimesBigger);
                RunSpriteNum++;
                StepsMovedSinceChange = 0;
            }
            StepsMovedSinceChange++;
        }

        protected void MoveLeft()
        {
            if (SpriteDirection == 1) SpriteDirection = 0;
            RunSpritesManager();
            BoundingBox.X = (int)(BoundingBox.X - velocity);
            foreach (Basic2d asset in Globals.GameAssets)
            {
                if (Collides(asset))
                {
                    BoundingBox.X = (int)(BoundingBox.X + velocity);
                }
            }
        }

        protected void MoveRight()
        {
            if (SpriteDirection == 0) SpriteDirection = 1;
            RunSpritesManager();
            BoundingBox.X = (int)(BoundingBox.X + velocity);
            foreach (Basic2d asset in Globals.GameAssets)
            {
                if (Collides(asset))
                {
                    BoundingBox.X = (int)(BoundingBox.X - velocity);
                }
            }
        }

        protected void MoveUp()
        {
            RunSpritesManager();
            BoundingBox.Y = (int)(BoundingBox.Y - velocity);
            foreach (Basic2d asset in Globals.GameAssets)
            {
                if (Collides(asset))
                {
                    BoundingBox.Y = (int)(BoundingBox.Y + velocity);
                }
            }

            foreach (Basic2d asset in Globals.TopBar)
            {
                if (Collides(asset))
                {
                    BoundingBox.Y = (int)(BoundingBox.Y + velocity);
                }
            }
        }

        protected void MoveDown()
        {
            RunSpritesManager();
            BoundingBox.Y = (int)(BoundingBox.Y + velocity);
            foreach (Basic2d asset in Globals.GameAssets)
            {
                if (Collides(asset))
                {
                    BoundingBox.Y = (int)(BoundingBox.Y - velocity);
                }
            }
        }

        protected void MoveLeftOnce()
        {
            if (SpriteDirection == 0) SpriteDirection = 1;
            TempModel = BasicModel;
            BoundingBox.X = (int)(BoundingBox.X - velocity);
            foreach (Basic2d asset in Globals.GameAssets)
            {
                if (Collides(asset))
                {
                    BoundingBox.X = (int)(BoundingBox.X + velocity);
                }
            }
        }

        protected void MoveRightOnce()
        {
            if (SpriteDirection == 0) SpriteDirection = 1;
            TempModel = BasicModel;
            BoundingBox.X = (int)(BoundingBox.X + velocity);
            foreach (Basic2d asset in Globals.GameAssets)
            {
                if (Collides(asset))
                {
                    BoundingBox.X = (int)(BoundingBox.X - velocity);
                }
            }
        }

        protected void MoveUpOnce()
        {
            TempModel = BasicModel;
            BoundingBox.Y = (int)(BoundingBox.Y - velocity);
            foreach (Basic2d asset in Globals.GameAssets)
            {
                if (Collides(asset))
                {
                    BoundingBox.Y = (int)(BoundingBox.Y + velocity);
                }
            }

            foreach (Basic2d asset in Globals.TopBar)
            {
                if (Collides(asset))
                {
                    BoundingBox.Y = (int)(BoundingBox.Y + velocity);
                }
            }
        }

        protected void MoveDownOnce()
        {
            TempModel = BasicModel;
            BoundingBox.Y = (int)(BoundingBox.Y + velocity);
            foreach (Basic2d asset in Globals.BackgroundElems)
            {
                if (Collides(asset))
                {
                    BoundingBox.Y = (int)(BoundingBox.Y - velocity);
                }
            }
        }

        protected virtual bool Collides(Basic2d asset)
        {
            Rectangle box = BoundingBox;
            box.Y -= box.Height;
            return box.Intersects(asset.BoundingBox);
        }
    }
}
