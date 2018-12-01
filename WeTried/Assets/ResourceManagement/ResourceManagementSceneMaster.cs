using UnityEngine;

public class ResourceManagementSceneMaster : MonoBehaviour
{
    public ResourceManagementCore CoreLogic { get; set; }
    public ResourceManagementTestUIManager UIManager { get; set; }
    public ResourceManagementSceneState SceneState { get; set; }


    #region Unity Lifecycles

    #endregion

    #region Implementations

    public void DayEnd()
    {
        SceneState = ResourceManagementSceneState.DayEnd;

        CoreLogic.DayEnd();
    }

    #endregion



}
