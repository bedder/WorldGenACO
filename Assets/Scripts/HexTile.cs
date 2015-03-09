using UnityEngine;
using System.Collections;

public enum HexType {
    TropicalForest0,
    TropicalForest1,
    Grassland,
    SubtropicalDesert,
    TemperateForest0,
    TemperateForest1,
    TemperateDesert,
    Taiga,
    Shrubland,
    Snow,
    Tundra,
    Bare,
    Scorched
}

public class HexTile : MonoBehaviour {
    // Tile navigation
    public HexTile[] neighbours;
    private bool passable;

    // Tile qualities
    public HexType type;
    private FoodSource foodSource;
    private Nest nest;
    private float pheromone;

    // Tile type definitions
    private static float[] tileHeightBounds = { 0, 10, 20, 30 };
    private static float[] tileMoistureBounds = { 10, 20, 30, 40, 50 };

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
    public void setType(HexTile[] newNeighbours, bool newPassable, float moisture) {
        neighbours = newNeighbours;
        passable = newPassable;
        type = getTileType(gameObject.transform.position.y, moisture);
        // TODO
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
    HexType getTileType(float height, float moisture) {
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
            if (value < bounds[i])
                return i - 1;
        }
        return bounds.Length;
    }

    // Unity logic functions
    void awake() {
        pheromone = 0f;
        nest = gameObject.GetComponent<Nest>();
        foodSource = gameObject.GetComponent<FoodSource>();
    }
}
