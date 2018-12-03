using System;
using UnityEngine;

// I know, I know, there's a more correct way to do all this, but in a hurry! ;P
public class SobStoryContainer : MonoBehaviour
{
    public SobStory[] sobStories;
}

[Serializable]
public class SobStory
{
    public string StoryTellerName;
    public string SobStoryText;
    public string Response;
}
