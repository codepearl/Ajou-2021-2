using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeButtonEvent : MonoBehaviour
{
    public GameObject sm;
    // Start is called before the first frame update
    public void change()
    {
        sm = GameObject.Find("Window_Graph");
        sm.GetComponent<Window_Graph>().changeData();

    }
}
