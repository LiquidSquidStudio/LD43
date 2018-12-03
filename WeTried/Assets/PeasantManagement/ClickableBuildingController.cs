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
    //public BuildingUIEvent MoveFromBuildingEvent;

    public Image FirstSlectImage;
    public Image SecondSelectImage;

    private bool _selected = false;

    public bool CanBeOrigin = true;
    public bool CanBeDestination = true;

    public virtual void OnClicked()
    {
        if (MoveToBuildingEvent != null)
        {
            MoveToBuildingEvent.Invoke(buildingType);
        }
    }

    private void ToggleSelectionState(Image toToggle)
    {
        _selected = !_selected;

        toToggle.enabled = _selected;
    }

    public void Deselect()
    {
        _selected = false;
        FirstSlectImage.enabled = false;
        SecondSelectImage.enabled = false;
    }

}
