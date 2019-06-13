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
        public int MaxHealth;
        public int Points;
        public bool Win;

        private int finish;

        public int Finish { get => finish; set => finish = value; }

        public Hero(string path, int X, int Y, float vel, int run, int attack, int die, float TimesBigger, int health, int strength)
        : base(path, X, Y, vel, run, attack, die, TimesBigger, health, strength)
        {
            for(int i = 0; i < 6; i++)
            {
                RunImgs.Add(Globals.content.Load<Texture2D>("2D\\HeroRun\\adventurer-run-0" + i));
                AttackImgs.Add(Globals.content.Load<Texture2D>("2D\\HeroAttack\\adventurer-attack1-0" + i));
                DieImgs.Add(Globals.content.Load<Texture2D>("2D\\HeroDie\\adventurer-die-0" + (i+1)));
            }
            HealTime = new MyTimer(4000, 4000);
            MaxHealth = health;
            Win = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!Alive) return;
            if (Health <= 0)
            {
                Health = 0;
                Die();
                return;
            }
            base.Update();

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
                    BoundingBox.Width = (int)(TempModel.Width * TimesBigger);
                    BoundingBox.Height = (int)(TempModel.Height * TimesBigger);
                }

                AssetsCollide(gameTime);
                Heal(gameTime);
            }
            if (BoundingBox.X < 0) BoundingBox.X = 0;
            if (BoundingBox.X > Finish)
            {
                Win = true;
                Alive = false;
            }
            BoundingBox.Y = MathHelper.Clamp(BoundingBox.Y, BoundingBox.Height, Globals.spriteBatch.GraphicsDevice.Viewport.Height);
        }

        private void Heal(GameTime gameTime)
        {
            if(Health < MaxHealth)
                if (HealTime.ItsTime())
                    Health += 10;
                else HealTime.UpdateTimer(gameTime);
        }

        private void AssetsCollide(GameTime gameTime)
        {
            foreach (Asset asset in Globals.InteractiveAssets)
            {
                if (Collides(asset)) asset.Execute(gameTime);
            }
        }

        protected override void HitEnemy()
        {
            foreach(Enemy enemy in Globals.EnemiesAssets)
            {
                if (!enemy.Alive) continue;
                if (CollidesEnemy(enemy)) enemy.Health -= Strength;
            }
        }

        private bool CollidesEnemy(Enemy enemy)
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
