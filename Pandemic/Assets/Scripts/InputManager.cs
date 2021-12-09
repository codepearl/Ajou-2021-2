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
    public InputField fatalityRateFieldChild;
    public InputField fatalityRateFieldAdult;
    public InputField fatalityRateFieldSenior;
    public InputField preventionRateField;
    public InputField numberOfContactsField;
    public InputField vaccinatedFatalityRateField;
    public InputField developPeriodField;
    public InputField vaccineCostField;
    public InputField threatingDayField;

    public void Init()
    {

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
        if (!numberOfContactsField.text.Equals(""))
        {
            Debug.Log("set numberOfContacts : " + Convert.ToInt32(numberOfContactsField.text));
            mainCamera.GetComponent<Algorithm>().numberOfContacts
                = Convert.ToInt32(numberOfContactsField.text);
        }
        //level2
        if (!fatalityRateFieldChild.text.Equals(""))
        {
            Debug.Log("set fatalityRateFieldChild : " + Convert.ToDouble(fatalityRateFieldChild.text));
            mainCamera.GetComponent<Algorithm>().fatalityRateList[0]
                = Convert.ToDouble(fatalityRateFieldChild.text);
        }
        if (!fatalityRateFieldAdult.text.Equals(""))
        {
            Debug.Log("set fatalityRateFieldAdult : " + Convert.ToDouble(fatalityRateFieldAdult.text));
            mainCamera.GetComponent<Algorithm>().fatalityRateList[1]
                = Convert.ToDouble(fatalityRateFieldAdult.text);
        }
        if (!fatalityRateFieldSenior.text.Equals(""))
        {
            Debug.Log("set fatalityRateFieldSenior : " + Convert.ToDouble(fatalityRateFieldSenior.text));
            mainCamera.GetComponent<Algorithm>().fatalityRateList[2]
                = Convert.ToDouble(fatalityRateFieldSenior.text);
        }
        if (!vaccinatedFatalityRateField.text.Equals(""))
        {
            Debug.Log("set vaccinatedFatalityRateField : " + Convert.ToDouble(vaccinatedFatalityRateField.text));
            mainCamera.GetComponent<Algorithm>().vaccinatedFatalityRate
                = Convert.ToDouble(vaccinatedFatalityRateField.text);
        }
        if (!developPeriodField.text.Equals(""))
        {
            Debug.Log("set developPeriodField : " + Convert.ToInt32(developPeriodField.text));
            mainCamera.GetComponent<Algorithm>().developPeriod
                = Convert.ToInt32(developPeriodField.text);
        }
        if (!vaccineCostField.text.Equals(""))
        {
            Debug.Log("set vaccineCostField : " + Convert.ToDouble(vaccineCostField.text));
            mainCamera.GetComponent<Algorithm>().vaccineCost
                = Convert.ToDouble(vaccineCostField.text);
        }
        if (!threatingDayField.text.Equals(""))
        {
            Debug.Log("set threatingDayField : " + Convert.ToInt32(threatingDayField.text));
            mainCamera.GetComponent<Algorithm>().threatingDay
                = Convert.ToInt32(threatingDayField.text);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
        transmissionRateField = GameObject.Find("transmissionRateField").GetComponent<InputField>();
        //level1
        //fatalityRateField = GameObject.Find("fatalityRateField").GetComponent<InputField>();
        fatalityRateFieldChild = GameObject.Find("fatalityRateFieldChild").GetComponent<InputField>();
        fatalityRateFieldAdult = GameObject.Find("fatalityRateFieldAdult").GetComponent<InputField>();
        fatalityRateFieldSenior = GameObject.Find("fatalityRateFieldSenior").GetComponent<InputField>();
        preventionRateField = GameObject.Find("preventionRateField").GetComponent<InputField>();
        numberOfContactsField = GameObject.Find("numberOfContactsField").GetComponent<InputField>();
        vaccinatedFatalityRateField = GameObject.Find("vaccinatedFatalityRateField").GetComponent<InputField>();
        developPeriodField = GameObject.Find("developPeriodField").GetComponent<InputField>();
        vaccineCostField = GameObject.Find("vaccineCostField").GetComponent<InputField>();
        threatingDayField = GameObject.Find("threatingDayField").GetComponent<InputField>();

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
