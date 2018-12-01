using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ResourceManagementCoreLogic
{
    #region Property
    public KingData King { get; private set; }
    public DragonData Dragon { get; private set; }
    public GameResourceState ResourceState { get; private set; }
    #endregion

    #region Constructor
    public ResourceManagementCoreLogic(KingData king, DragonData dragon, GameResourceState resourceState)
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

    public IEnumerable<Peasant> GetPeasantResource()
    {
        return ResourceState.Peasants;
    }

    public int GetNumberOfPeasants()
    {
        return ResourceState.nPeasants;
    }

    #endregion

    #endregion


    #region ImplementationDetail

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

