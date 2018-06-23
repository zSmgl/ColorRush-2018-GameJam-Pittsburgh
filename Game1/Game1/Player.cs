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
    enum PlayerState
    {
        alive,
        respawning,
        die
    }

    class Player : GameObject
    {
        #region Fields

        // Essentials
        private CustomHitBox hitbox;
        private CustomHitBox previousHitbox;
        private Rectangle previousSpritebox;
        private Color color;
        private bool isDebugging;

        // Movement
        private KeyboardState kb;
        private KeyboardState previousKb;
        private Dictionary<string, Keys> bindableKb;

        private int verticalVelocity;
        private int horizontalVelocity;

        private bool leftRight;
        private bool upDown;
        PlayerState playerState;
        PlayerState previousState;

        // Collision
        private CustomHitBox bottomChecker;
        private CustomHitBox topChecker;
        private CustomHitBox rightSideChecker;
        private CustomHitBox leftSideChecker;
        private bool bottomIntersects;
        private bool topIntersects;

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

        public PlayerState Playerstate
        {
            get { return playerState; }
            set { playerState = value; }
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

            //Movement
            bindableKb = new Dictionary<string, Keys>();
            verticalVelocity = 0;
            horizontalVelocity = 0;
            playerState = PlayerState.alive;

            //Collisions
            isDebugging = false;
            bottomIntersects = false;
            topIntersects = false;

            leftRight = false;
            upDown = false;
        }

        #endregion Constructor

        #region Movement

        private void Movement()
        {
            int tempVert = VerticalVelocity;
            int tempHori = HorizontalVelocity;

            if ((kb.IsKeyDown(bindableKb["left"])))
            {
                leftRight = true;
                Accelerate(horizontalVelocity, 5, 10, false);
            }

            if ((kb.IsKeyDown(bindableKb["right"])))
            {
                leftRight = true;
                Accelerate(horizontalVelocity, 5, 10, false);
            }

            if ((kb.IsKeyDown(bindableKb["left"])) && (kb.IsKeyDown(bindableKb["right"])))
            {
                horizontalVelocity = tempHori;
                leftRight = false;
            }

            if ((kb.IsKeyDown(bindableKb["up"])))
            {
                upDown = true;
                Accelerate(verticalVelocity, 5, 10, true);
            }

            if ((kb.IsKeyDown(bindableKb["down"])))
            {
                upDown = true;
                Accelerate(verticalVelocity, 5, 10, true);
            }

            if ((kb.IsKeyDown(bindableKb["down"])) && (kb.IsKeyDown(bindableKb["up"])))
            {
                verticalVelocity = tempVert;
                upDown = false;
            }

            if (!leftRight)
            {
                Decelerate(horizontalVelocity, 1, 0, false);
            }

            if (!upDown)
            {
                Decelerate(horizontalVelocity, 1, 0, false);
            }

            X += horizontalVelocity;
            Y += verticalVelocity;
        }

        #region Acceleration

        /// <summary>
        /// slowdown the object by the rate until the limit velocity is reached 
        /// </summary>
        /// <param name="velocityType"></param>
        /// <param name="rate"></param>
        public void Decelerate(int velocityType, int rate, int limit, bool vertical)
        {
            if (vertical)
            {
                if (verticalVelocity > 0)
                {
                    if (velocityType > limit)
                    {
                        velocityType -= rate;
                    }
                }
                else if (verticalVelocity < 0)
                {
                    if (velocityType > limit)
                    {
                        velocityType += rate;
                    }
                }
            }
            else
            {
                if (horizontalVelocity > 0)
                {
                    if (velocityType > limit)
                    {
                        velocityType -= rate;
                    }
                }
                else if (horizontalVelocity < 0)
                {
                    if (velocityType > limit)
                    {
                        velocityType += rate;
                    }
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
            if(vertical) 
            {
                if (verticalVelocity > 0)
                {
                    if (velocityType < limit)
                    {
                        velocityType += rate;
                    }
                }
                else 
                {
                    limit -= limit * 2;
                    if (velocityType > limit)
                    {
                        velocityType -= rate;
                    }
                }
                verticalVelocity = velocityType;
            }
            else 
            {
                if (horizontalVelocity > 0)
                {
                    if (velocityType < limit)
                    {
                        velocityType += rate;
                    }
                }
                else 
                {
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

        #endregion Movement

        public Meteor CheckColliderAgainstMeteor(Meteor m)
        {
            return m;
        }

        /// <summary>
        /// used to prevent holding down a key from spamming an action
        /// </summary>
        /// <param name="pressedKey"></param>
        /// <returns></returns>
        public bool SingleKeyPress(Keys pressedKey)
        {
            if (kb.IsKeyDown(pressedKey) && previousKb.IsKeyUp(pressedKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void StayOnScreen()
        {

        }

        public void Update(GameTime gameTime)
        {
            previousState = Playerstate;
            previousHitbox = Hitbox;
            previousSpritebox = Spritebox;

            previousKb = kb;
            kb = Keyboard.GetState();

            //Debugging code
            if (SingleKeyPress(Keys.F8))
            {
                //switch between debugging and not everytime you press combo
                isDebugging = !isDebugging;
            }

            #region COLLISIONBOXES
            // Going right
            if (horizontalVelocity > 0)
            {
                //X is right of player, Y is the same as player, width depends on horizontalVelocity, height is same as player
                rightSideChecker = hitbox.RightSide(Math.Abs(horizontalVelocity));
                leftSideChecker = hitbox.LeftSide(5);
            }
            // Going left
            else if (horizontalVelocity < 0)
            {
                leftSideChecker = hitbox.LeftSide(Math.Abs(horizontalVelocity));
                rightSideChecker = hitbox.RightSide(-5);
            }
            // Stationary
            else
            {
                leftSideChecker = hitbox.LeftSide(5);
                rightSideChecker = hitbox.Right(-5);
            }

            // Going Up
            if (verticalVelocity < 0)
            {
                //X is right of player, Y is the same as player, width depends on horizontalVelocity, height is same as player
                topChecker = hitbox.TopSide(Math.Abs(verticalVelocity));
                bottomChecker = hitbox.BotSide(-5);
            }
            // Going Left
            else if (verticalVelocity > 0)
            {
                bottomChecker = hitbox.TopSide(Math.Abs(verticalVelocity));
                topChecker = hitbox.BotSide(5);
            }
            else
            {
                topChecker = hitbox.GoingRight(5);
                bottomChecker = hitbox.GoingLeft(-5);
            }


            #endregion

            Movement();
        }

        public void CheckColliderAgainstEnemy(Meteor m)
        {
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(defaultSprite, spriteBox, Color.Red);
        }

    }
}
