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
    class GameOverManager
    {
        private List<Texture2D> menu;
        private Rectangle playRect;
        private Texture2D background;
        MouseState playerMouse;
        MouseState previousMouse;
        Rectangle mouseInteraction;
        Rectangle screenRect;
        SpriteFont font;
        string score;

        private int posX;
        private int posY;

        public GameOverManager(List<Texture2D> menu, Texture2D background, SpriteFont font)
        {
            this.menu = menu;
            this.background = background;
            playerMouse = Mouse.GetState();
            previousMouse = Mouse.GetState();
            screenRect = new Rectangle(0, 0, 1150, 950);
            playRect = new Rectangle(228, 150, 694, 234);
            this.font = font;
            

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
                return GameState.menu;
            }

            return GameState.gameover;

        }

        public void AddInfo(string score)
        {
            this.score = score;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(background, new Rectangle(0, 0, 1150, 950), Color.White);
            if (mouseInteraction.Intersects(playRect))
            {
                sb.Draw(menu[1], playRect, Color.White);
            }
            else
            {
                sb.Draw(menu[0], playRect, Color.White);
            }
            sb.DrawString(font, "SCORE : " + score, new Vector2(400, 550), Color.Black);
        }
    }
}