using System.Collections;
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
    public class BespokePlayerController : MonoBehaviour
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public Transform mouseIndicator;
        public Transform barrel;
        public Transform muzzle;
        public GameObject projectile;
        public float shotForce = 1f;
        public float shotRecoil = 2f;

       
        
        public bool controlEnabled = true;

        public static Vector3 mousePos;

        private Vector3 Worldpos;
        private Vector2 Worldpos2D;
        private float barrelAngle;

        void Awake()
        {

        }



        void Update()
        {

            if (controlEnabled)
            {
                mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
                Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);
                mouseIndicator.position = Worldpos2D;
                barrelAngle = Mathf.Atan2(Worldpos2D.y - transform.position.y, Worldpos2D.x - transform.position.x) * Mathf.Rad2Deg;
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
                    tankRB.AddForce(forceDirection*shotForce*-shotRecoil,ForceMode2D.Impulse);
                }

            }
        }
    }
}