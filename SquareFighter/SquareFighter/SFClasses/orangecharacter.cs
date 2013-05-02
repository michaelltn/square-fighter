using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameStateManagement;

namespace SquareFighter.SFClasses
{
    class OrangeCharacter : SFCharacter
    {
        public override SFCharacter Copy()
        {
            return new OrangeCharacter();
        }

        override protected void init()
        {
            this.lineColor = new Color(255, 127, 39);
            this.lineThickness = 5;

            body = new SFSquare(new Rectangle(-20, -80, 40, 70));
            body.lineColor = this.lineColor;
            body.lineThickness = this.lineThickness;

            arm1 = new SFSquare(new Rectangle(10, -60, 20, 20));
            arm1.lineColor = this.lineColor;
            arm1.lineThickness = this.lineThickness;

            arm2 = new SFSquare(new Rectangle(-18, -60, 20, 20));
            arm2.lineColor = this.lineColor;
            arm2.lineThickness = this.lineThickness;

            leg1 = new SFSquare(new Rectangle(10, -20, 20, 20));
            leg1.lineColor = this.lineColor;
            leg1.lineThickness = this.lineThickness;

            leg2 = new SFSquare(new Rectangle(-30, -20, 20, 20));
            leg2.lineColor = this.lineColor;
            leg2.lineThickness = this.lineThickness;
        }

        protected override void setupStats()
        {
            punchPower = 8;
            punchPriority = 2;
            punchKnockBack = 25;
            punchVelocityX = 0;

            kickPower = 8;
            kickPriority = 1;
            kickKnockBack = 0;
            kickVelocityX = 0;

            specialPower = 16;
            specialPriority = 0;
            specialKnockBack = 300;
            specialVelocityX = 200;

            walkSpeed = 200f;
            jumpPower = 700f;
        }

        SFAnimation anim = null;
        Rectangle otherRect = new Rectangle();
        Rectangle otherRect2 = new Rectangle();

        override protected void setupIdleAnim()
        {
            otherRect = body.defaultRect;
            otherRect.X -= 2;
            otherRect.Y += 4;
            otherRect.Width += 4;
            otherRect.Height -= 4;
            anim = SFAnimation.Create(IdleAnimRef, body.defaultRect, 3, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.25f);
                anim.addFrame(body.defaultRect, 0.25f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Body, anim, true);

            otherRect = arm1.defaultRect;
            otherRect.X += 4;
            otherRect.Y += 4;
            otherRect.Width += 2;
            otherRect.Height += 2;
            anim = SFAnimation.Create(IdleAnimRef, arm1.defaultRect, 3, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.25f);
                anim.addFrame(arm1.defaultRect, 0.25f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm1, anim, true);

            otherRect = arm2.defaultRect;
            otherRect.X -= 4;
            otherRect.Y += 4;
            otherRect.Width += 2;
            otherRect.Height += 2;
            anim = SFAnimation.Create(IdleAnimRef, arm2.defaultRect, 3, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.25f);
                anim.addFrame(arm2.defaultRect, 0.25f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm2, anim, true);

            otherRect = leg1.defaultRect;
            otherRect.X += 2;
            otherRect.Y -= 2;
            otherRect.Height += 2;
            anim = SFAnimation.Create(IdleAnimRef, leg1.defaultRect, 3, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.25f);
                anim.addFrame(leg1.defaultRect, 0.25f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg1, anim, true);

            otherRect = leg2.defaultRect;
            otherRect.X -= 2;
            otherRect.Y -= 2;
            otherRect.Height += 2;
            anim = SFAnimation.Create(IdleAnimRef, leg2.defaultRect, 3, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.25f);
                anim.addFrame(leg2.defaultRect, 0.25f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg2, anim, true);
        }

        protected override void setupHurtAnim()
        {
            otherRect = body.defaultRect;
            otherRect.X -= 10;
            otherRect.Y -= 10;
            otherRect.Width -= 10;
            anim = SFAnimation.Create(HurtAnimRef, body.defaultRect, 3);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.15f);
                anim.addFrame(body.defaultRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Body, anim);

            otherRect = arm1.defaultRect;
            otherRect.Y -= 20;
            anim = SFAnimation.Create(HurtAnimRef, arm1.defaultRect, 3);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.15f);
                anim.addFrame(arm1.defaultRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm1, anim);

            otherRect = arm2.defaultRect;
            otherRect.X -= 25;
            otherRect.Y -= 20;
            anim = SFAnimation.Create(HurtAnimRef, arm2.defaultRect, 3);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.15f);
                anim.addFrame(arm2.defaultRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm2, anim);

            otherRect = leg1.defaultRect;
            otherRect.Y -= 20;
            anim = SFAnimation.Create(HurtAnimRef, leg1.defaultRect, 3);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.15f);
                anim.addFrame(leg1.defaultRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg1, anim);

            otherRect = leg2.defaultRect;
            otherRect.Y -= 10;
            anim = SFAnimation.Create(HurtAnimRef, leg2.defaultRect, 3);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.15f);
                anim.addFrame(leg2.defaultRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg2, anim);
        }

        override protected void setupDeadAnim()
        {
            otherRect = body.defaultRect;
            otherRect.X -= 40;
            otherRect.Y -= 80;
            otherRect.Width = body.defaultRect.Height;
            otherRect.Height = body.defaultRect.Width;
            otherRect2 = otherRect;
            otherRect2.X -= 10;
            otherRect2.Y = -otherRect2.Height;
            anim = SFAnimation.Create(DeadAnimRef, body.defaultRect, 3, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.7f);
                anim.addFrame(otherRect2, 0.3f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Body, anim);

            otherRect = arm1.defaultRect;
            otherRect.X -= 70;
            otherRect.Y -= 120;
            otherRect2 = otherRect;
            otherRect2.X -= 10;
            otherRect2.Y = -otherRect2.Height;
            anim = SFAnimation.Create(DeadAnimRef, arm1.defaultRect, 3, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.7f);
                anim.addFrame(otherRect2, 0.3f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm1, anim);

            otherRect = arm2.defaultRect;
            otherRect.X -= 70;
            otherRect.Y -= 100;
            otherRect2 = otherRect;
            otherRect2.X -= 10;
            otherRect2.Y = -otherRect2.Height;
            anim = SFAnimation.Create(DeadAnimRef, arm2.defaultRect, 3, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.7f);
                anim.addFrame(otherRect2, 0.3f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm2, anim);

            otherRect = leg1.defaultRect;
            otherRect.X -= 10;
            otherRect.Y -= 150;
            otherRect2 = otherRect;
            otherRect2.X -= 10;
            otherRect2.Y = -otherRect2.Height - body.defaultRect.Width;
            anim = SFAnimation.Create(DeadAnimRef, leg1.defaultRect, 3, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.7f);
                anim.addFrame(otherRect2, 0.3f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg1, anim);

            otherRect = leg2.defaultRect;
            otherRect.Y -= 140;
            otherRect2 = otherRect;
            otherRect2.X -= 10;
            otherRect2.Y = -otherRect2.Height - body.defaultRect.Width;
            anim = SFAnimation.Create(DeadAnimRef, leg2.defaultRect, 3, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.7f);
                anim.addFrame(otherRect2, 0.3f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg2, anim);
        }

        protected override void setupWinAnim()
        {
            otherRect = body.defaultRect;
            anim = SFAnimation.Create(WinAnimRef, body.defaultRect, 2, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 2.0f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Body, anim);

            otherRect = arm1.defaultRect;
            otherRect.X = body.defaultRect.X + body.defaultRect.Width;
            otherRect2 = otherRect;
            otherRect2.Y = body.defaultRect.Y - arm1.defaultRect.Height;
            anim = SFAnimation.Create(WinAnimRef, arm1.defaultRect, 6, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.4f);
                anim.addFrame(otherRect2, 0.4f);
                anim.addFrame(otherRect, 0.4f);
                anim.addFrame(otherRect2, 0.4f);
                anim.addFrame(otherRect, 0.4f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm1, anim);

            otherRect = arm2.defaultRect;
            otherRect.X = body.defaultRect.X - arm2.defaultRect.Width;
            otherRect2 = otherRect;
            otherRect2.Y = body.defaultRect.Y - arm2.defaultRect.Height;
            anim = SFAnimation.Create(WinAnimRef, arm2.defaultRect, 6, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect2, 0.4f);
                anim.addFrame(otherRect, 0.4f);
                anim.addFrame(otherRect2, 0.4f);
                anim.addFrame(otherRect, 0.4f);
                anim.addFrame(otherRect2, 0.4f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm2, anim);

            otherRect = leg1.defaultRect;
            otherRect.X = body.defaultRect.X + body.defaultRect.Width - 5;
            anim = SFAnimation.Create(WinAnimRef, leg1.defaultRect, 2, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg1, anim);

            otherRect = leg2.defaultRect;
            otherRect.X = body.defaultRect.X - leg2.defaultRect.Width + 5;
            anim = SFAnimation.Create(WinAnimRef, leg2.defaultRect, 2, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg2, anim);
        }

        override protected void setupWalkForwardAnim()
        {
            otherRect.Width = (leg1.defaultRect.Width + leg2.defaultRect.Width)/2;
            otherRect.Height = (leg1.defaultRect.Height + leg2.defaultRect.Height) / 2;
            otherRect.X = (leg1.defaultRect.X + leg2.defaultRect.X) / 2;
            otherRect.Y = (leg1.defaultRect.Y + leg2.defaultRect.Y) / 2;

            otherRect2.Width = (leg1.defaultRect.Width + leg2.defaultRect.Width) / 2;
            otherRect2.Height = (leg1.defaultRect.Height + leg2.defaultRect.Height) / 2;
            otherRect2.X = (leg1.defaultRect.X + leg2.defaultRect.X) / 2;
            otherRect2.Y = (leg1.defaultRect.Y + leg2.defaultRect.Y) / 2 - 10;
            

            anim = SFAnimation.Create(WalkForwardAnimRef, leg1.defaultRect, 5, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.2f);
                anim.addFrame(leg2.defaultRect, 0.2f);
                anim.addFrame(otherRect2, 0.2f);
                anim.addFrame(leg1.defaultRect, 0.2f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg1, anim);

            anim = SFAnimation.Create(WalkForwardAnimRef, leg2.defaultRect, 5, true);
            if (anim != null)
            {
                anim.addFrame(otherRect2, 0.2f);
                anim.addFrame(leg1.defaultRect, 0.2f);
                anim.addFrame(otherRect, 0.2f);
                anim.addFrame(leg2.defaultRect, 0.2f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg2, anim);
        }

        override protected void setupWalkBackwardAnim()
        {
            otherRect.Width = (leg1.defaultRect.Width + leg2.defaultRect.Width) / 2;
            otherRect.Height = (leg1.defaultRect.Height + leg2.defaultRect.Height) / 2;
            otherRect.X = (leg1.defaultRect.X + leg2.defaultRect.X) / 2;
            otherRect.Y = (leg1.defaultRect.Y + leg2.defaultRect.Y) / 2 - 10;

            otherRect2.Width = (leg1.defaultRect.Width + leg2.defaultRect.Width) / 2;
            otherRect2.Height = (leg1.defaultRect.Height + leg2.defaultRect.Height) / 2;
            otherRect2.X = (leg1.defaultRect.X + leg2.defaultRect.X) / 2;
            otherRect2.Y = (leg1.defaultRect.Y + leg2.defaultRect.Y) / 2;


            anim = SFAnimation.Create(WalkBackwardAnimRef, leg1.defaultRect, 5, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.2f);
                anim.addFrame(leg2.defaultRect, 0.2f);
                anim.addFrame(otherRect2, 0.2f);
                anim.addFrame(leg1.defaultRect, 0.2f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg1, anim);

            anim = SFAnimation.Create(WalkBackwardAnimRef, leg2.defaultRect, 5, true);
            if (anim != null)
            {
                anim.addFrame(otherRect2, 0.2f);
                anim.addFrame(leg1.defaultRect, 0.2f);
                anim.addFrame(otherRect, 0.2f);
                anim.addFrame(leg2.defaultRect, 0.2f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg2, anim);
        }

        override protected void setupJumpAnim()
        {
            otherRect = leg1.defaultRect;
            otherRect.Width -= 4;
            otherRect.Height += 15;

            otherRect2 = leg1.defaultRect;
            otherRect2.Y -= 10;

            anim = SFAnimation.Create(JumpAnimRef, leg1.defaultRect, 3, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.1f);
                anim.addFrame(otherRect2, 0.3f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg1, anim);


            otherRect = leg2.defaultRect;
            otherRect.Width -= 4;
            otherRect.Height += 15;

            otherRect2 = leg2.defaultRect;
            otherRect2.Y -= 10;

            anim = SFAnimation.Create(JumpAnimRef, leg2.defaultRect, 3, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.1f);
                anim.addFrame(otherRect2, 0.3f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg2, anim);
        }

        override protected void setupPunchAnim()
        {
            otherRect = arm1.defaultRect;
            otherRect.X -= 20;

            otherRect2 = arm1.defaultRect;
            otherRect2.Width *= 3;

            anim = SFAnimation.Create(PunchAnimRef, arm1.defaultRect, 4);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.2f);
                anim.addFrame(otherRect2, 0.1f);
                anim.addFrame(arm1.defaultRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm1, anim);
        }

        override protected void setupKickAnim()
        {
            otherRect = leg1.defaultRect;
            otherRect.X -= 20;

            otherRect2 = leg1.defaultRect;
            otherRect2.Y -= 20;
            otherRect2.Width *= 4;

            anim = SFAnimation.Create(KickAnimRef, leg1.defaultRect, 4);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.2f);
                anim.addFrame(otherRect2, 0.1f);
                anim.addFrame(leg1.defaultRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg1, anim);
        }

        override protected void setupSpecialAnim()
        {
            specialAttackUsesArm1 = true;
            specialAttackUsesArm2 = true;

            SetSpeicalAnimationTrackingSquare(arm1);

            otherRect = arm1.defaultRect;
            otherRect.X -= 20;
            otherRect.Y += 10;

            otherRect2 = arm1.defaultRect;
            otherRect2.Width *= 3;
            otherRect2.Y += 10;

            anim = SFAnimation.Create(SpecialAnimRef, arm1.defaultRect, 4);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.2f);
                anim.addFrame(otherRect2, 0.1f);
                anim.addFrame(arm1.defaultRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm1, anim);

            otherRect = arm1.defaultRect;
            otherRect.X -= 20;
            otherRect.Y -= 10;

            otherRect2 = arm1.defaultRect;
            otherRect2.Width *= 3;
            otherRect2.Y -= 10;

            anim = SFAnimation.Create(SpecialAnimRef, arm2.defaultRect, 4);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.2f);
                anim.addFrame(otherRect2, 0.1f);
                anim.addFrame(arm2.defaultRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm2, anim);
        }

        override protected void setupBlockAnim()
        {
            otherRect = body.defaultRect;
            otherRect.Y = -body.defaultRect.Height;
            anim = SFAnimation.Create(BlockAnimRef, body.defaultRect, 2, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Body, anim);

            otherRect = arm1.defaultRect;
            otherRect.Width = 4;
            otherRect.Height = body.defaultRect.Height - 20;
            otherRect.X = body.defaultRect.X + body.defaultRect.Width + 8;
            otherRect.Y = body.defaultRect.Y;
            anim = SFAnimation.Create(BlockAnimRef, arm1.defaultRect, 2, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm1, anim);

            otherRect = arm2.defaultRect;
            otherRect.Width = body.defaultRect.Width - 5;
            otherRect.Height = 4;
            otherRect.X = body.defaultRect.X + 5;
            otherRect.Y = -body.defaultRect.Height - 8;
            anim = SFAnimation.Create(BlockAnimRef, arm2.defaultRect, 2, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Arm2, anim);

            otherRect = leg1.defaultRect;
            otherRect.X = body.defaultRect.X + body.defaultRect.Width - leg1.defaultRect.Width;
            otherRect.Y = -leg1.defaultRect.Height;
            anim = SFAnimation.Create(BlockAnimRef, leg1.defaultRect, 2, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg1, anim);

            otherRect = leg2.defaultRect;
            otherRect.X = body.defaultRect.X;
            otherRect.Y = -leg2.defaultRect.Height;
            anim = SFAnimation.Create(BlockAnimRef, leg2.defaultRect, 2, false, true);
            if (anim != null)
            {
                anim.addFrame(otherRect, 0.1f);
            }
            this.addAnimation(SFCharacter.CharacterPiece.Leg2, anim);
        }

    }
}
