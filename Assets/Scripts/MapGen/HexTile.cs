using UnityEngine;
using System.Collections;


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
    private float pheromone = 0f;
    private int visits = 0;

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
    public Nest getNest() {
        return nest;
    }
    public bool isFoodSource() {
        return (foodSource != null);
    }
    public FoodSource getFoodSource() {
        return foodSource;
    }
    public float getPheromone() {
        return pheromone;
    }
    public int getVisits() {
        return visits;
    }

    // Setters/modifiers
    public void addVisit() {
        visits++;
    }
    public void setType(HexTile[] newNeighbours, bool newPassable, float newMoisture) {
        neighbours = newNeighbours;
        passable = newPassable;
        moisture = newMoisture;
        type = Helper.getTileType(
            Helper.getCategory(height, tileHeightBounds),
            Helper.getCategory(moisture, tileMoistureBounds));
        if (type == HexType.ERROR)
            Destroy(gameObject);
        else 
            setMaterial(materials[(int)type]);
    }

    // Pheremone functions
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
    // Visualisation
    public void setVisualisationEnabled(bool enabled) {
        if (visualisationRenderer != null)
            visualisationRenderer.enabled = enabled;
    }
    public void setVisualisationColor(Color newColour) {
        setVisualisationEnabled(true);
        if (visualisationRenderer != null)
            visualisationRenderer.material.color = newColour;
    }
    // Links
    public void findFoodSource() {
        foodSource = GetComponentInChildren<FoodSource>();
    }
    public void findNest() {
        nest = GetComponentInChildren<Nest>();
    }
    // Internal functions
    private void setMaterial(Material newMaterial) {
        transform.Find("Base").renderer.material = newMaterial;
    }
}
