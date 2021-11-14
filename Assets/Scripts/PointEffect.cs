using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointEffect : MonoBehaviour
{
    void Start() {
        Destroy(gameObject, 1.0f);
    }

    public void SetText(string text) {
        Text textPoint = transform.Find("TextPoint").GetComponent<Text>();
        textPoint.text = text;
    }

}
