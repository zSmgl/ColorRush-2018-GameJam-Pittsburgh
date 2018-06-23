using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    class Base_Background
    {
        private Rectangle spritebox;
        private Texture2D defaultSprite;

        public int Y
        {
            get { return spritebox.Y; }
            set { spritebox.Y = value; }
        }

        public int Height
        {
            get { return spritebox.Height; }
        }

        public Base_Background(Texture2D defaultSprite, Rectangle spritebox)
        {
            this.spritebox = spritebox;
        }

        public void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }
    }
}
