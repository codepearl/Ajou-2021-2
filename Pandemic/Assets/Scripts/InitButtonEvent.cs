using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitButtonEvent : MonoBehaviour
{
    public GameObject gm;
    public void Init()
    {
        gm = GameObject.Find("Canvas");
        gm.GetComponent<InputManager>().Init();
    }
}
