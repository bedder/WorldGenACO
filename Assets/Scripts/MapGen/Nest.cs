using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Nest : MonoBehaviour {
    // Nest dynamics variables
    public float foodPerAnt = 0.1f;
    public float initialFood = 300f;
    public float allowedExcessFood = 100f;
    public int antSpawnFrequency = 5;
    public int maxAnts = 10;

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
        Ant newAnt = Instantiate(antPrefab, location.transform.position, Quaternion.identity) as Ant;
        newAnt.setLocation(location);
        newAnt.transform.parent = transform.parent;
        ants.Add(newAnt);
        nextAllowedAnt = currentTick + antSpawnFrequency;
    }

    public void tick() {
        // Update food levels
        food -= foodPerAnt * ants.Count;
        // Spawn ants if required
        if (food > allowedExcessFood && currentTick > nextAllowedAnt && ants.Count < maxAnts)
            spawnAnt();
        // Kill ants if required
        if (food < 0 && ants.Count > 0)
            ants.RemoveAt(0);
        currentTick++;
    }

    // Unity logic function
    void Awake() {
        SimulationSettings settings = GameObject.FindObjectOfType<SimulationSettings>();
        if (settings != null) {
            initialFood = settings.initialFood;
            foodPerAnt = settings.foodPerAnt;
            maxAnts = settings.maxAnts;
            antSpawnFrequency = settings.antSpawnFrequency;
        }

        ants = new List<Ant>();
        food = initialFood;
        location = gameObject.GetComponent<HexTile>();
    }
}
