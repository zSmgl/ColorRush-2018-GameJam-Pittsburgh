using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    class GameManager
    {
        private ScrollingBackground scrollingBackground;
        private Player player;
        private List<Projectiles> projectiles;
        private Texture2D projectileSprite;

        //Controlled Values
        private int scrollSpeed;

        public GameManager(ScrollingBackground scrollingBackground, Player player, Texture2D projectileSprite)
        {
            this.scrollingBackground = scrollingBackground;
            scrollSpeed = 160;

            this.player = player;

            projectiles = new List<Projectiles>();
            this.projectileSprite = projectileSprite;
        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            // Adds Proj to list if needed

            if (projectiles != null)
            {
                foreach (Projectiles p in projectiles)
                {
                    p.Update();
                }
            }

            if (player.FireProjectile())
            {
                foreach (Projectiles p in player.ProjectilesToAdd)
                {
                    Projectiles tempProj = p;
                    tempProj.DefaultSprite = projectileSprite;
                    projectiles.Add(tempProj);
                }
            }

            scrollingBackground.Update(-1, scrollSpeed, gameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            scrollingBackground.Draw(sb);
            player.Draw(sb);

            if (projectiles != null)
            {
                foreach (Projectiles p in projectiles)
                {
                    p.Draw(sb);
                }
            }

        }


    }
}
