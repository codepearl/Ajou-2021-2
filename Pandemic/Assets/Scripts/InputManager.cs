using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputManager : MonoBehaviour
{
    public Button nextButton;
    public Button initButton;

    public GameObject mainCamera;

    public InputField transmissionRateField;
    public InputField fatalityRateField;
    public InputField preventionRateField;
    public InputField shotPerDayField;
    public InputField numberOfContactsField;

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

    public void setVariable()
    {
        mainCamera = GameObject.Find("Main Camera");
        if(!transmissionRateField.text.Equals(""))
        {
            Debug.Log("set transmissionRate : " + Convert.ToDouble(transmissionRateField.text));
            mainCamera.GetComponent<Algorithm>().transmissionRate
                = Convert.ToDouble(transmissionRateField.text);
        }
        if (!fatalityRateField.text.Equals(""))
        {
            Debug.Log("set fatalityRate : " + Convert.ToDouble(fatalityRateField.text));
            mainCamera.GetComponent<Algorithm>().fatalityRate
               = Convert.ToDouble(fatalityRateField.text);
        }
        if (!preventionRateField.text.Equals(""))
        {
            Debug.Log("set preventionRate : " + Convert.ToDouble(preventionRateField.text));
            mainCamera.GetComponent<Algorithm>().preventionRate
                = Convert.ToDouble(preventionRateField.text);
        }
        if (!shotPerDayField.text.Equals(""))
        {
            Debug.Log("set shotPerDay : " + Convert.ToInt32(shotPerDayField.text));
            mainCamera.GetComponent<Algorithm>().shotPerDay
                = Convert.ToInt32(shotPerDayField.text);
        }
        if (!numberOfContactsField.text.Equals(""))
        {
            Debug.Log("set numberOfContacts : " + Convert.ToInt32(numberOfContactsField.text));
            mainCamera.GetComponent<Algorithm>().numberOfContacts
                = Convert.ToInt32(numberOfContactsField.text);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
        transmissionRateField = GameObject.Find("transmissionRateField").GetComponent<InputField>();
        fatalityRateField = GameObject.Find("fatalityRateField").GetComponent<InputField>();
        preventionRateField = GameObject.Find("preventionRateField").GetComponent<InputField>();
        shotPerDayField = GameObject.Find("shotPerDayField").GetComponent<InputField>();
        numberOfContactsField = GameObject.Find("numberOfContactsField").GetComponent<InputField>();

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
