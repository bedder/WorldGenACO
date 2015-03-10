using UnityEngine;
using System.Collections;

public class AntController : MonoBehaviour {
    public float timeBetweenTicks = 0.5f;
    private float nextTick;
    private bool isPaused;

    private void tick() {
        Nest[] nests = GameObject.FindObjectsOfType<Nest>();
        foreach (Nest nest in nests)
            nest.tick();

        Ant[] ants = GameObject.FindObjectsOfType<Ant>();
        foreach (Ant ant in ants)
            ant.tick();
        nextTick = Time.realtimeSinceStartup + timeBetweenTicks;

        HexTile[] tiles = GameObject.FindObjectsOfType<HexTile>();
        foreach (HexTile tile in tiles) {
            tile.diffusePheromone(0.25f);
            tile.decayPheromone(0.9f);
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
