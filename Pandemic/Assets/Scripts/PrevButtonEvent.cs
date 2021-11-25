using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevButtonEvent : MonoBehaviour
{
    public GameObject sm;
    // Start is called before the first frame update
    public void Prev()
    {
        sm = GameObject.Find("Main Camera");
        sm.GetComponent<SceneManager>().PrevScene();

    }
}
