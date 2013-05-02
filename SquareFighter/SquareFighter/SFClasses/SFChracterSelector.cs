using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameStateManagement;


namespace SquareFighter.SFClasses
{
    abstract class SFChracterSelector
    {

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }

        public void setColor(Color color)
        {
            foreach (SFSquare s in squares)
            {
                s.lineColor = color;
            }
        }

        #region Initialization

        public SFChracterSelector()
        {
            Scale = new Vector2(1f, 1f);

            this.init();

            foreach (SFSquare s in squares)
                s.PlayAnimation(defaultAnim);
        }

        abstract protected void init();

        #endregion


        #region Animation and Drawing

        protected const string defaultAnim = "default";
        protected const string selectAnim = "select";

        protected SFSquare[] squares;

        public void Draw(GameScreen screen)
        {
            foreach (SFSquare s in squares)
            {
                s.Draw(screen, this.Position, this.Scale);
            }
        }

        #endregion


        #region selection

        bool selected = false;
        public bool hasSelected { get { return selected; } }

        public void select()
        {
            if (!selected)
            {
                selected = true;
                foreach (SFSquare s in squares)
                {
                    s.PlayAnimation(selectAnim);
                }
            }

        }
        #endregion


        public void Update(GameTime gameTime)
        {
            foreach (SFSquare s in squares)
            {
                s.Update(gameTime);
            }
        }
    }
}
