using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed;
    public float lineOfSite;
    public float shootingRange;
    public GameObject waterBall;
	public GameObject waterBallOrigin;
    public float shootRate = 1f;
    private float nextShootTime;
    private GameObject player;
    [SerializeField] private int lifes;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.transform.position, this.transform.position);
        if (distanceFromPlayer <= lineOfSite && distanceFromPlayer >= shootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        } 
        else if (distanceFromPlayer < shootingRange && nextShootTime < Time.time)
        {
            ShootWaterBall();
        } 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, lineOfSite);
        Gizmos.DrawWireSphere(this.transform.position, shootingRange);
    }		
	private void ShootWaterBall()
	{
		Instantiate(waterBall, waterBallOrigin.transform.position, Quaternion.identity);
        nextShootTime = Time.time + shootRate;
	}
	
	void OnCollisionEnter2D(Collision2D col)
    {
       if (col.gameObject.CompareTag("Projectile"))
        {
        	lifes--;
            GetComponent<SpriteRenderer>().color = Color.gray;
            if(lifes <= 0) { 
                Destroy(gameObject); 
            }
        } 
    }
}
