using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Completed

{
public class GameManager : MonoBehaviour
{
    
    public BoardManager boardScript; 
    public GameObject PlayerSprite;
    
    public Text coinsText;
    public Text lifesText;
    
    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLostLife;
    public GameObject panelGameOver;
    
    private int level = 1; // ok, change it 
    
    public static GameManager Instance { get; private set;}
    
    // Everything can be edited, it's just a first sketch  !!!
    
    public enum State {MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, LOSTLIFE, GAMEOVER }
    
    private State _state;
    bool _isSwitchingState;
    
    private int _coins;
    public int Coin
    {
    	get {return _coins;}
    	set {_coins = value; 
    	coinsText.text = "COINS: " + _coins;}
    }
    
    private int _lifes;
    public int Lifes
    {
    	get {return _lifes;}
    	set {_lifes = value;
    	lifesText.text = "LIFES: " + _lifes;}
    }
    
    public void PlayClicked()
    {
    	SwitchState(State.INIT);
    }
    
    void Awake() 
    {
    	boardScript = GetComponent<BoardManager>();
    	Instance = this;
    	DontDestroyOnLoad(gameObject);	// needed?
    	InitGame();
    	SwitchState(State.MENU);	
    }
    // Start is called before the first frame update
    void Start()
    {
    	// Instance = this;
    	// SwitchState(State.MENU);
    }
    
    void InitGame()
    {
    	boardScript.SetupScene(level);
    }

    void Update()
    {
        switch(_state)
    	{
    		case State.MENU:
    			break;
    		case State.INIT:
    			break;	
    		case State.PLAY:
    			break;
    		case State.LEVELCOMPLETED:
    			break;
    		case State.LOSTLIFE:
    			break;
    		case State.LOADLEVEL:
    			break;
    		case State.GAMEOVER:
    			if(Input.anyKeyDown)
    			{
    				SwitchState(State.MENU);
    			}
    			break;
    	}
    }
    
    public void SwitchState(State newState, float delay = 0)
    {
    	StartCoroutine(SwitchDelay(newState, delay));
    }
    
    IEnumerator SwitchDelay(State newState, float delay)
    {
    	_isSwitchingState = true;
    	yield return new WaitForSeconds(delay);	
    	EndState();
    	_state = newState;
    	BeginState(newState);
    	_isSwitchingState = false;
    }
    
    private void BeginState(State newState)
    {
    	switch(newState)
    	{
    		case State.MENU:
    			Cursor.visible = true;
    			panelMenu.SetActive(true);
    			break;
    		case State.INIT:
    			Cursor.visible = false;
    			panelPlay.SetActive(true);
    			Instantiate(PlayerSprite);
    			SwitchState(State.LOADLEVEL);
    			break;	
    		case State.PLAY:
    			break;
    		case State.LEVELCOMPLETED:
    			level++;
    			SwitchState(State.LOADLEVEL, 2f);
    			break;
    		case State.LOADLEVEL:
    			// do other stuff
    			SwitchState(State.PLAY);
    			break;
    		case State.GAMEOVER:
    			level = 1;
    			panelGameOver.SetActive(true);
    			break;
    	}
    }
    
    private void EndState()
    {
    	switch(_state)
    	{
    		case State.MENU:
    			panelMenu.SetActive(false);
    			break;
    		case State.INIT:
    			break;	
    		case State.PLAY:
    			break;
    		case State.LEVELCOMPLETED:
    			break;
    		case State.LOADLEVEL:
    			break;
    		case State.GAMEOVER:
    			panelPlay.SetActive(false);
    			panelGameOver.SetActive(false);
    			break;
    	}
    }
}

}
