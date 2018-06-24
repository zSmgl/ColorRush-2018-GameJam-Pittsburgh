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
        }

        private void RepositionHitbox()
        {
            hitbox = new Rectangle(X + 10, Y + 4, hitbox.Width, hitbox.Height);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(defaultSprite, spriteBox, color);
        }
    }
}
