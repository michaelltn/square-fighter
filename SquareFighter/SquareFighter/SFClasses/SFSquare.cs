using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameStateManagement;


namespace SquareFighter.SFClasses
{
    class SFSquare
    {
        public static Texture2D SFTexture;

        public int lineThickness;
        public int lineOffset { get { return (lineThickness - 1) / 2; } }
        public Color lineColor;

        public List<SFAnimation> AnimationList { get; private set; }
        private SFAnimation currentAnimation, defaultAnimation;
        public SFAnimation CurrentAnimation { get { return currentAnimation; } }
        public SFAnimation DefaultAnimation { get { return defaultAnimation; } }

        public bool addAnimation(SFAnimation newAnimation, bool setAsDefault = false)
        {
            if (newAnimation == null) return false;
            if (AnimationList.Contains(newAnimation)) return false;

            AnimationList.Add(newAnimation);

            if (setAsDefault)
            {
                defaultAnimation = newAnimation;
                if (defaultAnimation.Loop == false)
                    defaultAnimation.Loop = true;
            }
            return true;
        }

        public bool PlayAnimation(string animationName, bool playDefaultIfNotFound = true)
        {
            foreach (SFAnimation a in AnimationList)
            {
                if (a.Name == animationName)
                {
                    return PlayAnimation(a);
                }
            }
            if (playDefaultIfNotFound)
            {
                return PlayAnimation(defaultAnimation);
            }
            return false;
        }

        private bool PlayAnimation(SFAnimation anim)
        {
            if (anim == null) return false;

            if (AnimationList.Contains(anim))
            {
                if (currentAnimation != null && currentAnimation != anim && currentAnimation.isPlaying)
                    currentAnimation.stop();

                currentAnimation = anim;
                if (!currentAnimation.Loop)
                {
                    currentAnimation.onAnimationComplete += onAnimationComplete;
                }
                currentAnimation.play();
            }

            return true;
        }

        private void onAnimationComplete(SFAnimation animation)
        {
            animation.onAnimationComplete -= onAnimationComplete;
            if (this.currentAnimation == animation)
            {
                this.currentAnimation = null;
                if (defaultAnimation != null)
                    this.PlayAnimation(defaultAnimation);
            }
        }

        public Rectangle defaultRect = new Rectangle();
        private Rectangle rect = new Rectangle(0, 0, 0, 0);
        public Rectangle Rect
        {
            get
            {
                if (this.currentAnimation == null)
                {
                    rect.X = defaultRect.X;
                    rect.Y = defaultRect.Y;
                    rect.Width = defaultRect.Width;
                    rect.Height = defaultRect.Height;
                }
                else
                {
                    this.currentAnimation.getCurrrentRect(ref rect);
                }
                return rect;
            }
        }
        public int Thickness { get; set; }
        private Color Color { get; set; }

        Rectangle scaledRectangle = new Rectangle();
        public Rectangle GetScaledRectangle(Vector2 scale)
        {
            scaledRectangle.Width = (int)(this.Rect.Width * Math.Abs(scale.X));
            if (scale.X > 0)
                scaledRectangle.X = (int)(this.Rect.Left * scale.X);
            else
                scaledRectangle.X = (int)(this.Rect.Right * scale.X);

            scaledRectangle.Height = (int)(this.Rect.Height * Math.Abs(scale.Y));
            if (scale.Y > 0)
                scaledRectangle.Y = (int)(this.Rect.Top * scale.Y);
            else
                scaledRectangle.Y = (int)(this.Rect.Bottom * scale.Y);

            return scaledRectangle;
        }

        Rectangle globalRectangle = new Rectangle();
        public Rectangle GetGlobalRectangle(Vector2 position, Vector2 scale)
        {
            globalRectangle = GetScaledRectangle(scale);
            globalRectangle.X += (int)position.X;
            globalRectangle.Y += (int)position.Y;
            return globalRectangle;
        }



        public SFSquare(Rectangle defaultRect)
        {
            this.defaultRect = defaultRect;
            AnimationList = new List<SFAnimation>();
        }


        public void Update(GameTime gameTime)
        {
            if (currentAnimation != null)
            {
                currentAnimation.Update(gameTime);
            }
            else
            {
                if (defaultAnimation != null)
                    this.PlayAnimation(defaultAnimation);
            }
        }


        Rectangle dr;
        Rectangle globalRect = new Rectangle();
        public void Draw(GameScreen screen, Vector2 position, Vector2 scale)
        {
            if (SFTexture != null)
            {
                SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;
                
                if (dr == null)
                {
                    dr = new Rectangle(0, 0, 0, 0);
                }

                globalRect = GetGlobalRectangle(position, scale);

                // Left Line
                dr.X = (int)(globalRect.Left - lineOffset);
                dr.Y = (int)(globalRect.Top - lineOffset);
                dr.Width = this.lineThickness;
                dr.Height = (int)(globalRect.Height + lineThickness);
                spriteBatch.Draw(SFTexture, dr, null, this.lineColor);

                // Right Line
                dr.X = (int)(globalRect.Right - lineOffset);
                dr.Y = (int)(globalRect.Top - lineOffset);
                dr.Width = this.lineThickness;
                dr.Height = (int)(globalRect.Height + lineThickness);
                spriteBatch.Draw(SFTexture, dr, null, this.lineColor);

                // Top Line
                dr.X = (int)(globalRect.Left + lineOffset);
                dr.Y = (int)(globalRect.Top - lineOffset);
                dr.Width = (int)(globalRect.Width - lineOffset * 2);
                dr.Height = this.lineThickness;
                spriteBatch.Draw(SFTexture, dr, null, this.lineColor);

                // Bottom Line
                dr.X = (int)(globalRect.Left + lineOffset);
                dr.Y = (int)(globalRect.Bottom - lineOffset);
                dr.Width = (int)(globalRect.Width - lineOffset * 2);
                dr.Height = this.lineThickness;
                spriteBatch.Draw(SFTexture, dr, null, this.lineColor);

            }
        }

    }
}
