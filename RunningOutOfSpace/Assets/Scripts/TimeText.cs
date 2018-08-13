using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeText : MonoBehaviour {
    public Text m_TimeText;
    public GameObject dang;
    // Use this for initialization
    void Start () {
        m_TimeText.text = "hi";
    }
	
	// Update is called once per frame
	void Update () {
        string Jay = "Days Remaining Until Flood" + " " + dang.GetComponent<GameManager>().Timeleft().ToString();
        m_TimeText.text = Jay;
    }
}
