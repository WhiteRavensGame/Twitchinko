using UnityEngine;

public class Pillpet : MonoBehaviour
{
    [SerializeField] private SpriteRenderer pillpetAppearance;

    [SerializeField] private Sprite bloatedSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite deathSprite;

    private int bloat;
    private float hungerTimer = 60;
    private float timeBeforeHungerTick = 60;
    private bool isAlive = true;
    private bool isMoving = false;
    private Vector3 destination = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hungerTimer = timeBeforeHungerTick;
        bloat = 2;
        isAlive = true;
        isMoving = false;
        destination = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        //Update hunger meter
        hungerTimer -= Time.deltaTime;
        if(hungerTimer <= 0)
        {
            bloat--;
            hungerTimer = timeBeforeHungerTick;
        }

        //determine sprite to render
        if (bloat > 0 && bloat < 5)
        {
            if (bloat >= 4)
                pillpetAppearance.sprite = bloatedSprite;
            else
                pillpetAppearance.sprite = normalSprite;
        }
        else
        {
            // if too fat or too starved, die.
            Die();
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

    public void Revive()
    {
        Start();
    }

    public void Move(float x, float y)
    {
        isMoving = true;
        destination = new Vector3(Mathf.Clamp(x, -8, 8), y, 0);
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
