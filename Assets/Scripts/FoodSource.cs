using UnityEngine;
using System.Collections;

public class FoodSource : MonoBehaviour {
    private float foodLevel = 200f;
    private float maxFood = 250f;
    private float restockRatePerSecond = 1.1f;
    private float nextRestock = 0f;

    private void restock() {
        if (foodLevel < 1) {
            foodLevel = 1;
        }

        foodLevel *= restockRatePerSecond;
        nextRestock = Time.realtimeSinceStartup + 1f;
    }

    private float getFood(float requestedFood) {
        if (requestedFood < foodLevel) {
            foodLevel -= requestedFood;
            return requestedFood;
        } else {
            foodLevel = 0;
            return requestedFood;
        }
    }

    void update() {
        if (Time.realtimeSinceStartup > nextRestock)
            restock();
    }
}
