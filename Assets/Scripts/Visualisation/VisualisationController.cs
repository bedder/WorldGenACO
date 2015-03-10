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
    public HexTile[] tiles;

    // Bounds
    public float maxHeight = 10f;
    public float maxPheremone = 10f;
    public float maxFootfall = 10f;

    // Visualisation colours
    private Color[] pheremoneColour = { new Color(1f, 1f, 1f, 1f), new Color(1f, 0f, 1f, 1f) };
    private Color[] waterColour = { new Color(1f, 0f, 1f, 1f), new Color(0f, 0f, 1f, 1f) };
    private Color[] heightColour = { new Color(1f, 1f, 0f, 1f), new Color(1f, 0f, 0f, 1f) };
    private Color[] footfall = { new Color(0f, 0f, 0f, 1f), new Color(1f, 1f, 1f, 1f) };

    // Internal logic functions
    private Color interpolateColour(Color[] colours, float amount) {
        return Color.Lerp(colours[0], colours[1], amount);
    }
    private void setColour() {
        foreach (HexTile tile in tiles) {
            if (tile != null) {
                switch (mode) {
                    case VisualisationMode.footfall:
                       Debug.Log("Footfall visualisation not yet implemented");
                       break;
                    case VisualisationMode.height:
                        tile.setVisualisationColor(interpolateColour(heightColour, tile.height / maxHeight));
                        break;
                    case VisualisationMode.none:
                        tile.setVisualisationEnabled(false);
                        break;
                    case VisualisationMode.pheremone:
                        tile.setVisualisationColor(interpolateColour(pheremoneColour, tile.getPheromone() / maxPheremone));
                        break;
                    case VisualisationMode.water:
                        tile.setVisualisationColor(interpolateColour(waterColour, tile.moisture));
                        break;
                }
            }
        }
    }

    // Unity logic functions
    void Awake() {
        mode = VisualisationMode.none;
    }
    void Start() {
        tiles = GameObject.FindObjectsOfType<HexTile>();
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
        setColour();
    }
}
