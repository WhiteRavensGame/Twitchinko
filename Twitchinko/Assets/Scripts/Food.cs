using UnityEngine;
using System.Collections.Generic;

public class Food : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> foodSprites;

    public Sprite cookieSprite;
    public Sprite hotdogSprite;

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        foodSprites = new List<Sprite>();
        //ChangeSprite();
    }

    public void ChangeSpriteHotdog()
    {
        //int rand = Random.Range(0, foodSprites.Count);
        spriteRenderer.sprite = hotdogSprite;
    }

    public void ChangeSpriteCookie()
    {
        spriteRenderer.sprite = cookieSprite;
    }
}
