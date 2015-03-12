using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderUpdater : MonoBehaviour {
    public string format = "0";
    public void setValue(float value) {
        GetComponent<Text>().text = value.ToString(format);
    }
}
