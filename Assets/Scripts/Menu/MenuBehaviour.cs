using UnityEngine;
using System.Collections;

public class MenuBehaviour : MonoBehaviour {
    public void startLevel() {
        Application.LoadLevel(1);
    }

    public void startCustomLevel() {
        Debug.Log("This is not yet implemented.");
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
