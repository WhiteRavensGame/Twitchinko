using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using TwitchIntegration;
using TMPro;

public class BoardManager : TwitchMonoBehaviour
{
    public static BoardManager Instance;

    private float startX = -3;
    private float endX = 3;
    private float startY = -3;
    private float endY = 3;
    private float spacingX = 1f;
    private float spacingY = 1f;

    private float slotStartX = -3.25f;
    private float slotEndX = 3.5f;
    private float slotSpacingX = 0.925f;
    private float slotY = -5;

    private int[,] grid;

    public GameObject[] pieces;
    public GameObject[] slots; 
    private List<GameObject> spawnedObjects;
    private List<GameObject> spawnedPlayers;

    public GameObject gameView;
    public GameObject waitingBlockerObject;

    [SerializeField]
    public GameObject pillpetPrefab;

    [Header("UI")]
    public GameObject timerHeader;
    public TextMeshProUGUI timerText;
    public TransparentWindowNoFS transparentWindowSettings;

    private GameState gameState;

    private float lobbyTime = 20;
    private float elapsedTime = 0;
    private float playTime = 15;



    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            spawnedObjects = new List<GameObject>();
            spawnedPlayers = new List<GameObject>();

            gameState = GameState.IDLE;
            gameView.SetActive(false);
            transparentWindowSettings.TurnScreenTransparent(true);
            //RegenerateBoard();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (gameState == GameState.STARTING)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = Mathf.CeilToInt(lobbyTime - elapsedTime).ToString();
            if(elapsedTime > lobbyTime) 
            {
                gameState = GameState.PLAYING;
                elapsedTime = 0;
                waitingBlockerObject.SetActive(false);
                timerHeader.SetActive(false);
            }
        }
        else if(gameState == GameState.PLAYING)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > playTime)
            {
                gameState = GameState.IDLE;
                elapsedTime = 0;
                waitingBlockerObject.SetActive(true);
                gameView.SetActive(false);
                transparentWindowSettings.TurnScreenTransparent(true);
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            gameView.SetActive(true);

            //QQQQ
            GameObject g = GameObject.FindGameObjectWithTag("Player");
            if(g != null) g.GetComponent<PillpetBall>().ResetBall(Random.Range(-3.5f, 3.5f), 4.5f);

            RegenerateBoard();
            
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            gameView.SetActive(false);
        }
    }

    public void RegenerateBoard()
    {
        //clear old board
        foreach(GameObject g in spawnedObjects)
        {
            Destroy(g);
        }
        spawnedObjects.Clear();

        //Destroy all old players.
        foreach (GameObject g in spawnedPlayers)
        {
            Destroy(g);
        }
        spawnedPlayers.Clear();

        //Initialize Board
        int width = Mathf.RoundToInt(Mathf.Abs(endX - startX) / spacingX) + 1;
        int height = Mathf.RoundToInt(Mathf.Abs(endY - startY) / spacingY) + 1;
        Debug.Log(width + "," + height);

        grid = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int rand = Random.Range(0, pieces.Length);
                float xPos = startX + i * spacingX;
                float yPos = startY + j * spacingY;

                if (pieces[rand] == null) continue;

                GameObject g = Instantiate(pieces[rand], new Vector3(xPos, yPos, 0), Quaternion.Euler(0, 0, 90), gameView.transform);
                spawnedObjects.Add(g);
            }
        }

        //Initialize Slots
        int slotsCount = Mathf.RoundToInt(Mathf.Abs(slotEndX - slotStartX) / slotSpacingX) + 1;

        int[] spikeIndex = new int[slotsCount];
        spikeIndex = Enumerable.Range(1, slotsCount).OrderBy(x => UnityEngine.Random.value).Take(3).ToArray();

        for (int i = 0; i < spikeIndex.Length; i++)
        {
            Debug.Log(spikeIndex[i]);
        }
        
        for (int i = 0; i < slotsCount; i++)
        {
            int rand = spikeIndex.Contains(i) ? 1 : 0;
            float xPos = slotStartX + i * slotSpacingX;
            float yPos = slotY;


            GameObject g = Instantiate(slots[rand], new Vector3(xPos, yPos, 0), Quaternion.identity, gameView.transform);
            spawnedObjects.Add(g);
        }

    }

    [TwitchCommand("pachinko")]
    public void StartGame(TwitchUser user)
    {
        if (gameState == GameState.IDLE)
        {
            transparentWindowSettings.TurnScreenTransparent(false);
            gameState = GameState.STARTING;
            gameView.SetActive(true);
            RegenerateBoard();
            Debug.Log($"{user.displayname} started a game!!!!");

            CreatePillpetBall(user);
            timerHeader.SetActive(true);
        }
        else if(gameState == GameState.STARTING)
        {
            Debug.Log($"{user.displayname} joined the game!!!!");

            //double check if the player already has a ball before spawning them. 
            CreatePillpetBall(user);
        }

        

    }

    private bool HasDuplicatePlayer(TwitchUser user)
    {
        foreach(GameObject g in spawnedPlayers)
        {
            if(g.GetComponent<PillpetBall>().GetPlayerAssigned().displayname == user.displayname)
            {
                return true;
            }
        }

        return false;
    }

    private void CreatePillpetBall(TwitchUser user)
    {
        //Prevent duplicate players from spawning in.
        if (HasDuplicatePlayer(user)) return;

        float randX = Random.Range(-3.2f, 3.2f);
        float randY = Random.Range(4.2f, 4.8f);
        GameObject g = Instantiate(pillpetPrefab, new Vector3(randX, randY, 0), Quaternion.identity, gameView.transform);
        spawnedPlayers.Add(g);
        PillpetBall p = g.GetComponent<PillpetBall>();
        p.AssignPlayer(user);
    }


}

public enum GameState
{
    IDLE = 0,
    STARTING,
    PLAYING,
    ENDING
}
