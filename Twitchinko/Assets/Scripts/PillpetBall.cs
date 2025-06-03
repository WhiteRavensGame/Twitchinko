using TMPro;
using TwitchIntegration;
using UnityEngine;

public class PillpetBall : MonoBehaviour
{
    [SerializeField] private SpriteRenderer pillpetAppearance;

    //[SerializeField] private Sprite bloatedSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite deathSprite;

    [SerializeField] private TextMeshProUGUI nameText;


    private Rigidbody2D rb;

    private TwitchUser user;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignPlayer(TwitchUser user)
    {
        this.user = user;
        nameText.text = user.displayname;

        if (user.displayname.Contains("Lemongrass"))
            pillpetAppearance.color = Color.yellow;
    }
    public TwitchUser GetPlayerAssigned()
    {
        return this.user;
    }

    private void DisplayWinSprite()
    {
        pillpetAppearance.sprite = happySprite;
    }

    private void DisplayLoseSprite()
    {
        pillpetAppearance.sprite = deathSprite;
    }

    public void ProcessWin()
    {
        DisplayWinSprite();
        //rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void ProcessDeath()
    {
        DisplayLoseSprite();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void ResetBall(float newX, float newY)
    {
        transform.position = new Vector3(newX, newY, transform.position.z);
        pillpetAppearance.sprite = normalSprite;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Death")
        {
            ProcessDeath();
        }
        else if(collision.transform.tag == "Food")
        {
            ProcessWin();
            Destroy(collision.gameObject);
        }
    }

}
