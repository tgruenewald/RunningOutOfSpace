using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class m_MyWood : MonoBehaviour {
    public Text m_woodText;
    public GameObject dang;
    // Use this for initialization
    void Start () {
        m_woodText.text = "hi";
    }
	
	// Update is called once per frame
	void Update () {
        string Jay = "Wood" + " " + dang.GetComponent<GameManager>().NumWood().ToString();
        m_woodText.text = Jay;

    }
}
