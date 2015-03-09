using UnityEngine;
using System.Collections;

public enum VisualisationMode {
    none,
    footfall,
    height,
    pheremone,
    water
};

public class VisualisationController : MonoBehaviour {
    public VisualisationMode mode;

    void Awake() {
        mode = VisualisationMode.none;
    }

    void Update() {
        if (Input.GetButtonDown("VisualisationFootfall")) { 
            mode = (mode == VisualisationMode.footfall ? VisualisationMode.none : VisualisationMode.footfall);
        } else if (Input.GetButtonDown("VisualisationHeight")) {
            mode = (mode == VisualisationMode.height ? VisualisationMode.none : VisualisationMode.height);
        } else if (Input.GetButtonDown("VisualisationPheremone")) {
            mode = (mode == VisualisationMode.pheremone ? VisualisationMode.none : VisualisationMode.pheremone); 
        } else if (Input.GetButtonDown("VisualisationWater")) {
            mode = (mode == VisualisationMode.water ? VisualisationMode.none : VisualisationMode.water);
        }
    }
}
