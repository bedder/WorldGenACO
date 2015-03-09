using UnityEngine;
using System.Collections;

public class VisualisationInstance : MonoBehaviour {
    // Behaviour variables
    private VisualisationController controller;
    private HexTile tile;
    public MeshRenderer visualisationRenderer;
    private VisualisationMode currentMode;

    public float maxHeight = 10f;
    public float maxPheremone = 10f;
    public float maxFootfall = 10f;

    // Materials
    public Color[] pheremoneColour;
    public Color[] waterColour;
    public Color[] heightColour;
    public Color[] footfall;

    Color interpolateColour(Color[] colours, float amount) {
        if (amount < 0 || colours.Length == 1)
            return colours[0];
        if (amount > 1)
            return colours[colours.Length - 1];
        float rangeSize = 1 / colours.Length;
        int lowerBucket = 0;
        while (amount > rangeSize) {
            lowerBucket++;
            amount -= rangeSize;
        }
        return Color.Lerp(colours[lowerBucket], colours[lowerBucket + 1], amount / rangeSize);
    }

    void Start() {
        tile = transform.parent.GetComponent<HexTile>();
        controller = GameObject.FindObjectOfType<VisualisationController>();
        currentMode = controller.mode;
    }

    void Update() {
        if (currentMode != controller.mode) {
            currentMode = controller.mode;
            if (currentMode == VisualisationMode.none) {
                visualisationRenderer.enabled = false;
                return;
            }
            visualisationRenderer.enabled = true;
            switch(currentMode) {
                case VisualisationMode.footfall:
                    Debug.Log("Visualistaion for footfall is yet to be implemented.");
                    return;
                case VisualisationMode.height:
                    visualisationRenderer.material.color = interpolateColour(heightColour, tile.transform.position.y / maxHeight);
                    return;
                case VisualisationMode.pheremone:
                    visualisationRenderer.material.color = interpolateColour(pheremoneColour, tile.getPheromone() / maxPheremone);
                    return;
                case VisualisationMode.water:
                    visualisationRenderer.material.color = interpolateColour(waterColour, tile.moisture);
                    return;
            }
        }
    }
}
