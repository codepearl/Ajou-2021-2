using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public Camera camera;
    public int mode = 0;

    public GameObject MainScene;
    public GameObject InputScene;
    public GameObject SimulationScene;
    public GameObject GraphScene;
    public GameObject ResultScene;

    public Button InitButton;
    public Canvas InputUI;
    public Canvas InputSceneCanvas;

    public void PrevScene()
    {
        switch (mode)
        {
            case 0:
                SetCameraToResult();
                mode = 4;
                
                break;
            case 1:
                SetCameraToMain();
                mode = 0;
                InputSceneCanvas.enabled = false;
                break;
            case 2:
                SetCameraToInput();
                mode = 1;
                InitButton.enabled = true;
                InputSceneCanvas.enabled = true;
                //InputUI.enabled = true;
                break;
            case 3:
                SetCameraToSimulation();
                SimulationScene.GetComponent<CircleMaker>().init();
                mode = 2;
                InitButton.enabled = false;
                break;
            case 4:
                SimulationScene.GetComponent<CircleMaker>().stop();
                SetCameraToGraph();
                mode = 3;
                
                break;
        }
    }

    public void NextScene()
    {
        switch (mode)
        {
            case 0:
                SetCameraToInput();
                mode = 1;
                InitButton.enabled = true;
                //InputUI.enabled = true;
                InputSceneCanvas.enabled = true;
                break;
            case 1:


                InputUI.GetComponent<InputManager>().setVariable();
                
                SetCameraToSimulation();
               
                SimulationScene.GetComponent<CircleMaker>().init();
                mode = 2;
                InitButton.enabled = false;
                InputSceneCanvas.enabled = false;
                //InputUI.enabled = false;
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

    void SetCameraToMain()
    {
        camera.transform.position = new Vector3(0, 0, -10);
    }
    void SetCameraToSimulation()
    {
        camera.transform.position = new Vector3(0, 40, -10);
        camera.GetComponent<Algorithm>().algo();
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

    void GetUI()
    {
        InputUI.GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCameraToMain();
        
        InputSceneCanvas.enabled = false;
        InitButton.enabled = false;
        //InputUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            if (mode != 1)
                NextScene();

        }*/

    }
}
