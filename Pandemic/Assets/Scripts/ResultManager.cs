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
    public Text fatalityRateValueChild;
    public Text fatalityRateValueAdult;
    public Text fatalityRateValueSenior;
    public Text InfectionValueChild;
    public Text InfectionValueAdult;
    public Text InfectionValueSenior;
    public Text populationValueChild;
    public Text populationValueAdult;
    public Text populationValueSenior;
    public Text vaccinatedValueChild;
    public Text vaccinatedValueAdult;
    public Text vaccinatedValueSenior;

    public void updateText()
    {

        transmissionRateValue.text = mainCamera.GetComponent<Algorithm>().transmissionRate.ToString();
        fatalityRateValue.text = mainCamera.GetComponent<Algorithm>().fatalityRate.ToString();
        preventionRateValue.text = mainCamera.GetComponent<Algorithm>().preventionRate.ToString();
        shotPerDayValue.text = mainCamera.GetComponent<Algorithm>().mu_s.ToString();// shotPerDay.ToString();
        numberOfContactsValue.text = mainCamera.GetComponent<Algorithm>().numberOfContacts.ToString();
        numberOfInfectionsValue.text = mainCamera.GetComponent<Algorithm>().numberOfInfections.ToString();
        numberOfVaccinatedValue.text = mainCamera.GetComponent<Algorithm>().numberOfVaccinated.ToString();
        populationValue.text = mainCamera.GetComponent<Algorithm>().population.ToString();
        numberOfDeathsValue.text = (50000000 - mainCamera.GetComponent<Algorithm>().population).ToString();
        fatalityRateValueChild.text = mainCamera.GetComponent<Algorithm>().fatalityRateList[0].ToString();
        fatalityRateValueAdult.text = mainCamera.GetComponent<Algorithm>().fatalityRateList[1].ToString();
        fatalityRateValueSenior.text = mainCamera.GetComponent<Algorithm>().fatalityRateList[2].ToString();
        populationValueChild.text = mainCamera.GetComponent<Algorithm>().popList[0].ToString();
        populationValueAdult.text = mainCamera.GetComponent<Algorithm>().popList[1].ToString();
        populationValueSenior.text = mainCamera.GetComponent<Algorithm>().popList[2].ToString();
        vaccinatedValueChild.text = mainCamera.GetComponent<Algorithm>().numberOfVaccinatedList[0].ToString();
        vaccinatedValueAdult.text = mainCamera.GetComponent<Algorithm>().numberOfVaccinatedList[1].ToString();
        vaccinatedValueSenior.text = mainCamera.GetComponent<Algorithm>().numberOfVaccinatedList[2].ToString();
        InfectionValueChild.text = (mainCamera.GetComponent<Algorithm>().dayinfectsList[0] 
            + mainCamera.GetComponent<Algorithm>().dayVaccinatedInfectsList[0]).ToString();
        InfectionValueAdult.text = (mainCamera.GetComponent<Algorithm>().dayinfectsList[1]
            + mainCamera.GetComponent<Algorithm>().dayVaccinatedInfectsList[1]).ToString();
        InfectionValueSenior.text = (mainCamera.GetComponent<Algorithm>().dayinfectsList[2]
            + mainCamera.GetComponent<Algorithm>().dayVaccinatedInfectsList[2]).ToString();
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
