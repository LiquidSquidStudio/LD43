using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour {

    private int _beginSceneIndex = 0;

    public void OnReplay()
    {
        ResetGameState();
        SceneManager.LoadScene(_beginSceneIndex);
    }

    private void ResetGameState()
    {
        PersistingData.gs = null;
        PersistingData.storyProgression = 0;
    }
}
