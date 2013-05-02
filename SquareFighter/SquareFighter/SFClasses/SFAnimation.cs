using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SquareFighter.SFClasses
{
    class SFAnimation
    {
        private string name;
        public string Name { get { return name; } }
        public override string ToString()
        {
            return name;
        }

        public static SFAnimation Create(string name, Rectangle firstFrame, int maximumNumberOfFrames, bool looping = false, bool clamped = false)
        {
            if (name.Length > 0 && maximumNumberOfFrames > 1)
            {
                return new SFAnimation(name, firstFrame, maximumNumberOfFrames, looping, clamped);
            }
            return null;
        }

        public static SFAnimation Clone(SFAnimation sourceAnimation)
        {
            if (sourceAnimation != null)
            {
                SFAnimation newAnimation = new SFAnimation(sourceAnimation.Name, sourceAnimation.frames[0], sourceAnimation.frames.Length, sourceAnimation.Loop, sourceAnimation.Clamp);
                for (int i = 1; i < sourceAnimation.frames.Length; i++)
                {
                    newAnimation.addFrame(sourceAnimation.frames[i], sourceAnimation.frameTimes[i]);
                }
                return newAnimation;
            }
            return null;
        }


        public bool Loop { get; set; }
        public bool Clamp { get; set; }

        private bool playing;
        public bool isPlaying { get { return playing; } }
        private bool paused;
        public bool isPaused { get { return paused; } }
        private double animationTime;
        public double AnimationTime { get { return playing ? animationTime : 0; } }

        public delegate void CompleteDelegate(SFAnimation animation);
        public event CompleteDelegate onAnimationComplete;

        private int currentStartFrame
        {
            get
            {
                if (playing)
                {
                    int index = 0;
                    double timePool = animationTime;
                    while (timePool > 0)
                    {
                        if (timePool >= frameTimes[index+1])
                        {
                            timePool -= frameTimes[++index];
                        }
                        else
                        {
                            timePool = 0;
                        }
                    }
                    return index;
                }
                return -1;
            }
        }
        
        private Rectangle[] frames;
        private Rectangle currentRect = new Rectangle();
        private double[] frameTimes; // [0] holdes the total time

        private int totalFrames;
        public int TotalFrames { get { return totalFrames; } }
        public double TotalTime { get { return totalFrames > 1 ? frameTimes[0] : 0; } }

        public bool addFrame(Rectangle rect, double frameTime)
        {
            if (totalFrames < frames.Length)
            {
                frames[totalFrames] = rect;
                frameTimes[totalFrames] = frameTime;
                frameTimes[0] += frameTime;
                totalFrames++;
            }
            return false;
        }

        public Rectangle CurrentRect
        {
            get
            {
                return currentRect;
            }
        }

        public bool getCurrrentRect(ref Rectangle rect)
        {
            if (playing)
            {
                rect = currentRect;
                return true;
            }
            rect.X = 0;
            rect.Y = 0;
            rect.Width = 0;
            rect.Height = 0;
            return false;
        }


        private SFAnimation(string name, Rectangle firstFrame, int maximumNumberOfFrames, bool looping, bool clamped)
        {
            this.name = name;
            this.Loop = looping;
            this.Clamp = clamped;

            frames = new Rectangle[maximumNumberOfFrames];
            frameTimes = new double[maximumNumberOfFrames];

            frames[0] = firstFrame;
            frameTimes[0] = 0;
            totalFrames = 1;

            playing = false;
            paused = false;
        }


        public bool play(bool restartIfPlaying = true)
        {
            if (totalFrames > 1)
            {
                if (!playing || restartIfPlaying)
                {
                    animationTime = 0;
                    currentRect = frames[0];
                }
                paused = false;
                playing = true;
                return true;
            }
            return false;
        }

        public void stop()
        {
            playing = false;
            paused = false;
            animationTime = 0;
            currentRect = frames[0];
        }

        public bool pause()
        {
            if (playing && !paused)
            {
                paused = true;
                return true;
            }
            return false;
        }

        public bool resume()
        {
            if (playing && paused)
            {
                paused = false;
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            if (playing && !paused)
            {
                animationTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (this.Loop)
                {
                    while (animationTime >= frameTimes[0])
                        animationTime -= frameTimes[0];
                }

                if (animationTime < frameTimes[0])
                {
                    int f1 = this.currentStartFrame;
                    int f2 = f1 + 1;

                    float t = (float)animationTime;
                    for (int f = 1; f < f2; f++)
                        t -= (float)frameTimes[f];
                    t /= (float)frameTimes[f2];
                    //Console.WriteLine("frame: " + f1.ToString() + "  time: " + t.ToString());

                    currentRect.X = (int)Math.Round(MathHelper.Lerp(frames[f1].X, frames[f2].X, t));
                    currentRect.Y = (int)Math.Round(MathHelper.Lerp(frames[f1].Y, frames[f2].Y, t));
                    currentRect.Width = (int)Math.Round(MathHelper.Lerp(frames[f1].Width, frames[f2].Width, t));
                    currentRect.Height = (int)Math.Round(MathHelper.Lerp(frames[f1].Height, frames[f2].Height, t));
                }
                else
                {
                    if (this.Clamp)
                    {
                        animationTime = frameTimes[0];
                        currentRect.X = (int)Math.Round((float)frames[frames.Length - 1].X);
                        currentRect.Y = (int)Math.Round((float)frames[frames.Length - 1].Y);
                        currentRect.Width = (int)Math.Round((float)frames[frames.Length - 1].Width);
                        currentRect.Height = (int)Math.Round((float)frames[frames.Length - 1].Height);
                    }
                    else
                    {
                        this.stop();
                        if (onAnimationComplete != null)
                        {
                            onAnimationComplete(this);
                        }
                    }
                }
            }
        }

    }
}
