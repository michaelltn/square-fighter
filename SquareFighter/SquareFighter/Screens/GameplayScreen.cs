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
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        Random random = new Random();

        float pauseAlpha;

        InputAction pauseAction;
        PlayerIndex inputPlayer;

        SFCharacter p1;
        SFCharacter p2;

        float gameOverTime = 3f;

        #endregion


        #region Health

        int p1Health = 100;
        int p2Health = 100;

        SFSquare p1HealthBar;
        SFSquare p2HealthBar;

        Vector2 healthScale = new Vector2(1f, 1f);

        void damageP1(int value, float knockBack = 0)
        {
            p1Health -= value;
            if (p1Health < 0) p1Health = 0;
            p1HealthBar.defaultRect.Width = p1Health;

            if (p1Health > 0)
                p1.hurt(knockBack);
            else
                p1.kill();
        }

        void damageP2(int value, float knockBack = 0)
        {
            p2Health -= value;
            if (p2Health < 0) p2Health = 0;
            p2HealthBar.defaultRect.Width = p2Health;
            p2HealthBar.defaultRect.X = SFGame.ScreenWidth - p2HealthBar.defaultRect.Width - 10;

            if (p2Health > 0)
                p2.hurt(knockBack);
            else
                p2.kill();
        }

        #endregion


        #region Physics

        int leftWallPosition = 30;
        int rightWallPosition = SFGame.ScreenWidth - 30;
        int floorPosition = SFGame.ScreenHeight - 20;
        float gravity = 1280.0f;
        Vector2 newPosition = Vector2.Zero;

        float pushDistance = 50.0f;
        float pushFactor = 6.0f;

        bool p1GotHit, p2GotHit;
        void AttackResolution()
        {
            p1GotHit = false;
            p2GotHit = false;
            if (p1.isAttacking && p1.AttackDidHit(p2))
            {
                p2GotHit = true;
            }
            if (p2.isAttacking && p2.AttackDidHit(p1))
            {
                p1GotHit = true;
            }

            if (p1GotHit && p2GotHit)
            {
                if (p1.currentPriority > p2.currentPriority)
                    p1GotHit = false;
                if (p2.currentPriority > p1.currentPriority)
                    p2GotHit = false;
            }

            if (p1GotHit)
            {
                damageP1(p2.currentPower, p2.currentKnockBack);
            }

            if (p2GotHit)
            {
                damageP2(p1.currentPower, p1.currentKnockBack);
            }

            if (p1.isDead && !p2.isDead)
                win(p2);
            if (p2.isDead && !p1.isDead)
                win(p1);
            if (p1.isDead && p2.isDead)
                tie();
        }

        void ApplyPushBack(GameTime gameTime)
        {
            if (p1.Position.Y == floorPosition && p2.Position.Y == floorPosition)
            {
                float playerDistance = Math.Abs(p1.Position.X - p2.Position.X);
                if (playerDistance < pushDistance)
                {
                    if (p1.Position.X < p2.Position.X)
                    {
                        p1.Position = new Vector2(
                            p1.Position.X - (pushDistance - playerDistance) * pushFactor * (float)gameTime.ElapsedGameTime.TotalSeconds,
                            p1.Position.Y);
                        p2.Position = new Vector2(
                            p2.Position.X + (pushDistance - playerDistance) * pushFactor * (float)gameTime.ElapsedGameTime.TotalSeconds,
                            p2.Position.Y);
                    }
                    else
                    {
                        p1.Position = new Vector2(
                            p1.Position.X + (pushDistance - playerDistance) * pushFactor * (float)gameTime.ElapsedGameTime.TotalSeconds,
                            p1.Position.Y);
                        p2.Position = new Vector2(
                            p2.Position.X - (pushDistance - playerDistance) * pushFactor * (float)gameTime.ElapsedGameTime.TotalSeconds,
                            p2.Position.Y);
                    }
                }
            }
        }

        #endregion


        #region Game State

        enum GameState { Playing, P1Win, P2Win, Tie };
        GameState state;

        void win(SFCharacter winner)
        {
            winner.win();
            if (winner == p1)
                state = GameState.P1Win;
            else
                state = GameState.P2Win;
        }

        void tie()
        {
            state = GameState.Tie;
        }

        #endregion




        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(SFCharacter player1Character, SFCharacter player2Character)
        {
            if (player1Character == null || player2Character == null)
            {
                throw new Exception("SFCharacter cannot be null");
            }

            state = GameState.Playing;

            p1 = player1Character;
            p2 = player2Character;

            p1.Position = new Vector2(SFGame.ScreenWidth / 4, floorPosition);
            p2.Position = new Vector2(SFGame.ScreenWidth * 3 / 4, floorPosition);

            if (p1.isFacingLeft)
                p1.turnAround();

            if (p2.isFacingRight)
                p2.turnAround();

            p1HealthBar = new SFSquare(new Rectangle(10, 10, p1Health, 20));
            p1HealthBar.lineThickness = p1.body.lineThickness;
            p1HealthBar.lineColor = p1.body.lineColor;
            p2HealthBar = new SFSquare(new Rectangle(SFGame.ScreenWidth - p2Health - 10, 10, p2Health, 20));
            p2HealthBar.lineThickness = p2.body.lineThickness;
            p2HealthBar.lineColor = p2.body.lineColor;


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

                // initialize the characters health, etc...

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
                // update player 1
                if (p1.Position.Y < floorPosition)
                {
                    p1.Velocity = new Vector2(p1.Velocity.X, p1.Velocity.Y + gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                p1.Update(gameTime);

                newPosition = p1.Position;
                if (p1.Velocity.Y > 0 && newPosition.Y > floorPosition)
                {
                    newPosition.Y = floorPosition;
                    p1.land();
                }
                if (newPosition.X < leftWallPosition) newPosition.X = leftWallPosition;
                if (newPosition.X > rightWallPosition) newPosition.X = rightWallPosition;

                p1.Position = newPosition;


                // update player 2
                if (p2.Position.Y < floorPosition)
                {
                    p2.Velocity = new Vector2(p2.Velocity.X, p2.Velocity.Y + gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                p2.Update(gameTime);

                newPosition = p2.Position;
                if (p2.Velocity.Y > 0 && newPosition.Y > floorPosition)
                {
                    newPosition.Y = floorPosition;
                    p2.land();
                }
                if (newPosition.X < leftWallPosition) newPosition.X = leftWallPosition;
                if (newPosition.X > rightWallPosition) newPosition.X = rightWallPosition;

                p2.Position = newPosition;

                // detect and resolve attack collisions
                if (state == GameState.Playing)
                {
                    AttackResolution();

                    // correct facing
                    if (p1.Position.X < p2.Position.X)
                    {
                        if (p1.isFacingLeft && !p1.isAttacking)
                            p1.turnAround();

                        if (p2.isFacingRight && !p2.isAttacking)
                            p2.turnAround();
                    }
                    else
                    {
                        if (p1.isFacingRight && !p1.isAttacking)
                            p1.turnAround();

                        if (p2.isFacingLeft && !p2.isAttacking)
                            p2.turnAround();
                    }

                    // apply push back
                    ApplyPushBack(gameTime);
                }
                else
                {
                    gameOverTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (gameOverTime <= 0)
                    {
                        LoadingScreen.Load(ScreenManager, true, this.ControllingPlayer, new CharacterSelectScreen());
                    }
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
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                if (state == GameState.Playing)
                {
                    // player 1 input
                    if (p1.Position.Y >= floorPosition)
                    {
                        if (input.IsKeyPressed(Keys.A, null, out inputPlayer) && !input.IsKeyPressed(Keys.D, null, out inputPlayer))
                        {
                            p1.walkLeft();
                        }
                        else if (input.IsKeyPressed(Keys.D, null, out inputPlayer) && !input.IsKeyPressed(Keys.A, null, out inputPlayer))
                        {
                            p1.walkRight();
                        }
                        else
                        {
                            p1.standStill();
                        }
                        if (input.IsNewKeyPress(Keys.W, null, out inputPlayer))
                        {
                            p1.jump();
                        }
                    }

                    if (input.IsKeyPressed(Keys.S, null, out inputPlayer))
                    {
                        p1.block();
                    }
                    else if (p1.isBlocking)
                    {
                        p1.stopBlocking();
                    }


                    if (input.IsNewKeyPress(Keys.T, null, out inputPlayer))
                    {
                        p1.punch();
                    }
                    if (input.IsNewKeyPress(Keys.Y, null, out inputPlayer))
                    {
                        p1.kick();
                    }
                    if (input.IsNewKeyPress(Keys.U, null, out inputPlayer))
                    {
                        p1.special();
                    }


                    // player 2 input
                    if (p2.Position.Y >= floorPosition)
                    {
                        if (input.IsKeyPressed(Keys.Left, null, out inputPlayer) && !input.IsKeyPressed(Keys.Right, null, out inputPlayer))
                        {
                            p2.walkLeft();
                        }
                        else if (input.IsKeyPressed(Keys.Right, null, out inputPlayer) && !input.IsKeyPressed(Keys.Left, null, out inputPlayer))
                        {
                            p2.walkRight();
                        }
                        else
                        {
                            p2.standStill();
                        }
                        if (input.IsNewKeyPress(Keys.Up, null, out inputPlayer))
                        {
                            p2.jump();
                        }
                    }

                    if (input.IsKeyPressed(Keys.Down, null, out inputPlayer))
                    {
                        p2.block();
                    }
                    else if (p2.isBlocking)
                    {
                        p2.stopBlocking();
                    }


                    if (input.IsNewKeyPress(Keys.NumPad1, null, out inputPlayer))
                    {
                        p2.punch();
                    }
                    if (input.IsNewKeyPress(Keys.NumPad2, null, out inputPlayer))
                    {
                        p2.kick();
                    }
                    if (input.IsNewKeyPress(Keys.NumPad3, null, out inputPlayer))
                    {
                        p2.special();
                    }

                } // end dead check
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

            p1.Draw(this);
            p2.Draw(this);

            p1HealthBar.Draw(this, Vector2.Zero, healthScale);
            p2HealthBar.Draw(this, Vector2.Zero, healthScale);

            switch (state)
            {
                case GameState.P1Win:
                    // draw p1 win stuff
                    break;
                case GameState.P2Win:
                    // draw p2 win stuff
                    break;
                case GameState.Tie:
                    // draw tie stuff
                    break;
            }

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
