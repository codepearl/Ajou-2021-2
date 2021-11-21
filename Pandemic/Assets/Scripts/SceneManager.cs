using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public Camera camera;
    public int mode = 0;

    public GameObject MainScene;
    public GameObject InputScene;
    public GameObject SimulationScene;
    public GameObject GraphScene;
    public GameObject ResultScene;

    void SetCameraToMain()
    {
        camera.transform.position = new Vector3(0, 0, -10);
    }
    void SetCameraToSimulation()
    {
        camera.transform.position = new Vector3(0, 40, -10);
    }

    void SetCameraToGraph()
    {
        camera.transform.position = new Vector3(0, 60, -10);
    }

    void SetCameraToInput()
    {
        camera.transform.position = new Vector3(0, 20, -10);
    }
    void SetCameraToResult()
    {
        camera.transform.position = new Vector3(0, 80, -10);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCameraToMain();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (mode)
            {
                case 0:
                    SetCameraToInput();
                    mode = 1;
                    break;
                case 1:
                    SetCameraToSimulation();
                    SimulationScene.GetComponent<CircleMaker>().init();
                    mode = 2;
                    break;
                case 2:
                    SimulationScene.GetComponent<CircleMaker>().stop();
                    SetCameraToGraph();
                    mode = 3;
                    break;
                case 3:
                    SetCameraToResult();
                    mode = 4;
                    break;

                case 4:
                    SetCameraToMain();
                    mode = 0;
                    break;
            }

        }

    }
}
