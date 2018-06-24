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
        private UserInterface uI;
        private Random rng;

        //Controlled Values
        private int scrollSpeed;

        public GameManager(ScrollingBackground scrollingBackground, Player player, Texture2D projectileSprite, List<Texture2D> meteorSprites, UserInterface uI)
        {
            this.scrollingBackground = scrollingBackground;
            scrollSpeed = 90;

            this.player = player;
            player.ScreenBorder = new Vector2(950, 950);

            projectiles = new List<Projectiles>();
            this.projectileSprite = projectileSprite;
            // Test the two last values of this.
            spawner = new RandomTimer(3000, new Random(), 130, 200);
            meteors = new List<Meteor>();
            this.meteorSprites = meteorSprites;

            this.uI = uI;

            rng = new Random();
        }

        public GameState Update(GameTime gameTime)
        {
            // Remove Old Stuff on Screen if didnt explode
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].IsActive == false)
                {
                    projectiles.RemoveAt(i);
                }
            }

            for (int i = 0; i < meteors.Count; i++)
            {
                if (meteors[i].IsActive == false)
                {
                    meteors.RemoveAt(i);
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

                foreach (Projectiles p in projectiles)
                {
                    p.Update();
                }

                foreach (Meteor m in meteors)
                {
                    m.Update();
                }

                foreach(Projectiles p in projectiles)
                {
                    foreach(Meteor m in meteors)
                    {
                        
                        if (m.Spritebox.Intersects(p.Hitbox) && m.IsActive && ComplementsCheck(p, m))
                        {
                            m.IsActive = false;
                            p.IsActive = false;
                            player.PlayerScore += (m.PointVaue * player.Multiplier);
                            break;
                        }
                    }
                }

            foreach (Meteor m in meteors)
            {

                if (m.Spritebox.Intersects(player.Hitbox) && m.IsActive)
                {
                    m.IsActive = false;
                    if (player.IsInvincible)
                    {
                        player.PlayerScore += (m.PointVaue * player.Multiplier);
                    }
                    else
                    {
                        player.Playerstate = PlayerState.die;
                        break;
                    }
                }
            }

            if (uI.Update(player, gameTime))
            {
                return GameState.gameover;
            }



            scrollingBackground.Update(1, scrollSpeed, gameTime);

            if(player.Playerstate == PlayerState.die)
            {
                return GameState.gameover;
            }

            return GameState.play;
        }

        private bool ComplementsCheck(Projectiles p, Meteor m)
        {
            if(p.Color == Color.Black)
            {
                return true;
            }

            if (p.Color == Color.Red && m.Color == Color.LawnGreen)
            {
                return true;
            }
            else if(p.Color == Color.Yellow && m.Color == Color.Fuchsia)
            {
                return true;
            }
            else if (p.Color == Color.Blue && m.Color == Color.Orange)
            {
                return true;
            }

            return false;
        }

        public string GetInfo()
        {
            return player.PlayerScore.ToString();
        }

        public void Draw(SpriteBatch sb)
        {
            scrollingBackground.Draw(sb);
            uI.Draw(sb);
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
