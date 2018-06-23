using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    class ScrollingBackground : GameObject
    {
        private List<Base_Background> background;

        public ScrollingBackground(List<Base_Background> background)
        {
            background[1].Y = background[0].Y + background[0].Height;
            background[2].Y = background[1].Y + background[1].Height;
            background[3].Y = background[2].Y + background[2].Height;
            background[4].Y = background[3].Y + background[3].Height;

            this.background = background;
            isActive = true;
            isDrawn = true;
        }

        public void Update(int aDirection, int aSpeed, GameTime gameTime)
        {
            if (background[0].Y < -background[0].Height)
            {
                background[0].Y = background[4].Y + background[4].Height;
            }

            if (background[1].Y < -background[1].Height)
            {
                background[1].Y = background[0].Y + background[0].Height;
            }

            if (background[2].Y < -background[2].Height)
            {
                background[2].Y = background[1].Y + background[1].Height;
            }

            if (background[3].Y < -background[3].Height)
            {
                background[3].Y = background[2].Y + background[2].Height;
            }

            if (background[4].Y < -background[4].Height)
            {
                background[4].Y = background[3].Y + background[3].Height;
            }

            background[0].Y += (int)(aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            background[1].Y += (int)(aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            background[2].Y += (int)(aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            background[3].Y += (int)(aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            background[4].Y += (int)(aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        public override void Draw(SpriteBatch sb)
        {
            foreach(Base_Background b in background)
            {
                b.Draw(sb);
            }
        }
    }
}
