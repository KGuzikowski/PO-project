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
    public class World
    {
        private UI ui;
        private int WorldX;
        Random random;

        private int EnemiesNumber;
        private int BuildingsNumber;
        private int ObstacklesNumber;
        private int FoodNumber;
        private int FireNumber;
        private int BgAssetsNumber;

        public World(int WorldX, int EnemiesNumber, int BuildingsNumber, int ObstacklesNumber,
            int FoodNumber, int FireNumber, int BgAssetsNumber, GraphicsDevice graphics)
        {
            this.WorldX = WorldX;
            this.EnemiesNumber = EnemiesNumber;
            this.BuildingsNumber = BuildingsNumber;
            this.ObstacklesNumber = ObstacklesNumber;
            this.FoodNumber = FoodNumber;
            this.FireNumber = FireNumber;
            this.BgAssetsNumber = BgAssetsNumber;

            random = new Random();

            Globals.GameAssets = new List<Basic2d>();
            Globals.BackgroundElems = new List<Basic2d>();
            Globals.BackgroundAssets = new List<Basic2d>();
            Globals.InteractiveAssets = new List<Asset>();
            Globals.EnemiesAssets = new List<Enemy>();
            
            SetHero();
            ui = new UI(graphics, WorldX);
            SetEnemies(EnemiesNumber);
            SetMap(BuildingsNumber, ObstacklesNumber, FoodNumber, FireNumber, BgAssetsNumber);
        }

        public void Update(GameTime gameTime)
        {
            if (Globals.Hero.Alive)
            {
                foreach (Enemy asset in Globals.EnemiesAssets)
                {
                    asset.Update();
                }

                Globals.Hero.Update(gameTime);
                ui.Update();
            }
        }

        public void Draw()
        {
            if (Globals.Hero.Alive)
            {
                foreach (Basic2d asset in Globals.BackgroundElems)
                {
                    asset.Draw();
                }

                foreach (Basic2d asset in Globals.BackgroundAssets)
                {
                    asset.Draw();
                }

                foreach (Asset asset in Globals.InteractiveAssets)
                {
                    asset.Draw();
                }

                foreach (Basic2d asset in Globals.GameAssets)
                {
                    asset.Draw();
                }

                foreach (Enemy asset in Globals.EnemiesAssets)
                {
                    if (!asset.Alive) continue;
                    asset.Draw();
                }

                Globals.Hero.Draw();
            }
            ui.Draw();
        }

        private void SetMap(int buildingsNumber, int obstacklesNumber, int foodNumber, int fireNumber, int bgAssetsNumber)
        {
            int y = Globals.spriteBatch.GraphicsDevice.Viewport.Height;
            int MinY = Globals.TopBar[0].BoundingBox.Height;

            GenerateObstackles(y, MinY, buildingsNumber, obstacklesNumber);
            GenerateBG(y, MinY, foodNumber, fireNumber, bgAssetsNumber);
        }

        private void GenerateObstackles(int WorldY, int minY, int Buildings, int Other)
        {
            for (int i = 0; i < 5; i++)
                Globals.GameAssets.Add(new Basic2d("2D\\world\\Buildings\\building_1", -200, i * 210, 0.4f));

            for (int i = 1; i < 5; i++)
                Globals.GameAssets.Add(new Basic2d("2D\\world\\Buildings\\building_1", WorldX, i * 210 + 30, 0.4f));

            int MinY = minY + Globals.Hero.BoundingBox.Height;

            for (int i = 0; i < Buildings; i++)
            {
                int num = random.Next(5);
                Globals.GameAssets.Add(new Basic2d("2D\\world\\Buildings\\building_" + num, 0, 0, 0.4f));

                int MaxX = WorldX - Globals.GameAssets[Globals.GameAssets.Count - 1].BoundingBox.Width;
                int MaxY = WorldY - Globals.GameAssets[Globals.GameAssets.Count - 1].BoundingBox.Height;
                
                FindPos(3, Globals.GameAssets.Count - 1, MaxX, MinY, MaxY);
            }

            for (int i = 0; i < Other; i++)
            {
                int num = random.Next(4);
                Globals.GameAssets.Add(new Basic2d("2D\\world\\Assets\\decor_" + num, 0, 0, 0.4f));

                int MaxX = WorldX - Globals.GameAssets[Globals.GameAssets.Count - 1].BoundingBox.Width;
                int MaxY = WorldY - Globals.GameAssets[Globals.GameAssets.Count - 1].BoundingBox.Height;
                
                FindPos(3, Globals.GameAssets.Count - 1, MaxX, MinY, MaxY);
            }
        }

        private void GenerateBG(int WorldY, int MinY, int foodNum, int fireNum, int Other)
        {
            for (int i = 0; i < foodNum; i++)
            {
                int num = random.Next(5);
                int strength = random.Next(2, 6) * 10;
                Globals.InteractiveAssets.Add(new Asset("2D\\world\\Food\\food_" + num, 0, 0, 3, 1, strength));

                int MaxX = WorldX - Globals.InteractiveAssets[Globals.InteractiveAssets.Count - 1].BoundingBox.Width;
                int MaxY = WorldY - Globals.InteractiveAssets[Globals.InteractiveAssets.Count - 1].BoundingBox.Height;
                
                FindPos(2, Globals.InteractiveAssets.Count - 1, MaxX, MinY, MaxY);
            }

            for (int i = 0; i < fireNum; i++)
            {
                int strength = random.Next(1, 3) * 10;
                Globals.InteractiveAssets.Add(new Asset("2D\\world\\Assets\\fire", 0, 0, 0.5f, 2, strength));

                int MaxX = WorldX - Globals.InteractiveAssets[Globals.InteractiveAssets.Count - 1].BoundingBox.Width;
                int MaxY = WorldY - Globals.InteractiveAssets[Globals.InteractiveAssets.Count - 1].BoundingBox.Height;
                
                FindPos(2, Globals.InteractiveAssets.Count - 1, MaxX, MinY, MaxY);
            }

            for (int i = 0; i < 4; i++)
                for (int j = -6; j < 0; j++)
                    Globals.BackgroundElems.Add(new Basic2d("2D\\world\\land_6", j * 256, i * 256, 1.00f));
            Globals.BackgroundElems.Add(new Basic2d("2D\\world\\lake_vertical", -500, -100, 1.50f));
            Globals.BackgroundElems.Add(new Basic2d("2D\\world\\Plants\\greenery_1", -600, WorldY - 400, 1.00f));

            bool ok = false;
            int GrassX = 0;
            while (!ok)
            {
                Globals.BackgroundElems.Add(new Basic2d("2D\\world\\grass", GrassX, 0, 6.00f));
                if (Globals.BackgroundElems[Globals.BackgroundElems.Count - 1].BoundingBox.Right >= WorldX)
                    ok = true;
                GrassX += Globals.BackgroundElems[Globals.BackgroundElems.Count - 1].BoundingBox.Width;
            }

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 6; j++)
                    Globals.BackgroundElems.Add(new Basic2d("2D\\world\\land_6", j * 256 + WorldX, i * 256, 1.00f));
            Globals.BackgroundElems.Add(new Basic2d("2D\\world\\lake_vertical", WorldX + 500, -100, 1.50f));
            Globals.BackgroundElems.Add(new Basic2d("2D\\world\\Plants\\greenery_1", WorldX + 600, WorldY - 400, 1.00f));

            for (int i = 0; i < Other; i++)
            {
                int num = random.Next(12);
                Globals.BackgroundAssets.Add(new Basic2d("2D\\world\\Assets\\other_" + num, 0, 0, 0.5f));

                int MaxX = WorldX - Globals.BackgroundAssets[Globals.BackgroundAssets.Count - 1].BoundingBox.Width;
                int MaxY = WorldY - Globals.BackgroundAssets[Globals.BackgroundAssets.Count - 1].BoundingBox.Height;
                
                FindPos(1, Globals.BackgroundAssets.Count - 1, MaxX, MinY, MaxY);
            }
        }

        private void SetHero()
        {
            int y = Globals.spriteBatch.GraphicsDevice.Viewport.Height;
            int HeroX, HeroY;
            HeroX = 200;
            HeroY = y - 200;
            Globals.Hero = new Hero("2D\\adventurer-idle-00", HeroX, HeroY, 6.00f, 6, 5, 15, 4.00f, 100, 40)
            {
                Finish = WorldX
            };
        }

        private void SetEnemies(int enemyNum)
        {
            int WorldY = Globals.spriteBatch.GraphicsDevice.Viewport.Height;

            for(int j = 0; j < enemyNum; j++)
            {
                int range = random.Next(3, 6) * 100;
                int type = random.Next(1, 5);
                int life = random.Next(5, 8) * 10;
                int damage = random.Next(2, 5) * 10;

                Globals.EnemiesAssets.Add(new Enemy("2D\\Enemies\\Minotaur\\Minotaur", type,
                                               0, 0, 2.00f, 6, 7, 10, 3.00f, range, life, damage));
                for (int i = 0; i < 8; i++)
                {
                    Globals.EnemiesAssets[j].RunImgs.Add(
                        Globals.content.Load<Texture2D>("2D\\Enemies\\Minotaur\\Run\\Minotaur_run_0" + i)
                    );
                    Globals.EnemiesAssets[j].AttackImgs.Add(
                        Globals.content.Load<Texture2D>("2D\\Enemies\\Minotaur\\Attack\\Minotaur_attack_0" + i)
                    );
                    Globals.EnemiesAssets[j].DieImgs.Add(
                        Globals.content.Load<Texture2D>("2D\\Enemies\\Minotaur\\Die\\Minotaur_die_0" + i)
                    );
                }

                int MinY = Globals.TopBar[0].BoundingBox.Height + Globals.EnemiesAssets[j].BoundingBox.Height;
                int MaxX = WorldX - Globals.EnemiesAssets[j].BoundingBox.Width;

                FindEnemyPos(j, MaxX, MinY, WorldY);
            }
        }

        private void FindEnemyPos(int currentElem, int MaxX, int MinY, int MaxY)
        {
            bool ok = false;
            Vector2 pos = new Vector2();
            while (!ok)
            {
                pos = RandomPos(MaxX, MinY, MaxY);
                Globals.EnemiesAssets[currentElem].BoundingBox.X = (int)pos.X;
                Globals.EnemiesAssets[currentElem].BoundingBox.Y = (int)pos.Y;
                if(currentElem == 0)
                {
                    ok = true;
                    break;
                }
                for (int i = 0; i < currentElem; i++)
                {
                    if (i == currentElem) continue;
                    if (Globals.EnemiesAssets[currentElem].BoundingBox.Intersects(Globals.EnemiesAssets[i].BoundingBox))
                        break;
                    else if (Globals.EnemiesAssets[currentElem].BoundingBox.Intersects(Globals.Hero.BoundingBox))
                        break;

                    if (i == currentElem - 1) ok = true;
                }
            }
        }

        // Kind = 1 is a background element
        // Kind = 2 is a interactive asset
        // Kind = 3 is a obstackle
        private void FindPos(int Kind, int currentElem, int MaxX, int MinY, int MaxY)
        {
            bool ok = false;
            Vector2 pos = new Vector2();
            while (!ok)
            {
                pos = RandomPos(MaxX, MinY, MaxY);
                if(Kind == 3)
                {
                    Globals.GameAssets[currentElem].BoundingBox.X = (int)pos.X;
                    Globals.GameAssets[currentElem].BoundingBox.Y = (int)pos.Y;
                    if (CollidesCharacter(Globals.GameAssets[currentElem])) continue;
                    if (currentElem == 0)
                    {
                        ok = true;
                        break;
                    }
                    for (int i = 0; i < currentElem; i++)
                    {
                        if (Globals.GameAssets[currentElem].BoundingBox.Intersects(Globals.GameAssets[i].BoundingBox))
                            break;
                        if (i == currentElem - 1) ok = true;
                    }
                }
                else if(Kind == 2)
                {
                    Globals.InteractiveAssets[currentElem].BoundingBox.X = (int)pos.X;
                    Globals.InteractiveAssets[currentElem].BoundingBox.Y = (int)pos.Y;
                    if (CollidesCharacter(Globals.InteractiveAssets[currentElem])) continue;
                    bool ok1 = false, ok2 = false;
                    for (int i = 0; i < Globals.GameAssets.Count; i++)
                    {
                        if (Globals.InteractiveAssets[currentElem].BoundingBox.Intersects(Globals.GameAssets[i].BoundingBox))
                            break;
                        if (i == Globals.GameAssets.Count - 1) ok2 = true;
                    }
                    if (currentElem == 0)
                    {
                        ok = true;
                        break;
                    }
                    for (int i = 0; i < currentElem; i++)
                    {
                        if (i == currentElem) continue;
                        if (Globals.InteractiveAssets[currentElem].BoundingBox.Intersects(Globals.InteractiveAssets[i].BoundingBox))
                            break;
                        if (i == currentElem - 1) ok1 = true;
                    }
                    if (ok1 && ok2) ok = true;
                }
                else
                {
                    Globals.BackgroundAssets[currentElem].BoundingBox.X = (int)pos.X;
                    Globals.BackgroundAssets[currentElem].BoundingBox.Y = (int)pos.Y;
                    bool ok1 = false, ok2 = false, ok3 = false;
                    for (int i = 0; i < Globals.GameAssets.Count; i++)
                    {
                        if (Globals.BackgroundAssets[currentElem].BoundingBox.Intersects(Globals.GameAssets[i].BoundingBox))
                            break;
                        if (i == Globals.GameAssets.Count - 1) ok2 = true;
                    }
                    for (int i = 0; i < Globals.InteractiveAssets.Count; i++)
                    {
                        if (Globals.BackgroundAssets[currentElem].BoundingBox.Intersects(Globals.InteractiveAssets[i].BoundingBox))
                            break;
                        if (i == Globals.InteractiveAssets.Count - 1) ok3 = true;
                    }
                    if (currentElem == 0)
                    {
                        ok = true;
                        break;
                    }
                    for (int i = 0; i < currentElem; i++)
                    {
                        if (i == currentElem) continue;
                        if (Globals.BackgroundAssets[currentElem].BoundingBox.Intersects(Globals.BackgroundAssets[i].BoundingBox))
                            break;
                        if (i == currentElem - 1) ok1 = true;
                    }
                    if (ok1 && ok2 && ok3) ok = true;
                }
            }
        }

        private Vector2 RandomPos(int MaxX, int MinY, int MaxY)
        {
            return new Vector2(random.Next(MaxX), random.Next(MinY, MaxY));
        }

        private bool CollidesCharacter(Basic2d asset)
        {
            Rectangle box;
            foreach (Enemy enemy in Globals.EnemiesAssets)
            {
                box = enemy.BoundingBox;
                box.Y -= box.Height;
                if (asset.BoundingBox.Intersects(box)) return true;
            }

            box = Globals.Hero.BoundingBox;
            box.Y -= box.Height;
            if (asset.BoundingBox.Intersects(box)) return true;

            return false;
        }
    }
}