using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButtonEvent : MonoBehaviour
{
    public GameObject sm;
        // Start is called before the first frame update
    public void Next()
    {
        sm = GameObject.Find("Main Camera");
        sm.GetComponent<SceneManager>().NextScene();
        
    }
}
