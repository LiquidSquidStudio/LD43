using System.Collections.Generic;
using UnityEngine;

public class Peasant : MonoBehaviour
{
    public int Level { get; set; }
    public ResourceLocations CurrentLocation { get; set; }
    public float StatBoostBonus { get; set; }
    public IEnumerable<MaterialResourceType> ResourceAffinity { get; set; }

    public Peasant(ResourceLocations resourceLocations, float statBoostBonus = 0.0f, IEnumerable<MaterialResourceType> resourceAffinity = null)
    {
        CurrentLocation = resourceLocations;
        StatBoostBonus = statBoostBonus;
        ResourceAffinity = resourceAffinity ?? new List<MaterialResourceType>();
    }
}

