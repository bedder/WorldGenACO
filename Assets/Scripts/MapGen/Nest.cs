using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Nest : MonoBehaviour {
    // Nest dynamics variables
    public float foodPerAntPerTick = 0.1f;
    public float initialFood = 300f;
    public float allowedExcessFood = 100f;
    public int ticksBetweenAntSpawns = 5;
    public int supportedAnts = 10;

    // Prefab for spawning new ants
    public Ant antPrefab;

    // Internal variables
    private float food;
    private List<Ant> ants;
    private int nextAllowedAnt = 0;
    private int currentTick = 0;
    private HexTile location;

    // Trivial Getters

    // Setters/modifiers
    public void setLocation(HexTile newLocation) {
        location = newLocation;
    }
    public void addFood(float newFood) {
        food += newFood;
    }
    private void spawnAnt() {
        Debug.Log("Spawning ant");
        Ant newAnt = Instantiate(antPrefab, location.transform.position, Quaternion.identity) as Ant;
        Debug.Log(newAnt);
        Debug.Log(location);
        newAnt.setLocation(location);
        newAnt.transform.parent = transform.parent;
        ants.Add(newAnt);
        nextAllowedAnt = currentTick + ticksBetweenAntSpawns;
    }

    public void tick() {
        // Update food levels
        food -= foodPerAntPerTick * ants.Count;
        // Spawn ants if required
        if (food > allowedExcessFood && currentTick > nextAllowedAnt && ants.Count < supportedAnts)
            spawnAnt();
        // Kill ants if required
        if (food < 0 && ants.Count > 0)
            ants.RemoveAt(0);
        currentTick++;
    }

    // Unity logic function
    void Awake() {
        ants = new List<Ant>();
        food = initialFood;
        location = gameObject.GetComponent<HexTile>();
    }
}
