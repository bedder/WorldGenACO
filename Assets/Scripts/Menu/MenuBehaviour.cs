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
        System.Diagnostics.Process.Start("http://iggi.org.uk");
    }

    public void openYccsaWebsite() {
        System.Diagnostics.Process.Start("https://www.york.ac.uk/yccsa/");
    }

    public void quit() {
        Application.Quit();
    }
}
