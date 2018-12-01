using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ResourceManagementCoreLogic
{
    #region Property
    public KingModel King { get; private set; }
    public DragonData Dragon { get; private set; }
    public GameResourceState ResourceState { get; private set; }
    #endregion

    #region Constructor
    public ResourceManagementCoreLogic(KingModel king, DragonData dragon, GameResourceState resourceState)
    {
        King = king;
        Dragon = dragon;
        ResourceState = resourceState;
    }
    #endregion

    #region Implementation

    #region For the UI



    public void AddWoodResource(int incrementValue)
    {
        AddResource(GameResource.Wood, incrementValue);
    }

    public void SubtractWoodResource(int decrementValue)
    {
        SubtractResource(GameResource.Wood, decrementValue);
    }

    public int GetWoodResource()
    {
        return ResourceState.nWoodResources;
    }

    public void AddIronResource(int incrementValue)
    {
        AddResource(GameResource.Iron, incrementValue);
    }

    public void SubtractIronResource(int decrementValue)
    {
        SubtractResource(GameResource.Iron, decrementValue);
    }

    public int GetIronResource()
    {
        return ResourceState.nIronResources;
    }

    public void AddFoodResource(int incrementValue)
    {
        AddResource(GameResource.Food, incrementValue);
    }

    public void SubtractFoodResource(int decrementValue)
    {
        SubtractResource(GameResource.Food, decrementValue);
    }

    public int GetFoodResource()
    {
        return ResourceState.nFoodResources;
    }

    public void AddPeasantResource(int incrementValue)
    {
        AddResource(GameResource.Peasants, incrementValue);
    }

    public void SubtractPeasantResource(int decrementValue)
    {
        SubtractResource(GameResource.Peasants, decrementValue);
    }

    public int GetPeasantResource()
    {
        return ResourceState.nPeasants;
    }

    public void AddWeaponResource(int incrementValue)
    {
        AddResource(GameResource.Weapon, incrementValue);
    }

    public void SubtractWeaponResource(int decrementValue)
    {
        SubtractResource(GameResource.Weapon, decrementValue);
    }

    public int GetWeaponResource()
    {
        return ResourceState.nWeaponResources;
    }

    #endregion

    #endregion


    #region ImplementationDetail

    private void AddResource(GameResource gameResource, int incrementValue)
    {
        if (incrementValue <= 0)
            return;

        switch (gameResource)
        {
            case GameResource.Wood:
                ResourceState.nWoodResources += incrementValue;
                break;
            case GameResource.Iron:
                ResourceState.nIronResources += incrementValue;
                break;
            case GameResource.Food:
                ResourceState.nFoodResources += incrementValue;
                break;
            case GameResource.Weapon:
                ResourceState.nWeaponResources += incrementValue;
                break;
            case GameResource.Peasants:
                ResourceState.nPeasants += incrementValue;
                break;
          
            default:
                throw new ArgumentException("Unknown resource passed in to add");

                break;
        }
    }

    private void SubtractResource(GameResource gameResource, int decrementValue)
    {
        if (decrementValue <= 0) return;

        int newValue = 0;
        switch (gameResource)
        {
            case GameResource.Wood:
                newValue = ResourceState.nWoodResources - decrementValue;
                ResourceState.nWoodResources = Math.Max(0, newValue);
                break;

            case GameResource.Iron:
                newValue = ResourceState.nIronResources - decrementValue;
                ResourceState.nIronResources = Math.Max(0, newValue);
                break;

            case GameResource.Food:
                newValue = ResourceState.nFoodResources - decrementValue;
                ResourceState.nFoodResources = Math.Max(0, newValue);
                break;

            case GameResource.Weapon:
                newValue = ResourceState.nWeaponResources - decrementValue;
                ResourceState.nWeaponResources = Math.Max(0, newValue);
                break;
            case GameResource.Peasants:
                newValue = ResourceState.nPeasants - decrementValue;
                ResourceState.nPeasants = Math.Max(0, newValue);
                break;

            default:
                throw new ArgumentException("Unknown resource passed in to add");
                
        }
    }

    #endregion

}

