using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SobStoryText : MonoBehaviour {

    public Button[] buttons;
    public SobStory[] sobStories;
    public Text ssText;
    public Button nextButton;

    [Range(0, 10)]
    public float waitTime;
    int currentSSIndex;
    int activeSSIndex1 = 0;
    int activeSSIndex2 = 1;
    int sceneIndex = 0;

    void Start()
    {
        if (buttons != null) 
            EnableDisableNameButtons(true,false);

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        PresentFirstSobstory();
    }

    public SobStory GetSobStory(int ssIndex)
    {
        var sobStory = sobStories[ssIndex];
        return sobStory;
    }

    void PresentFirstSobstory()
    {
        var ss = GetSobStory(activeSSIndex1);
        ssText.text = ss.StoryTellerName + ": " + ss.SobStoryText;

        nextButton.gameObject.SetActive(true);
    }

    void PresentSecondSobstory()
    {
        var ss = GetSobStory(activeSSIndex2);
        ssText.text = ss.StoryTellerName + ": " + ss.SobStoryText;

        nextButton.gameObject.SetActive(true);
    }

    void PresentChoices(int ss1, int ss2)
    {
        InitializeNameButtons(ss1, ss2);
    }

    void InitializeNextButton()
    {
        nextButton.gameObject.SetActive(true);
        nextButton.enabled = true;

        if (currentSSIndex % 2 == 0)
            nextButton.onClick.AddListener(() => PresentSecondSobstory());
        else
            nextButton.onClick.AddListener(() => PresentChoices(activeSSIndex1, activeSSIndex2));
    }

    void InitializeNameButtons(int ssIndex1, int ssIndex2)
    {
        buttons[0].GetComponentInChildren<Text>().text = sobStories[ssIndex1].StoryTellerName;
        buttons[1].GetComponentInChildren<Text>().text = sobStories[ssIndex2].StoryTellerName;

        EnableDisableNameButtons(true, true);

        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];

            int buttonIndex = i;
            button.onClick.AddListener(() => ShowResponse(buttonIndex));
        }
    }

    void ShowResponse(int buttonIndex)
    {
        if (buttonIndex % 2 == 0)
            ssText.text = sobStories[activeSSIndex1].Response;
        else
            ssText.text = sobStories[activeSSIndex2].Response;

        StartCoroutine(MoveToNextDayAfterDelay());
    }

    void EnableDisableNameButtons(bool isEnabled, bool isGOEnabled)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Button>().enabled = isEnabled;
            buttons[i].gameObject.SetActive(isGOEnabled);
        }
    }

    IEnumerator MoveToNextDayAfterDelay()
    {
        EnableDisableNameButtons(false,true);
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(sceneIndex);
    }
}

[Serializable]
public class SobStory
{
    public string StoryTellerName;
    public string SobStoryText;
    public string Response;
}
