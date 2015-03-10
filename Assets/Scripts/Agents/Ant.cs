using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AntMode {
    foraging,
    toFood,
    toNest
}

public class Ant : MonoBehaviour {
    // Internal logic variables 
    private HexTile location;
    private List<HexTile> previousLocations;
    private AntMode mode;

    // Setters/modifiers
    void setLocation(HexTile newLocation) {
        location = newLocation;
    }

    // Public logic functions
    public void tick() {
        switch (mode) {
            case AntMode.foraging:
                if (followPheromone(location.getPheromone())) {
                    previousLocations.RemoveRange(0, previousLocations.Count - 1);
                    mode = AntMode.toFood;
                } else {
                    randomStep();
                }
                break;
            case AntMode.toFood:
                float highestPheromone = 0;
                HexTile bestNeighbour = null;
                foreach (HexTile neighbour in location.neighbours) {
                    if (neighbour != null
                            && neighbour.isPassable()
                            && (location.isNest() || neighbour != previousLocations[0])
                            && neighbour.getPheromone() > highestPheromone) {
                        highestPheromone = neighbour.getPheromone();
                        bestNeighbour = neighbour;
                    }
                }
                if (bestNeighbour == null) {
                    mode = AntMode.foraging;
                    randomStep();
                } else {
                    previousLocations[0] = location;
                    location = bestNeighbour;
                }
                break;
            case AntMode.toNest:
                // TODO
                break;
        }
    }
    public bool followPheromone(float pheromone) {
        // TODO
        return true;
    }
    public void randomStep() {
        while (true) {
            int rnd = Random.Range(0, 6);
            if (location.neighbours[rnd] != null
                    && location.neighbours[rnd].isPassable()
                    && location.neighbours[rnd] != previousLocations[previousLocations.Count - 1]) {
                previousLocations.Add(location);
                location = location.neighbours[rnd];
                return;
            }
        }
    }

    // Unity logic functions
    void awake() {
        location = null;
        previousLocations = new List<HexTile>();
        mode = AntMode.foraging;
    }
}
