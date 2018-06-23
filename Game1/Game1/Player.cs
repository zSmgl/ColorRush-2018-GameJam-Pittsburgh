using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    enum PlayerDirection
    {
        up,
        down,
        left,
        right,
        idle,
    }

    class Player : GameObject
    {
        #region Fields

        // Essentials
        private CustomHitBox hitbox;
        private Color color;

        // Movement
        private KeyboardState kb;
        private KeyboardState previousKb;
        private Dictionary<string, Keys> bindableKb;

        private int verticalVelocity;
        private int horizontalVelocity;

        private bool isFacingRight;
        PlayerDirection playerDirection;

        #endregion Fields

        #region Properties

        public CustomHitBox Hitbox
        {
           get { return hitbox; }
           set { hitbox = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Dictionary<string, Keys> BindableKb
        {
            get { return bindableKb; }
            set { bindableKb = value; }
        }

        public bool IsFacingRight
        {
            get { return isFacingRight; }
        }

        public int VerticalVelocity
        {
            get { return verticalVelocity; }
            set { verticalVelocity = value; }
        }

        public int HorizontalVelocity
        {
            get { return horizontalVelocity; }
            set { horizontalVelocity = value; }
        }

        public PlayerDirection PlayerDirection
        {
            get { return playerDirection; }
            set { playerDirection = value; }
        }

        #endregion Properties

        #region Constructor

        public Player(Texture2D defaultSprite, Rectangle spriteBox, Color color, CustomHitBox hitbox)
        {
            DefaultSprite = defaultSprite;
            Spritebox = spriteBox;
            this.hitbox = hitbox;
            this.color = color;

            isActive = true;
            isDrawn = true;

            bindableKb = new Dictionary<string, Keys>();
            isFacingRight = true;
            verticalVelocity = 0;
            horizontalVelocity = 0;
        }

        #endregion Constructor

        #region Methods

        #region Acceleration

        /// <summary>
        /// slowdown the object by the rate until the limit velocity is reached 
        /// </summary>
        /// <param name="velocityType"></param>
        /// <param name="rate"></param>
        public void Decelerate(int velocityType, int rate, int limit, bool vertical)
        {
            if (isFacingRight)
            {
                if (velocityType > limit)
                {
                    velocityType -= rate; //reduce velocity normally
                }
            }
            else
            {
                if (velocityType < limit)
                {
                    velocityType += rate; //increase velocity since moving left is negative
                                          //needed to prevent player from hovering in air if they decelerate on an edge
                }
            }

            if (vertical)
            {
                verticalVelocity = velocityType;
            }
            else
            {
                horizontalVelocity = velocityType;
            }
        }

        /// <summary>
        /// speed up the object by the rate until the limit velocity is reached
        /// </summary>
        /// <param name="velocityType"></param>
        /// <param name="rate"></param>
        /// <param name="limit"></param>
        public void Accelerate(int velocityType, int rate, int limit, bool vertical)
        {
            if (vertical)
            {
                if (velocityType < limit)
                {
                    velocityType += rate;
                    verticalVelocity = velocityType;
                }
            }
            else //horizontal
            {
                if (isFacingRight)
                {
                    if (velocityType < limit)
                    {
                        velocityType += rate;
                    }
                }
                else //facing left
                {
                    //moving left means moving negatively (decrease value past 0 until negative limit is hit)
                    limit -= limit * 2;
                    if (velocityType > limit)
                    {
                        velocityType -= rate;
                    }
                }
                horizontalVelocity = velocityType;
            }
        }

        #endregion Acceleration

        public Meteor CheckColliderAgainstMeteor(Meteor m)
        {
            return m;
        }

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        #endregion Methods

    }
}
