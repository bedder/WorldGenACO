using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Nest : MonoBehaviour {
    // Nest dynamics variables
    public float foodPerAntPerSecond = 0.1f;
    public float allowedExcessFood = 100f;
    public float timeBetweenAntSpawns = 5f;
    public int supportedAnts = 10;

    // Prefab for spawning new ants
    public GameObject antPrefab;

    // Internal variables
    private float food = 100f;
    private List<Ant> ants;
    private float nextAllowedAnt = 0f;

    // Trivial Getters

    // Setters/modifiers
    public void addFood(float newFood) {
        food += newFood;
    }
    private void spawnAnt() {
        Ant newAnt = Instantiate(antPrefab) as Ant;
        newAnt.setLocation(GetComponent<HexTile>());
        ants.Add(newAnt);
        nextAllowedAnt = Time.realtimeSinceStartup + timeBetweenAntSpawns;
    }

    // Unity logic function
    void awake() {
        ants = new List<Ant>();
    }
    void update() {
        // Update food levels
        food -= (Time.deltaTime * foodPerAntPerSecond * ants.Count);
        // Spawn ants if required
        if (food > allowedExcessFood && Time.realtimeSinceStartup > nextAllowedAnt && ants.Count < supportedAnts)
            spawnAnt();
        // Kill ants if required
        if (food < 0 && ants.Count > 0)
            ants.RemoveAt(0);
    }
}
