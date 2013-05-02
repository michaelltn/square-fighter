using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameStateManagement;


namespace SquareFighter.SFClasses
{
    abstract class SFCharacter
    {
        #region Animation Constants

        public const String IdleAnimRef = "idle";
        public const String HurtAnimRef = "hurt";
        public const String DeadAnimRef = "dead";
        public const String WinAnimRef = "win";
        public const String WalkForwardAnimRef = "walk forward";
        public const String WalkBackwardAnimRef = "walk backward";
        public const String JumpAnimRef = "jump";
        public const String PunchAnimRef = "punch";
        public const String KickAnimRef = "kick";
        public const String SpecialAnimRef = "special";
        public const String BlockAnimRef = "block";

        #endregion


        #region Initialization

        public SFCharacter()
        {
            Scale = new Vector2(1f, 1f);

            this.init();
            this.setupStats();
            this.setupIdleAnim();
            this.setupHurtAnim();
            this.setupDeadAnim();
            this.setupWinAnim();
            this.setupWalkForwardAnim();
            this.setupWalkBackwardAnim();
            this.setupJumpAnim();
            this.setupPunchAnim();
            this.setupKickAnim();
            this.setupSpecialAnim();
            this.setupBlockAnim();

            state = CharacterState.Idle;
            hitFlag = false;
            jumpAttackFlag = false;
            jumpFlag = false;
        }

        abstract public SFCharacter Copy();

        abstract protected void init();
        abstract protected void setupStats();
        abstract protected void setupIdleAnim();
        abstract protected void setupHurtAnim();
        abstract protected void setupDeadAnim();
        abstract protected void setupWinAnim();
        abstract protected void setupWalkForwardAnim();
        abstract protected void setupWalkBackwardAnim();
        abstract protected void setupJumpAnim();
        abstract protected void setupPunchAnim();
        abstract protected void setupKickAnim();
        abstract protected void setupSpecialAnim();
        abstract protected void setupBlockAnim();

        #endregion


        #region Stats

        public float walkSpeed = 200f;
        public float jumpPower = 700f;

        public int currentPower;
        public int currentPriority;
        public int currentKnockBack;

        public int punchPower = 1;
        public int punchPriority = 0;
        public int punchKnockBack = 0;
        public float punchVelocityX = 0;
        public int kickPower = 1;
        public int kickPriority = 0;
        public int kickKnockBack = 0;
        public float kickVelocityX = 0; 
        public int specialPower = 1;
        public int specialPriority = 0;
        public int specialKnockBack = 0;
        public float specialVelocityX = 0;

        #endregion


        #region State Properties

        public enum CharacterState { None, Idle, Hurting, Dead, Winner, WalkingForward, WalkingBackward, Jumping, Attacking, Blocking };
        private CharacterState state;

        public bool canMove { get { return !jumpFlag && (this.isIdle || this.isWalking); } }
        public bool canJump { get { return !jumpFlag && (this.isIdle || this.isWalking); } }
        public bool canJumpAttack { get { return !jumpAttackFlag && (this.isIdle || this.isWalking || this.isJumping); } }
        public bool canAttack { get { return !jumpFlag && (this.isIdle || this.isWalking); } }
        public bool canBlock { get { return !jumpFlag && (this.isIdle || this.isWalking); } }
        public bool canBeHit { get { return !this.isHurting && !this.isBlocking && !this.isDead && !this.isWinner; } }

        public bool isIdle { get { return state == CharacterState.Idle; } }
        public bool isHurting { get { return state == CharacterState.Hurting; } }
        public bool isDead { get { return state == CharacterState.Dead; } }
        public bool isWinner { get { return state == CharacterState.Winner; } }
        public bool isWalking { get { return state == CharacterState.WalkingForward || state == CharacterState.WalkingBackward; } }
        public bool isWalkingForward { get { return state == CharacterState.WalkingForward; } }
        public bool isWalkingBackward { get { return state == CharacterState.WalkingBackward; } }
        public bool isJumping { get { return state == CharacterState.Jumping; } }
        public bool isAttacking { get { return state == CharacterState.Attacking; } }
        public bool isBlocking { get { return state == CharacterState.Blocking; } }

        bool hitFlag;
        bool jumpAttackFlag;
        bool jumpFlag;
        public bool isInAir { get { return jumpFlag; } }

        public bool isWalkingLeft
        {
            get {
                return
                    (state == CharacterState.WalkingForward && this.isFacingLeft) ||
                    (state == CharacterState.WalkingBackward && this.isFacingRight);
            }
        }
        public bool isWalkingRight
        {
            get
            {
                return
                    (state == CharacterState.WalkingForward && this.isFacingRight) ||
                    (state == CharacterState.WalkingBackward && this.isFacingLeft);
            }
        }
        

        #endregion


        #region Animation and Drawing

        public enum CharacterPiece { Body, Arm1, Arm2, Leg1, Leg2 };
        public SFSquare body, arm1, arm2, leg1, leg2;

        public Color lineColor;
        public int lineThickness;

        public bool addAnimation(CharacterPiece bodyPart, SFAnimation animation, bool setAsDefault = false)
        {
            switch (bodyPart)
            {
                case CharacterPiece.Body:
                    return body.addAnimation(animation, setAsDefault);
                case CharacterPiece.Arm1:
                    return arm1.addAnimation(animation, setAsDefault);
                case CharacterPiece.Arm2:
                    return arm2.addAnimation(animation, setAsDefault);
                case CharacterPiece.Leg1:
                    return leg1.addAnimation(animation, setAsDefault);
                case CharacterPiece.Leg2:
                    return leg2.addAnimation(animation, setAsDefault);
            }
            return false;
        }

        public void playAnimation(string animationName)
        {
            body.PlayAnimation(animationName);
            arm1.PlayAnimation(animationName);
            arm2.PlayAnimation(animationName);
            leg1.PlayAnimation(animationName);
            leg2.PlayAnimation(animationName);
        }

        public void Draw(GameScreen screen)
        {
            arm2.Draw(screen, this.Position, this.Scale);
            leg2.Draw(screen, this.Position, this.Scale);
            body.Draw(screen, this.Position, this.Scale);
            arm1.Draw(screen, this.Position, this.Scale);
            leg1.Draw(screen, this.Position, this.Scale);
        }

        #endregion


        #region Physics

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Scale { get; set; }

        public bool CheckHitAgainst(Rectangle globalRect)
        {
            if (this.body.GetGlobalRectangle(this.Position, this.Scale).Intersects(globalRect)) return true;
            return false;
        }

        public bool AttackDidHit(SFCharacter other)
        {
            if (!hitFlag && this.isAttacking && other.canBeHit)
            {
                if ((attackingWithArm1 && other.CheckHitAgainst(this.arm1.GetGlobalRectangle(this.Position, this.Scale))) ||
                    (attackingWithArm2 && other.CheckHitAgainst(this.arm2.GetGlobalRectangle(this.Position, this.Scale))) ||
                    (attackingWithLeg1 && other.CheckHitAgainst(this.leg1.GetGlobalRectangle(this.Position, this.Scale))) ||
                    (attackingWithLeg2 && other.CheckHitAgainst(this.leg2.GetGlobalRectangle(this.Position, this.Scale))) ||
                    (attackingWithBody && other.CheckHitAgainst(this.body.GetGlobalRectangle(this.Position, this.Scale))))
                {
                    hitFlag = true;
                    return true;
                }
            }
            return false;
        }

        #endregion


        #region Actions

        private bool attackingWithArm1 = false;
        private bool attackingWithArm2 = false;
        private bool attackingWithLeg1 = false;
        private bool attackingWithLeg2 = false;
        private bool attackingWithBody = false;

        private void setNotAttacking()
        {
            attackingWithArm1 = false;
            attackingWithArm2 = false;
            attackingWithLeg1 = false;
            attackingWithLeg2 = false;
            attackingWithBody = false;
            hitFlag = false;
        }

        private void attackWithArm1() { attackingWithArm1 = true; }
        private void attackWithArm2() { attackingWithArm2 = true; }
        private void attackWithLeg1() { attackingWithLeg1 = true; }
        private void attackWithLeg2() { attackingWithLeg2 = true; }
        private void attackWithBody() { attackingWithBody = true; }

        public bool specialAttackUsesArm1 = false;
        public bool specialAttackUsesArm2 = false;
        public bool specialAttackUsesLeg1 = false;
        public bool specialAttackUsesLeg2 = false;
        public bool specialAttackUsesBody = false;

        private void attackWithSpecial()
        {
            if (specialAttackUsesArm1) attackWithArm1();
            if (specialAttackUsesArm2) attackWithArm2();
            if (specialAttackUsesLeg1) attackWithLeg1();
            if (specialAttackUsesLeg2) attackWithLeg2();
            if (specialAttackUsesBody) attackWithBody();
        }

        public bool hurt(float knockBackVelocity = 0)
        {
            if (!isHurting)
            {
                state = CharacterState.Hurting;
                playAnimation(HurtAnimRef);
                body.CurrentAnimation.onAnimationComplete += hurtAnimationComplete;
                setNotAttacking();
                this.Velocity = new Vector2(isFacingRight ? -knockBackVelocity : knockBackVelocity, this.Velocity.Y);
                return true;
            }
            return false;
        }

        private void hurtAnimationComplete(SFAnimation anim)
        {
            anim.onAnimationComplete -= hurtAnimationComplete;
            state = CharacterState.Idle;
            playAnimation(IdleAnimRef);
            this.Velocity = new Vector2(0, this.Velocity.Y);
        }

        public bool kill()
        {
            if (!isDead)
            {
                state = CharacterState.Dead;
                playAnimation(DeadAnimRef);
                setNotAttacking();
                if (!jumpFlag)
                    this.Velocity = new Vector2(0, 0);
                return true;
            }
            return false;
        }

        public bool win()
        {
            if (!isDead)
            {
                state = CharacterState.Winner;
                playAnimation(WinAnimRef);
                setNotAttacking();
                if (!jumpFlag)
                    this.Velocity = new Vector2(0, 0);
                return true;
            }
            return false;
        }

        public bool punch()
        {
            if (canAttack || (canJumpAttack && jumpFlag && !jumpAttackFlag))
            {
                if (isJumping) jumpAttackFlag = true;
                if (!jumpFlag)
                    this.Velocity = new Vector2(isFacingRight ? punchVelocityX : -punchVelocityX, this.Velocity.Y);
                state = CharacterState.Attacking;
                playAnimation(PunchAnimRef);
                arm1.CurrentAnimation.onAnimationComplete += attackComplete;
                currentPower = punchPower;
                currentPriority = punchPriority;
                currentKnockBack = punchKnockBack;
                attackWithArm1();
                return true;
            }
            return false;
        }
        public bool kick()
        {
            if (canAttack || (canJumpAttack && jumpFlag && !jumpAttackFlag))
            {
                if (isJumping) jumpAttackFlag = true;
                if (!jumpFlag)
                    this.Velocity = new Vector2(isFacingRight ? kickVelocityX : -kickVelocityX, this.Velocity.Y);
                state = CharacterState.Attacking;
                playAnimation(KickAnimRef);
                leg1.CurrentAnimation.onAnimationComplete += attackComplete;
                currentPower = kickPower;
                currentPriority = kickPriority;
                currentKnockBack = kickKnockBack;
                attackWithLeg1();
                return true;
            }
            return false;
        }

        private SFSquare specialBodyPart = null;
        protected bool SetSpeicalAnimationTrackingSquare(SFSquare bodyPart)
        {
            if (bodyPart == this.arm1 ||
                bodyPart == this.arm2 ||
                bodyPart == this.leg1 ||
                bodyPart == this.leg2 ||
                bodyPart == this.body)
            {
                specialBodyPart = bodyPart;
                return true;
            }
            return false;
        }
        public bool special()
        {
            if (canAttack && !jumpFlag && specialBodyPart != null)
            {
                state = CharacterState.Attacking;
                playAnimation(SpecialAnimRef);
                specialBodyPart.CurrentAnimation.onAnimationComplete += attackComplete;
                currentPower = specialPower;
                currentPriority = specialPriority;
                currentKnockBack = specialKnockBack;
                attackWithSpecial();
                this.Velocity = new Vector2(isFacingRight ? specialVelocityX : -specialVelocityX, this.Velocity.Y);
                return true;
            }
            return false;
        }
        private void attackComplete(SFAnimation animation)
        {
            animation.onAnimationComplete -= attackComplete;
            state = CharacterState.Idle;
            playAnimation(IdleAnimRef);
            setNotAttacking();
            if (!jumpFlag)
                this.Velocity = new Vector2(0, this.Velocity.Y);
        }

        public bool jump()
        {
            if (canJump)
            {
                state = CharacterState.Jumping;
                playAnimation(JumpAnimRef);
                this.Velocity = new Vector2(this.Velocity.X, -jumpPower);
                setNotAttacking();
                jumpFlag = true;
                return true;
            }
            return false;
        }

        public void land()
        {
            if (jumpFlag)
            {
                state = CharacterState.Idle;
                playAnimation(IdleAnimRef);
                setNotAttacking();
                jumpFlag = false;
                jumpAttackFlag = false;
                this.Velocity = new Vector2(0, 0);
            }
        }

        public bool block()
        {
            if (canBlock)
            {
                state = CharacterState.Blocking;
                playAnimation(BlockAnimRef);
                setNotAttacking();
                this.Velocity = new Vector2(0, this.Velocity.Y);
                return true;
            }
            return false;
        }

        public void stopBlocking()
        {
            if (isBlocking)
            {
                state = CharacterState.Idle;
                playAnimation(IdleAnimRef);
                this.Velocity = new Vector2(0, this.Velocity.Y);
            }
        }

        public void standStill()
        {
            if (isWalking)
            {
                state = CharacterState.Idle;
                playAnimation(IdleAnimRef);
                setNotAttacking();
                this.Velocity = new Vector2(0, this.Velocity.Y);
            }
        }

        public bool walkLeft()
        {
            if (isIdle || isWalkingRight)
            {
                if (isFacingRight)
                    walkBackward();
                else
                    walkForward();
                setNotAttacking();
            }
            if (isWalking)
            {
                this.Velocity = new Vector2(-walkSpeed, this.Velocity.Y);
                return true;
            }
            return false;
        }

        public bool walkRight()
        {
            if (isIdle || isWalkingLeft)
            {
                if (isFacingLeft)
                    walkBackward();
                else
                    walkForward();
                setNotAttacking();
            }
            if (isWalking)
            {
                this.Velocity = new Vector2(walkSpeed, this.Velocity.Y);
                return true;
            }
            return false;
        }

        private void walkForward()
        {
            state = CharacterState.WalkingForward;
            playAnimation(WalkForwardAnimRef);
        }

        private void walkBackward()
        {
            state = CharacterState.WalkingBackward;
            playAnimation(WalkBackwardAnimRef);
        }

        Vector2 temp = Vector2.Zero;
        public void turnAround()
        {
            temp = this.Scale;
            temp.X *= -1f;
            this.Scale = temp;
        }

        public bool isFacingLeft { get { return this.Scale.X < 0; } }
        public bool isFacingRight { get { return this.Scale.X > 0; } }

        public void faceLeft()
        {
            if (this.isFacingRight)
                turnAround();
        }

        public void faceRight()
        {
            if (this.isFacingLeft)
                turnAround();
        }

        #endregion



        public void Update(GameTime gameTime)
        {
            body.Update(gameTime);
            arm1.Update(gameTime);
            arm2.Update(gameTime);
            leg1.Update(gameTime);
            leg2.Update(gameTime);

            this.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

    }
}
