﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceManagementCore : MonoBehaviour
{
    #region Property
    public KingData King { get; private set; }
    public DragonData Dragon { get; private set; }
    public GameResourceState ResourceState { get; private set; }
    #endregion

    #region PrivateFields
    [SerializeField]
    [Tooltip("Number of peasants on game start")]
    private int _nPeasantsOnStartup = 100;

    [SerializeField]
    [Tooltip("Stat boost multiplier")]
    private float _statBoostMax = 5;

    [SerializeField]
    [Tooltip("Percentage chance of gaining a random affinity")]
    [Range(0.0f, 100.0f)]
    private float _affinityChancePercent = 10.0f;

    #endregion


    #region Unity lifecycles
    public void Awake()
    {
        Initialise();
    }
    #endregion

    #region Implementation

    #region For the UI

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

    public void AddPeasant(int incrementValue)
    {
        throw new NotImplementedException();
    }

    public void SubtractPeasantResource(int decrementValue)
    {
        throw new NotImplementedException();
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

    #region Core Game mechanics

    public GameResourceState DayEnd()
    {
        throw new NotImplementedException();
    }

    public void Initialise()
    {
        Random.InitState(Guid.NewGuid().GetHashCode()); // pretty much guarantees uniqueness between gameruns

        if (King == null) { King = new KingData(); }   
        if (Dragon == null) { Dragon = new DragonData(); }   
        if (ResourceState == null) {

            var peasants = GeneratePeasants(_nPeasantsOnStartup);
            ResourceState = new GameResourceState(peasants:peasants);
        }
    }

    #endregion

    #endregion

    #region ImplementationDetail

    private IEnumerable<Peasant> GeneratePeasants(int nPeasants)
    {
        var peasants = new List<Peasant>();

        int count = 0;

        while (count < nPeasants)
        {
            int level = 0;
            var location = ResourceLocation.CrowdPit;
            float statBoost = GenerateRandomStatBoost();
            var affinity = AssignRandomAffinity();
            bool transit = false;

            peasants.Add(
                new Peasant(
                    level: level,
                    location: location,
                    statBoostBonus: statBoost,
                    resourceAffinity: affinity,
                    inTransit: transit
                ));
        }

        return peasants;
    }

    private List<MaterialResourceType> AssignRandomAffinity()
    {
        var result = new List<MaterialResourceType>();

        if (RandomChance(_affinityChancePercent))
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

    private float GenerateRandomStatBoost()
    {
        return Random.Range(0.0f, _statBoostMax);
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
