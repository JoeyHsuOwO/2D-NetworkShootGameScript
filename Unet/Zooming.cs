using UnityEngine;
using System.Collections;

//物件隨螢幕大小變形
public class Zooming : MonoBehaviour {

    public float baseWidth = 1024;
    public float baseHeight = 768;
    public float baseOrthographicSize = 5;

    void Awake()
    {
        float newOrthographicSize = (float)Screen.height / (float)Screen.width * this.baseWidth / this.baseHeight * this.baseOrthographicSize;
        GetComponent<Camera>().orthographicSize = Mathf.Max(newOrthographicSize, this.baseOrthographicSize);
    }
}
