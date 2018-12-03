using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManagementTestUIInputManager : MonoBehaviour {

    #region Public Fields
    [Header("Make sure to wire this up before startup")]
    public Dropdown DropdownFrom;
    public Dropdown DropdownTo;
    public InputField InputQuantity;
    public ResourceManagementCore ResourceManager;

    #endregion

    #region Unity Life Cycle

    public void Start()
    {
        if (DropdownFrom == null || DropdownTo == null || InputQuantity == null || ResourceManager == null)
        {
            throw new ArgumentNullException("UI Field", "Test UI Input Manager public fields not wired up");
        }

        //Debug.Log("Populating Dropdowns...");
        PopulateDropdowns();
        //Debug.Log("Populated!");
    }

    #endregion

    #region Implementation

    public void MovePeasants()
    {
        var from = GetFromLocation();
        var to = GetToLocation();
        var nPeasants = GetNumberOfPeasantsToMove();

        if (ShouldMovePeasants(from, to, nPeasants))
        {
            Debug.Log("Moving " + nPeasants.ToString() + " from " + from.ToString() + " to " + to.ToString());

            var peasantsAtSource = ResourceManager.GetPeasantsAt(from).Take(nPeasants);

            foreach (var peasant in peasantsAtSource)
            {
                peasant.CurrentLocation = to;
            }
        } else
        {
            Debug.Log("Moving Condition is invalid");
        }
    }

    private bool ShouldMovePeasants(ResourceLocation from, ResourceLocation to, int nPeasants)
    {
        var nPeastantsAtSource = ResourceManager.GetNumberOfPeastantsAt(from);

        return (from != to && nPeasants >= 0 && nPeasants <= nPeastantsAtSource);
    }

    #endregion

    #region Implementation Detail

    private void PopulateDropdowns()
    {
        var locationNames = Enum.GetNames(typeof(ResourceLocation));
        List<string> locations = new List<string>(locationNames);

        DropdownFrom.options = new List<Dropdown.OptionData>();
        DropdownTo.options = new List<Dropdown.OptionData>();
        DropdownFrom.AddOptions(locations);
        DropdownTo.AddOptions(locations);
    }

    private ResourceLocation GetFromLocation()
    {
        var dropdownIndex = DropdownFrom.value;
        return (ResourceLocation)dropdownIndex;
    }

    private ResourceLocation GetToLocation()
    {
        var dropdownIndex = DropdownTo.value;
        return (ResourceLocation)dropdownIndex;
    }

    private int GetNumberOfPeasantsToMove()
    {
        var inputText = InputQuantity.text;
        int result = 0;

        if (int.TryParse(inputText, out result))
        {
            return result;
        } else
        {
            Debug.Log("Invalid text detected in Input Field. Please keep it as whole numbers and no characters.");
        }

        return result;
    }

    #endregion
}