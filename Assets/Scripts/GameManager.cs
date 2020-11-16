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
        private int level = 1; // ok, change it 
        [SerializeField] private Image healthbarImage;
        [SerializeField] private Sprite[] healthbarImages;
        [SerializeField] private int neededForUpgradeCoins = 10;
        public static GameManager Instance { get; private set; }


        public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, UPGRADE, GAMEOVER }

        private State _state;
        bool _isSwitchingState;


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

        public void ButtonFasterClicked()
        {
            Debug.Log("faster");
            // code for faster player
            SwitchState(State.PLAY);
        }

        public void ButtonShieldClicked()
        {
            Debug.Log("shield");
            // code for (color) shield for player
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
                            healthbarImage.sprite = healthbarImages[Lifes] as Sprite;
                            Debug.Log("change Image!!");
                        }
                        lostLife = false;
                    }
                    if (Coin == neededForUpgradeCoins)
                    {
                        SwitchState(State.UPGRADE);
                    }
                    break;
                case State.LEVELCOMPLETED:
                    break;
                case State.UPGRADE:
                    break;
                case State.LOADLEVEL:
                    break;
                case State.GAMEOVER:
                    if (Input.anyKeyDown)
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
                    healthbarImage.sprite = healthbarImages[Lifes] as Sprite;
                    SwitchState(State.LOADLEVEL);
                    break;
                case State.PLAY:
                    panelPlay.SetActive(true);
                    break;
                case State.LEVELCOMPLETED:
                    exitReached = false;
                    Level++;
                    panelLevelCompleted.SetActive(true);
                    SwitchState(State.LOADLEVEL);
                    break;
                case State.LOADLEVEL:
                    // do other stuff
                    InitGame(); // set up board 
                    SwitchState(State.PLAY, 2f);
                    break;
                case State.UPGRADE:
                    panelPlay.SetActive(false);
                    PauseGame();    // test it tho... 
                    Cursor.visible = true;
                    panelUpgrade.SetActive(true);
                    break;
                case State.GAMEOVER:
                    Level = 1;
                    // drop all the updates
                    Coin = 0;
                    Lifes = 5;
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
                    panelPlay.SetActive(true);
                    break;
                case State.LEVELCOMPLETED:
                    break;
                case State.LOADLEVEL:
                    panelLevelCompleted.SetActive(false);
                    break;
                case State.UPGRADE:
                    Coin = 0;
                    panelUpgrade.SetActive(false);
                    panelPlay.SetActive(true);
                    Cursor.visible = false;
                    ResumeGame();   // test it tho... 
                    break;
                case State.GAMEOVER:
                    panelPlay.SetActive(false);
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
