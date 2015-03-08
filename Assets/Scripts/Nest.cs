using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Nest : MonoBehaviour {
    public float foodPerAntPerSecond = 0.1f;
    public float allowedExcessFood = 100f;
    public float timeBetweenAntSpawns = 5f;

    public GameObject antPrefab;

    private float food = 100f;
    private int supportedAnts = 1;
    private List<Ant> ants;
    private float nextAllowedAnt = 0f;

    public void addFood(float newFood) { food += newFood; }
    private void spawnAnt() {
        Ant newAnt = Instantiate(antPrefab) as Ant;
        ants.Add(newAnt);
        nextAllowedAnt = Time.realtimeSinceStartup + timeBetweenAntSpawns;
    }

    void update() {
        // Update food levels
        food -= (Time.deltaTime * foodPerAntPerSecond * ants.Count);

        // Spawn ants if required
        if (food > allowedExcessFood && Time.realtimeSinceStartup > nextAllowedAnt) {
            spawnAnt();
        }

        // Kill ants if required
        if (food < 0 && ants.Count > 0) {
            ants.RemoveAt(0);
        }
    }
}
