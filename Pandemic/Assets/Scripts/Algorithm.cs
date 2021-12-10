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

    public double vaccineCost = 73.375;
    public int dayInfects;
    public int numberOfVaccinated = 0;
    public int numberOfDeaths = 0;
    public int numberOfInfections = 14;
    public int threatingDay = 14;

    public int[] graphRecord = new int[3650];
    public int[] childInfectRecord = new int[3650];
    public int[] adultInfectRecord = new int[3650];
    public int[] seniorInfectRecord = new int[3650];

    public int[] popList;
    public int day = 0;

    public int[] dayinfectsList = new int[3]; // ��� �������ڿ� ���� ������ ��
    public int[] dayVaccinatedInfectsList = new int[3]; // ��� �����ڿ� ���� ������ ��

    public int numberOfContacts = 100;

    public double mu_s, sigma_s;
    public double mu_c, sigma_c;


    public int shotPerDay;
    public double preventionRate;
    public double vaccinatedTransRate = (1 - 0.2);

    public double[] fatalityRateList = new double[3]; // �� �α��� ġ��� �迭

    public int[] numberofInfectionsList = new int[3]; // ��� �������� �α��� �� ������ ��

    public int[] numberOfVaccinatedList = new int[3]; // ���ɺ� ��� ������ ��

    public double vaccinatedFatalityRate = (1 - 0.02); // ��� �¾��� ��쿡 ġ���

    public class dayVaccinating
    {
        public double[] howOldAreYou;

        public dayVaccinating()
        {
            howOldAreYou = new double[3];
            howOldAreYou[0] = 0; // child
            howOldAreYou[1] = 0; // adult
            howOldAreYou[2] = 0; // senior
        }
    };

    public class dayVaccinatingList
    {
        public dayVaccinating[] a;

        public dayVaccinatingList()
        {
            a = new dayVaccinating[3650];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = new dayVaccinating();
            }
        }
    };

    public class wrapper
    {
        public dayVaccinatingList d;
        public int population;
        public wrapper()
        {
            d = new dayVaccinatingList();
            population = 0;
        }

    }

    wrapper[] children;

    dayVaccinatingList[] childrenByCrossOver;
    dayVaccinatingList[] childrenByMutation;

    wrapper[] topteer;
    wrapper[] saving;
    public wrapper[] winner;

    public int developPeriod;

    public wrapper[] selection(wrapper[] w, int select) // ������ ��ǰ
    {
        wrapper[] topTeer;
        wrapper movement;
        topTeer = new wrapper[select];
        for (int i = 0; i < w.Length; i++)
        {
            movement = w[i];

            for (int j = 0; j < select; j++)
            {
                if (topTeer[j] == null)
                {
                    topTeer[j] = movement;
                    break;
                }

                else if (topTeer[j].population < movement.population)
                {
                    if (j == (select - 1))
                    {
                        topTeer[j] = movement;
                        break;
                    }

                    else
                    {
                        topTeer[j + 1] = topTeer[j];
                        topTeer[j] = movement;
                        break;
                    }
                }
            }
        }
        return topTeer;
    }
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

    public dayVaccinatingList FirstVaccinatingCreate(int endDay)
    {
        // GA�� �⺻���� �⺻ //



        dayVaccinatingList VaccineScheduleList = new dayVaccinatingList(); // ��¥�� ��� ���� ���� ����Ʈ

        for (int i = 0; i < endDay; i++) // ��¥�� ��� ���� ���� �迭 ����
        {
            if (i < developPeriod)
            {
                VaccineScheduleList.a[i].howOldAreYou[0] = 0;
                VaccineScheduleList.a[i].howOldAreYou[1] = 0;
                VaccineScheduleList.a[i].howOldAreYou[2] = 0;
            }
            else
            {
                System.Random rand = new System.Random();
                double youngVacRate = rand.Next(1000) / (double)1000;
                double adultVacRate = rand.Next(1000 - (int)(youngVacRate * 1000)) / (double)1000;
                double oldVacRate = 1 - (youngVacRate + adultVacRate);

                VaccineScheduleList.a[i].howOldAreYou[0] = youngVacRate;
                VaccineScheduleList.a[i].howOldAreYou[1] = adultVacRate;
                VaccineScheduleList.a[i].howOldAreYou[2] = oldVacRate;
            }
        }
        return VaccineScheduleList;
    }

    public dayVaccinatingList[] Mutation(wrapper[] topTeer, int endDay)
    {
        dayVaccinatingList[] childrenList;
        childrenList = new dayVaccinatingList[topTeer.Length];

        for (int i = 0; i < childrenList.Length; i++)
        {
            childrenList[i] = new dayVaccinatingList();

            for (int day = 0; day < endDay; day++)
            {
                childrenList[i].a[day].howOldAreYou[0] = topTeer[i].d.a[day].howOldAreYou[1];           // ù ��°(0��°) ���� û��� ��з��� �̼��� ��з���
                childrenList[i].a[day].howOldAreYou[1] = topTeer[i].d.a[day].howOldAreYou[2];           // ù ��°(0��°) ���� ���� ��з��� û��� ��з���
                childrenList[i].a[day].howOldAreYou[2] = topTeer[i].d.a[day].howOldAreYou[0];           // ù ��°(0��°) ���� �̼��� ��з��� ���� ��з���
            }

        }
        return childrenList;
    }

    public int FitnessCheck(dayVaccinatingList d, bool final)
    {
        startPop = 50000000; // �α��� ����
        population = startPop;
        //// �α� ���� ���� ���� ////


        double youngRate = 0.167; // �̼����� �ʱ� ���� (10�� ����)
        double adultRate = 0.587; // û��� �ʱ� ���� (20�� ~ 50��)
        double oldRate = 1 - (youngRate + adultRate); // ���� �ʱ� ���� (50�� �̻�)

        // �� ���ɴ뺰 �α��� �迭

        popList[0] = (int)(population * youngRate); // �̼����� �α��� �ʱ⼳��
        popList[1] = (int)(population * adultRate); // û��� �α��� �ʱ⼳��
        popList[2] = (int)(population * oldRate); // ���� �α��� �ʱ⼳��

        double[] popRateList = new double[3]; // �α� ����(����)
        for (int i = 0; i < 3; i++)
            popRateList[i] = (double)popList[i] / (double)population;

        double mu_c, sigma_c;
        mu_c = (double)100; /// numberOfContacts; // �Ϸ� ������ �� ���
        sigma_c = mu_c / 5;

        //// ���̷��� ���� ���� ���� ////
        ///
        

        

        int[] dayDeathsList = new int[3]; // ���ɺ� ����� ��
        for (int i = 0; i < 3; i++)
            dayDeathsList[i] = 0;

        numberOfInfections = 14; // �ʱ� ������ ����

        for (int i = 0; i < 2; i++)
            dayinfectsList[i] = (int)(numberOfInfections * popRateList[i]);
        dayinfectsList[2] = numberOfInfections - (dayinfectsList[0] + dayinfectsList[1]); // ���� �Ϸ� ������ ��

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
        day = 0;

        //threatingDay = 14; // ġ�� �Ⱓ



        //// ��� ���� ���� ���� ////



        numberOfVaccinatedList[0] = 0; // �̼����� ��� ������ ��
        numberOfVaccinatedList[1] = 0; // û��� ��� ������ ��
        numberOfVaccinatedList[2] = 0; // ���� ��� ������ ��

        for (int i = 0; i < 3; i++)
            numberOfVaccinated += numberOfVaccinatedList[i]; // �� ��� ������

        int[] shotPerDayList = new int[3];


        
        mu_s = popList[1] * vaccineCost; // ��� ������ ���
        sigma_s = mu_s / 8;


        //// ��� ������ ////

        day = 0; // ��¥ Counting
        int endDay = 3650; // ������ �ϼ�
        int[] dayRecord = new int[endDay]; // ��¥ (�׷��� X��)
        int[,] infectedRecord = new int[3, endDay]; // ���ɴ뺰 ���� ������ �� ���
        int[,] vaccinatedRecord = new int[3, endDay]; // ���ɴ뺰 ��� ������ �� ���
        int[,] deadRecord = new int[3, endDay]; // ���ɴ뺰 ����� �� ���
        int[,] infectsPerDayList = new int[3, endDay]; // ��¥�� �������� ������ �� ��� (ġ�� ����)
        int[,] vaccinatedinfectsPerDayList = new int[3, endDay]; // ��¥�� ������ ������ �� ��� (ġ�� ����)
       
        for (int i = 0; i < 3; i++)
        {
            graphRecord[i] = 0;
            childInfectRecord[i] = 0;
            adultInfectRecord[i] = 0;
            seniorInfectRecord[i] = 0;
        }



        //// ���� ////



        //// ���� Simulation ���� ////

        while (vaccinatedRate < 1 & numberOfInfections < population & day < endDay)
        {
            // �Ϸ� �ϰ� : ����-> ���� -> ��� -> ��� ���� -> ����
            // Console.WriteLine("");
            // Console.WriteLine("��¥ : " + day);

            System.Random rand = new System.Random();


            mu_s = (popList[1] - numberofInfectionsList[1] - numberofVaccinatedInfectionsList[1]) / vaccineCost; // ��� ������ ���
            sigma_s = mu_s / 8;

            numberOfContacts = (int)norm(mu_c, sigma_c); // ������ �� ����
            numberOfContacts = (int)(numberOfContacts * (1 - Math.Pow(infectionsRate, 0.05)) * Math.Pow(Math.Max(1, day - 1500), 0.01) + 3);


            // print(numberOfContacts, infectionsRate, numberOfInfections)
            // �ڷγ��� ���������� �ۿ� �ȳ���, 3���� ������ ����
            // 1500�� ���� �����ڷγ� ���� (������ ��� ������ ���� 2�� �ҵ��� ����)

            // Console.Write("������ : ");
            for (int i = 0; i < 3; i++)
            {
                dayinfectsList[i] = (int)((numberOfInfections * (double)numberOfContacts * transmissionRate * (1 - vaccinatedRate) * (1 - infectionsRate)) * popRateList[i]);
                dayinfectsList[i] += (int)((double)rand.Next(20, 31) * popRateList[i] * (1 - vaccinatedRate));
                dayinfectsList[i] = Math.Abs(dayinfectsList[i]);
                dayVaccinatedInfectsList[i] = (int)((numberOfInfections * (double)numberOfContacts * transmissionRate * (1 - vaccinatedTransRate) * vaccinatedRate * (1 - infectionsRate)) * popRateList[i]);
                dayVaccinatedInfectsList[i] += (int)(rand.Next(20, 31) * popRateList[i] * vaccinatedRate);
                dayVaccinatedInfectsList[i] = Math.Abs(dayVaccinatedInfectsList[i]);               // �����ڷ� ���� ���� + �ܺ� ���� (20~30)
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
                    if (dayCount == threatingDay - 1 || dayCount == day)
                    {
                        dayCount = 0;
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
                    if (dayCount == threatingDay - 1 || dayCount == day)
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

            shotPerDay = (int)mu_s;// (norm(mu_s, sigma_s));

            int reallyshot;
            // Console.Write("��� ������ : ");
            for (int i = 0; i < 3; i++)
            {
                shotPerDayList[i] = (int)(shotPerDay * d.a[day].howOldAreYou[i]);

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

                if (changingnumber > numberofInfectionsList[i])
                    changingnumber = numberofInfectionsList[i];

                numberofInfectionsList[i] -= changingnumber;
                numberofVaccinatedInfectionsList[i] += changingnumber;
                int dayCount = 0;

                for (int j = changingnumber; j < 0; j = j - rand.Next(0, Math.Max(0, (int)j)))
                {

                    if (dayCount == threatingDay - 1 || dayCount == day)
                    {
                        dayCount = 0;
                        continue;
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
                if (final)
                {
                    graphRecord[day] += infectedRecord[i, day]; //�׷����� ���� �Ϸ� ������ �� ���
                    childInfectRecord[day] = infectedRecord[0, day];
                    adultInfectRecord[day] = infectedRecord[1, day];
                    seniorInfectRecord[day] = infectedRecord[2, day];
                }
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
            //Debug.Log("��� ���� : {0}" + numberOfVaccinated);
            //Debug.Log("���� ������ : {0}" + numberOfInfections);
            //Debug.Log("������: {0}" + population);
        }
        if(final)
        {
            Debug.Log("���� ��¥ : {0}" + day);
            Debug.Log("���� ������ : {0}" + numberOfInfections);
            Debug.Log("��� ���� : {0}" + numberOfVaccinated);
            Debug.Log("������: {0}" + population);

            setCircleMakerVariableLevel2();
        }

        return population;
    }


    public dayVaccinatingList[] CrossOver(wrapper[] topTeer, int endDay)
    {
        dayVaccinatingList[] childrenList;            //crossover�� ���� ���� 24���� �ڽ� ��ü
        childrenList = new dayVaccinatingList[24];

        for (int i = 0; i < 24; i++)
            childrenList[i] = new dayVaccinatingList();
        int o = 0;          // dayVaccinatingList�� 0���� 23���� �ֱ� ���� index
        while (o < 24)
        {
            for (int j = 0; j < 4; j++)         // ��ģ index
            {
                for (int k = 0; k < j; k++)         // ��ģ index
                {
                    if (j != k)         // ��ģ�� ��ģ�� �ٸ� ����̾�� ��
                    {
                        int m = 0;          // dayVaccinating�� 0���� endDay-1���� �ֱ� ���� index
                        while (m < endDay)
                        {
                            childrenList[o].a[m] = topTeer[j].d.a[m];         // �Ƶ��� ��ģ�� 3��ŭ ����
                            childrenList[o].a[m + 1] = topTeer[j].d.a[m + 1];
                            childrenList[o].a[m + 2] = topTeer[j].d.a[m + 2];
                            childrenList[o].a[m + 3] = topTeer[k].d.a[m + 3];         // �Ƶ��� ��ģ�� 2��ŭ ����
                            childrenList[o].a[m + 4] = topTeer[k].d.a[m + 4];

                            childrenList[o + 1].a[m] = topTeer[k].d.a[m];         // ���� ��ģ�� 3��ŭ ����
                            childrenList[o + 1].a[m + 1] = topTeer[k].d.a[m + 1];
                            childrenList[o + 1].a[m + 2] = topTeer[k].d.a[m + 2];
                            childrenList[o + 1].a[m + 3] = topTeer[j].d.a[m + 3];         // ���� ��ģ�� 2��ŭ ����
                            childrenList[o + 1].a[m + 4] = topTeer[j].d.a[m + 4];

                            m = m + 5;
                        }
                        o = o + 2;
                    }
                }
            }
        }
        return childrenList;            // 24�� �ڽ� ��ü�� ��ȯ
    }

    public void level2()
    {

        int endDay = 3650;
        int numberOfSelect = 4;
        int numberOfMutation = 1;
        int targetGeneration = 30;

        children = new wrapper[28];
        saving = new wrapper[numberOfSelect * targetGeneration];

        for (int generation = 0; generation < targetGeneration; generation++)
        {
            Console.WriteLine("����� : {0}", generation);

            //System.Threading.Tasks.Parallel.For(0, children.Length, i =>
            for (int i = 0; i < children.Length; i++)
            {
                if (generation == 0)
                {
                    children[i] = new wrapper();
                    dayVaccinatingList Solution;
                    Solution = FirstVaccinatingCreate(endDay);

                    children[i].d = Solution;
                    children[i].population = FitnessCheck(Solution, false);
                }
                else
                    children[i].population = FitnessCheck(children[i].d, false);
            };

            topteer = selection(children, numberOfSelect);

            childrenByCrossOver = CrossOver(topteer, endDay);
            childrenByMutation = Mutation(topteer, endDay);
            int crossovered = childrenByCrossOver.Length;

            for (int i = 0; i < children.Length; i++)
            {
                if (i < crossovered)
                    children[i].d = childrenByCrossOver[i];
                else
                    children[i].d = childrenByMutation[i - crossovered];
            }
        }



        winner = selection(children, 3);

        Console.WriteLine("1�� ������ �� : {0}", winner[0].population);
        Console.WriteLine("1�� ����� �� : {0}", (50000000 - winner[0].population));
        Console.WriteLine("2�� ������ �� : {0}", winner[1].population);
        Console.WriteLine("2�� ����� �� : {0}", (50000000 - winner[1].population));
        Console.WriteLine("3�� ������ �� : {0}", winner[2].population);
        Console.WriteLine("3�� ����� �� : {0}", (50000000 - winner[2].population));

       
        FitnessCheck(winner[0].d, true); // 1������ �׷��� �׸��� �ϴ� �޼ҵ�

        //variable set
        // =>result manager

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

            numberOfContacts = (int)mu_c; // (int)norm(mu_c, sigma_c);
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
        popList = new int[3];
        double youngRate = 0.167; // �̼����� �ʱ� ���� (10�� ����)
        double adultRate = 0.587; // û��� �ʱ� ���� (20�� ~ 50��)
        double oldRate = 1 - (youngRate + adultRate); // ���� �ʱ� ���� (50�� �̻�)

        popList[0] = (int)(population * youngRate); // �̼����� �α��� �ʱ⼳��
        popList[1] = (int)(population * adultRate); // û��� �α��� �ʱ⼳��
        popList[2] = (int)(population * oldRate); // ���� �α��� �ʱ⼳��

        fatalityRateList[0] = 0.0001; // �̼����� ġ���
        fatalityRateList[1] = 0.00104; // û��� ġ���
        fatalityRateList[2] = 0.034796; // ���� ġ���
        preventionRate = rand.Next(850, 950) / (double)1000;
        
        mu_s = 400000; // ��� ������ ���
        sigma_s = mu_s / 8;
        shotPerDay = (int)mu_s;//(norm(mu_s, sigma_s));
        numberOfContacts = 30;
        developPeriod = 150;

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