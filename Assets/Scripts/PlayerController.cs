using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    private Rigidbody2D _rigidBody;

	private float horizontal;
	private float vertical;
    void Start()
    {
    	_rigidBody = GetComponent<Rigidbody2D>();
    	Vector3 pos = transform.position;
    	Debug.Log("player: " + pos); 
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
        _rigidBody.velocity = new Vector2 (horizontal * speed, vertical * speed);
    }
    
    void OnCollisionEnter2D(Collision2D col)
     {
         if (col.gameObject.name == "fire-03")
         {
             Object.Destroy(col.gameObject);
             
         }
     }
}