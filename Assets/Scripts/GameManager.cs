using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Completed

{
    public class GameManager : MonoBehaviour
    {

        public BoardManager boardScript;

        public Text coinsText;
        public Text levelText;

        public GameObject panelMenu;
        public GameObject panelPlay;
        public GameObject panelUpgrade;
        public GameObject panelGameOver;
        public GameObject panelLevelCompleted;

        [HideInInspector] public bool exitReached;
        [HideInInspector] public bool lostLife;
		[HideInInspector] public bool chooseShield = false;
        [HideInInspector] public bool useSpeed = false;
		
        [SerializeField] private Image healthbarImage;
        [SerializeField] private Sprite[] healthbarImages;
        [SerializeField] private int neededForUpgradeCoins = 10;
        
        public static GameManager Instance { get; private set; }
        public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, UPGRADE, GAMEOVER }
    
        private State _state;
        private bool _isSwitchingState;


        private int _coins;
        public int Coin
        {
            get { return _coins; }
            set
            {
                _coins = value;
                coinsText.text = "x " + _coins;
            }
        }

        private int _lifes;
        public int Lifes
        {
            get { return _lifes; }
            set { _lifes = value; }
        }
		
		private int level = 1;  
        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                levelText.text = "LEVEL: " + level;
            }
        }

        public void PlayClicked()
        {
            SwitchState(State.INIT);
        }
        
        public void GoBackToMenuClicked()
        {
            Debug.Log("button go back to menu clicked");
            SwitchState(State.MENU);
        }

        public void ButtonFasterClicked()
        {
            Debug.Log("faster");
            useSpeed = true;
            SwitchState(State.PLAY);
        }

        public void ButtonShieldClicked()
        {
            Debug.Log("shield");
            // code for (color) shield for player

            chooseShield = true;
            
            SwitchState(State.PLAY);
        }

        void Awake()
        {
            boardScript = GetComponent<BoardManager>();
            Instance = this;
            DontDestroyOnLoad(gameObject);  // needed?
                                            // InitGame(); -> do it in the play state 
            SwitchState(State.MENU);
        }
        
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
            switch (_state)
            {
                case State.MENU:
                    break;
                case State.INIT:
                    break;
                case State.PLAY:
                    SetExitActiveIfCoinsCollected();
                    if (exitReached)
                    {
                        Debug.Log("exit reached in play");
                        exitReached = false;
                        SwitchState(State.LEVELCOMPLETED);
                    }
                    // check for a lost life
                    if (lostLife)
                    {
                        Lifes--;
                        if (Lifes < 0)  //  < 0 cuz the lifes are 6, but from 0 till 5 
                        {
                            SwitchState(State.GAMEOVER);
                        }
                        else
                        {
                            // maybe try with Dictionary and load from Resources 
							healthbarImage.sprite = healthbarImages[Lifes] as Sprite;
                            Debug.Log("change Image!!");
                        }
                        lostLife = false;
                    }
                    if (Coin == neededForUpgradeCoins)
                    {
                        SwitchState(State.UPGRADE);
                    }

                    if (chooseShield)
                    {
						boardScript.playerPrefab.GetComponent<PlayerController>().useShield();
                        chooseShield = false;
                        
                        // TODO: still include timeout or whatever
                    }
                    if (useSpeed)
                    {
                        boardScript.playerPrefab.GetComponent<PlayerController>().useSpeed(1);
                        useSpeed = false;
                    }

                    break;
                case State.LEVELCOMPLETED:
                    break;
                case State.UPGRADE:
                    break;
                case State.LOADLEVEL:
                    break;
                case State.GAMEOVER:
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
            switch (newState)
            {
                case State.MENU:
                    Cursor.visible = true;
                    panelMenu.SetActive(true);
                    break;
                case State.INIT:
                    Coin = 0;
                    Lifes = 5;
                    Level = 1;
                    Cursor.visible = false;
                    panelPlay.SetActive(true);
                    healthbarImage.sprite = healthbarImages[Lifes] as Sprite;	// überflüssig?
                    SwitchState(State.LOADLEVEL);
                    break;
                case State.PLAY:
                	ResumeGame();
                    panelPlay.SetActive(true);
					break;
                case State.LEVELCOMPLETED:
                    //exitReached = false;
                    Level++;
                    panelLevelCompleted.SetActive(true);
                    SwitchState(State.LOADLEVEL);
                    break;
                case State.LOADLEVEL:
                    // do other stuff
                    InitGame(); // set up board 		// consider: if player is every time instantiated, is there any info that should be kept with him
                    SwitchState(State.PLAY, 2f);
                    break;
                case State.UPGRADE:
                    PauseGame();    // test it tho... 
                    Cursor.visible = true;
                    panelUpgrade.SetActive(true);
                    break;
                case State.GAMEOVER:
                	PauseGame();
                    Level = 1;
                    // drop all the updates
                    Coin = 0;
                    Lifes = 6;
                    Cursor.visible = true;
                    panelGameOver.SetActive(true);
                    break;
            }
        }

        private void EndState()
        {
            switch (_state)
            {
                case State.MENU:
                    panelMenu.SetActive(false);
                    break;
                case State.INIT:
                    break;
                case State.PLAY:
                    panelPlay.SetActive(false);
                    break;
                case State.LEVELCOMPLETED:
                    break;
                case State.LOADLEVEL:
                    panelLevelCompleted.SetActive(false);
                    break;
                case State.UPGRADE:
                    Coin = 0;
                    panelUpgrade.SetActive(false);
                    //panelPlay.SetActive(true);
                    Cursor.visible = false;

                    ResumeGame();   // test it tho... 
                    break;
                case State.GAMEOVER:
                    panelGameOver.SetActive(false);
                    break;
            }
        }

        public bool IsSwitchingStates()
        {
            return _isSwitchingState;
        }

        private void SetExitActiveIfCoinsCollected()
        {
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
            if (coins.Length == 0)
            {
                boardScript.exitTile.SetActive(true);
            }
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }

        private void ResumeGame()
        {
            Time.timeScale = 1;
        }
    }

}
