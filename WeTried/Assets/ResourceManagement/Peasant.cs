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
    public float MoveSpeed = 20;
    public float ReachedLocationRadius = 5;
    public CrowdController Controller { get; set; }

    float distance;
    Vector3 _target;
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

    Vector3 CalculateDirection(Vector3 target)
    {
        return (target - transform.position).normalized;
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
        Debug.Log("I died.");
        Controller.ResourceCore.RemovePeasant(this);
        Destroy(gameObject);
    }

    public void StartMoving(ResourceLocation destination, Vector3 tar)
    {
        Debug.Log("Starting to move");
        _currentDestination = destination;
        _target = tar;
        IsInTrasit = true;
    }

    void MovePeasant()
    {
        var dir = CalculateDirection(_target);
        transform.Translate(dir * MoveSpeed * Time.deltaTime);
    }

    bool HasReachedLocation()
    {
        bool locCheck = false;

        distance = (_target - transform.position).magnitude;

        if (distance <= ReachedLocationRadius)
            locCheck = true;

        return locCheck;
    }

    public void ReachedLocation()
    {
        CurrentLocation = _currentDestination;
        IsInTrasit = false;
        Controller.OnPeasantReachedDestination(CurrentLocation);
    }

    #endregion

}

