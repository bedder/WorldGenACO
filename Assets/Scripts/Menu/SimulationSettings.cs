using UnityEngine;
using System.Collections;

public class SimulationSettings : MonoBehaviour {
    // Map params
    public int nNests = 5;
    public int nFoodSources = 20;
    public float heightFactor = 10f;
    public float perlinFactor = 0.03f;

    // Food source
    public int restockFrequency = 100;
    public float restockFactor = 1.5f;

    // Nest
    public float initialFood = 300f;
    public float foodPerAnt = 0.1f;
    public int maxAnts = 1;
    public int antSpawnFrequency = 5;

    // Ant
    public float pheremoneRelease = 100f;
    public float pheremoneAttraction = 1f;
    public float pheremoneFollowing = 2f;
    public float terrainFollowing = 1f;

    // Pheremone
    public float pheremoneDiffusion = 0.1f;
    public float pheremoneDecay = 0.95f;

    // Unity logic functions
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
