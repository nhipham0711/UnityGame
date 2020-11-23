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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
}
