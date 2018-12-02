using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;
using UnityEngine;

public class GameState
{
    public KingData King { get; private set; }
    public DragonData Dragon { get; private set; }
    public GameResourceState ResourceState { get; private set; }
    public int CurrentDay { get; private set; }

    #region Constructor
    public GameState(int nPeasants, float affinityPercent, float statBoostMax)
    {
        Initialise(nPeasants, affinityPercent, statBoostMax);
    }

    private void Initialise(int nPeasants, float affinityPercent, float statBoostMax)
    {
        if (King == null) { King = new KingData(); }
        if (Dragon == null) { Dragon = new DragonData(); }
        if (ResourceState == null)
        {

            var peasants = GeneratePeasants(nPeasants, affinityPercent, statBoostMax);
            ResourceState = new GameResourceState(peasants: peasants);
        }
    }
    #endregion

    #region Accessors
    public void AddWoodResource(int incrementValue)
    {
        AddMaterialResource(MaterialResourceType.Wood, incrementValue);
    }

    public void SubtractWoodResource(int decrementValue)
    {
        SubtractMaterialResource(MaterialResourceType.Wood, decrementValue);
    }

    public int GetWoodResource()
    {
        return ResourceState.nWoodResources;
    }

    public void AddIronResource(int incrementValue)
    {
        AddMaterialResource(MaterialResourceType.Iron, incrementValue);
    }

    public void SubtractIronResource(int decrementValue)
    {
        SubtractMaterialResource(MaterialResourceType.Iron, decrementValue);
    }

    public int GetIronResource()
    {
        return ResourceState.nIronResources;
    }

    public void AddFoodResource(int incrementValue)
    {
        AddMaterialResource(MaterialResourceType.Food, incrementValue);
    }

    public void SubtractFoodResource(int decrementValue)
    {
        SubtractMaterialResource(MaterialResourceType.Food, decrementValue);
    }

    public int GetFoodResource()
    {
        return ResourceState.nFoodResources;
    }

    public void AddWeaponResource(int incrementValue)
    {
        AddMaterialResource(MaterialResourceType.Weapon, incrementValue);
    }

    public void SubtractWeaponResource(int decrementValue)
    {
        SubtractMaterialResource(MaterialResourceType.Weapon, decrementValue);
    }

    public int GetWeaponResource()
    {
        return ResourceState.nWeaponResources;
    }

    public IEnumerable<Peasant> GetPeasantsAt(ResourceLocation location)
    {
        return ResourceState.Peasants.Where(p => p.CurrentLocation == location);
    }

    public IEnumerable<Peasant> GetPeasants()
    {
        return ResourceState.Peasants;
    }

    public int GetNumberOfPeasants()
    {
        return ResourceState.nPeasants;
    }
    
    #endregion

    #region Private helpers

    private IEnumerable<Peasant> GeneratePeasants(int nPeasants, float affinityPercent, float statBoostMax)
    {
        var peasants = new List<Peasant>();

        int count = 0;

        while (count < nPeasants)
        {
            int level = 0;
            var location = ResourceLocation.CrowdPit;
            float statBoost = GenerateRandomStatBoost(statBoostMax);
            var affinity = AssignRandomAffinity(affinityPercent);
            bool transit = false;

            peasants.Add(
                new Peasant(
                    level: level,
                    location: location,
                    statBoostBonus: statBoost,
                    resourceAffinity: affinity,
                    inTransit: transit
                ));

            count++;
        }

        return peasants;
    }

    private List<MaterialResourceType> AssignRandomAffinity(float affinityPercent)
    {
        var result = new List<MaterialResourceType>();

        if (RandomChance(affinityPercent))
        {
            result.Add(SelectRandomRersource());
        }

        return result;

    }

    private MaterialResourceType SelectRandomRersource()
    {
        int nResourceTypes = Enum.GetNames(typeof(MaterialResourceType)).Length;
        int randommIndex = (int)Random.Range(0.0f, nResourceTypes);
        return (MaterialResourceType)randommIndex;
    }

    private bool RandomChance(float chance)
    {
        var comparer = Random.Range(0.0f, 100.0f);

        return (comparer <= chance);
    }

    private float GenerateRandomStatBoost(float statBoostMax)
    {
        return Random.Range(0.0f, statBoostMax);
    }

    private void AddMaterialResource(MaterialResourceType gameResource, int incrementValue)
    {
        if (incrementValue <= 0)
            return;

        switch (gameResource)
        {
            case MaterialResourceType.Wood:
                ResourceState.nWoodResources += incrementValue;
                break;
            case MaterialResourceType.Iron:
                ResourceState.nIronResources += incrementValue;
                break;
            case MaterialResourceType.Food:
                ResourceState.nFoodResources += incrementValue;
                break;
            case MaterialResourceType.Weapon:
                ResourceState.nWeaponResources += incrementValue;
                break;

            default:
                throw new ArgumentException("Unknown resource passed in to add");
        }

    }

    private void SubtractMaterialResource(MaterialResourceType gameResource, int decrementValue)
    {
        if (decrementValue <= 0) return;

        int newValue = 0;
        switch (gameResource)
        {
            case MaterialResourceType.Wood:
                newValue = ResourceState.nWoodResources - decrementValue;
                ResourceState.nWoodResources = Math.Max(0, newValue);
                break;

            case MaterialResourceType.Iron:
                newValue = ResourceState.nIronResources - decrementValue;
                ResourceState.nIronResources = Math.Max(0, newValue);
                break;

            case MaterialResourceType.Food:
                newValue = ResourceState.nFoodResources - decrementValue;
                ResourceState.nFoodResources = Math.Max(0, newValue);
                break;

            case MaterialResourceType.Weapon:
                newValue = ResourceState.nWeaponResources - decrementValue;
                ResourceState.nWeaponResources = Math.Max(0, newValue);
                break;

            default:
                throw new ArgumentException("Unknown resource passed in to add");

        }
    }
    
    #endregion
}
