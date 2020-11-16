using UnityEngine;
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

        private float horizontal;
        private float vertical;

        void Start()
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            _rigidBody = GetComponent<Rigidbody2D>();
            Vector3 pos = transform.position;
            // UnityEngine.Debug.Log("player: " + pos);
        }

        void Update()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        private void FixedUpdate()
        {
            // if( horizontal != 0 && vertical !=0) // check for diagonal movement?
            // velocity is both the speed and the direction 
            _rigidBody.velocity = new Vector2(horizontal * speed, vertical * speed);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.name == "fire-03")
            {
                Object.Destroy(col.gameObject);

            }
            else if (col.gameObject.CompareTag("Enemy"))
            {

                StartCoroutine(Attacked());
                gm.Lifes--;
                // the GM would check and know what to do, if the lifes < 0 
//                if (gm.Lifes <= 0)
//               {
//                    //!!!!I tried to switch the state from the GameManager but still havent found any solution so I'm just gonna load the panelGameOver right now 
//                    // gm.panelGameOver.SetActive(true);
//                    
//                }
                    // 15.11: will do it analogically to the exit with a boolean flag (for the GM) - only has to know that there was a collision with an enemy 
                    gm.lostLife = true;
            }
        }
        IEnumerator Attacked()
        {

            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.5f);
            GetComponent<SpriteRenderer>().color = Color.white;

        }
    }
}