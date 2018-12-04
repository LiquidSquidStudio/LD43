using UnityEngine;

public class WinController : MonoBehaviour {

    private void Start()
    {
        AudioController ac = FindObjectOfType<AudioController>();
        ac.StopAllClips();
        ac.PlayClip("Success");
        ac.PlayClip("WinSmall");
    }
}
