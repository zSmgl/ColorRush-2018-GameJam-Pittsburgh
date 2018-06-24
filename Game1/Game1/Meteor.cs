using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    enum Size
    {
        Small = 1,
        Medium = 2,
        Large = 3
    }
    class Meteor : GameObject
    {
        #region Fields

        private Color color;
        private Size size;
        private int fallSpeed;
        private int border;

        #endregion Fields

        #region Properties

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Size Size
        {
            get
            {
                return size;
            }
        }

        #endregion Properties

        public Meteor(Random rng, int range, int border)
        {
            int sizeDet = rng.Next(0, 11);
            Vector2 hW;
            // 20% Small
            if (sizeDet <= 2)
            {
                hW = new Vector2(32, 32);
                size = Size.Small;
                
            }
            // 50% Medium
            else if (sizeDet <= 7)
            {
                hW = new Vector2(64, 64);
                size = Size.Medium;
            }
            // 30% Large
            else
            {
                hW = new Vector2(128, 128);
                size = Size.Large;
            }

            int xPos = rng.Next((int)hW.X, range - (int)hW.X);

            // Color Stuff

            int colorDet = rng.Next(1, 4);
            if (colorDet == 1)
            {
                color = Color.Purple;
            }
            else if(colorDet == 2)
            {
                color = Color.Orange;
            }
            else
            {
                color = Color.Green;
            }

            int fallDet = rng.Next(1, 4);

            if(fallDet == 1)
            {
                fallSpeed = 8;
            }
            else if (fallDet == 2)
            {
                fallSpeed = 12;
            }
            else if (fallDet == 2)
            {
                fallSpeed = 10;
            }

            spriteBox = new Rectangle(xPos, (-(int)hW.Y - rng.Next(0,20)), (int)hW.X, (int)hW.Y);
            this.border = border;
            isDrawn = true;
            isActive = true;
        }

        public void Update()
        {
            if(Y >= border)
            {
                isDrawn = false;
                isActive = false;
            }
            Y += fallSpeed;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (isDrawn)
            {
                sb.Draw(defaultSprite, spriteBox, color);
            }
        }
    }
}
