using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AntMode {
    foraging, toFood, toNest
}

public class Ant : MonoBehaviour {
    private HexTile currentLocation;
    private List<HexTile> previousLocations;
    private AntMode mode;

    public void tick() {
        switch (mode) {
            case AntMode.foraging:
                if (followPheromone(currentLocation.getPheromone())) {
                    previousLocations.RemoveRange(0, previousLocations.Count - 1);
                    mode = AntMode.toFood;
                } else {
                    randomStep();
                }
                break;
            case AntMode.toFood:
                float highestPheromone = 0;
                HexTile bestNeighbour = null;
                foreach (HexTile neighbour in currentLocation.neighbours) {
                    if (neighbour != null
                            && neighbour.isPassable()
                            && (currentLocation.isNest() || neighbour != previousLocations[0])
                            && neighbour.getPheromone() > highestPheromone) {
                        highestPheromone = neighbour.getPheromone();
                        bestNeighbour = neighbour;
                    }
                }
                if (bestNeighbour == null) {
                    mode = AntMode.foraging;
                    randomStep();
                } else {
                    previousLocations[0] = currentLocation;
                    currentLocation = bestNeighbour;
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
            if (currentLocation.neighbours[rnd] != null
                    && currentLocation.neighbours[rnd].isPassable()
                    && currentLocation.neighbours[rnd] != previousLocations[previousLocations.Count - 1]) {
                previousLocations.Add(currentLocation);
                currentLocation = currentLocation.neighbours[rnd];
                return;
            }
        }
    }
}
