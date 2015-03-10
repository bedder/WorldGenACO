using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AntMode {
    foraging,
    toNest
}

public class Ant : MonoBehaviour {
    // Behaviour variables
    public float pheremoneFollowingConstant = 0.1f;
    public float burden = 10f;
    private float carrying = 0f;
    public float pheremoneReleaseValue = 1f;

    // Internal logic variables 
    private HexTile location;
    private List<HexTile> previousLocations;
    private AntMode mode;

    // Setters/modifiers
    public void setLocation(HexTile newLocation) {
        location = newLocation;
    }

    // Public logic functions
    public void tick() {
        switch (mode) {
            case AntMode.foraging:
                if (carrying == 0 && location.isFoodSource()) {
                    carrying = location.getFoodSource().takeFood(burden);
                    mode = AntMode.toNest;
                } else if (shouldFollowPheremone()) {
                    previousLocations.Add(location);
                    move(followPheremone());
                } else {
                    previousLocations.Add(location);
                    move(moveRandomly());
                }
                break;
            case AntMode.toNest:
                if (location.isNest()) {
                    location.getNest().addFood(carrying);
                    carrying = 0;
                    mode = AntMode.foraging;
                    return;
                } else if (shouldFollowPheremone()) {
                    previousLocations.Clear();
                    move(followPheremone());
                } else {
                    move(retraceSteps());
                }
                location.addPheromone(pheremoneReleaseValue);
                break;
        }
        location.addVisit();
    }
    public bool shouldFollowPheremone() {
        float pheremone = location.getPheromone();
        float t = (pheremone + 6f) / pheremoneFollowingConstant;
        float s = 1f / (1f + Mathf.Pow(2.71f, t));
        return (Random.value < s);
    }
    private HexTile followPheremone() {
        float[] pheremoneValues = { 0, 0, 0, 0, 0, 0 };
        float totalPheremone = 0;
        for (int dir = 0 ; dir < 6 ; dir++) {
            // Sum neighbouring pheremone strengths
            pheremoneValues[dir] = (location.neighbours[dir] == null ? 0 : location.neighbours[dir].getPheromone());
            totalPheremone += pheremoneValues[dir];
        }
        if (totalPheremone < 0.1f)
            return moveRandomly();
        float randomNumber = Random.value;
        float soFar = 0;
        for (int dir = 0 ; dir < 6 ; dir++) {
            // Select neighbour with probability proportional to the pheremone strength
            soFar += pheremoneValues[dir];
            if (location.neighbours[dir] != null && soFar > randomNumber)
                return location.neighbours[dir];
        }
        return null;
    }
    private HexTile moveRandomly() {
        if (previousLocations.Count == 0) {
            // Move to any valid adjoining space
            while (true) {
                int i = Random.Range(0, 6);
                if (location.neighbours[i] != null)
                    return location.neighbours[i];
            }
        } else {
            // Move to any valid adjoining space WITHOUT immediately backtracking
            while (true) {
                int i = Random.Range(0, 6);
                if (location.neighbours[i] != null && location.neighbours[i] != previousLocations[previousLocations.Count - 1])
                    return location.neighbours[i];
            }
        }
    }
    private HexTile retraceSteps() {
        if (previousLocations.Count == 0)
            return moveRandomly();
        foreach (HexTile neighbour in location.neighbours) {
            if (neighbour == previousLocations[previousLocations.Count - 1])
                return neighbour;
        }
        return moveRandomly();
    }
    private void move(HexTile tile) {
        location = tile;
        transform.position = location.transform.position;
    }

    // Unity logic functions
    void Awake() {
        previousLocations = new List<HexTile>();
        mode = AntMode.foraging;
    }
}
