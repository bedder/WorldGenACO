﻿using UnityEngine;
using System.Collections;

public class FoodSource : MonoBehaviour {
    // Food source dynamics variables
    public int restockFrequency = 100;
    public float restockFactor = 1.5f;
    public float initFoodLevel = 200f;
    public float maxFood = 250f;

    // Internal variables
    private float foodLevel;
    private int nextRestock;
    private HexTile location;

    // Internal functions
    private void restock() {
        if (foodLevel < 1)
            foodLevel = 1;
        foodLevel *= restockFactor;
    }
    public float takeFood(float requestedFood) {
        if (requestedFood < foodLevel) {
            foodLevel -= requestedFood;
            return requestedFood;
        } else {
            float remainingFood = foodLevel;
            foodLevel = 0;
            Destroy(gameObject);
            return remainingFood;
        }
    }
    public void tick() {
        if (location == null)
            location = gameObject.GetComponentInParent<HexTile>();
        nextRestock--;
        if (nextRestock < 1) {
            restock();
            nextRestock = restockFrequency;
        }
    }

    // Unity logic functions
    void Awake() {
        SimulationSettings settings = GameObject.FindObjectOfType<SimulationSettings>();
        if (settings != null) {
            restockFrequency = settings.restockFrequency;
            restockFactor = settings.restockFactor;
        }
        foodLevel = initFoodLevel;
        nextRestock = 0;
    }
}
