using UnityEngine;
using System.Collections;

public enum HexType {
    TropicalForest0 = 0,
    TropicalForest1 = 1,
    Grassland = 2,
    SubtropicalDesert = 3,
    TemperateForest0 = 4,
    TemperateForest1 = 5,
    TemperateDesert = 6,
    Taiga = 7,
    Shrubland = 8,
    Snow = 9,
    Tundra = 10,
    Bare = 11,
    Scorched = 12
}

public class HexTile : MonoBehaviour {
    // Tile navigation
    public HexTile[] neighbours;
    private bool passable;

    // Tile qualities
    public HexType type;
    public float moisture;
    public float height;
    private FoodSource foodSource;
    private Nest nest;
    private float pheromone;

    // Tile type definitions
    public Material[] materials;
    public float[] tileHeightBounds = { 0, 0.75f, 1.50f, 2.25f };
    public float[] tileMoistureBounds = { 0.2f, 0.4f, 0.6f, 0.8f };

    // Visualisation parameters
    public MeshRenderer visualisationRenderer;

    // Trivial getters
    public bool isPassable() {
        return passable;
    }
    public bool isNest() {
        return (nest != null);
    }
    public bool isFoodSource() {
        return (foodSource != null);
    }
    public float getPheromone() {
        return pheromone;
    }

    // Setters/modifiers
    public void setType(HexTile[] newNeighbours, bool newPassable, float newMoisture) {
        neighbours = newNeighbours;
        passable = newPassable;
        moisture = newMoisture;

        type = getTileType(gameObject.transform.position.y);
        setMaterial(materials[(int)type]);
    }
    public void addPheromone(float newPheromone) {
        pheromone += newPheromone;
    }
    public void decayPheromone(float decayFactor) {
        pheromone *= decayFactor;
    }
    public void diffusePheromone(float diffuseFactor) {
        float diffusedPheromone = pheromone * diffuseFactor;
        pheromone -= diffusedPheromone;
        uint nNeighbours = 0;
        foreach (HexTile neighbour in neighbours)
            if (neighbour != null)
                nNeighbours++;
        foreach (HexTile neighbour in neighbours)
            if (neighbour != null)
                neighbour.addPheromone(diffusedPheromone / nNeighbours);
    }

    // Internal functions
    HexType getTileType(float height) {
        int heightCategory = getTileCategory(height, tileHeightBounds);
        int moistureCategory = getTileCategory(moisture, tileMoistureBounds);
        switch (heightCategory) {
            case 0:
                switch (moistureCategory) {
                    case 0:
                        return HexType.SubtropicalDesert;
                    case 1:
                        return HexType.Grassland;
                    case 2:
                    case 3:
                        return HexType.TropicalForest1;
                    case 4:
                    case 5:
                    default:
                        return HexType.TropicalForest0;
                }
            case 1:
                switch (moistureCategory) {
                    case 0:
                        return HexType.TemperateDesert;
                    case 1:
                    case 2:
                        return HexType.Grassland;
                    case 3:
                    case 4:
                        return HexType.TropicalForest1;
                    case 5:
                    default:
                        return HexType.TropicalForest0;
                }
            case 2:
                switch (moistureCategory) {
                    case 0:
                    case 1:
                        return HexType.TemperateDesert;
                    case 2:
                    case 3:
                        return HexType.Shrubland;
                    case 4:
                    case 5:
                    default:
                        return HexType.Taiga;
                }
            case 3:
                switch (moistureCategory) {
                    case 0:
                        return HexType.Scorched;
                    case 1:
                        return HexType.Bare;
                    case 2:
                        return HexType.Tundra;
                    case 3:
                    case 4:
                    case 5:
                    default:
                        return HexType.Snow;
                }
            default:
                Destroy(gameObject);
                return HexType.Bare;
        }
    }
    int getTileCategory(float value, float[] bounds) {
        for (int i = 0 ; i < bounds.Length ; i++) {
            if (value <= bounds[i])
                return i - 1;
        }
        return bounds.Length - 1;
    }
    void setMaterial(Material newMaterial) {
        transform.Find("Base").renderer.material = newMaterial;
    }

    public void setVisualisationEnabled(bool enabled) {
        if (visualisationRenderer != null)
            visualisationRenderer.enabled = enabled;
    }
    public void setVisualisationColor(Color newColour) {
        setVisualisationEnabled(true);
        if (visualisationRenderer != null)
            visualisationRenderer.material.color = newColour;
    }
    public void findFoodSource() {
        foodSource = GetComponent<FoodSource>();
    }
    public void findNest() {
        nest = GetComponent<Nest>();
    }

    // Unity logic functions
    void awake() {
        pheromone = 0f;
        nest = gameObject.GetComponent<Nest>();
        foodSource = gameObject.GetComponent<FoodSource>();
    }
}
