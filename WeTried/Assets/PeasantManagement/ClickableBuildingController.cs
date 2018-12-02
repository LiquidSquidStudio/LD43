using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BuildingUIEvent : UnityEvent<ResourceLocation> { }

public class ClickableBuildingController : MonoBehaviour {

    public ResourceLocation buildingType;
    public BuildingUIEvent clickedThisBuilding;

    public virtual void OnClicked()
    {
            clickedThisBuilding.Invoke(buildingType);
        Debug.Log("You clicked the " + buildingType);
    }
}
