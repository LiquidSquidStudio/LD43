using UnityEngine;

public class SacrificialChoices : MonoBehaviour {

    public GameObject[] peopleToSacrifice;

    public Transform choiceLoc1;
    public Transform choiceLoc2;

    public void SpawnChoiceAnims(int whichChoice)
    {
        int goIndex = whichChoice * 2;
        Instantiate(peopleToSacrifice[goIndex], choiceLoc1);
        Instantiate(peopleToSacrifice[goIndex+1], choiceLoc2);
    }
}
