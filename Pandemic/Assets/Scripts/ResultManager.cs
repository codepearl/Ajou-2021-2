using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public GameObject mainCamera;
    public Text transmissionRateValue;
    public Text fatalityRateValue;
    public Text preventionRateValue;
    public Text shotPerDayValue;
    public Text numberOfContactsValue;
    public Text numberOfInfectionsValue;
    public Text numberOfVaccinatedValue;
    public Text populationValue;
    public Text numberOfDeathsValue;

    public void updateText()
    {

        transmissionRateValue.text = mainCamera.GetComponent<Algorithm>().transmissionRate.ToString();
        fatalityRateValue.text = mainCamera.GetComponent<Algorithm>().fatalityRate.ToString();
        preventionRateValue.text = mainCamera.GetComponent<Algorithm>().preventionRate.ToString();
        shotPerDayValue.text = mainCamera.GetComponent<Algorithm>().shotPerDay.ToString();
        numberOfContactsValue.text = mainCamera.GetComponent<Algorithm>().numberOfContacts.ToString();
        numberOfInfectionsValue.text = mainCamera.GetComponent<Algorithm>().numberOfInfections.ToString();
        numberOfVaccinatedValue.text = mainCamera.GetComponent<Algorithm>().numberOfVaccinated.ToString();
        populationValue.text = mainCamera.GetComponent<Algorithm>().population.ToString();
        numberOfDeathsValue.text = (50000000 - mainCamera.GetComponent<Algorithm>().population).ToString();

    }



    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
