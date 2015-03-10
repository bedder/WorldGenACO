using UnityEngine;
using System.Collections;

public class CirclingCamera : MonoBehaviour {
    // CirclingCamera Dynamics Variables
    public float rotationRate = 5f;

    // Unity logical functions
    void Update() {
        gameObject.transform.Rotate(Vector3.up, rotationRate * Time.deltaTime);
    }
}
