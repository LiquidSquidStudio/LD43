using UnityEngine;

public class PeasantManager : MonoBehaviour
{
    // Using this to pass in the correct transform into the peasants for movement;
    // Take the location of the mouse and determine if a location and where

    public GameObject[] buildings;
    public GameObject peasantPrefab;    // Gonna change this later, shouldn't be spawned in or anything, but just doing it this way for now
    public int targetLocIndex;

    GameObject selectedPeasant;

    ClickableBuildingController[] cbControllers;
    Transform[] entranceLocs;

    private void Awake()
    {
        InitializeBuildingComponents();
    }

    private void Start()
    {
        foreach (var cbc in cbControllers)
        {
            cbc.clickedThisBuilding.RemoveListener(SetPeasantMoving);
            cbc.clickedThisBuilding.AddListener(SetPeasantMoving);
        }
    }

    void InitializeBuildingComponents()
    {
        cbControllers = new ClickableBuildingController[buildings.Length];
        entranceLocs= new Transform[buildings.Length];
        for (int i = 0; i < buildings.Length; i++)
        {
            ClickableBuildingController cbc = buildings[i].GetComponent<ClickableBuildingController>();
            cbControllers[i] = cbc;
            Transform entrance = buildings[i].GetComponentInChildren<Transform>();
            entranceLocs[i] = entrance;
        }
    }

    void Update ()
    {
        if (selectedPeasant == null)
        {
            selectedPeasant = SelectNextPeasant();
        }
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
