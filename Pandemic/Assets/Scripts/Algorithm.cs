using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Algorithm : MonoBehaviour
{

    public int population = 50000000;

    public double transmissionRate = 0.0089;
    public double fatalityRate = 0.000201;
    public double vaccinatedRate = 0;
    public double infectionsRate = 0;

    public int dayInfects;
    public int numberOfVaccinated = 0;
    public int numberOfDeaths = 0;
    public int numberOfInfections = 14;
    int threatingDay = 14;

    public int day = 0;

    public int numberOfContacts;

    public int shotPerDay;
    public double preventionRate;

    public double norm(double mean, double stdDev)
    {
        System.Random rand = new System.Random(); //reuse this if you are generating many
        double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
        double randNormal =
                     mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        return randNormal;
    }

    public void algo()
    {
        int[] dayList = new int[3650];
        int[] infectedList = new int[3650];
        int[] vaccinatedList = new int[3650];
        int[] deadList = new int[3650];
        int[] infectsPerDayList = new int[3650];


        double mu_c, sigma_c, mu_s, sigma_s;

        mu_c = 30.0;
        sigma_c = 5.0;
        mu_s = 400000;
        sigma_s = 50000;

        while (vaccinatedRate < 1 && numberOfInfections < population && day < 3650)
        {

            ///�Ϸ� �ϰ� : ���� -> ��� -> ��� ���� -> ����


            System.Random rand = new System.Random();

            dayList[day] = day;
            infectedList[day] = numberOfInfections;
            vaccinatedList[day] = numberOfVaccinated;
            deadList[day] = 50000000 - population;

            numberOfContacts = (int)norm(mu_c, sigma_c);
            numberOfContacts = (int)(numberOfContacts * (1 - Math.Pow(infectionsRate, 0.1)) * Math.Pow(Math.Max(1, day - 1500), 0.2) + 3);



            /// print(numberOfContacts, infectionsRate, numberOfInfections)
            /// �ڷγ��� ���������� �ۿ� �ȳ���, 3���� ������ ����
            /// 1500 ���� �����ڷγ� ����(������ ��� ����)���� 2�� �ҵ��� ����

            preventionRate = norm(0.9, 0.05);
            shotPerDay = (int)(norm(mu_s, sigma_s));

            dayInfects = (int)(numberOfInfections * numberOfContacts * transmissionRate * (1 - vaccinatedRate) + rand.Next(20, 31));
            infectsPerDayList[day] = dayInfects;

            numberOfInfections += dayInfects;

            /// �����ڷ� ���� ���� + �ܺ� ���� (20~30)

            if (day > threatingDay)
            {
                if (infectedList[day - 1] > 500)
                    numberOfInfections -= infectsPerDayList[day - threatingDay];
            }

            infectionsRate = numberOfInfections / (double)population;
            numberOfDeaths = (int)(numberOfInfections * fatalityRate);
            population -= numberOfDeaths;

            if (numberOfVaccinated > population)
                numberOfVaccinated = population;

            vaccinatedRate = numberOfVaccinated / (double)population;
            numberOfVaccinated += (int)(shotPerDay * preventionRate * (1 - vaccinatedRate) * Math.Max(infectionsRate, 0.01));
            /// Ȯ���ڰ� ���������� ��� ����ڰ� �þ
            /// ��� �������� ���������� ������ ������ ���� ������ ��
            /// 0.01%�� ������ ������ �Ϸ翡 ����� ���߷��� ��.

            numberOfInfections -= (int)(numberOfInfections * (shotPerDay / (population - numberOfVaccinated)));
            /// ������ �� ��� ���� �� �񰨿��� ��
            /// Console.WriteLine(day +" : "+ numberOfVaccinated);

            day++;

            /// Console.WriteLine("{0}", numberOfInfections);
        }

        Debug.Log("���� ������ : {0}" + numberOfInfections);
        Debug.Log("��� ���� : {0}" + numberOfVaccinated);
        Debug.Log("������: {0}" + population);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


/*
 * public class Algorithm : MonoBehaviour
{

    public int population = 50000000;

    public float transmissionRate = 0.0089f;
    public float fatalityRate = 0.000201f;
    public float vaccinatedRate = 0;
    public float infectionsRate = 0;

    public int dayInfects;
    public int numberOfVaccinated = 0;
    public int numberOfDeaths = 0;
    public int numberOfInfections = 14;

    public int day = 0;

    public int numberOfContacts;

    public int shotPerDay;
    public float preventionRate;

    public float norm(float mean, float stdDev)
    {
       // Random rand = new Random(); //reuse this if you are generating many
        float u1 = 1.0f - Random.Range(0,1); //uniform(0,1] random doubles
        float u2 = 1.0f - Random.Range(0,1);
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                     Mathf.Sin(2.0f *  Mathf.PI * u2); //random normal(0,1)
        float randNormal =
                     mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        return randNormal;
    }

    public void algo()
    {

        int[] dayList = new int[3650];
        int[] infectedList = new int[3650];
        int[] vaccinatedList = new int[3650];
        int[] deadList = new int[3650];
        int[] infectsPerDayList = new int[3650];
        int threatingDay = 14;


        float mu_c, sigma_c, mu_s, sigma_s;

        mu_c = 30.0f;
        sigma_c = 5.0f;
        mu_s = 400000;
        sigma_s = 50000;

        while (vaccinatedRate < 1 && numberOfInfections < population && day < 3650)
        {

            ///�Ϸ� �ϰ� : ���� -> ��� -> ��� ���� -> ����

            dayList[day] = day;
            infectedList[day] = numberOfInfections;
            vaccinatedList[day] = numberOfVaccinated;
            deadList[day] = 50000000 - population;

            numberOfContacts = (int)norm(mu_c, sigma_c);
            numberOfContacts = (int)(numberOfContacts * (1 - Mathf.Pow(infectionsRate, 0.1f)) * Mathf.Pow(Mathf.Max(1, day - 1500), 0.2f) + 3);



            /// print(numberOfContacts, infectionsRate, numberOfInfections)
            /// �ڷγ��� ���������� �ۿ� �ȳ���, 3���� ������ ����
            /// 1500 ���� �����ڷγ� ����(������ ��� ����)���� 2�� �ҵ��� ����

            preventionRate = norm(0.9f, 0.05f);
            shotPerDay = (int)(norm(mu_s, sigma_s));

            dayInfects = (int)(numberOfInfections * numberOfContacts * transmissionRate * (1 - vaccinatedRate) + Random.Range(20, 31)); //20,31
            infectsPerDayList[day] = dayInfects;

            numberOfInfections += dayInfects;

            /// �����ڷ� ���� ���� + �ܺ� ���� (20~30)

            if (day > threatingDay)
            {
                if (infectedList[day - 1] > 500)
                    numberOfInfections -= infectsPerDayList[day - threatingDay];
            }

            infectionsRate = numberOfInfections / population;
            numberOfDeaths = (int)(numberOfInfections * fatalityRate);
            population -= numberOfDeaths;

            if (numberOfVaccinated > population)
                numberOfVaccinated = population;

            vaccinatedRate = numberOfVaccinated / population;
            numberOfVaccinated += (int)(shotPerDay * preventionRate * (1 - vaccinatedRate) * Mathf.Max(infectionsRate, 0.01f));
            /// Ȯ���ڰ� ���������� ��� ����ڰ� �þ
            /// ��� �������� ���������� ������ ������ ���� ������ ��
            /// 0.01%�� ������ ������ �Ϸ翡 ����� ���߷��� ��.

            numberOfInfections -= (int)(numberOfInfections * (shotPerDay / (population - numberOfVaccinated)));
            /// ������ �� ��� ���� �� �񰨿��� ��
            /// Console.WriteLine(day +" : "+ numberOfVaccinated);

            day++;

            /// Console.WriteLine("{0}", numberOfInfections);
        }

        Debug.Log("���� ������ : {0}" + numberOfInfections);
        Debug.Log("��� ���� : {0}" + numberOfVaccinated);
        Debug.Log("������: {0}" + population);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

*/