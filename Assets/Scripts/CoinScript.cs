using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
public class CoinScript : MonoBehaviour
{
    [HideInInspector] public GameManager gm;
    
    void Awake()
    {
    	gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
 	{
 		if (collision.gameObject.CompareTag("Player")) 
     	{
       	 	Destroy(gameObject);
       	 	gm.Coin++;
     	}
 	}
}
}