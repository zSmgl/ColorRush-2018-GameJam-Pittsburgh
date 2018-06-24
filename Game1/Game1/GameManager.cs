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
        private RandomTimer spawner;
        private List<Texture2D> meteorSprites;
        private List<Meteor> meteors;
        private Random rng;

        //Controlled Values
        private int scrollSpeed;

        public GameManager(ScrollingBackground scrollingBackground, Player player, Texture2D projectileSprite, List<Texture2D> meteorSprites)
        {
            this.scrollingBackground = scrollingBackground;
            scrollSpeed = 160;

            this.player = player;
            player.ScreenBorder = new Vector2(950, 950);

            projectiles = new List<Projectiles>();
            this.projectileSprite = projectileSprite;
            // Test the two last values of this.
            spawner = new RandomTimer(3000, new Random(), 165, 500);
            meteors = new List<Meteor>();
            this.meteorSprites = meteorSprites;

            rng = new Random();
        }

        public void Update(GameTime gameTime)
        {
            // Remove Old Stuff on Screen if didnt explode
            if(projectiles!= null)
            {
                for (int i = projectiles.Count; i < 0; i--)
                {
                    if (projectiles[i].IsActive == false)
                    {
                        projectiles.RemoveAt(i);
                    }
                }
            }

            if(meteors!= null)
            {
                for (int i = meteors.Count; i < 0; i--)
                {
                    if (meteors[i].IsActive == false)
                    {
                        meteors.RemoveAt(i);
                    }
                }
            }

            // Add Meteors if its time
            if (spawner.UpdateTimer(gameTime))
            {
                meteors.Add(new Meteor(rng, (int)player.ScreenBorder.X, (int)player.ScreenBorder.Y));
                if(meteors[meteors.Count - 1].Size == Size.Small)
                {
                    meteors[meteors.Count - 1].DefaultSprite = meteorSprites[0];
                }
                else if(meteors[meteors.Count - 1].Size == Size.Medium)
                {
                    meteors[meteors.Count - 1].DefaultSprite = meteorSprites[1];
                }
                else
                {
                    meteors[meteors.Count - 1].DefaultSprite = meteorSprites[2];
                }
            }

            // Player Movement
            player.Update(gameTime);

            // Player Extension and Adds Proj to list if needed
            if (player.FireProjectile())
            {
                foreach (Projectiles p in player.ProjectilesToAdd)
                {
                    Projectiles tempProj = p;
                    tempProj.DefaultSprite = projectileSprite;
                    projectiles.Add(tempProj);
                }
            }

            // Update Projectiles if Any

            if (projectiles != null)
            {
                foreach (Projectiles p in projectiles)
                {
                    p.Update();
                }
            }

            // Update Meteors if any
            if (meteors != null)
            {
                foreach (Meteor m in meteors)
                {
                    m.Update();
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

            if (meteors != null)
            {
                foreach (Meteor m in meteors)
                {
                    m.Draw(sb);
                }
            }

        }


    }
}
