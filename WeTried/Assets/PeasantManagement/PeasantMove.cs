using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasantMove : MonoBehaviour {

    // Move the peasant to the clicked building
    // Can probably include some more elaborate movement scheme later on if need be

    [Range(20,100)]
    public float moveSpeed;
    public bool isMoving = false;
    public float reachedLocationRadius;

    Vector2 dir;
    float distance;
    Transform target;

    public void StartMoving(Transform tar)
    {
        target = tar;
        dir = (tar.position - transform.position).normalized;
        isMoving = true;
    }

    void Update ()
    {
        if (isMoving)
            MovePeasant();

        if (isMoving && HasReachedLocation())
            ReachedLocation();
    }

    void MovePeasant()
    {
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    bool HasReachedLocation()
    {
        bool locCheck = false;

        distance = (target.position - transform.position).magnitude;

        if (distance <= reachedLocationRadius)
            locCheck = true;

        return locCheck;
    }

    public void ReachedLocation()
    {
        Debug.Log("peasant has reached the building");
        Destroy(this.gameObject);
    }
}
