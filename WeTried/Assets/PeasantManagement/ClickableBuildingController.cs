using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class BuildingUIEvent : UnityEvent<ResourceLocation> { }

public class ClickableBuildingController : MonoBehaviour {

    public ResourceLocation buildingType;
    public BuildingUIEvent clickedThisBuilding;

    public Image SelectedImage;

    public static HashSet<ClickableBuildingController> allSelectableBuildings = new HashSet<ClickableBuildingController>();
    public static HashSet<ClickableBuildingController> currentlySelectedBuildings = new HashSet<ClickableBuildingController>();
    private bool _selected = false;

    private void Start()
    {
        allSelectableBuildings.Add(this);
    }

    public virtual void OnClicked()
    {
        clickedThisBuilding.Invoke(buildingType);
        Debug.Log("You clicked the " + buildingType);

        if (currentlySelectedBuildings.Count > 2)
        {
            DeselectAll();
        } else
        {
            ToggleSelectionState();

        }
    }

    private void ToggleSelectionState()
    {
        _selected = !_selected;

        if (_selected)
        {
            currentlySelectedBuildings.Add(this);
        } else
        {
            currentlySelectedBuildings.Remove(this);
        }
        SelectedImage.enabled = _selected;
    }

    public void Deselect()
    {
        _selected = false;
    }

    private void DeselectAll()
    {
        foreach (var building in allSelectableBuildings)
        {
            building.Deselect();
        }
    }

    void ToggleImageAppearance()
    {

    }

   
}
