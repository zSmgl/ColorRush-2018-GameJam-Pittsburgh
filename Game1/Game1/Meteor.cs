using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    class Meteor : GameObject
    {
        #region Fields

        private Rectangle hitbox;
        private Color color;

        #endregion Fields

        #region Properties

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

        #endregion Properties

        public Meteor(Texture2D defaultSprite, Rectangle spriteBox, Color color, Rectangle hitbox)
        {
            DefaultSprite = defaultSprite;
            Spritebox = spriteBox;
            this.hitbox = hitbox;
            this.color = color;
        }

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }
    }
}
