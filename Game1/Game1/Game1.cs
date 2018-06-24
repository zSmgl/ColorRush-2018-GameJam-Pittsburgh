using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

enum GameState
{
    menu,
    play,
    gameover,
    scores
}

namespace Game1
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Background

        ScrollingBackground bg;
        List<Base_Background> baseBackground;
        Rectangle backgroundSize;

        #endregion Background
        #region Player
        Player defaultPlayer;
        #endregion Player
        #region Meteor
        List<Texture2D> meteorSizes;
        #endregion Meteor
        #region UI
        private List<Texture2D> primaryColor;
        private List<Texture2D> complementColor;
        private List<Texture2D> whiteLine;
        #endregion UI
        #region Menu Stuff
        private List<Texture2D> play;
        #endregion

        Boolean drawPrevious;
        GameState previousState;
        GameState gameState;
        MenuManager menu;
        UserInterface uI;
        GameManager gM;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1150;
            graphics.PreferredBackBufferHeight = 950;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Debug Textures
            #region Background

            baseBackground = new List<Base_Background>();
            backgroundSize = graphics.GraphicsDevice.Viewport.Bounds;
            backgroundSize.Width = backgroundSize.Width - 200;
            for(int i = 1; i < 6; i++)
            {
                baseBackground.Add(new Base_Background(Content.Load<Texture2D>("debug\\bg_" + i + ""), backgroundSize));
            }

            bg = new ScrollingBackground(baseBackground, Content.Load<Texture2D>("debug\\shader"));

            #endregion Background

            #region Player

            defaultPlayer = new Player(Content.Load<Texture2D>("debug\\player"), Content.Load<Texture2D>("debug\\hitbox"),
                new Rectangle(628 - 128, 800 - 128, 128, 128), new Rectangle(0, 0, 72, 104));
            defaultPlayer.BindableKb.Add("left", Keys.A);
            defaultPlayer.BindableKb.Add("right", Keys.D);
            defaultPlayer.BindableKb.Add("up", Keys.W);
            defaultPlayer.BindableKb.Add("down", Keys.S);
            defaultPlayer.BindableKb.Add("shoot", Keys.Space);

            #endregion Player

            #region Meteor

            meteorSizes = new List<Texture2D>();
            for (int i = 1; i < 4; i++)
            {
                meteorSizes.Add(Content.Load<Texture2D>("debug\\meteor_" + i + ""));
            }

            #endregion Meteor

            #region UI

            primaryColor = new List<Texture2D>();
            for (int i = 0; i < 4; i++)
            {
                primaryColor.Add(Content.Load<Texture2D>("ui\\prim_" + i + ""));
            }
            complementColor = new List<Texture2D>();
            for (int i = 0; i < 4; i++)
            {
                complementColor.Add(Content.Load<Texture2D>("ui\\comp_" + i + ""));
            }
            whiteLine = new List<Texture2D>();
            whiteLine.Add(Content.Load<Texture2D>("ui\\whiteline1"));
            whiteLine.Add(Content.Load<Texture2D>("ui\\whiteline2"));

            uI = new UserInterface(Content.Load<SpriteFont>("spriteFont"),
                primaryColor, complementColor, whiteLine, Content.Load<Texture2D>("ui\\blackBG"));
            #endregion UI

            #region MenuManager
            play = new List<Texture2D>();
            for (int i = 0; i < 2; i++)
            {
                play.Add(Content.Load<Texture2D>("menu\\play" + i + ""));
            }

            menu = new MenuManager(play, Content.Load<Texture2D>("menu\\menubg"));
            #endregion Menu Manager

            gM = new GameManager(bg, defaultPlayer, Content.Load<Texture2D>("debug\\projectile"), meteorSizes, uI);

            gameState = GameState.menu;

            




        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            drawPrevious = false;

            //GameStateMachine(gameTime);
            gM.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (drawPrevious)
            {
                switch(previousState)
                {
                    case GameState.menu:
                        menu.Draw(spriteBatch);
                        break;
                    case GameState.play:
                        gM.Draw(spriteBatch);
                        break;
                    case GameState.gameover:
                        scoreScreen.Draw(spriteBatch);
                        break;
                }
            }
            else
            {
                switch (gameState)
                {
                    case GameState.menu:
                        menu.Draw(spriteBatch);
                        break;
                    case GameState.play:
                        gM.Draw(spriteBatch);
                        break;
                    case GameState.gameover:
                        scoreScreen.Draw(spriteBatch);
                        break;
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private GameState GameStateMachine(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.menu:
                    this.IsMouseVisible = true;
                    if (menu.Update() == GameState.menu)
                    {
                        return GameState.menu;
                    }
                    gM.Reset();
                    this.IsMouseVisible = false;
                    drawPrevious = true;
                    return GameState.play;

                case GameState.play:
                    if( gM.Update(gameTime) == GameState.play)
                    {
                        return GameState.play;
                    }
                    scoreScreen.AddInfo(gM.GetInfo());
                    drawPrevious = true;
                    return GameState.gameover;

                case GameState.gameover:
                    if (scoreScreen.Update() == GameState.gameover)
                    {
                        return GameState.gameover;
                    }
                    drawPrevious = true;
                    return GameState.menu;
            }
        }
    }
}
