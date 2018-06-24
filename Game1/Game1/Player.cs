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
        private Rectangle hitbox;
        private Rectangle previousHitbox;
        private Texture2D hitSprite;
        private Rectangle previousSpritebox;
        private Dictionary<Color, bool> activeColor;

        //Proj Spawns

        // Determinants
        private bool isDebugging;
        private bool isInvincible;
        private bool canShoot;
        Timer shootTime;

        // Movement
        private KeyboardState kb;
        private KeyboardState previousKb;
        private Dictionary<string, Keys> bindableKb;

        private int verticalVelocity;
        private int horizontalVelocity;
        private int maxSpeed;
        private Vector2 screenBorder;

        private bool leftRight;
        private bool upDown;
        PlayerState playerState;
        PlayerState previousState;

        //Projectiles
        List<Projectiles> spawnProjectiles;

        // Collision
        private Rectangle bottomChecker;
        private Rectangle topChecker;
        private Rectangle rightSideChecker;
        private Rectangle leftSideChecker;
        private bool bottomIntersects;
        private bool topIntersects;

        #endregion Fields

        #region Properties

        public Vector2 ScreenBorder
        {
            set { screenBorder = value; }
            get { return screenBorder; }
        }

        public List<Projectiles> ProjectilesToAdd
        {
            get { return spawnProjectiles; }
        }

        public Rectangle Hitbox
        {
           get { return hitbox; }
           set { hitbox = value; }
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

        public bool IsDebugging
        {
            get { return isDebugging; }
        }

        public Vector2 SpawnA
        {
            get { return new Vector2(hitbox.X + 10, Y); }
        }
        private Vector2 SpawnO
        {
            get { return new Vector2(hitbox.X + 32, Y); }
        }
        private Vector2 SpawnZ
        {
            get { return new Vector2(hitbox.X + 52, Y); }
        }
        private Vector2 Spawn1
        {
            get { return new Vector2(hitbox.X + 22, Y); }
        }
        private Vector2 Spawn2
        {
            get { return new Vector2(hitbox.X + 42, Y); }
        }

        #endregion Properties

        #region Constructor

        public Player(Texture2D defaultSprite, Texture2D hitSprite, Rectangle spriteBox, Rectangle hitbox)
        {
            DefaultSprite = defaultSprite;
            Spritebox = spriteBox;
            this.hitSprite = hitSprite;
            this.hitbox = hitbox;
            spawnProjectiles = new List<Projectiles>();

            // Color Add
            activeColor = new Dictionary<Color, bool>();
            activeColor.Add(Color.White, false);
            activeColor.Add(Color.Red, true);
            activeColor.Add(Color.Blue, true);
            activeColor.Add(Color.Yellow, true);
            activeColor.Add(Color.Black, false);

            RepositionHitbox();

            isActive = true;
            isDrawn = true;
            canShoot = true;
            shootTime = new Timer(360);

            //Movement
            bindableKb = new Dictionary<string, Keys>();
            verticalVelocity = 0;
            horizontalVelocity = 0;
            playerState = PlayerState.alive;
            maxSpeed = 7;

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
                horizontalVelocity -= 5;
            }

            if ((kb.IsKeyDown(bindableKb["right"])))
            {
                leftRight = true;
                horizontalVelocity += 5;
            }

            if ((kb.IsKeyDown(bindableKb["left"])) && (kb.IsKeyDown(bindableKb["right"])))
            {
                horizontalVelocity = tempHori;
                leftRight = false;
            }

            if ((kb.IsKeyDown(bindableKb["up"])))
            {
                upDown = true;
                verticalVelocity -= 5;
            }

            if ((kb.IsKeyDown(bindableKb["down"])))
            {
                upDown = true;
                verticalVelocity += 5;
            }

            if ((kb.IsKeyDown(bindableKb["down"])) && (kb.IsKeyDown(bindableKb["up"])))
            {
                verticalVelocity = tempVert;
                upDown = false;
            }

            if (!leftRight)
            {
                if(horizontalVelocity < 0)
                {
                    horizontalVelocity++;
                }
                else if (horizontalVelocity > 0)
                {
                    horizontalVelocity--;
                }
            }

            if (!upDown)
            {
                if (verticalVelocity < 0)
                {
                    verticalVelocity++;
                }
                else if (verticalVelocity > 0)
                {
                    verticalVelocity--;
                }
            }

            CheckSpeed();
            X += horizontalVelocity;
            Y += verticalVelocity;
            RepositionHitbox();
        }

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



        private void StayOnScreen()
        {
            if(X < -28)
            {
                X = -28;
            }
            if(Y < -12)
            {
                Y = -12;
            }
            if(X > screenBorder.X - Width + 28)
            {
                X = (int)screenBorder.X - Width + 28;
            }
            if (Y > screenBorder.Y - Height + 12)
            {
                Y = (int)screenBorder.Y - Height + 12;
            }

            RepositionHitbox();
        }

        /// <summary>
        /// Keeps speed in check
        /// </summary>
        /// 
        private void CheckSpeed()
        {
            //if velocity has gone out of bounds of max speed, set it such that it is at its max
            if (horizontalVelocity > maxSpeed)
            {
                horizontalVelocity = maxSpeed;
            }
            if (verticalVelocity > maxSpeed)
            {
                verticalVelocity = maxSpeed;
            }
            if (horizontalVelocity < maxSpeed * -1)
            {
                horizontalVelocity = maxSpeed * -1;
            }
            if (verticalVelocity < maxSpeed * -1)
            {
                verticalVelocity = maxSpeed * -1;
            }
        }

        private void RepositionHitbox()
        {
            hitbox = new Rectangle(X + 28, Y + 12, hitbox.Width, hitbox.Height);
        }

        public void Update(GameTime gameTime)
        {
            previousState = Playerstate;
            previousHitbox = Hitbox;
            previousSpritebox = Spritebox;
            upDown = false;
            leftRight = false;

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
                rightSideChecker = new Rectangle(X + hitbox.Width, Y + 10, Math.Abs(horizontalVelocity), hitbox.Height - 20);
                leftSideChecker = new Rectangle(X, Y + 10, 5, hitbox.Height - 20);
            }
            // Going left
            else if (horizontalVelocity < 0)
            {
                leftSideChecker = new Rectangle(X - Math.Abs(horizontalVelocity), Y + 10, Math.Abs(horizontalVelocity), hitbox.Height - 20);
                rightSideChecker = new Rectangle(X + hitbox.Width - 5, Y + 10, 5, hitbox.Height - 20);
            }
            // Stationary
            else
            {
                leftSideChecker = new Rectangle(X, Y + 10, 5, hitbox.Height - 20);
                rightSideChecker = new Rectangle(X + hitbox.Width - 5, Y + 10, 5, hitbox.Height - 20);
            }

            // Going Up
            if (verticalVelocity < 0)
            {
                //X is right of player, Y is the same as player, width depends on horizontalVelocity, height is same as player
                topChecker = new Rectangle(X + 10, Y - Math.Abs(verticalVelocity), hitbox.Width - 20, Math.Abs(verticalVelocity));
                bottomChecker = new Rectangle(X + 10, Y + hitbox.Height -5, hitbox.Width - 20, 5);
            }
            // Going Down
            else if (verticalVelocity > 0)
            {
                bottomChecker = new Rectangle(X + 10, Y + hitbox.Height, hitbox.Width - 20, Math.Abs(verticalVelocity));
                topChecker = new Rectangle(X + 10, Y, hitbox.Width - 20, 5);
            }
            else
            {
                topChecker = new Rectangle(X + 10, Y, hitbox.Width - 20, 5);
                bottomChecker = new Rectangle(X + 10, Y + hitbox.Height - 5, hitbox.Width - 20, 5);
            }


            #endregion

            Movement();
            StayOnScreen();

            if(canShoot == false)
            {
                canShoot = shootTime.UpdateTimer(gameTime);
            }
            
        }

        public bool FireProjectile()
        {
            spawnProjectiles.Clear();

            // Set Texture int in here

            if (activeColor[Color.White] == true)
            {
                return false;
            }

            else if (activeColor[Color.Black] == true)
            {
                spawnProjectiles.Add(new Projectiles(SpawnA, Color.Black));
                spawnProjectiles.Add(new Projectiles(SpawnO, Color.Black));
                spawnProjectiles.Add(new Projectiles(SpawnZ, Color.Black));
            }
            else if (activeColor[Color.Blue] && activeColor[Color.Yellow] && activeColor[Color.Red])
            {
                spawnProjectiles.Add(new Projectiles(SpawnA, Color.Blue));
                spawnProjectiles.Add(new Projectiles(SpawnO, Color.Yellow));
                spawnProjectiles.Add(new Projectiles(SpawnZ, Color.Red));
            }
            else if (activeColor[Color.Blue] && activeColor[Color.Yellow])
            {
                spawnProjectiles.Add(new Projectiles(Spawn1, Color.Blue));
                spawnProjectiles.Add(new Projectiles(Spawn2, Color.Yellow));
            }
            else if (activeColor[Color.Blue] && activeColor[Color.Red])
            {
                spawnProjectiles.Add(new Projectiles(Spawn1, Color.Blue));
                spawnProjectiles.Add(new Projectiles(Spawn2, Color.Red));
            }
            else if (activeColor[Color.Yellow] && activeColor[Color.Red])
            {
                spawnProjectiles.Add(new Projectiles(Spawn1, Color.Yellow));
                spawnProjectiles.Add(new Projectiles(Spawn2, Color.Red));
            }
            else if (activeColor[Color.Yellow])
            {
                spawnProjectiles.Add(new Projectiles(SpawnO, Color.Yellow));
            }
            else if (activeColor[Color.Blue])
            {
                spawnProjectiles.Add(new Projectiles(SpawnO, Color.Blue));
            }
            else
            {
                spawnProjectiles.Add(new Projectiles(SpawnO, Color.Red));
            }

            if ((kb.IsKeyDown(bindableKb["shoot"])) && canShoot)
            {
                canShoot = false;
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (isDrawn)
            {
                sb.Draw(defaultSprite, spriteBox, Color.White);
            }
        }

    }
}
