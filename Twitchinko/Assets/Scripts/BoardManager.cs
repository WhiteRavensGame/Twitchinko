using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class BoardManager : MonoBehaviour
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

    public GameObject gameView;


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            spawnedObjects = new List<GameObject>();
            RegenerateBoard();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            gameView.SetActive(true);

            //QQQQ
            GameObject.FindGameObjectWithTag("Player").GetComponent<PillpetBall>().ResetBall(Random.Range(-3.5f, 3.5f), 4.5f);

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
}
