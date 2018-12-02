using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[System.Serializable]
//public class PeasantEvent : UnityEvent<Peasant> { }

public class Peasant : MonoBehaviour
{
    #region Properties

    public int Level { get; set; }
    public ResourceLocation CurrentLocation { get; set; }
    public float StatBoostBonus { get; set; }
    public IEnumerable<MaterialResourceType> ResourceAffinity { get; set; }
    public bool IsInTrasit { get; set; }

    //public PeasantEvent creation;

    public float MoveSpeed = 20;
    public float ReachedLocationRadius = 5;

    Vector2 dir;
    float distance;
    Vector3 target;
    ResourceLocation _currentDestination;

    #endregion

    #region Implementation

    private void Start()
    {
        //creation.Invoke(this);
    }

    private void Update()
    {
        if (IsInTrasit)
            MovePeasant();

        if (IsInTrasit && HasReachedLocation())
            ReachedLocation();
    }

    private void OnDisable()
    {
        IsInTrasit = false;

    }

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

    public void StartMoving(ResourceLocation destination, Vector3 tar)
    {
        Debug.Log("Starting to move");
        _currentDestination = destination;
        target = tar;
        dir = (target - transform.position).normalized;
        IsInTrasit = true;
    }

    void MovePeasant()
    {
        transform.Translate(dir * MoveSpeed * Time.deltaTime);
    }

    bool HasReachedLocation()
    {
        bool locCheck = false;

        distance = (target - transform.position).magnitude;

        if (distance <= ReachedLocationRadius)
            locCheck = true;

        return locCheck;
    }

    public void ReachedLocation()
    {
        CurrentLocation = _currentDestination;
        Debug.Log("peasant has reached the building");
    }

    #endregion

}

