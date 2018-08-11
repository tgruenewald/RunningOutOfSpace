using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class My_HayText : MonoBehaviour {
    public Text m_HayText;
    public GameObject dang;
	// Use this for initialization
	void Start () {
        m_HayText.text = "hi";
    }
	
	// Update is called once per frame
	void Update () {
        string Jay = "Hay"+" "+dang.GetComponent<GameManager>().NumHay().ToString()+"/"+dang.GetComponent<GameManager>().Maxhay().ToString();
        m_HayText.text = Jay;
	}
}
