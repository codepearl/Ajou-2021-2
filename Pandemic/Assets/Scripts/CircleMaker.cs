using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircleMaker : MonoBehaviour
{
    public GameObject Circle;
    public int targetCircle;
    public int currentCircle;
    public float time;
    public int days;
    public bool working = false;
    List<GameObject> circleList = new List<GameObject>();

    public void init()
    {
        currentCircle = 0;
        //targetCircle = 5000; // 50000000;
        time = 1;
        days = 0;
        working = true;
    }

    public void stop()
    {
        currentCircle = 0;
        targetCircle = 1000;
        time = 1;
        days = 0;
        foreach(GameObject i in circleList)
        {
            Destroy(i);
        }
        circleList.Clear();
        working = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCircle = 0;
        targetCircle = 1000;
        time = 1;
        days = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(working)
        {
            if(time >=1)
            {
                time = 0;
                days++;
                if (targetCircle > currentCircle)
                {
                    //for(int i=0; i<1000; i++)
                    {
                        circleList.Add(Instantiate(Circle, this.transform.position + new Vector3(Random.Range(-8.0f, 17.0f) * 0.1f, Random.Range(-10.0f, 15.0f) * 0.1f, -2), this.transform.rotation, this.transform) as GameObject);
                        //Debug.Log("make circle " + currentCircle);
                        currentCircle += 10000;
                    }

                }
                else
                    working = false;
            }
            time += 0.03f;// 1.0f;// 0.005f;
        }
    }
}
