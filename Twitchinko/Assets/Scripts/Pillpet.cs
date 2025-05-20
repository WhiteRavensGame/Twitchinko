using TMPro;
using UnityEngine;

public class Pillpet : MonoBehaviour
{
    [SerializeField] private SpriteRenderer pillpetAppearance;

    [SerializeField] private Sprite bloatedSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite deathSprite;

    [SerializeField] private TextMeshProUGUI textName;
    public string pillpetName;

    [SerializeField] private GameManager gameManager;

    private int bloat;
    private float hungerTimer = 60;
    private float timeBeforeHungerTick = 60;
    private bool isAlive = true;
    private bool isMoving = false;
    private Vector3 destination = Vector3.zero;

    public float timeAlive = 0;

    void Start()
    {
        ResetStats();
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void SetName(string newName)
    {
        pillpetName = newName;
        if(pillpetName.Length > 14) 
            textName.text = pillpetName.Substring(0, 14);
        else
            textName.text = pillpetName;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        timeAlive += Time.deltaTime;
        gameManager.UpdateTimeUI(timeAlive);
        


        //Update hunger meter
        hungerTimer -= Time.deltaTime;
        if(hungerTimer <= 0)
        {
            bloat--;
            hungerTimer = timeBeforeHungerTick;
        }

        //determine sprite to render
        if (bloat > 0 && bloat < 6)
        {
            if (bloat >= 4)
                pillpetAppearance.sprite = bloatedSprite;
            else
                pillpetAppearance.sprite = normalSprite;
        }
        else if(bloat >= 6)
        {
            // if too fat, die.
            Die();
            gameManager.RecordTime(timeAlive);
        }
        else //if starved, be immortal?
        {

        }

        if(isMoving)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, destination, Time.deltaTime * 3);
            if (Vector3.Distance(this.transform.position, destination) <= 0.1f) //reached destination
            {
                isMoving = false;
            }
        }
        
    }

    private void Die()
    {
        Debug.Log("YAK!");
        pillpetAppearance.sprite = deathSprite;
        isAlive = false;
    }

    public void ResetStats()
    {
        hungerTimer = timeBeforeHungerTick;
        bloat = 2;
        isAlive = true;
        isMoving = false;
        destination = Vector3.zero;
        timeAlive = 0;
    }

    public void Revive()
    {
        gameManager.RecordTime(timeAlive);
        ResetStats();
    }

    public void Move(float x, float y)
    {
        isMoving = true;
        destination = transform.position + new Vector3(x, 0, 0);
        destination = new Vector3(Mathf.Clamp(destination.x, -8, 8), destination.y, destination.z);

        if (x > 0) pillpetAppearance.flipX = false;
        else pillpetAppearance.flipX = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Food")
        {
            collision.gameObject.SetActive(false);
            bloat++;
        }
    }
}
