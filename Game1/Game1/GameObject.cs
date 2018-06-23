using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    abstract class GameObject
    {
        #region Fields

        protected Rectangle spriteBox;
        protected Texture2D defaultSprite;
        protected bool isActive;
        protected bool isDrawn;

        #endregion Fields

        #region Properties

        public int X
        {
            get { return spriteBox.X; }
            set { spriteBox.X = value; }
        }

        public int Y
        {
            get { return spriteBox.Y; }
            set { spriteBox.Y = value; }
        }

        public Vector2 Position
        {
            get { return new Vector2(spriteBox.X, spriteBox.Y); }
            set { spriteBox.X = (int)value.X; spriteBox.Y = (int)value.Y; }
        }

        public int Width
        {
            get { return spriteBox.Width; }
            set { spriteBox.Width = value; }
        }

        public int Height
        {
            get { return spriteBox.Height; }
            set { spriteBox.Height = value; }
        }

        public Texture2D DefaultSprite
        {
            get { return this.defaultSprite; }
            set { defaultSprite = value; }
        }

        public virtual Rectangle Spritebox
        {
            get { return this.spriteBox; }
            set { this.spriteBox = value; }
        }

        /// <summary>
        /// Returns true if the object is active.
        /// </summary>
        public bool IsActive
        {
            get { return this.isActive; }
            set { this.isActive = value; }
        }

        /// <summary>
        /// Returns true if the object is meant to be drawn this frame.
        /// </summary>
        public bool IsDrawn
        {
            get { return isDrawn; }
            set { isDrawn = value; }
        }

        #endregion Properties

        #region Methods

        public abstract void Draw(SpriteBatch sb);

        #endregion Methods



    }
}
