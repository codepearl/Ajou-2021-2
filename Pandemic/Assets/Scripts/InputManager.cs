using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputManager : MonoBehaviour
{
    public Button nextButton;
    public Button initButton;

    public float variable1;
    public float variable2;
    public float variable3;
    public float variable4;
    public float variable5;

    public void Init()
    {
        variable1 = 0;
        variable2 = 0;
        variable3 = 0;
        variable4 = 0;
        variable5 = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
        //initButton = GameObject.Find("initButton").GetComponent<Button>();
        //nextButton = GameObject.Find("nextButton").GetComponent<Button>();
        //initButton.onClick.AddListener(Init);
        //nextButton.onClick.AddListener(gameObject.GetComponent<SceneManager>().NextScene);
        //GameObject.Find("initButton").GetComponent<Text>().text = "초기화";
        //GameObject.Find("nextButton").GetComponent<Text>().text = "다음";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
