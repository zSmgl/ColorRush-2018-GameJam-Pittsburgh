﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        GameManager gM;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 900;
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
            for(int i = 1; i < 6; i++)
            {
                baseBackground.Add(new Base_Background(Content.Load<Texture2D>("debug\\bg_" + i + ""), backgroundSize));
            }

            bg = new ScrollingBackground(baseBackground);

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

            gM = new GameManager(bg, defaultPlayer, Content.Load<Texture2D>("debug\\projectile"));




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
            gM.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
