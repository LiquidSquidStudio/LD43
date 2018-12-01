using UnityEngine;

public class PeasantManager : MonoBehaviour
{
    // Using this to pass in the correct transform into the peasants for movement;
    // Take the location of the mouse and determine if a location and where
    public Transform[] entranceLocs;
    public GameObject peasantPrefab;    // Gonna change this later, shouldn't be spawned in or anything, but just doing it this way for now
    public int targetLocIndex;

    GameObject selectedPeasant;

    public ClickableBuildingController[] cbControllers;

    private void Start()
    {
        foreach (var cbc in cbControllers)
        {
            cbc.clickedThisBuilding.RemoveListener(SetPeasantMoving);
            cbc.clickedThisBuilding.AddListener(SetPeasantMoving);
        }
    }

    void Update ()
    {
        if (selectedPeasant == null)
        {
            selectedPeasant = SelectNextPeasant();
        }

        //if (Input.GetKeyDown("space"))
        //{
        //    SetPeasantMoving(targetLocIndex);
        //}
	}

    // At the moment we're just spawning in a new one, but need to get a crowd manager/list of the peasants from oli whenever we're incorporating them
    GameObject SelectNextPeasant()
    {
        GameObject selectedPeasant = Instantiate(peasantPrefab, transform.position, transform.rotation);
        return selectedPeasant;
    }

    public void SetPeasantMoving(ResourceLocation location)
    {
        int locationIndex = (int) location - 2;     // correction based on system difference atm
        PeasantMove moveControls = selectedPeasant.GetComponent<PeasantMove>();
        moveControls.StartMoving(entranceLocs[locationIndex]);
    }
}
