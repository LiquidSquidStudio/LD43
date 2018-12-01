using System.Collections.Generic;
using System.Linq;
using System;

public class GameResourceState
{
    public int nWoodResources { get; set; }
    public int nIronResources { get; set; }
    public int nFoodResources { get; set; }
    public int nWeaponResources { get; set; }
    /// <summary>
    /// Readonly property
    /// </summary>
    public int nPeasants {
        get
        {
            return Peasants.Count();
        }
    }
    public IEnumerable<Peasant> Peasants { get; private set; }

    public GameResourceState()
    {
        this.nWoodResources = 0;
        this.nIronResources = 0;
        this.nFoodResources = 0;
        this.nWeaponResources = 0;
        Peasants = new List<Peasant>();

    }

    public GameResourceState(int nWoodResources=0, int nIronResources=0, int nFoodResources=0, int nWeaponResources=0, IEnumerable<Peasant> peasants=null)
    {
        this.nWoodResources = nWoodResources;
        this.nIronResources = nIronResources;
        this.nFoodResources = nFoodResources;
        this.nWeaponResources = nWeaponResources;
        Peasants = peasants ?? new List<Peasant>();
    }

    public void UpdatePeastants(IEnumerable<Peasant> peasants)
    {
        if (peasants == null)
        {
            throw new ArgumentNullException("peasants", "To update peasants, the list cannot be null");
        }

        Peasants = peasants;

    }
}

