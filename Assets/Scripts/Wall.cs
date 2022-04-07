using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
    public Text text;
    void Start()
    {
        canvas = this.gameObject.GetComponent<Canvas>();
        text = canvas.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
