using System.Collections.Generic;
using UnityEngine;

public class Peasant : MonoBehaviour
{
    public int Level { get; set; }
    public ResourceLocations ResourceLocations { get; set; }
    public float StatBoostBonus { get; set; }
    public IEnumerable<GameResource> ResourceAffinity { get; set; }

    public Peasant(ResourceLocations resourceLocations, float statBoostBonus = 0.0f, IEnumerable<GameResource> resourceAffinity = null)
    {
        ResourceLocations = resourceLocations;
        StatBoostBonus = statBoostBonus;
        ResourceAffinity = resourceAffinity;
    }
}

