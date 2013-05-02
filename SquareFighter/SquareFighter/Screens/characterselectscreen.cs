#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;
using SquareFighter.SFClasses;
#endregion

namespace SquareFighter
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class CharacterSelectScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        Random random = new Random();

        float pauseAlpha;

        InputAction pauseAction;

        PlayerIndex inputPlayer;

        SFCharacter[] characters = new SFCharacter[4];


        Player1Selector p1Selector;
        Player2Selector p2Selector;

        bool isLoading = false;
        float loadWaitTime = 2f;

        int characerHeight = 100;
        
        int p1Index = 0, p2Index = 1;
        void p1IndexLeft()
        {
            p1Index--;
            if (p1Index < 0) p1Index = 3;
            if (p1Index == p2Index) p1IndexLeft();
        }
        void p1IndexRight()
        {
            p1Index++;
            if (p1Index > 3) p1Index = 0;
            if (p1Index == p2Index) p1IndexRight();
        }
        void p2IndexLeft()
        {
            p2Index--;
            if (p2Index < 0) p2Index = 3;
            if (p2Index == p1Index) p2IndexLeft();
        }
        void p2IndexRight()
        {
            p2Index++;
            if (p2Index > 3) p2Index = 3;
            if (p2Index == p1Index) p2IndexRight();
        }

        void p1Select()
        {
            if (!p1Selector.hasSelected)
            {
                p1Selector.select();
                if (p2Selector.hasSelected)
                {
                    isLoading = true;
                }
            }
        }

        void p2Select()
        {
            if (!p2Selector.hasSelected)
            {
                p2Selector.select();
                if (p1Selector.hasSelected)
                {
                    isLoading = true;
                }
            }
        }

        void LoadGame()
        {
            SFCharacter p1Copy = characters[p1Index].Copy();
            SFCharacter p2Copy = characters[p2Index].Copy();
            LoadingScreen.Load(ScreenManager, true, this.ControllingPlayer, new GameplayScreen(p1Copy, p2Copy));
        }

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public CharacterSelectScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                gameFont = content.Load<SpriteFont>("gamefont");

                SFSquare.SFTexture = content.Load<Texture2D>("blank");

                #region Character Initialization

                characters[0] = new OrangeCharacter();
                characters[1] = new YellowCharacter();
                characters[2] = new BlueCharacter();
                characters[3] = new WhiteCharacter();

                characters[0].Position = new Vector2(SFGame.ScreenWidth * 1 / 5, SFGame.ScreenHeight - 20);
                characters[1].Position = new Vector2(SFGame.ScreenWidth * 2 / 5, SFGame.ScreenHeight - 20);
                characters[2].Position = new Vector2(SFGame.ScreenWidth * 3 / 5, SFGame.ScreenHeight - 20);
                characters[3].Position = new Vector2(SFGame.ScreenWidth * 4 / 5, SFGame.ScreenHeight - 20);

                p1Selector = new Player1Selector();
                p1Selector.setColor(characters[p1Index].body.lineColor);
                p1Selector.Position = new Vector2(characters[p1Index].Position.X, characters[p2Index].Position.Y - characerHeight);

                p2Selector = new Player2Selector();
                p2Selector.setColor(characters[p2Index].body.lineColor);
                p2Selector.Position = new Vector2(characters[p2Index].Position.X, characters[p2Index].Position.Y - characerHeight);

                #endregion
                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }

        }


        public override void Deactivate()
        {
            base.Deactivate();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                foreach (SFCharacter c in characters)
                {
                    c.Velocity = Vector2.Zero;
                    c.Update(gameTime);
                }
                
                p1Selector.Update(gameTime);
                p2Selector.Update(gameTime);

                if (isLoading)
                {
                    loadWaitTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (loadWaitTime <= 0)
                        LoadGame();
                }
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Player 1 Input
                if (input.IsNewKeyPress(Keys.A, null, out inputPlayer))
                {
                    if (!p1Selector.hasSelected)
                    {
                        p1IndexLeft();
                        p1Selector.setColor(characters[p1Index].body.lineColor);
                        p1Selector.Position = new Vector2(characters[p1Index].Position.X, characters[p1Index].Position.Y - characerHeight);
                    }
                }
                if (input.IsNewKeyPress(Keys.D, null, out inputPlayer))
                {
                    if (!p1Selector.hasSelected)
                    {
                        p1IndexRight();
                        p1Selector.setColor(characters[p1Index].body.lineColor);
                        p1Selector.Position = new Vector2(characters[p1Index].Position.X, characters[p1Index].Position.Y - characerHeight);
                    }
                }


                if (input.IsNewKeyPress(Keys.T, null, out inputPlayer))
                {
                    characters[p1Index].punch();
                    p1Select();
                }
                if (input.IsNewKeyPress(Keys.Y, null, out inputPlayer))
                {
                    characters[p1Index].kick();
                    p1Select();
                }
                if (input.IsNewKeyPress(Keys.U, null, out inputPlayer))
                {
                    characters[p1Index].special();
                    p1Select();
                }


                // Player 2 Input
                if (input.IsNewKeyPress(Keys.Left, null, out inputPlayer))
                {
                    if (!p2Selector.hasSelected)
                    {
                        p2IndexLeft();
                        p2Selector.setColor(characters[p2Index].body.lineColor);
                        p2Selector.Position = new Vector2(characters[p2Index].Position.X, characters[p2Index].Position.Y - characerHeight);
                    }
                }
                if (input.IsNewKeyPress(Keys.Right, null, out inputPlayer))
                {
                    if (!p2Selector.hasSelected)
                    {
                        p2IndexRight();
                        p2Selector.setColor(characters[p2Index].body.lineColor);
                        p2Selector.Position = new Vector2(characters[p2Index].Position.X, characters[p2Index].Position.Y - characerHeight);
                    }
                }

                // debug input
                if (input.IsNewKeyPress(Keys.B, null, out inputPlayer))
                {
                    foreach (SFCharacter c in characters)
                        c.block();
                }
                if (input.IsNewKeyPress(Keys.J, null, out inputPlayer))
                {
                    foreach (SFCharacter c in characters)
                        c.jump();
                }
                if (input.IsNewKeyPress(Keys.H, null, out inputPlayer))
                {
                    foreach (SFCharacter c in characters)
                        c.hurt();
                }
                if (input.IsNewKeyPress(Keys.K, null, out inputPlayer))
                {
                    foreach (SFCharacter c in characters)
                        c.kill();
                }


                if (input.IsNewKeyPress(Keys.NumPad1, null, out inputPlayer))
                {
                    characters[p2Index].punch();
                    p2Select();
                }
                if (input.IsNewKeyPress(Keys.NumPad2, null, out inputPlayer))
                {
                    characters[p2Index].kick();
                    p2Select();
                }
                if (input.IsNewKeyPress(Keys.NumPad3, null, out inputPlayer))
                {
                    characters[p2Index].special();
                    p2Select();
                }
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            foreach (SFCharacter c in characters)
            {
                c.Draw(this);
            }

            p1Selector.Draw(this);
            p2Selector.Draw(this);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
