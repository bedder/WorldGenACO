using UnityEngine;
using System.Collections;

public class FoodSource : MonoBehaviour {
    // Food source dynamics variables
    public float restockRatePerSecond = 1.1f;
    public float initFoodLevel = 200f;
    public float maxFood = 250f;

    // Internal variables
    private float foodLevel;
    private float nextRestock;

    // Internal functions
    private void restock() {
        if (foodLevel < 1)
            foodLevel = 1;
        foodLevel *= restockRatePerSecond;
        nextRestock = Time.realtimeSinceStartup + 1f;
    }
    public float takeFood(float requestedFood) {
        if (requestedFood < foodLevel) {
            foodLevel -= requestedFood;
            return requestedFood;
        } else {
            float remainingFood = foodLevel;
            foodLevel = 0;
            return remainingFood;
        }
    }

    // Unity logic functions
    void awake() {
        foodLevel = initFoodLevel;
        nextRestock = 0f;
    }
    void update() {
        if (Time.realtimeSinceStartup > nextRestock)
            restock();
    }
}
