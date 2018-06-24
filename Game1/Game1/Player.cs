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
        private Vector2 ssCords;

        // Score
        private double playerScore;
        private double previousScore;  // Used to figure out if the multiplier should Increment
        private int multiplier;     // Multiplier for Score
        private int streak;         // Keeps track of shot in a row.
        private Timer streakBeGone; // Time between gaining points where you could lose points

        // Determinants
        private bool isDebugging;
        private bool isInvincible;
        private bool canShoot;
        private bool isReloading;
        private int shotCount;
        private Timer shootTime;
        private Timer reloadingTime;
        private Timer invincibleTime;


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

        #endregion Fields

        #region Properties

        public Vector2 ScreenBorder
        {
            set { screenBorder = value; }
            get { return screenBorder; }
        }

        public bool IsReloading
        {
            get { return isReloading; }
        }

        public bool IsInvincible
        {
            get { return isInvincible; }
        }

        public int Streak
        {
            get { return streak; }
        }

        public double PlayerScore
        {
            set { playerScore = value; }
            get { return playerScore; }
        }

        public int ShotCount
        {
            get { return shotCount; }
        }

        public int Multiplier
        {
            get { return multiplier; }
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
            activeColor.Add(Color.Blue, false);
            activeColor.Add(Color.Yellow, false);
            activeColor.Add(Color.Black, false);

            ssCords = new Vector2(2, 1);

            RepositionHitbox();

            isActive = true;
            isDrawn = true;

            // MilliSeconds
            shootTime = new Timer(250);         
            invincibleTime = new Timer(10000);
            reloadingTime = new Timer(2000);
            streakBeGone = new Timer(5000);

            canShoot = true;
            isReloading = false;
            isInvincible = false;

            shotCount = 50;
            streak = 0;
            playerScore = 0;
            previousScore = 0;
            multiplier = 1;

            //Movement
            bindableKb = new Dictionary<string, Keys>();
            verticalVelocity = 0;
            horizontalVelocity = 0;
            playerState = PlayerState.alive;
            maxSpeed = 7;

            //Collisions
            isDebugging = false;

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
            if(previousScore != playerScore)
            {
                streak++;
                streakBeGone.ResetTimer();
                previousScore = playerScore;
            }
            else
            {
                if (streakBeGone.UpdateTimer(gameTime) == true)
                {
                    streak = 0;
                    isInvincible = false;
                    activeColor[Color.Black] = false;
                    activeColor[Color.White] = false;
                    activeColor[Color.Yellow] = false;
                    activeColor[Color.Blue] = false;
                    activeColor[Color.Red] = true;
                }
            }

            previousState = Playerstate;
            previousHitbox = Hitbox;
            previousSpritebox = Spritebox;
            upDown = false;
            leftRight = false;

            previousKb = kb;
            kb = Keyboard.GetState();

            //Scoring
            if (streak >= 50)
            {
                multiplier = 16;
                if(streak % 100 == 0)
                {
                    isInvincible = true;
                    activeColor[Color.Black] = true;
                }
            }
            else if (streak > 30)
            {
                multiplier = 8;
            }
            else if (streak > 20)
            {
                multiplier = 4;
                activeColor[Color.Yellow] = true;
            }
            else if (streak > 10)
            {
                multiplier = 2;
                activeColor[Color.Blue] = true;
            }
            else
            {
                multiplier = 1;
            }

            //Debugging code
            if (SingleKeyPress(Keys.F8))
            {
                //switch between debugging and not everytime you press combo
                isDebugging = !isDebugging;
            }

            if (isInvincible)
            {
                    if (invincibleTime.UpdateTimer(gameTime) == true)
                    {
                    isInvincible = false;
                    activeColor[Color.Black] = false;
                    isReloading = true;
                    }
            }

            Movement();
            StayOnScreen();

            if (shotCount == 0)
            {
                shotCount = 50;
                isReloading = true;
                activeColor[Color.White] = true;
            }

            if (isReloading)
            {
                if(reloadingTime.UpdateTimer(gameTime) == true)
                {
                    isReloading = false;
                    activeColor[Color.White] = false;
                }
            }

            if (canShoot == false)
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
                ssCords = new Vector2(0, 0);
                return false;
            }

            else if (activeColor[Color.Black] == true)
            {
                spawnProjectiles.Add(new Projectiles(SpawnA, Color.Black));
                spawnProjectiles.Add(new Projectiles(SpawnO, Color.Black));
                spawnProjectiles.Add(new Projectiles(SpawnZ, Color.Black));
                ssCords = new Vector2(1, 0);
            }
            else if (activeColor[Color.Blue] && activeColor[Color.Yellow] && activeColor[Color.Red])
            {
                spawnProjectiles.Add(new Projectiles(SpawnA, Color.Blue));
                spawnProjectiles.Add(new Projectiles(SpawnO, Color.Yellow));
                spawnProjectiles.Add(new Projectiles(SpawnZ, Color.Red));
                ssCords = new Vector2(2, 0);
            }
            else if (activeColor[Color.Blue] && activeColor[Color.Yellow])
            {
                spawnProjectiles.Add(new Projectiles(Spawn1, Color.Blue));
                spawnProjectiles.Add(new Projectiles(Spawn2, Color.Yellow));
                ssCords = new Vector2(1, 2);
            }
            else if (activeColor[Color.Blue] && activeColor[Color.Red])
            {
                spawnProjectiles.Add(new Projectiles(Spawn1, Color.Blue));
                spawnProjectiles.Add(new Projectiles(Spawn2, Color.Red));
                ssCords = new Vector2(0, 2);
            }
            else if (activeColor[Color.Yellow] && activeColor[Color.Red])
            {
                spawnProjectiles.Add(new Projectiles(Spawn1, Color.Yellow));
                spawnProjectiles.Add(new Projectiles(Spawn2, Color.Red));
                ssCords = new Vector2(2, 2);
            }
            else if (activeColor[Color.Yellow])
            {
                spawnProjectiles.Add(new Projectiles(SpawnO, Color.Yellow));
                ssCords = new Vector2(1, 1);
            }
            else if (activeColor[Color.Blue])
            {
                spawnProjectiles.Add(new Projectiles(SpawnO, Color.Blue));
                ssCords = new Vector2(0, 1);
            }
            else
            {
                spawnProjectiles.Add(new Projectiles(SpawnO, Color.Red));
                ssCords = new Vector2(2, 1);
            }

            if ((kb.IsKeyDown(bindableKb["shoot"])) && canShoot && !isReloading)
            {
                canShoot = false;
                shotCount--;
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (isDrawn)
            {
                if (isInvincible)
                {
                    sb.Draw(defaultSprite, new Vector2(X, Y),
                        new Rectangle((int)ssCords.X * 128, (int)ssCords.Y * 128, Width, Height), Color.Black);
                }
                else
                {
                    sb.Draw(defaultSprite, new Vector2(X, Y),
                        new Rectangle((int)ssCords.X * 128, (int)ssCords.Y * 128, Width, Height), Color.White);
                }
            }
        }

    }
}
