﻿using UnityEngine;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace Completed
{
    public class PlayerController : MonoBehaviour
    {

        public GameManager gm;
        public float speed;
        private Rigidbody2D _rigidBody;
        [SerializeField] private GameObject shield;
		[SerializeField] private GameObject bulletPrefab;
		[SerializeField] private float bulletSpeed;
        private float horizontal;
        private float vertical;
        private bool shieldActive = false;


        void Start()
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            UnityEngine.Debug.Log("found shield");
            shield.SetActive(false);
            _rigidBody = GetComponent<Rigidbody2D>();
            Vector3 pos = transform.position;
            // UnityEngine.Debug.Log("player: " + pos);
        }

        void Update()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            if(Input.GetKeyDown(KeyCode.D))
            {
            	FireRight();
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
            	FireLeft();
            }
            if(Input.GetKeyDown(KeyCode.W))
            {
            	FireUp();
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
            	FireDown();
            }
        }

        private void FixedUpdate()
        {
            // if( horizontal != 0 && vertical !=0) // check for diagonal movement?
            // velocity is both the speed and the direction 
            _rigidBody.velocity = new Vector2(horizontal * speed, vertical * speed);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Enemy") && !shieldActive)
            {

                StartCoroutine(Attacked());
                gm.LostLife();
            }
            
            if (col.gameObject.CompareTag("Nemo") )
            {
                gm.NemoWasFound();
            }

        }
        IEnumerator Attacked()
        {

            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.5f);
            GetComponent<SpriteRenderer>().color = Color.white;

        }

        public void useShield()
        {
            shieldActive = true;
            shield.SetActive(true);
            UnityEngine.Debug.Log("Shield activated");
            Invoke("DeactivateShield", 5.0f);
        }

        private void DeactivateShield()
        {
            shieldActive = false;
            shield.SetActive(false);
            UnityEngine.Debug.Log("Shield deactivated");
        }
        
        public void useSpeed(float speedUp)
        {
            speed += speedUp;
        }
        
        private void FireRight()
        {
        	GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        	bullet.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
		    Destroy(bullet, 1f);
        }
        
        private void FireLeft()
        {
        	GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        	bullet.GetComponent<Rigidbody2D>().velocity = transform.right * -1 * bulletSpeed;
		    Destroy(bullet, 1f);
        }
        
        private void FireUp()
        {
        	GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        	bullet.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
		    Destroy(bullet, 1f);
        }
        
        private void FireDown()
        {
        	GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        	bullet.GetComponent<Rigidbody2D>().velocity = transform.up * -1 * bulletSpeed;
		    Destroy(bullet, 1f);
        }
    }
}