using System.Collections.Generic;
using UnityEngine;

public class Peasant : MonoBehaviour
{
    #region Properties

    public int Level { get; set; }
    public ResourceLocation CurrentLocation { get; set; }
    public float StatBoostBonus { get; set; }
    public IEnumerable<MaterialResourceType> ResourceAffinity { get; set; }
    public bool IsInTrasit { get; set; }

    #endregion

    #region Implementation

    public Peasant(int level = 0, ResourceLocation location = ResourceLocation.CrowdPit, float statBoostBonus = 0.0f, IEnumerable<MaterialResourceType> resourceAffinity = null, bool inTransit = false)
    {
        Level = level;
        CurrentLocation = location;
        StatBoostBonus = statBoostBonus;
        ResourceAffinity = resourceAffinity ?? new List<MaterialResourceType>();
        IsInTrasit = inTransit;
    }

    public void Die()
    {
        Destroy(this.transform.gameObject);
    }

    #endregion

}

