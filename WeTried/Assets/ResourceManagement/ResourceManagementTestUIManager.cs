using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManagementTestUIManager : MonoBehaviour
{
    #region Public Fields
    [Header("Wire up before starting the game - UI Text elements to update.")]
    public Text TotalPeasants;
    public Text TotalWood;
    public Text TotalFood;
    public Text TotalIron;
    public Text TotalWeapons;
    public Text CurrentDay;

    public Text MinePeasants;
    public Text FarmPeasants;
    public Text BlackSmithPeasants;
    public Text CrowdPeasants;
    public Text SacrificePesants;
    public Text LakePeasants;
    public Text ForrestPeasants;
    #endregion

    #region Implementation

    public void UpdateUI(GameResourceState resourceState, int currentDay)
    {
        Debug.Log("Updating UI");

        TotalPeasants.text = resourceState.nPeasants.ToString();
        TotalWood.text = resourceState.nWoodResources.ToString();
        TotalFood.text = resourceState.nFoodResources.ToString();
        TotalIron.text = resourceState.nIronResources.ToString();
        TotalWeapons.text = resourceState.nWeaponResources.ToString();
        CurrentDay.text = currentDay.ToString();

        var peasants = resourceState.Peasants;
        MinePeasants.text = GetPeastantsAt(peasants, ResourceLocation.Mine).ToString();
        FarmPeasants.text = GetPeastantsAt(peasants, ResourceLocation.Farm).ToString();
        BlackSmithPeasants.text = GetPeastantsAt(peasants, ResourceLocation.BlackSmiths).ToString();
        CrowdPeasants.text = GetPeastantsAt(peasants, ResourceLocation.CrowdPit).ToString();
        SacrificePesants.text = GetPeastantsAt(peasants, ResourceLocation.SacrificialPen).ToString();
        LakePeasants.text = GetPeastantsAt(peasants, ResourceLocation.Lake).ToString();
        ForrestPeasants.text = GetPeastantsAt(peasants, ResourceLocation.Forrest).ToString();

    }

    #endregion

    #region Private Helpers

    private int GetPeastantsAt(IEnumerable<Peasant> peasants, ResourceLocation location)
    {
        return peasants.Count(p => p.CurrentLocation == location);
    }

    #endregion

}
