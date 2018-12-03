using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteContainer : MonoBehaviour {

    public Sprite[] sprites;

    public Sprite GetSprite(int index)
    {
        Sprite selectedSprite = sprites[index];
        return selectedSprite;
    }

    public void SetSprites()
    {
        for (int i = 0; i < 2; i++)
        {
            var sp = GetComponentInChildren<SpriteRenderer>();
            sp.sprite = GetSprite(i);
        }
    }
}
