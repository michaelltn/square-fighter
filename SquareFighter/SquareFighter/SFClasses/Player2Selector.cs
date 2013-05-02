using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameStateManagement;


namespace SquareFighter.SFClasses
{
    class Player2Selector: SFChracterSelector
    {
        Rectangle otherRect = new Rectangle();
        SFAnimation anim;

        protected override void init()
        {
            squares = new SFSquare[5];
            squares[0] = new SFSquare(new Rectangle(-6, -4,  12, 4));
            squares[1] = new SFSquare(new Rectangle(-6, -12,  4, 8));
            squares[2] = new SFSquare(new Rectangle(-6, -16, 12, 4));
            squares[3] = new SFSquare(new Rectangle( 2, -24,  4, 8));
            squares[4] = new SFSquare(new Rectangle(-6, -28, 12, 4));

            foreach (SFSquare s in squares)
                s.lineThickness = 1;


            // default animation
            otherRect = squares[0].defaultRect;
            otherRect.Width += 4;
            otherRect.X -= 2;
            otherRect.Height -= 1;
            otherRect.Y += 1;
            anim = SFAnimation.Create(defaultAnim, squares[0].defaultRect, 3, true);
            anim.addFrame(otherRect, 0.25);
            anim.addFrame(squares[0].defaultRect, 0.25);
            squares[0].addAnimation(anim);

            otherRect = squares[1].defaultRect;
            otherRect.Width += 2;
            otherRect.X -= 3;
            otherRect.Height -= 2;
            otherRect.Y += 3;
            anim = SFAnimation.Create(defaultAnim, squares[1].defaultRect, 3, true);
            anim.addFrame(otherRect, 0.25);
            anim.addFrame(squares[1].defaultRect, 0.25);
            squares[1].addAnimation(anim);

            otherRect = squares[2].defaultRect;
            otherRect.Width += 4;
            otherRect.X -= 2;
            otherRect.Height -= 1;
            otherRect.Y += 4;
            anim = SFAnimation.Create(defaultAnim, squares[2].defaultRect, 3, true);
            anim.addFrame(otherRect, 0.25);
            anim.addFrame(squares[2].defaultRect, 0.25);
            squares[2].addAnimation(anim);

            otherRect = squares[3].defaultRect;
            otherRect.Width += 2;
            otherRect.X += 1;
            otherRect.Height -= 2;
            otherRect.Y += 6;
            anim = SFAnimation.Create(defaultAnim, squares[3].defaultRect, 3, true);
            anim.addFrame(otherRect, 0.25);
            anim.addFrame(squares[3].defaultRect, 0.25);
            squares[3].addAnimation(anim);

            otherRect = squares[4].defaultRect;
            otherRect.Width += 4;
            otherRect.X -= 2;
            otherRect.Height -= 1;
            otherRect.Y += 7;
            anim = SFAnimation.Create(defaultAnim, squares[4].defaultRect, 3, true);
            anim.addFrame(otherRect, 0.25);
            anim.addFrame(squares[4].defaultRect, 0.25);
            squares[4].addAnimation(anim);


            // select animation
            anim = SFAnimation.Create(selectAnim, squares[0].defaultRect, 5);
            anim.addFrame(squares[1].defaultRect, 0.125);
            anim.addFrame(squares[2].defaultRect, 0.125);
            anim.addFrame(squares[3].defaultRect, 0.125);
            anim.addFrame(squares[4].defaultRect, 0.125);
            squares[0].addAnimation(anim);

            anim = SFAnimation.Create(selectAnim, squares[1].defaultRect, 5);
            anim.addFrame(squares[2].defaultRect, 0.125);
            anim.addFrame(squares[3].defaultRect, 0.125);
            anim.addFrame(squares[4].defaultRect, 0.125);
            anim.addFrame(squares[0].defaultRect, 0.125);
            squares[1].addAnimation(anim);

            anim = SFAnimation.Create(selectAnim, squares[2].defaultRect, 5);
            anim.addFrame(squares[3].defaultRect, 0.125);
            anim.addFrame(squares[4].defaultRect, 0.125);
            anim.addFrame(squares[0].defaultRect, 0.125);
            anim.addFrame(squares[1].defaultRect, 0.125);
            squares[2].addAnimation(anim);

            anim = SFAnimation.Create(selectAnim, squares[3].defaultRect, 5);
            anim.addFrame(squares[4].defaultRect, 0.125);
            anim.addFrame(squares[0].defaultRect, 0.125);
            anim.addFrame(squares[1].defaultRect, 0.125);
            anim.addFrame(squares[2].defaultRect, 0.125);
            squares[3].addAnimation(anim);

            anim = SFAnimation.Create(selectAnim, squares[4].defaultRect, 5);
            anim.addFrame(squares[0].defaultRect, 0.125);
            anim.addFrame(squares[1].defaultRect, 0.125);
            anim.addFrame(squares[2].defaultRect, 0.125);
            anim.addFrame(squares[3].defaultRect, 0.125);
            squares[4].addAnimation(anim);

        }
    }
}
