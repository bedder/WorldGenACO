﻿using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {
    // Private variables
    public Camera[] cameras;
    int currentCamera = 0;

    // Internal functions
    void setCamera(int newCamera) {
        if (newCamera < cameras.Length) {
            for (int i = 0 ; i < cameras.Length ; i++)
                cameras[i].gameObject.SetActive(i == newCamera);
            currentCamera = newCamera;
        }
    }

    // Unity logical functions
    void Start() {
        setCamera(0);
    }

    void Update() {
        if (Input.GetButtonDown("Camera1") && cameras.Length >= 1)
            setCamera(0);
        if (Input.GetButtonDown("Camera2") && cameras.Length >= 2)
            setCamera(1);
        if (Input.GetButtonDown("Camera3") && cameras.Length >= 3)
            setCamera(2);
        if (Input.GetButtonDown("CameraToggle")) {
            currentCamera = (currentCamera + 1) % cameras.Length;
            setCamera(currentCamera);
        }
    }
}
