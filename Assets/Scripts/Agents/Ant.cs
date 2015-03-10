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
    public List<HexTile> previousLocations;
    private bool currentlyWandering = false;
    private int currentWanderDirection;
    public AntMode mode;

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
                    currentlyWandering = false;
                    mode = AntMode.toNest;
                } else {
                    previousLocations.Add(location);
                    if (adjacentFoodSource() != null) {
                        move(adjacentFoodSource());
                    } else if (shouldFollowPheremone()) {
                        move(followPheremone());
                    } else {
                        move(moveRandomly());
                    }
                }
                break;
            case AntMode.toNest:
                if (location.isNest()) {
                    location.getNest().addFood(carrying);
                    carrying = 0;
                    mode = AntMode.foraging;
                    return;
                } else if (adjacentNest() != null) {
                    move(adjacentNest());
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
    public HexTile adjacentFoodSource() {
        foreach (HexTile neighbour in location.neighbours)
            if (neighbour != null && neighbour.isFoodSource())
                return neighbour;
        return null;
    }
    public HexTile adjacentNest() {
        foreach (HexTile neighbour in location.neighbours)
            if (neighbour != null && neighbour.isNest())
                return neighbour;
        return null;
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
        if (!currentlyWandering) {
            currentWanderDirection = Random.Range(0, 5);
            currentlyWandering = true;
        }
        HexTile directed = directedRandomMovement();
        if (directed != null)
            return directed;
        while (true) {
            int i = Random.Range(0, 6);
            if (location.neighbours[i] != null && (previousLocations.Count == 0 || location.neighbours[i] != previousLocations[previousLocations.Count - 1])) {
                currentWanderDirection = i;
                return location.neighbours[i];
            }
        }
    }
    private HexTile directedRandomMovement() {
        int[] dirs = {
            ((currentWanderDirection + 5) % 6),
            currentWanderDirection,
            ((currentWanderDirection + 1) % 6) };
        List<int> possibleDirections = new List<int>();
        foreach (int dir in dirs)
            if (location.neighbours[dir] != null)
                possibleDirections.Add(dir);
        if (possibleDirections.Count == 0)
            return null;
        currentWanderDirection = possibleDirections[Random.Range(0, possibleDirections.Count - 1)];
        return location.neighbours[currentWanderDirection];
    }
    private HexTile retraceSteps() {
        if (previousLocations.Count == 0)
            return moveRandomly();
        foreach (HexTile neighbour in location.neighbours) {
            if (neighbour == previousLocations[previousLocations.Count - 1]) {
                previousLocations.RemoveAt(previousLocations.Count - 1);
                return neighbour;
            }
        }
        previousLocations.Clear();
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
