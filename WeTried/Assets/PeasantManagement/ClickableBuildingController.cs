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

    public Image FirstSlectImage;
    public Image SecondSelectImage;

    public static HashSet<ClickableBuildingController> allSelectableBuildings = new HashSet<ClickableBuildingController>();
    public static HashSet<ClickableBuildingController> currentlySelectedBuildings = new HashSet<ClickableBuildingController>();
    private bool _selected = false;

    public bool CanBeOrigin = true;
    public bool CanBeDestination = true;

    private void Start()
    {
        allSelectableBuildings.Add(this);
    }

    public virtual void OnClicked()
    {
        clickedThisBuilding.Invoke(buildingType);
        Debug.Log("You clicked the " + buildingType);

        if (currentlySelectedBuildings.Count > 1)
        {
            DeselectAll();
        }
        else if (currentlySelectedBuildings.Count == 1)
        {
            if (currentlySelectedBuildings.Contains(this))
            {
                ToggleSelectionState(FirstSlectImage);
            }
            else
            {
                if (CanBeDestination)
                {
                    ToggleSelectionState(SecondSelectImage);
                }
            }
        }
        else
        {
            if (CanBeOrigin)
            {
                ToggleSelectionState(FirstSlectImage);
            }
        }
    }

    private void ToggleSelectionState(Image toToggle)
    {
        _selected = !_selected;

        if (_selected)
        {
            currentlySelectedBuildings.Add(this);
        } else
        {
            currentlySelectedBuildings.Remove(this);
        }

        toToggle.enabled = _selected;
    }

    public void Deselect()
    {
        _selected = false;
        FirstSlectImage.enabled = false;
        SecondSelectImage.enabled = false;
    }

    private void DeselectAll()
    {
        foreach (var building in allSelectableBuildings)
        {
            building.Deselect();
            currentlySelectedBuildings.Remove(building);
        }
    }

}
