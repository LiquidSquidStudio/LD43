using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRandomiser : MonoBehaviour {

    public Sprite[] sprites;

    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start ()
    {
        if (sr != null)
            RandomiseSprite();
    }

    void RandomiseSprite()
    {
        int rand = Random.Range(0, sprites.Length);
        sr.sprite = sprites[rand];
    }
}
