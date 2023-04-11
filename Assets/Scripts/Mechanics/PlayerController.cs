﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public Transform mouseIndicator;
        public Transform barrel;
        public Transform muzzle;
        public GameObject projectile;
        public float shotForce = 1f;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        //internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        public static Vector3 mousePos;

        private Vector3 Worldpos;
        private Vector2 Worldpos2D;
        private float barrelAngle;

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            //animator = GetComponent<Animator>();
        }



        protected override void Update()
        {
            
            if (controlEnabled)
            {
                mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
                Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);
                mouseIndicator.position = Worldpos2D;
                barrelAngle = Mathf.Atan2(Worldpos2D.y-transform.position.y, Worldpos2D.x - transform.position.x) * Mathf.Rad2Deg;
                barrel.rotation = Quaternion.Euler(new Vector3(0, 0, barrelAngle));
                if (Input.GetButtonDown("Fire1"))
                {
                    Rigidbody2D tankRB = GetComponent<Rigidbody2D>();
                    GameObject shotProjectile = Instantiate(projectile);
                    shotProjectile.transform.position = muzzle.transform.position;
                    shotProjectile.transform.rotation = muzzle.transform.rotation;
                    Rigidbody2D shotProjectileRB = shotProjectile.GetComponent<Rigidbody2D>();
                    float angleInRadians = barrelAngle * Mathf.Deg2Rad;
                    Vector2 forceDirection = new(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
                    shotProjectileRB.AddForce(forceDirection * shotForce, ForceMode2D.Impulse);
                         
                }
                //move.x = Input.GetAxis("Horizontal");
                //if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                //    jumpState = JumpState.PrepareToJump;
                //else if (Input.GetButtonUp("Jump"))
                //{
                //    stopJump = true;
                //    Schedule<PlayerStopJump>().player = this;
                //}

            }
            //else
            //{
            //    move.x = 0;
            //}
            //UpdateJumpState();
            base.Update();
        }

        //void UpdateJumpState()
        //{
        //    jump = false;
        //    switch (jumpState)
        //    {
        //        case JumpState.PrepareToJump:
        //            jumpState = JumpState.Jumping;
        //            jump = true;
        //            stopJump = false;
        //            break;
        //        case JumpState.Jumping:
        //            if (!IsGrounded)
        //            {
        //                Schedule<PlayerJumped>().player = this;
        //                jumpState = JumpState.InFlight;
        //            }
        //            break;
        //        case JumpState.InFlight:
        //            if (IsGrounded)
        //            {
        //                Schedule<PlayerLanded>().player = this;
        //                jumpState = JumpState.Landed;
        //            }
        //            break;
        //        case JumpState.Landed:
        //            jumpState = JumpState.Grounded;
        //            break;
        //    }
        //}

        //protected override void ComputeVelocity()
        //{
        //    if (jump && IsGrounded)
        //    {
        //        velocity.y = jumpTakeOffSpeed * model.jumpModifier;
        //        jump = false;
        //    }
        //    else if (stopJump)
        //    {
        //        stopJump = false;
        //        if (velocity.y > 0)
        //        {
        //            velocity.y = velocity.y * model.jumpDeceleration;
        //        }
        //    }

        //    if (move.x > 0.01f)
        //        spriteRenderer.flipX = false;
        //    else if (move.x < -0.01f)
        //        spriteRenderer.flipX = true;

        //    //animator.SetBool("grounded", IsGrounded);
        //    //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        //    targetVelocity = move * maxSpeed;
        //}

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}