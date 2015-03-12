using UnityEngine;
using System.Collections;

public class AntController : MonoBehaviour {
    public float timeBetweenTicks = 0f;
    public float diffusionFactor = 0.1f;
    public float decayFactor = 0.95f;
    private float nextTick;
    private bool isPaused;

    private void tick() {
        Nest[] nests = GameObject.FindObjectsOfType<Nest>();
        foreach (Nest nest in nests)
            nest.tick();

        FoodSource[] foodSources = GameObject.FindObjectsOfType<FoodSource>();
        foreach (FoodSource foodSource in foodSources)
            foodSource.tick();

        Ant[] ants = GameObject.FindObjectsOfType<Ant>();
        foreach (Ant ant in ants)
            ant.tick();
        nextTick = Time.realtimeSinceStartup + timeBetweenTicks;

        HexTile[] tiles = GameObject.FindObjectsOfType<HexTile>();
        foreach (HexTile tile in tiles) {
            tile.diffusePheromone(diffusionFactor);
            tile.decayPheromone(decayFactor);
        }
    }

    void Awake() {
        isPaused = false;
        nextTick = 0f;
    }

    void Update() {
        if (Input.GetButtonDown("Pause")) {
            isPaused =! isPaused;
            if (isPaused)
                nextTick = Time.realtimeSinceStartup + timeBetweenTicks;
        }

        if ((!isPaused && nextTick < Time.realtimeSinceStartup) ||
            (isPaused && Input.GetButtonDown("ManualTick"))) {
            tick();
        }
    }
}
