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
    public class Hero : Character
    {
        private MyTimer HealTime;
        private int MaxHealth;

        public Hero(string path, int X, int Y, float vel, int run, int attack, int TimesBigger, int health, int strength)
        : base(path, X, Y, vel, run, attack, TimesBigger, health, strength)
        {
            for(int i = 0; i < 6; i++)
            {
                RunImgs.Add(Globals.content.Load<Texture2D>("2D\\HeroRun\\adventurer-run-0" + i));
                AttackImgs.Add(Globals.content.Load<Texture2D>("2D\\HeroAttack\\adventurer-attack1-0" + i));
            }
            HealTime = new MyTimer(4000, 4000);
            MaxHealth = health;
        }

        public void Update(GameTime gameTime)
        {
            base.Update();

            Heal(gameTime);

            if (!AttackInProgress)
            {
                if (Globals.NewState.IsKeyDown(Keys.Space))
                    AttackInProgress = true;
                else if (Globals.NewState.IsKeyDown(Keys.Up) && !Globals.OldState.IsKeyDown(Keys.Up))
                    MoveUpOnce();
                else if (Globals.NewState.IsKeyDown(Keys.Down) && !Globals.OldState.IsKeyDown(Keys.Down))
                    MoveDownOnce();
                else if (Globals.NewState.IsKeyDown(Keys.Left) && !Globals.OldState.IsKeyDown(Keys.Left))
                    MoveLeftOnce();
                else if (Globals.NewState.IsKeyDown(Keys.Right) && !Globals.OldState.IsKeyDown(Keys.Right))
                    MoveRightOnce();
                else if (Globals.NewState.IsKeyDown(Keys.Up) && Globals.OldState.IsKeyDown(Keys.Up))
                    MoveUp();
                else if (Globals.NewState.IsKeyDown(Keys.Down) && Globals.OldState.IsKeyDown(Keys.Down))
                    MoveDown();
                else if (Globals.NewState.IsKeyDown(Keys.Left) && Globals.OldState.IsKeyDown(Keys.Left))
                    MoveLeft();
                else if (Globals.NewState.IsKeyDown(Keys.Right) && Globals.OldState.IsKeyDown(Keys.Right))
                    MoveRight();
                else
                {
                    RunSpriteNum = 0;
                    TempModel = BasicModel;
                    StepsMovedSinceChange = 0;
                    BoundingBox.Width = TempModel.Width * TimesBigger;
                    BoundingBox.Height = TempModel.Height * TimesBigger;
                }
            }
            BoundingBox.X = MathHelper.Clamp(BoundingBox.X, 0, Globals.spriteBatch.GraphicsDevice.Viewport.Width - BoundingBox.Width);
            BoundingBox.Y = MathHelper.Clamp(BoundingBox.Y, BoundingBox.Height, Globals.spriteBatch.GraphicsDevice.Viewport.Height);
        }

        private void Heal(GameTime gameTime)
        {
            if(Health < MaxHealth && !AttackInProgress)
                if (HealTime.ItsTime())
                {
                    Health += 10;
                }
                else HealTime.UpdateTimer(gameTime);
        }

        protected override void HitEnemy()
        {
            foreach(Enemy enemy in Globals.EnemiesAssets)
            {
                if (!enemy.Alive) continue;
                if (CollidesEnemy(enemy)) enemy.Health -= Strength;
            }
        }

        protected bool CollidesEnemy(Enemy enemy)
        {
            Rectangle box = enemy.BoundingBox;
            if (BoundingBox.X < enemy.BoundingBox.X)
                box.X += box.Width / 4;
            else if (BoundingBox.X > enemy.BoundingBox.X)
                box.X -= box.Width / 4;

            return BoundingBox.Intersects(box);
        }
    }
}
