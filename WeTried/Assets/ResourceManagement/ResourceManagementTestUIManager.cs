using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManagementTestUIManager : MonoBehaviour
{
    #region Public Fields
    [Header("Wire up before starting the game - Core Logic.")]
    public ResourceManagementCore CoreLogic;

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
    public Text ForestPeasants;
    #endregion

    #region Unity Life Cycles

    private void Start()
    {
        if (CoreLogic == null) throw new InvalidOperationException("For the UI manager CoreLogic reference should not be null");

    }

    private void OnEnable()
    {
        // whenever you subscribe it's important to unsubscribe
        CoreLogic.OnUIUpdate += UpdateUI;
    }

    private void OnDisable()
    {
        CoreLogic.OnUIUpdate -= UpdateUI;
    }

    #endregion

    #region Implementation

    public void UpdateUI()
    {
        var resourceState = CoreLogic.CurrentGameState;
        UpdateUI(resourceState);
    }

    public void UpdateUI(GameState grs)
    {
        Debug.Log("Updating UI");

        TotalPeasants.text = grs.ResourceState.nPeasants.ToString();
        TotalWood.text = grs.ResourceState.nWoodResources.ToString();
        TotalFood.text = grs.ResourceState.nFoodResources.ToString();
        TotalIron.text = grs.ResourceState.nIronResources.ToString();
        TotalWeapons.text = grs.ResourceState.nWeaponResources.ToString();
        CurrentDay.text = grs.CurrentDay.ToString();

        var peasants = grs.ResourceState.Peasants;
        MinePeasants.text = grs.GetPeasantsAt(ResourceLocation.Mine).ToString();
        FarmPeasants.text = grs.GetPeasantsAt(ResourceLocation.Farm).ToString();
        BlackSmithPeasants.text = grs.GetPeasantsAt(ResourceLocation.BlackSmiths).ToString();
        CrowdPeasants.text = grs.GetPeasantsAt(ResourceLocation.CrowdPit).ToString();
        SacrificePesants.text = grs.GetPeasantsAt(ResourceLocation.SacrificialPen).ToString();
        LakePeasants.text = grs.GetPeasantsAt(ResourceLocation.Lake).ToString();
        ForestPeasants.text = grs.GetPeasantsAt(ResourceLocation.Forest).ToString();
    }

    #endregion
}
