using UnityEngine;

public class ChoiceController : MonoBehaviour {

    public SobStoryText ssText;
    public SacrificialChoices sChoice;

	void Start ()
    {
        int sp = GetStoryProgression();
        SetStoryProgression(sp);
	}

	int GetStoryProgression()
    {
        int sp = PersistingData.storyProgression;
        return sp;
    }

    void SetStoryProgression(int sp)
    {
        ssText.SetSSIndex(sp);
        sChoice.SpawnChoiceAnims(sp);
    }
}
