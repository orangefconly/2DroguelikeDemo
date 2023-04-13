using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public BoardManager boardManager;

    public float levelStartDelay = 2f;
    public float turnDelay = 0.2f;
    public int level = 9;
    public int playerFoodPoint = 100;
    public bool playerTurn = true;
    public GameObject player;

    private TMP_Text levelText;
    private GameObject levelImage;
    private List<Enemy> enemies;
    private bool enemiesMoving = false;
    private bool doingSetup;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardManager = GetComponent<BoardManager>();
        Initgame();
       
    }

    
    private void Update()
    {
        if (enemiesMoving || playerTurn || doingSetup)
        { return; }
        StartCoroutine(MoveEnemies());

    }
    public void AddEnemyToList(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Instance.level++;
        Instance.Initgame();
    }

    void Initgame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<TMP_Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
       
        enemies.Clear();
        boardManager.SetupScene(level);
        //Instantiate(player,new Vector3(0,0,0), Quaternion.identity);
        Invoke("HideLevelImage", levelStartDelay);

    }

    void HideLevelImage()
    {        
        levelImage.SetActive(false);

        doingSetup = false;
    }
    public void GameOver()
    {
        levelImage.SetActive(true);
        levelText.text = "ƒ„ªÓ¡À" + level + "ÃÏ";
        Invoke("QuitApplication",3.0f);
        this.enabled = false;
    }
   
    void QuitApplication()
    {
        Application.Quit();
    }
        IEnumerator MoveEnemies()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(turnDelay+0.3f);

        if(enemies.Count==0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for(int i = 0; i<enemies.Count;i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime+0.3f);

        }
        playerTurn = true;
        enemiesMoving = false;

    }
  
}
