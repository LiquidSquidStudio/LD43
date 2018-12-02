using UnityEngine;
using UnityEngine.UI;

public class ThroneRoomSpriteSelector : MonoBehaviour {

    public Sprite[] throneRoomSprites;

    [Range(1,3)]
    public int selectedSprite = 1;
    Image image;

	// Use this for initialization
	void Start () {
        image = GetComponentInChildren<Image>();
        ChangeSprite(selectedSprite - 1);
	}

    public void ChangeSprite(int spriteIndex)
    {
        if (throneRoomSprites[spriteIndex] != null)
        {
            image.sprite = throneRoomSprites[spriteIndex];
        }
    }

    private void Update()
    {
        ChangeSprite(selectedSprite - 1);
    }
}
