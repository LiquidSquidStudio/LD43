using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SobStoryText : MonoBehaviour
{
    public Text ssText;
    //public Text scrollingText; For the future
    public Button[] buttons;
    public Button nextButton;

    [Range(0, 10)]
    public float waitTime;

    SobStoryContainer ssc;
    int activeSSIndex = 0;

    int sceneIndex = 0;

    private void Awake()
    {
        ssc = GetComponent<SobStoryContainer>();
        if (buttons != null)
            EnableDisableNameButtons(true, false);

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);
    }

    public void SetSSIndex(int sp)
    {
        activeSSIndex = sp * 2;
        PresentFirstSobstory();
    }

    public SobStory GetSobStory(int ssIndex)
    {
        var sobStory = ssc.sobStories[ssIndex];
        return sobStory;
    }

    public void PresentFirstSobstory()
    {
        var ss = GetSobStory(activeSSIndex);
        ssText.text = ss.StoryTellerName + ": " + ss.SobStoryText;

        InitializeNextButton(false);
    }

    void PresentSecondSobstory()
    {
        var ss = GetSobStory(activeSSIndex + 1);
        ssText.text = ss.StoryTellerName + ": " + ss.SobStoryText;

        InitializeNextButton(true);
    }

    void PresentChoices(int ss1)
    {
        InitializeNameButtons(ss1);
        nextButton.gameObject.SetActive(false);

        ssText.text = "Who do will you Sacrifice?";
    }

    void InitializeNextButton(bool isSecond)
    {
        nextButton.gameObject.SetActive(true);
        nextButton.enabled = true;

        nextButton.GetComponentInChildren<Text>().text = "Next";

        if (!isSecond)
            nextButton.onClick.AddListener(() => PresentSecondSobstory());
        else
            nextButton.onClick.AddListener(() => PresentChoices(activeSSIndex));
    }

    void InitializeNameButtons(int ssIndex)
    {
        buttons[0].GetComponentInChildren<Text>().text = ssc.sobStories[ssIndex].StoryTellerName;
        buttons[1].GetComponentInChildren<Text>().text = ssc.sobStories[ssIndex + 1].StoryTellerName;

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
            ssText.text = ssc.sobStories[activeSSIndex].Response;
        else
            ssText.text = ssc.sobStories[activeSSIndex + 1].Response;

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
        EnableDisableNameButtons(false, true);
        yield return new WaitForSeconds(waitTime);
        PersistingData.storyProgression++;
        Debug.Log("story progression " + PersistingData.storyProgression);

        SceneManager.LoadScene(sceneIndex);
    }
}
