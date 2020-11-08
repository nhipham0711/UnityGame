using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
public class ExitScript : MonoBehaviour
{
	[HideInInspector] public GameManager gm;
    
    void Awake()
    {
    	gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
 	{
     	if (collision.gameObject.CompareTag("Player") && !gm.IsSwitchingStates())	// second bool check is probably not beautiful, but out of desperation when debugging 
     	{
       	 	Debug.Log("exit reached");
       	 	gm.exitReached = true;
     	}
 	}
}
}
