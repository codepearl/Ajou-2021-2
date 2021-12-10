using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleMaker : MonoBehaviour
{
    public Text transmissionValue;
    public int p, pt;
    public GameObject Circle;
    public int targetCircle;
    public int currentCircle;
    public float time;
    public int days;
    public bool working = false;
    List<GameObject> circleList = new List<GameObject>();
    int targetDays = -1;

    public void init()
    {
        currentCircle = 0;
        //targetCircle = 5000; // 50000000;
        time = 1;
        days = 0;
        working = true;
        p = 0; pt = 0;
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
        Color color = Color.red;
        color.a = 0.2f;
        currentCircle = 0;
        targetCircle = 1000;
        time = 1;
        days = 0;
        Circle.GetComponent<Renderer>().material.SetColor("_Color", color);
    }

    // Update is called once per frame
    void Update()
    {
        if(working)
        {
            transmissionValue.text = p.ToString();
            //if (time >=1)
            //
           //     time = 0;
                
                if (targetCircle > p)
                {
                    p+=200;
                    pt+=200;
                    //for(int i=0; i<1000; i++)
                    if(pt >= 10000)
                    {
                        int n = Random.Range(0,3);
                        Vector3 v = new Vector3();
                        switch (n)
                        {
                        case 0:
                            v = new Vector3(Random.Range(-10.0f, -17.0f) * 0.1f + 0.5f, Random.Range(6.0f, 12.0f) * 0.1f - 0.15f, -2);
                            break;
                        case 1:
                            v = new Vector3(Random.Range(-10.0f, -20.0f) * 0.1f + 0.8f, Random.Range(2.0f, 17.0f) * 0.1f - 0.1f, -2);
                            break;
                        case 2:
                            v = new Vector3(Random.Range(-14.0f, 14.0f) * 0.1f, Random.Range(-10.0f, 20.0f) * 0.1f - 1.5f, -2);
                            break;
                        }

                        pt = 0;
                        circleList.Add(Instantiate(Circle, this.transform.position + v, this.transform.rotation, this.transform) as GameObject);
                        //Debug.Log("make circle " + currentCircle);
                        //currentCircle += 10000;
                    }

                }
                else
                {
                    working = false;
                    p = targetCircle;
                    transmissionValue.text = p.ToString();
                }    
                    
            //}
            //time += 0.03f;// 1.0f;// 0.005f;
            if(targetDays != -1)
            {
             //   if(targetDays > )
            }
        }
    }
}
