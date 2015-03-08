using UnityEngine;
using System.Collections;

public enum HexType {
    TropicalForest0, TropicalForest1, Grassland, SubtropicalDesert,
    TemperateForest0, TemperateForest1, TemperateDesert,
    Taiga, Shrubland,
    Snow, Tundra, Bare, Scorched
}

public class HexTile : MonoBehaviour {
    // Tile navigation
    public Vector3 location;
    public HexTile[] neighbours;
    private bool passable;

    // Tile qualities
    public HexType type;
    private FoodSource foodSource;
    private Nest nest;
    private float pheromone = 0;

    public bool isPassable()   { return passable;             }
    public bool isNest()       { return (nest != null);       }
    public bool isFoodSource() { return (foodSource != null); }

    public float getPheromone()                   { return pheromone;          }
    public void addPheromone(float newPheromone)  { pheromone += newPheromone; }
    public void decayPheromone(float decayFactor) { pheromone *= decayFactor;  }
    public void diffusePheromone(float diffuseFactor) {
        float diffusedPheromone = pheromone * diffuseFactor;
        pheromone -= diffusedPheromone;

        uint nNeighbours = 0;
        foreach (HexTile neighbour in neighbours) {
            if (neighbour != null) {
                nNeighbours++;
            }
        }
        foreach (HexTile neighbour in neighbours) {
            if (neighbour != null)
                neighbour.addPheromone(diffusedPheromone / nNeighbours);
        }

    }
}
