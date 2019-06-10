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

        public World()
        {
            Globals.GameAssets = new List<Basic2d>();
            Globals.BackgroundElems = new List<Basic2d>();
            Globals.InteractiveAssets = new List<Asset>();
            Globals.EnemiesAssets = new List<Enemy>();

            SetMap();
            SetCharacters();

            ui = new UI();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Basic2d asset in Globals.BackgroundElems)
            {
                asset.Update();
            }

            foreach (Asset asset in Globals.InteractiveAssets)
            {
                asset.Update();
            }

            foreach (Basic2d asset in Globals.GameAssets)
            {
                asset.Update();
            }

            foreach (Enemy asset in Globals.EnemiesAssets)
            {
                asset.Update();
            }

            Globals.Hero.Update(gameTime);
            ui.Update();
        }

        public void Draw()
        {
            foreach (Basic2d asset in Globals.BackgroundElems)
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
            ui.Draw();
        }

        private void SetMap()
        {
            int y = Globals.spriteBatch.GraphicsDevice.Viewport.Height;
            for (int i = 0; i < 4; i++)
                for (int j = -6; j < 0; j++)
                    Globals.BackgroundElems.Add(new Basic2d("2D\\world\\land_6", j * 256, i * 256, 1.00f));
            
            Globals.BackgroundElems.Add(new Basic2d("2D\\world\\grass", 0, 0, 6.00f));

            Globals.BackgroundElems.Add(new Basic2d("2D\\world\\lake_vertical", -500, -100, 1.50f));
            Globals.BackgroundElems.Add(new Basic2d("2D\\world\\Plants\\greenery_1", -600, y - 400, 1.00f));

            for (int i = 0; i < 5; i++)
                Globals.GameAssets.Add(new Basic2d("2D\\world\\Buildings\\building_1", -200, i * 210, 0.4f));
        }

        private void SetCharacters()
        {
            int HeroX, HeroY;
            HeroX = 200;
            HeroY = Globals.spriteBatch.GraphicsDevice.Viewport.Height - 200;
            Globals.Hero = new Hero("2D\\adventurer-idle-00", HeroX, HeroY, 6.00f, 6, 5, 4.00f, 100, 40);

            SetEnemies();
        }

        private void SetEnemies()
        {
            int WindowY = Globals.spriteBatch.GraphicsDevice.Viewport.Height;
            int WindowX = Globals.spriteBatch.GraphicsDevice.Viewport.Width;
            int enemies = 0;

            Globals.EnemiesAssets.Add(new Enemy("2D\\Enemies\\Minotaur\\Minotaur", 1,
                                               200, WindowY, 2.00f, 6, 7, 3.00f, 500, 70, 30));
            enemies++;

            for (int i = 0; i < 8; i++)
            {
                Globals.EnemiesAssets[enemies - 1].RunImgs.Add(
                    Globals.content.Load<Texture2D>("2D\\Enemies\\Minotaur\\Run\\Minotaur_run_0" + i)
                );
                Globals.EnemiesAssets[enemies - 1].AttackImgs.Add(
                    Globals.content.Load<Texture2D>("2D\\Enemies\\Minotaur\\Attack\\Minotaur_attack_0" + i)
                );
                Globals.EnemiesAssets[enemies - 1].DieImgs.Add(
                    Globals.content.Load<Texture2D>("2D\\Enemies\\Minotaur\\Die\\Minotaur_die_0" + i)
                );
            }
        }
    }
}
