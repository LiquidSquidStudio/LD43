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
    public BuildingUIEvent MoveToBuildingEvent;
    public BuildingUIEvent MoveFromBuildingEvent;

    public Image FirstSlectImage;
    public Image SecondSelectImage;

    public static HashSet<ClickableBuildingController> AllSelectableBuildings = new HashSet<ClickableBuildingController>();
    public static HashSet<ClickableBuildingController> CurrentlySelectedBuildings = new HashSet<ClickableBuildingController>();
    private bool _selected = false;

    public bool CanBeOrigin = true;
    public bool CanBeDestination = true;

    private void Start()
    {
        AllSelectableBuildings.Add(this);
    }

    public virtual void OnClicked()
    {
        if (CurrentlySelectedBuildings.Count > 1)
        {
            DeselectAll();
        }
        else if (CurrentlySelectedBuildings.Count == 1)
        {
            if (CurrentlySelectedBuildings.Contains(this))
            {
                ToggleSelectionState(FirstSlectImage);
            }
            else
            {
                if (CanBeDestination)
                {
                    ToggleSelectionState(SecondSelectImage);
                    Debug.Log("You clicked the " + buildingType);
                    if (MoveToBuildingEvent != null)
                    {
                        Debug.Log("Invoking Event to go to: " + buildingType);
                        MoveToBuildingEvent.Invoke(buildingType);
                    }
                }
            }
        }
        else
        {
            if (CanBeOrigin)
            {
                ToggleSelectionState(FirstSlectImage);
                if (MoveFromBuildingEvent != null)
                {
                    MoveFromBuildingEvent.Invoke(buildingType);
                }
            }
        }
    }

    private void ToggleSelectionState(Image toToggle)
    {
        _selected = !_selected;

        if (_selected)
        {
            CurrentlySelectedBuildings.Add(this);
        } else
        {
            CurrentlySelectedBuildings.Remove(this);
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
        foreach (var building in AllSelectableBuildings)
        {
            building.Deselect();
            CurrentlySelectedBuildings.Remove(building);
        }
    }
}
