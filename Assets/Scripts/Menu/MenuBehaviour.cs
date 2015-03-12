using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuBehaviour : MonoBehaviour {
    public SimulationSettings settingsPrefab;
    public void startLevel() {
        Application.LoadLevel(1);
    }

    public void startCustomLevel() {
        SimulationSettings settings = Instantiate(settingsPrefab) as SimulationSettings;
        if (settings != null) {
            settings.nNests = (int)sliderValue("NumberOfAntNests");
            settings.nFoodSources = (int)sliderValue("NumberOfFoodSources");
            settings.perlinFactor = sliderValue("WorldSmoothness");
            settings.heightFactor = sliderValue("WorldHeightScale");
            settings.restockFrequency = (int)sliderValue("RestockFrequency");
            settings.restockFactor = sliderValue("RestockFactor");
            settings.initialFood = sliderValue("InitialFood");
            settings.foodPerAnt = sliderValue("FoodConsumption");
            settings.maxAnts = (int)sliderValue("MaxAnts");
            settings.antSpawnFrequency = (int)sliderValue("SpawnFrequency");
            settings.pheremoneRelease = sliderValue("PheremoneReleaseQuantity");
            settings.pheremoneAttraction = sliderValue("PheremoneAttractionCapture");
            settings.pheremoneFollowing = sliderValue("AntPheremoneFollowing");
            settings.terrainFollowing = sliderValue("AntTerrainFollowing");
            //settings.pheremoneDecay = sliderValue("PheremoneDecay");
            //settings.pheremoneDiffusion = sliderValue("PheremoneDiffusion");
        }
        Application.LoadLevel(1);
    }

    private float sliderValue(string name) {
        return GameObject.Find(name).GetComponentInChildren<Slider>().value;
    }

    public void openIggiWebsite() {
        Application.OpenURL("http://iggi.org.uk");
    }

    public void openYccsaWebsite() {
        Application.OpenURL("https://www.york.ac.uk/yccsa/");
    }

    public void quit() {
        Application.Quit();
    }
}
