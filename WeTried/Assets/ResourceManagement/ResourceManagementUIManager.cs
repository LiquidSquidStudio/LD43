using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManagementUIManager : MonoBehaviour
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
        UpdateUI();
    }

    private void OnEnable()
    {
        CoreLogic.UpdateUIEvent.AddListener(UpdateUI);

    }

    private void OnDisable()
    {
        CoreLogic.UpdateUIEvent.RemoveListener(UpdateUI);
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
        //Debug.Log("Updating UI");

        TotalPeasants.text = grs.ResourceState.nPeasants.ToString();
        TotalWood.text = grs.ResourceState.nWoodResources.ToString();
        TotalFood.text = grs.ResourceState.nFoodResources.ToString();
        TotalIron.text = grs.ResourceState.nIronResources.ToString();
        TotalWeapons.text = grs.ResourceState.nWeaponResources.ToString();
        CurrentDay.text = grs.CurrentDay.ToString();

        var peasants = grs.ResourceState.Peasants;
        MinePeasants.text = grs.GetNumberOfPeasantsAt(ResourceLocation.Mine).ToString();
        FarmPeasants.text = grs.GetNumberOfPeasantsAt(ResourceLocation.Farm).ToString();
        BlackSmithPeasants.text = grs.GetNumberOfPeasantsAt(ResourceLocation.BlackSmiths).ToString();
        CrowdPeasants.text = grs.GetNumberOfPeasantsAt(ResourceLocation.CrowdPit).ToString();
        SacrificePesants.text = grs.GetNumberOfPeasantsAt(ResourceLocation.SacrificialPen).ToString();
        LakePeasants.text = grs.GetNumberOfPeasantsAt(ResourceLocation.Lake).ToString();
        ForestPeasants.text = grs.GetNumberOfPeasantsAt(ResourceLocation.Forest).ToString();
    }

    #endregion
}
