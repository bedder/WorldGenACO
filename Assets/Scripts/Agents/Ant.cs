using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AntMode {
    foraging,
    following,
    toNest
}

public class Ant : MonoBehaviour {
    // Behaviour variables
    public float burden = 10f;
    private float carrying = 0f;
    public float pheremoneRelease = 100f;
    public float pheremoneAttraction = 1f;
    public float pheremoneFollowing = 2f;
    public float terrainFollowing = 1f;

    // Internal logic variables 
    private HexTile location;
    private List<HexTile> previousLocations;
    private bool currentlyWandering = false;
    private int currentWanderDirection;
    private AntMode mode;

    // Setters/modifiers
    public void setLocation(HexTile newLocation) {
        location = newLocation;
    }

    // Public logic functions
    public void tick() {
        switch (mode) {
            case AntMode.foraging:
            case AntMode.following:
                if (carrying == 0 && location.isFoodSource()) {
                    carrying = location.getFoodSource().takeFood(burden);
                    location.addPheromone(10f * pheremoneRelease * (carrying / burden));
                    currentlyWandering = false;
                    mode = AntMode.toNest;
                } else {
                    rememberLocation();
                    HexTile food = adjacentFoodSource();
                    if (food != null) {
                        move(food);
                    } else if (shouldFollowPheremone()) {
                        mode = AntMode.following;
                        move(followPheremone());
                    } else {
                        mode = AntMode.foraging;
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
                } else {
                    move(retraceSteps());
                }
                location.addPheromone(pheremoneRelease);
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
        float t = pheremone * pheremoneAttraction - pheremoneRelease;
        float s = 1f / (1f + Mathf.Pow(2.71f, t));
        return (Random.value < s);
    }
    private HexTile followPheremone() {
        int[] directions = { (currentWanderDirection + 5) % 6,
                             currentWanderDirection,
                             (currentWanderDirection + 1) % 6 };
        float[] motivation = { 0, 0, 0, 0, 0, 0};
        float totalMotivation = 0;
        foreach (int dir in directions) {
            if (dir != currentWanderDirection && location.neighbours[dir] != null) {
                motivation[dir] += Mathf.Pow(location.neighbours[dir].getPheromone(), pheremoneFollowing);
                motivation[dir] += Mathf.Pow(location.height - location.neighbours[dir].height, terrainFollowing);
                totalMotivation += motivation[dir];
            }
        }
        if (totalMotivation < 0.1f)
            return moveRandomly();
        float randomNumber = Random.value * totalMotivation;
        float soFar = 0;
        foreach (int dir in directions) {
            soFar += motivation[dir];
            if (soFar > randomNumber) {
                currentWanderDirection = dir;
                return location.neighbours[dir];
            }
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
            currentWanderDirection, currentWanderDirection, currentWanderDirection,
            currentWanderDirection, currentWanderDirection, currentWanderDirection,
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
        Debug.Log("An ant has gotten lost!");
        previousLocations.Clear();
        carrying = 0f;
        return moveRandomly();
    }
    private void rememberLocation() {
        int lastIndex = previousLocations.FindIndex(tile => (tile == location));
        if (lastIndex != -1 && lastIndex != previousLocations.Count - 1)
            previousLocations.RemoveRange(lastIndex + 1, previousLocations.Count - lastIndex - 2);
        previousLocations.Add(location);
    }
    private void move(HexTile tile) {
        if (location == null) {
            Debug.Log("Trying to move to a null location!");
            return;
        }
        location = tile;
        transform.position = location.transform.position;
    }

    // Unity logic functions
    void Awake() {
        SimulationSettings settings = GameObject.FindObjectOfType<SimulationSettings>();
        if (settings != null) {
            pheremoneRelease = settings.pheremoneRelease;
            pheremoneAttraction = settings.pheremoneAttraction;
            pheremoneFollowing = settings.pheremoneFollowing;
            terrainFollowing = settings.terrainFollowing;
        }
        previousLocations = new List<HexTile>();
        mode = AntMode.foraging;
    }
}
