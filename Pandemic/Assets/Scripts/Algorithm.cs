using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    GameObject sm;

    public int startPop = 50000000; // 인구수 설정
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

    public double[] fatalityRateList = new double[3]; // 각 인구별 치명률 배열

    public int[] numberofInfectionsList = new int[3]; // 백신 미접종자 인구별 총 감염자 수

    public int[] numberOfVaccinatedList = new int[3]; // 연령별 백신 접종자 수

    public double vaccinatedFatalityRate = 0.02; // 백신 맞았을 경우에 치명률

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
        startPop = 50000000; // 인구수 설정
        population = startPop;
        //// 인구 관련 설정 변수 ////


        double youngRate = 0.167; // 미성년자 초기 비율 (10대 이하)
        double adultRate = 0.587; // 청장년 초기 비율 (20대 ~ 50대)
        double oldRate = 1 - (youngRate + adultRate); // 노인 초기 비율 (50대 이상)

        int[] popList = new int[3]; // 각 연령대별 인구수 배열

        popList[0] = (int)(population * youngRate); // 미성년자 인구수 초기설정
        popList[1] = (int)(population * adultRate); // 청장년 인구수 초기설정
        popList[2] = (int)(population * oldRate); // 노인 인구수 초기설정

        double[] popRateList = new double[3]; // 인구 비율(계산용)
        for (int i = 0; i < 3; i++)
            popRateList[i] = (double)popList[i] / (double)population;

        double mu_c, sigma_c;
        mu_c = (double) numberOfContacts; // 하루 접촉자 수 평균
        sigma_c = mu_c / 5;

        //// 바이러스 관련 설정 변수 ////
        ///
        

        

        int[] dayDeathsList = new int[3]; // 연령별 사망자 수
        for (int i = 0; i < 3; i++)
            dayDeathsList[i] = 0;

        numberOfInfections = 14; // 초기 감염자 숫자

        int[] dayinfectsList = new int[3]; // 백신 미접종자에 대한 감염자 수
        for (int i = 0; i < 2; i++)
            dayinfectsList[i] = (int)(numberOfInfections * popRateList[i]);
        dayinfectsList[2] = numberOfInfections - (dayinfectsList[0] + dayinfectsList[1]); // 노인 하루 감염자 수

        int[] dayVaccinatedInfectsList = new int[3]; // 백신 접종자에 대한 감염자 수
        for (int i = 0; i < 3; i++)
            dayVaccinatedInfectsList[i] = 0;


        for (int i = 0; i < 3; i++)
            numberofInfectionsList[i] = dayinfectsList[i];

        int[] numberofVaccinatedInfectionsList = new int[3]; // 백신 접종자 인구별 총 감염자 수
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

        threatingDay = 14; // 치료 기간



        //// 백신 관련 설정 변수 ////



        numberOfVaccinatedList[0] = 0; // 미성년자 백신 접종자 수
        numberOfVaccinatedList[1] = 0; // 청장년 백신 접종자 수
        numberOfVaccinatedList[2] = 0; // 노인 백신 접종자 수

        numberOfVaccinated = 0;

        for (int i = 0; i < 3; i++)
            numberOfVaccinated += numberOfVaccinatedList[i]; // 총 백신 접종량

        int[] shotPerDayList = new int[3];





        //// 기록 변수들 ////

        day = 0; // 날짜 Counting
        int endDay = 3650; // 끝나는 일수
        int[] dayRecord = new int[endDay]; // 날짜 (그래프 X축)
        int[,] infectedRecord = new int[3, endDay]; // 연령대별 누적 감염자 수 기록
        int[,] vaccinatedRecord = new int[3, endDay]; // 연령대별 백신 접종자 수 기록
        int[,] deadRecord = new int[3, endDay]; // 연령대별 사망자 수 기록
        int[,] infectsPerDayList = new int[3, endDay]; // 날짜별 미접종자 감염자 수 기록 (치유 목적)
        int[,] vaccinatedinfectsPerDayList = new int[3, endDay]; // 날짜별 접종자 감염자 수 기록 (치유 목적)



        //// 생성 ////
        double[,] VaccineScheduleList;

        if (generation == 0)
            VaccineScheduleList = FirstVaccinatingCreate(endDay);
        else
            VaccineScheduleList = dayVaccinatingList;




        //// 실제 Simulation 시작 ////

        while (vaccinatedRate < 1 & numberOfInfections < population & day < endDay)
        {
            // 하루 일과 : 집계-> 감염 -> 사망 -> 백신 접종 -> 마감
            // Console.WriteLine("");
            // Console.WriteLine("날짜 : " + day);

            System.Random rand = new System.Random();



            //// 감염 단계 ////
            //numberOfContacts = (int)norm(mu_c, sigma_c); // 접촉자 수 선정
            //numberOfContacts = (int)(numberOfContacts * (1 - Math.Pow(infectionsRate, 0.05)) * Math.Pow(Math.Max(1, day - 1500), 0.01) + 3);

            // print(numberOfContacts, infectionsRate, numberOfInfections)
            // 코로나가 심해질수록 밖에 안나감, 3명은 무조건 만남
            // 1500일 이후 위드코로나 진행 (만나는 사람 증가로 인한 2차 팬데믹 유도)

            // Console.Write("감염자 : ");
            for (int i = 0; i < 3; i++)
            {
                dayinfectsList[i] = (int)((numberOfInfections * (double)numberOfContacts * transmissionRate * (1 - vaccinatedRate) * (1 - infectionsRate)) * popRateList[i]);
                dayinfectsList[i] += (int)((double)rand.Next(20, 31) * popRateList[i] * (1 - vaccinatedRate));
                dayVaccinatedInfectsList[i] = (int)((numberOfInfections * (double)numberOfContacts * transmissionRate * vaccinatedTransRate * vaccinatedRate * (1 - infectionsRate)) * popRateList[i]);
                dayVaccinatedInfectsList[i] += (int)(rand.Next(20, 31) * popRateList[i] * vaccinatedRate);

                // 감염자로 인한 전파 + 외부 유입 (20~30)
                numberofInfectionsList[i] += dayinfectsList[i]; // 연령별 총 감염자수 추가
                numberofVaccinatedInfectionsList[i] += dayVaccinatedInfectsList[i];

                // Console.Write(numberofInfectionsList[i] + " ");
                // Console.Write(numberofVaccinatedInfectionsList[i] + " ");
            }
            // Console.WriteLine("");



            //// 사망 단계 ////
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



            //// 백신 접종 단계 ////
            //preventionRate = rand.Next(850, 950) / (double)1000;
            

            int reallyshot;
            // Console.Write("백신 접종량 : ");
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

                // 확진자가 많아질수록 백신 희망자가 늘어남
                // 백신 접종률이 높아질수록 안이한 생각에 맞지 않으려 함
                // 0.01%의 국민은 무조건 하루에 백신을 맞추려고 함.
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


            //// 치료 단계 ////
            if (day > threatingDay)
            {
                int totalInfected = 0;
                // Console.Write("치료 될 감염자 : ");
                for (int i = 0; i < 3; i++)
                {
                    totalInfected += numberofInfectionsList[i] + numberofVaccinatedInfectionsList[i];
                    // Console.Write(infectsPerDayList[i, day - threatingDay] + " ");
                    // Console.Write(vaccinatedinfectsPerDayList[i, day - threatingDay] + " ");
                }
                // Console.WriteLine("");
                // Console.Write("치료 후 총감염자 : ");
                if (totalInfected > 100)
                    for (int i = 0; i < 3; i++) // 연령별 감염자 치료날짜 이후 비감염자 됨
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


            //// 집계 단계 ////

            population = popList[0] + popList[1] + popList[2]; // 인구 집계
                                                               // Console.Write("인구수 : ");
            for (int i = 0; i < 3; i++)
            {
                // Console.Write("{0} ", popList[i]);
                popRateList[i] = popList[i] / (double)population;
            }
            // Console.WriteLine(" ");

            dayRecord[day] = day; // 날짜 기록

            for (int i = 0; i < 3; i++)
            {
                infectsPerDayList[i, day] = dayinfectsList[i]; // 하루 미접종자 감염자 수 기록
                vaccinatedinfectsPerDayList[i, day] = dayVaccinatedInfectsList[i]; // 하루 접종자 감염자 수 기록
                infectedRecord[i, day] = numberofInfectionsList[i] + numberofVaccinatedInfectionsList[i]; // 총 감염자 수 기록
                deadRecord[i, day] = startPop - popList[i]; // 사망자 기록
            }


            for (int i = 0; i < 3; i++)
            {
                numberOfVaccinated += numberOfVaccinatedList[i]; // 총 백신 접종량 집계
                vaccinatedRecord[i, day] = numberOfVaccinatedList[i]; // 백신 접종자 수 기록
            }

            numberOfInfections = 0;
            numberOfVaccinated = 0;
            for (int i = 0; i < 3; i++)
            {
                numberOfInfections += numberofInfectionsList[i] + numberofVaccinatedInfectionsList[i]; // 총 감염자 수 불러오기
                numberOfVaccinated += numberOfVaccinatedList[i]; // 총 백신 접종량 불러오기
            }

            infectionsRate = (numberOfInfections / (double)population);
            vaccinatedRate = (numberOfVaccinated / (double)population);


            day++; // 하루 일과 마침
            Debug.Log("백신 접종 : {0}" + numberOfVaccinated);
            Debug.Log("최종 감염자 : {0}" + numberOfInfections);
            Debug.Log("생존자: {0}" + population);
        }
        Debug.Log("최종 날짜 : {0}" + day);
        Debug.Log("최종 감염자 : {0}" + numberOfInfections);
        Debug.Log("백신 접종 : {0}" + numberOfVaccinated);
        Debug.Log("생존자: {0}" + population);
        setCircleMakerVariableLevel2();

        return population;
    }

    public static double[,] FirstVaccinatingCreate(int endDay)
    {
        // GA의 기본적인 기본 //

        double[,] dayVaccinatingList = new double[3, endDay]; // 날짜별 백신 접종 비율 리스트

        for (int i = 0; i < endDay; i++) // 날짜별 백신 접종 비율 배열 생성
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

            ///하루 일과 : 감염 -> 사망 -> 백신 접종 -> 집계


            System.Random rand = new System.Random();

            dayList[day] = day;
            infectedList[day] = numberOfInfections;
            vaccinatedList[day] = numberOfVaccinated;
            deadList[day] = 50000000 - population;

            numberOfContacts = (int)norm(mu_c, sigma_c);
            numberOfContacts = (int)(numberOfContacts * (1 - Math.Pow(infectionsRate, 0.1)) * Math.Pow(Math.Max(1, day - 1500), 0.2) + 3);



            /// print(numberOfContacts, infectionsRate, numberOfInfections)
            /// 코로나가 심해질수록 밖에 안나감, 3명은 무조건 만남
            /// 1500 이후 위드코로나 진행(만나는 사람 증가)으로 2차 팬데믹 유도

            preventionRate = norm(0.9, 0.05);
            shotPerDay = (int)(norm(mu_s, sigma_s));

            dayInfects = (int)(numberOfInfections * numberOfContacts * transmissionRate * (1 - vaccinatedRate) + rand.Next(20, 31));
            infectsPerDayList[day] = dayInfects;

            numberOfInfections += dayInfects;

            /// 감염자로 인한 전파 + 외부 유입 (20~30)

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
            /// 확진자가 많아질수록 백신 희망자가 늘어남
            /// 백신 접종률이 높아질수록 안이한 생각에 맞지 않으려 함
            /// 0.01%의 국민은 무조건 하루에 백신을 맞추려고 함.

            numberOfInfections -= (int)(numberOfInfections * (shotPerDay / (population - numberOfVaccinated)));
            /// 감염자 중 백신 접종 시 비감염자 됨
            /// Console.WriteLine(day +" : "+ numberOfVaccinated);

            day++;

            /// Console.WriteLine("{0}", numberOfInfections);
        }

        Debug.Log("최종 감염자 : " + numberOfInfections);
        Debug.Log("백신 접종 : " + numberOfVaccinated);
        Debug.Log("생존자 : " + population);
        setCircleMakerVariableLevel1();
    }

    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();

        fatalityRateList[0] = 0.0001; // 미성년자 치명률
        fatalityRateList[1] = 0.00104; // 청장년 치명률
        fatalityRateList[2] = 0.034796; // 노인 치명률
        preventionRate = rand.Next(850, 950) / (double)1000;
        double mu_s, sigma_s;
        mu_s = 400000; // 백신 접종량 평균
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

            ///하루 일과 : 감염 -> 사망 -> 백신 접종 -> 집계

            dayList[day] = day;
            infectedList[day] = numberOfInfections;
            vaccinatedList[day] = numberOfVaccinated;
            deadList[day] = 50000000 - population;

            numberOfContacts = (int)norm(mu_c, sigma_c);
            numberOfContacts = (int)(numberOfContacts * (1 - Mathf.Pow(infectionsRate, 0.1f)) * Mathf.Pow(Mathf.Max(1, day - 1500), 0.2f) + 3);



            /// print(numberOfContacts, infectionsRate, numberOfInfections)
            /// 코로나가 심해질수록 밖에 안나감, 3명은 무조건 만남
            /// 1500 이후 위드코로나 진행(만나는 사람 증가)으로 2차 팬데믹 유도

            preventionRate = norm(0.9f, 0.05f);
            shotPerDay = (int)(norm(mu_s, sigma_s));

            dayInfects = (int)(numberOfInfections * numberOfContacts * transmissionRate * (1 - vaccinatedRate) + Random.Range(20, 31)); //20,31
            infectsPerDayList[day] = dayInfects;

            numberOfInfections += dayInfects;

            /// 감염자로 인한 전파 + 외부 유입 (20~30)

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
            /// 확진자가 많아질수록 백신 희망자가 늘어남
            /// 백신 접종률이 높아질수록 안이한 생각에 맞지 않으려 함
            /// 0.01%의 국민은 무조건 하루에 백신을 맞추려고 함.

            numberOfInfections -= (int)(numberOfInfections * (shotPerDay / (population - numberOfVaccinated)));
            /// 감염자 중 백신 접종 시 비감염자 됨
            /// Console.WriteLine(day +" : "+ numberOfVaccinated);

            day++;

            /// Console.WriteLine("{0}", numberOfInfections);
        }

        Debug.Log("최종 감염자 : {0}" + numberOfInfections);
        Debug.Log("백신 접종 : {0}" + numberOfVaccinated);
        Debug.Log("생존자: {0}" + population);
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