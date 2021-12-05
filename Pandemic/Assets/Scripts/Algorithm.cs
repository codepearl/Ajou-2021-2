using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    GameObject sm;

    public int startPop = 50000000; // �α��� ����
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

    public int numberOfContacts = 30;

    public int shotPerDay;
    public double preventionRate;
    public double vaccinatedTransRate;

    public double[] fatalityRateList = new double[3]; // �� �α��� ġ��� �迭

    public int[] numberofInfectionsList = new int[3]; // ��� �������� �α��� �� ������ ��

    public int[] numberOfVaccinatedList = new int[3]; // ���ɺ� ��� ������ ��

    public double vaccinatedFatalityRate = 0.02; // ��� �¾��� ��쿡 ġ���

    void setCircleMakerVariableLevel1()
    {
        sm = GameObject.Find("SimulationScene");
        sm.GetComponent<CircleMaker>().targetCircle = numberOfInfections;
    }

    void setCircleMakerVariableLevel2()
    {
        sm = GameObject.Find("SimulationScene");
        sm.GetComponent<CircleMaker>().targetCircle = numberOfInfections;
    }

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

    public int FitnessCheck(int generation, double[,] dayVaccinatingList)
    {
        startPop = 50000000; // �α��� ����
        population = startPop;
        //// �α� ���� ���� ���� ////


        double youngRate = 0.167; // �̼����� �ʱ� ���� (10�� ����)
        double adultRate = 0.587; // û��� �ʱ� ���� (20�� ~ 50��)
        double oldRate = 1 - (youngRate + adultRate); // ���� �ʱ� ���� (50�� �̻�)

        int[] popList = new int[3]; // �� ���ɴ뺰 �α��� �迭

        popList[0] = (int)(population * youngRate); // �̼����� �α��� �ʱ⼳��
        popList[1] = (int)(population * adultRate); // û��� �α��� �ʱ⼳��
        popList[2] = (int)(population * oldRate); // ���� �α��� �ʱ⼳��

        double[] popRateList = new double[3]; // �α� ����(����)
        for (int i = 0; i < 3; i++)
            popRateList[i] = (double)popList[i] / (double)population;

        double mu_c, sigma_c;
        mu_c = (double) numberOfContacts; // �Ϸ� ������ �� ���
        sigma_c = mu_c / 5;

        //// ���̷��� ���� ���� ���� ////
        ///
        

        

        int[] dayDeathsList = new int[3]; // ���ɺ� ����� ��
        for (int i = 0; i < 3; i++)
            dayDeathsList[i] = 0;

        numberOfInfections = 14; // �ʱ� ������ ����

        int[] dayinfectsList = new int[3]; // ��� �������ڿ� ���� ������ ��
        for (int i = 0; i < 2; i++)
            dayinfectsList[i] = (int)(numberOfInfections * popRateList[i]);
        dayinfectsList[2] = numberOfInfections - (dayinfectsList[0] + dayinfectsList[1]); // ���� �Ϸ� ������ ��

        int[] dayVaccinatedInfectsList = new int[3]; // ��� �����ڿ� ���� ������ ��
        for (int i = 0; i < 3; i++)
            dayVaccinatedInfectsList[i] = 0;


        for (int i = 0; i < 3; i++)
            numberofInfectionsList[i] = dayinfectsList[i];

        int[] numberofVaccinatedInfectionsList = new int[3]; // ��� ������ �α��� �� ������ ��
        for (int i = 0; i < 3; i++)
            numberofVaccinatedInfectionsList[i] = dayVaccinatedInfectsList[i];

        vaccinatedRate = 0;
        infectionsRate = 0;
        dayInfects = 0;
        numberOfVaccinated = 0;
        numberOfDeaths = 0;
        numberOfInfections = 14;
        threatingDay = 14;
        day = 0;

        threatingDay = 14; // ġ�� �Ⱓ



        //// ��� ���� ���� ���� ////



        numberOfVaccinatedList[0] = 0; // �̼����� ��� ������ ��
        numberOfVaccinatedList[1] = 0; // û��� ��� ������ ��
        numberOfVaccinatedList[2] = 0; // ���� ��� ������ ��

        numberOfVaccinated = 0;

        for (int i = 0; i < 3; i++)
            numberOfVaccinated += numberOfVaccinatedList[i]; // �� ��� ������

        int[] shotPerDayList = new int[3];





        //// ��� ������ ////

        day = 0; // ��¥ Counting
        int endDay = 3650; // ������ �ϼ�
        int[] dayRecord = new int[endDay]; // ��¥ (�׷��� X��)
        int[,] infectedRecord = new int[3, endDay]; // ���ɴ뺰 ���� ������ �� ���
        int[,] vaccinatedRecord = new int[3, endDay]; // ���ɴ뺰 ��� ������ �� ���
        int[,] deadRecord = new int[3, endDay]; // ���ɴ뺰 ����� �� ���
        int[,] infectsPerDayList = new int[3, endDay]; // ��¥�� �������� ������ �� ��� (ġ�� ����)
        int[,] vaccinatedinfectsPerDayList = new int[3, endDay]; // ��¥�� ������ ������ �� ��� (ġ�� ����)



        //// ���� ////
        double[,] VaccineScheduleList;

        if (generation == 0)
            VaccineScheduleList = FirstVaccinatingCreate(endDay);
        else
            VaccineScheduleList = dayVaccinatingList;




        //// ���� Simulation ���� ////

        while (vaccinatedRate < 1 & numberOfInfections < population & day < endDay)
        {
            // �Ϸ� �ϰ� : ����-> ���� -> ��� -> ��� ���� -> ����
            // Console.WriteLine("");
            // Console.WriteLine("��¥ : " + day);

            System.Random rand = new System.Random();



            //// ���� �ܰ� ////
            //numberOfContacts = (int)norm(mu_c, sigma_c); // ������ �� ����
            //numberOfContacts = (int)(numberOfContacts * (1 - Math.Pow(infectionsRate, 0.05)) * Math.Pow(Math.Max(1, day - 1500), 0.01) + 3);

            // print(numberOfContacts, infectionsRate, numberOfInfections)
            // �ڷγ��� ���������� �ۿ� �ȳ���, 3���� ������ ����
            // 1500�� ���� �����ڷγ� ���� (������ ��� ������ ���� 2�� �ҵ��� ����)

            // Console.Write("������ : ");
            for (int i = 0; i < 3; i++)
            {
                dayinfectsList[i] = (int)((numberOfInfections * (double)numberOfContacts * transmissionRate * (1 - vaccinatedRate) * (1 - infectionsRate)) * popRateList[i]);
                dayinfectsList[i] += (int)((double)rand.Next(20, 31) * popRateList[i] * (1 - vaccinatedRate));
                dayVaccinatedInfectsList[i] = (int)((numberOfInfections * (double)numberOfContacts * transmissionRate * vaccinatedTransRate * vaccinatedRate * (1 - infectionsRate)) * popRateList[i]);
                dayVaccinatedInfectsList[i] += (int)(rand.Next(20, 31) * popRateList[i] * vaccinatedRate);

                // �����ڷ� ���� ���� + �ܺ� ���� (20~30)
                numberofInfectionsList[i] += dayinfectsList[i]; // ���ɺ� �� �����ڼ� �߰�
                numberofVaccinatedInfectionsList[i] += dayVaccinatedInfectsList[i];

                // Console.Write(numberofInfectionsList[i] + " ");
                // Console.Write(numberofVaccinatedInfectionsList[i] + " ");
            }
            // Console.WriteLine("");



            //// ��� �ܰ� ////
            for (int i = 0; i < 3; i++)
            {
                dayDeathsList[i] = (int)(numberofInfectionsList[i] * fatalityRateList[i]);
                numberofInfectionsList[i] -= dayDeathsList[i];
                popList[i] -= dayDeathsList[i];

                int dayCount = 1;
                for (int j = dayDeathsList[i]; j < 0; j = j - rand.Next(0, Math.Max(0, (int)j)))
                {
                    if (dayCount == threatingDay - 1)
                    {
                        dayCount = 1;
                    }
                    infectsPerDayList[i, day - dayCount] -= j;

                    if (infectsPerDayList[i, day - dayCount] < 0)
                    {
                        j += (0 - infectsPerDayList[i, day - dayCount]);
                        infectsPerDayList[i, day - dayCount] = 0;
                    }

                    dayCount = dayCount + 1;
                }

                dayDeathsList[i] = (int)(numberofVaccinatedInfectionsList[i] * fatalityRateList[i] * vaccinatedFatalityRate);
                numberOfVaccinatedList[i] -= dayDeathsList[i];
                numberofVaccinatedInfectionsList[i] -= dayDeathsList[i];
                popList[i] -= dayDeathsList[i];

                dayCount = 1;
                for (int j = dayDeathsList[i]; j < 0; j = j - rand.Next(0, Math.Max(0, (int)j)))
                {
                    if (dayCount == threatingDay - 1)
                    {
                        dayCount = 1;
                    }
                    vaccinatedinfectsPerDayList[i, day - dayCount] -= j;

                    if (vaccinatedinfectsPerDayList[i, day - dayCount] < 0)
                    {
                        j += (0 - vaccinatedinfectsPerDayList[i, day - dayCount]);
                        vaccinatedinfectsPerDayList[i, day - dayCount] = 0;
                    }

                    dayCount = dayCount + 1;
                }
            }



            //// ��� ���� �ܰ� ////
            //preventionRate = rand.Next(850, 950) / (double)1000;
            

            int reallyshot;
            // Console.Write("��� ������ : ");
            for (int i = 0; i < 3; i++)
            {
                shotPerDayList[i] = (int)(shotPerDay * VaccineScheduleList[i, day]);

                reallyshot = (int)(shotPerDayList[i] * preventionRate * (1 - vaccinatedRate) * Math.Max(infectionsRate, 0.01));
                if (reallyshot < 0)
                    reallyshot = 0;

                if (numberOfVaccinatedList[i] + reallyshot < popList[i])
                    numberOfVaccinatedList[i] += reallyshot;
                else
                {
                    reallyshot = popList[i] - numberOfVaccinatedList[i];
                    numberOfVaccinatedList[i] = popList[i];
                }
                // Console.Write(numberOfVaccinatedList[i] + " ");
                // Console.Write(reallyshot + " ");

                // Ȯ���ڰ� ���������� ��� ����ڰ� �þ
                // ��� �������� ���������� ������ ������ ���� ������ ��
                // 0.01%�� ������ ������ �Ϸ翡 ����� ���߷��� ��.
                int changingnumber = 0;
                if ((popList[i] - numberOfVaccinatedList[i]) > 0)
                    changingnumber = (int)(numberofInfectionsList[i] / (double)(popList[i] - numberOfVaccinatedList[i]) * reallyshot);

                numberofInfectionsList[i] -= changingnumber;
                numberofVaccinatedInfectionsList[i] += changingnumber;
                int dayCount = 1;

                for (int j = changingnumber; j < 0; j = j - rand.Next(0, Math.Max(0, (int)j)))
                {

                    if (dayCount == threatingDay - 1)
                    {
                        dayCount = 1;
                    }
                    infectsPerDayList[i, day - dayCount] -= j;
                    vaccinatedinfectsPerDayList[i, day - dayCount] += j;

                    if (infectsPerDayList[i, day - dayCount] < 0)
                    {
                        j += (0 - infectsPerDayList[i, day - dayCount]);
                        infectsPerDayList[i, day - dayCount] = 0;
                    }

                    dayCount = dayCount + 1;
                }

            }
            // Console.WriteLine("");


            //// ġ�� �ܰ� ////
            if (day > threatingDay)
            {
                int totalInfected = 0;
                // Console.Write("ġ�� �� ������ : ");
                for (int i = 0; i < 3; i++)
                {
                    totalInfected += numberofInfectionsList[i] + numberofVaccinatedInfectionsList[i];
                    // Console.Write(infectsPerDayList[i, day - threatingDay] + " ");
                    // Console.Write(vaccinatedinfectsPerDayList[i, day - threatingDay] + " ");
                }
                // Console.WriteLine("");
                // Console.Write("ġ�� �� �Ѱ����� : ");
                if (totalInfected > 100)
                    for (int i = 0; i < 3; i++) // ���ɺ� ������ ġ�ᳯ¥ ���� �񰨿��� ��
                    {
                        numberofInfectionsList[i] -= infectsPerDayList[i, day - threatingDay];
                        if (numberofInfectionsList[i] < 0)
                            numberofInfectionsList[i] = 0;

                        numberofVaccinatedInfectionsList[i] -= vaccinatedinfectsPerDayList[i, day - threatingDay];
                        if (numberofVaccinatedInfectionsList[i] < 0)
                            numberofVaccinatedInfectionsList[i] = 0;

                        // Console.Write(numberofInfectionsList[i] + " ");
                        // Console.Write(numberofVaccinatedInfectionsList[i] + " ");
                    }

            }
            // Console.WriteLine(" ");


            //// ���� �ܰ� ////

            population = popList[0] + popList[1] + popList[2]; // �α� ����
                                                               // Console.Write("�α��� : ");
            for (int i = 0; i < 3; i++)
            {
                // Console.Write("{0} ", popList[i]);
                popRateList[i] = popList[i] / (double)population;
            }
            // Console.WriteLine(" ");

            dayRecord[day] = day; // ��¥ ���

            for (int i = 0; i < 3; i++)
            {
                infectsPerDayList[i, day] = dayinfectsList[i]; // �Ϸ� �������� ������ �� ���
                vaccinatedinfectsPerDayList[i, day] = dayVaccinatedInfectsList[i]; // �Ϸ� ������ ������ �� ���
                infectedRecord[i, day] = numberofInfectionsList[i] + numberofVaccinatedInfectionsList[i]; // �� ������ �� ���
                deadRecord[i, day] = startPop - popList[i]; // ����� ���
            }


            for (int i = 0; i < 3; i++)
            {
                numberOfVaccinated += numberOfVaccinatedList[i]; // �� ��� ������ ����
                vaccinatedRecord[i, day] = numberOfVaccinatedList[i]; // ��� ������ �� ���
            }

            numberOfInfections = 0;
            numberOfVaccinated = 0;
            for (int i = 0; i < 3; i++)
            {
                numberOfInfections += numberofInfectionsList[i] + numberofVaccinatedInfectionsList[i]; // �� ������ �� �ҷ�����
                numberOfVaccinated += numberOfVaccinatedList[i]; // �� ��� ������ �ҷ�����
            }

            infectionsRate = (numberOfInfections / (double)population);
            vaccinatedRate = (numberOfVaccinated / (double)population);


            day++; // �Ϸ� �ϰ� ��ħ
            Debug.Log("��� ���� : {0}" + numberOfVaccinated);
            Debug.Log("���� ������ : {0}" + numberOfInfections);
            Debug.Log("������: {0}" + population);
        }
        Debug.Log("���� ��¥ : {0}" + day);
        Debug.Log("���� ������ : {0}" + numberOfInfections);
        Debug.Log("��� ���� : {0}" + numberOfVaccinated);
        Debug.Log("������: {0}" + population);
        setCircleMakerVariableLevel2();

        return population;
    }

    public static double[,] FirstVaccinatingCreate(int endDay)
    {
        // GA�� �⺻���� �⺻ //

        double[,] dayVaccinatingList = new double[3, endDay]; // ��¥�� ��� ���� ���� ����Ʈ

        for (int i = 0; i < endDay; i++) // ��¥�� ��� ���� ���� �迭 ����
        {
            System.Random rand = new System.Random();
            double youngVacRate = rand.Next(1000) / (double)1000;
            double adultVacRate = rand.Next(1000 - (int)(youngVacRate * 1000)) / (double)1000;
            double oldVacRate = 1 - (youngVacRate + adultVacRate);

            dayVaccinatingList[0, i] = youngVacRate;
            dayVaccinatingList[1, i] = adultVacRate;
            dayVaccinatingList[2, i] = oldVacRate;
        }

        return dayVaccinatingList;
    }

    public void level2()
    {
        FitnessCheck(0, null);
    }

    public void level1()
    {
        int[] dayList = new int[3650];
        int[] infectedList = new int[3650];
        int[] vaccinatedList = new int[3650];
        int[] deadList = new int[3650];
        int[] infectsPerDayList = new int[3650];

        vaccinatedRate = 0;
        infectionsRate = 0;
        dayInfects = 0;
        numberOfVaccinated = 0;
        numberOfDeaths = 0;
        numberOfInfections = 14;
        threatingDay = 14;
        day = 0;


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

        Debug.Log("���� ������ : " + numberOfInfections);
        Debug.Log("��� ���� : " + numberOfVaccinated);
        Debug.Log("������ : " + population);
        setCircleMakerVariableLevel1();
    }

    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();

        fatalityRateList[0] = 0.0001; // �̼����� ġ���
        fatalityRateList[1] = 0.00104; // û��� ġ���
        fatalityRateList[2] = 0.034796; // ���� ġ���
        preventionRate = rand.Next(850, 950) / (double)1000;
        double mu_s, sigma_s;
        mu_s = 400000; // ��� ������ ���
        sigma_s = mu_s / 8;
        shotPerDay = (int)(norm(mu_s, sigma_s));

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