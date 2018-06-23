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

        //Controlled Values
        private int scrollSpeed;

        public GameManager(ScrollingBackground scrollingBackground, Player player)
        {
            this.scrollingBackground = scrollingBackground;
            scrollSpeed = 160;

            this.player = player;
        }

        public void Update(GameTime gameTime)
        {
            scrollingBackground.Update(-1, scrollSpeed, gameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            scrollingBackground.Draw(sb);
            player.Draw(sb);
        }
    }
}
