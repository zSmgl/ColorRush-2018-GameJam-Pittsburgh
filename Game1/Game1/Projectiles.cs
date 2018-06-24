using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    class Projectiles : GameObject
    {
        private Rectangle hitbox;
        private Color color;
        private int velocityConstant;

        public Rectangle Hitbox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Projectiles(Vector2 spawnCord, Color color)
        {
            hitbox = new Rectangle((int)spawnCord.X, (int)spawnCord.Y, 12, 26);
            spriteBox = new Rectangle(hitbox.X - 10, hitbox.Y - 4, 32, 32);
            this.color = color;
            isActive = true;
            isDrawn = true;
            velocityConstant = 10;
        }

        public void Update()
        {
            spriteBox.Y -= velocityConstant;
            RepositionHitbox();
            ActiveorNo();
        }

        private void RepositionHitbox()
        {
            hitbox = new Rectangle(X + 10, Y + 4, hitbox.Width, hitbox.Height);
        }

        private void ActiveorNo()
        {
                if (Y - Height <= -Height)
                {
                    isActive = false;
                    isDrawn = false;
                }
            
        }

        public override void Draw(SpriteBatch sb)
        {
            if(isDrawn)
            {
                sb.Draw(defaultSprite, spriteBox, color);
            }
        }
    }
}
