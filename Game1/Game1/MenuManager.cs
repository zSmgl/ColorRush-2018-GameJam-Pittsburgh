using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class MenuManager
    {
        private List<Texture2D> play;
        private Rectangle playRect;
        private Texture2D background;
        MouseState playerMouse;
        MouseState previousMouse;
        Rectangle mouseInteraction;
        Rectangle screenRect;

        private int posX;
        private int posY;

        public MenuManager(List<Texture2D> play, Texture2D background)
        {
            this.play = play;
            this.background = background;
            playerMouse = Mouse.GetState();
            previousMouse = Mouse.GetState();
            screenRect = new Rectangle(0, 0, 1150, 950);
            playRect = new Rectangle(228, 550, 694, 234);

        }

        public GameState Update()
        {
            previousMouse = playerMouse;
            playerMouse = Mouse.GetState();
            posX = playerMouse.X;
            posY = playerMouse.Y;
            mouseInteraction = new Rectangle(posX, posY, 10, 10);

            if (playerMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                mouseInteraction.Intersects(playRect);
                return GameState.play;
            }

            return GameState.menu;

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(background, new Rectangle(0,0,1150, 950), Color.White);
            if (mouseInteraction.Intersects(playRect))
            {
                sb.Draw(play[1], playRect, Color.White);
            }
            else
            {
                sb.Draw(play[0], playRect, Color.White);
            }
        }
    }
}
