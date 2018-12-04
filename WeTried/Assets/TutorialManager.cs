using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    public GameObject[] popups;
    private int popUpIndex = 0;
    public Button nextButton;
    public float waitTime = 1.5f;
    bool popupActive = false;

    private void Awake()
    {
        foreach(GameObject go in popups)
            go.SetActive(false);
    }

    void Update ()
    {
        if (!popupActive && waitTime <= 0)
            InitializePopups();
        else if (!popupActive)
            waitTime -= Time.deltaTime;
        else
            nextButton.gameObject.SetActive(true);
    }

    void InitializePopups()
    {
        for (int i = 0; i < popups.Length; i++)
        {
            if (i == popUpIndex)
                popups[i].SetActive(true);
            else
                popups[i].SetActive(false);
        }
        popupActive = true;
    }

    public void NextTutorial()
    {
        Debug.Log("button pushed");
        popUpIndex++;
        popupActive = false;
        nextButton.gameObject.SetActive(false);

        foreach (GameObject go in popups)
            go.SetActive(false);
    }
}
