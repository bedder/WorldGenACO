using UnityEngine;
using System.Collections;

public class CirclingCamera : MonoBehaviour {
    // CirclingCamera Dynamics Variables
    public float rotationRate = 5f;

    void Update() {
        gameObject.transform.Rotate(Vector3.up, rotationRate * Time.deltaTime);
    }
}
