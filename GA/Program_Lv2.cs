using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace 알고리즘
{
    class Program
    {
        public static double norm(double mean, double stdDev)
        {
            Random rand = new Random(); //reuse this if you are generating many
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                         mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
            return randNormal;
        }

        public static double[,] FirstVaccinatingCreate(int endDay)
        {
            // GA의 기본적인 기본 //

            double[,] dayVaccinatingList = new double[3, endDay]; // 날짜별 백신 접종 비율 리스트

            for (int i = 0; i < endDay; i++) // 날짜별 백신 접종 비율 배열 생성
            {
                Random rand = new Random();
                double youngVacRate = rand.Next(1000) / (double)1000;
                double adultVacRate = rand.Next(1000 - (int)(youngVacRate * 1000)) / (double)1000;
                double oldVacRate = 1 - (youngVacRate + adultVacRate);

                dayVaccinatingList[0, i] = youngVacRate;
                dayVaccinatingList[1, i] = adultVacRate;
                dayVaccinatingList[2, i] = oldVacRate;
            }

            return dayVaccinatingList;
        }



        public static double[,] CrossOver(double[,] dayVaccinatingList, int endDay)
        {
            // dayVaccinatingList를 4개로 복제하는 임시 코드
            double[] dayVaccinatingLists = new int[4];
            for (int i = 0; i < 4; i++)
            {
                dayVaccinatingLists[i] = dayVaccinatingList;
            }


            double[] childrenList = new int[24];            //crossover를 통해 만들어낼 24개의 자식 개체
            int n = 0;          // childrenList에 sonList, daughterList를 넣기 위한 index

            
            for (int j = 0; j < 4; j++)         // 4개 list 조합하여 cross over
            {
                for (int k = 0; k < 4; k++)
                    if (j != k)         // 자신을 제외한 나머지 요소와 매칭
                    {
                        double[,] sonList = new double[3, endDay];          // 첫 번째 자식 리스트 생성
                        double[,] daughterList = new double[3, endDay];          // 두 번째 자식 리스트 생성

                        int m = 0;
                        
                        while (m < endDay)          // 아들은 아빠의 3, 엄마의 2 / 딸은 엄마의 3, 아빠의 2를 닮음 (임시 crossover방식, 버그 없이 구현되면 변경 예정)
                        {
                            sonList[m] = dayVaccinatingLists[j, m];
                            sonList[m + 1] = dayVaccinatingLists[j, m + 1];
                            sonList[m + 2] = dayVaccinatingLists[j, m + 2];
                            sonList[m + 3] = dayVaccinatingLists[k, m + 3];
                            sonList[m + 4] = dayVaccinatingLists[k, m + 4];

                            daughterList[m] = dayVaccinatingLists[k, m];
                            daughterList[m + 1] = dayVaccinatingLists[k, m + 1];
                            daughterList[m + 2] = dayVaccinatingLists[k, m + 2];
                            daughterList[m + 3] = dayVaccinatingLists[j, m + 3];
                            daughterList[m + 4] = dayVaccinatingLists[j, m + 4];

                            m = m + 5;
                        }

                        childrenList[n] = sonList;          //자식 개체 append
                        childrenList[n + 1] = daughterList;         //자식 개체 append
                        n = n + 2;                        
                    }
                    
            }

            return childrenList;            //자식 개체들 반환
        }

        public static int FitnessCheck(int generation, double[,] dayVaccinatingList)
        {

            //// 인구 관련 설정 변수 ////

            int startPop = 50000000; // 인구수 설정
            int population = startPop;
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

            int numberOfContacts; // 하루 접촉자 수
            double mu_c, sigma_c;
            mu_c = 30.0; // 하루 접촉자 수 평균
            sigma_c = mu_c / 5;

            //// 바이러스 관련 설정 변수 ////

            double transmissionRate = 0.0089; // 감염률
            double vaccinatedTransRate = 0.2; // 백신 맞았을 경우에 감염률

            double[] fatalityRateList = new double[3]; // 각 인구별 치명률 배열

            fatalityRateList[0] = 0.0001; // 미성년자 치명률
            fatalityRateList[1] = 0.00104; // 청장년 치명률
            fatalityRateList[2] = 0.034796; // 노인 치명률

            double vaccinatedFatalityRate = 0.02; // 백신 맞았을 경우에 치명률

            double vaccinatedRate = 0; // 전체 인구 중 접종자 비율
            double infectionsRate = 0; // 전체 인구 중 감염자 비율

            int numberOfDeaths = 0; // 사망자 수
            int[] dayDeathsList = new int[3]; // 연령별 사망자 수
            for (int i = 0; i < 3; i++)
                dayDeathsList[i] = 0;

            int numberOfInfections = 14; // 초기 감염자 숫자

            int[] dayinfectsList = new int[3]; // 백신 미접종자에 대한 감염자 수
            for (int i = 0; i < 2; i++)
                dayinfectsList[i] = (int)(numberOfInfections * popRateList[i]);
            dayinfectsList[2] = numberOfInfections - (dayinfectsList[0] + dayinfectsList[1]); // 노인 하루 감염자 수

            int[] dayVaccinatedInfectsList = new int[3]; // 백신 접종자에 대한 감염자 수
            for (int i = 0; i < 3; i++)
                dayVaccinatedInfectsList[i] = 0;

            int[] numberofInfectionsList = new int[3]; // 백신 미접종자 인구별 총 감염자 수
            for (int i = 0; i < 3; i++)
                numberofInfectionsList[i] = dayinfectsList[i];

            int[] numberofVaccinatedInfectionsList = new int[3]; // 백신 접종자 인구별 총 감염자 수
            for (int i = 0; i < 3; i++)
                numberofVaccinatedInfectionsList[i] = dayVaccinatedInfectsList[i];


            int threatingDay = 14; // 치료 기간



            //// 백신 관련 설정 변수 ////

            int[] numberOfVaccinatedList = new int[3]; // 연령별 백신 접종자 수

            numberOfVaccinatedList[0] = 0; // 미성년자 백신 접종자 수
            numberOfVaccinatedList[1] = 0; // 청장년 백신 접종자 수
            numberOfVaccinatedList[2] = 0; // 노인 백신 접종자 수

            int numberOfVaccinated = 0;

            for (int i = 0; i < 3; i++)
                numberOfVaccinated += numberOfVaccinatedList[i]; // 총 백신 접종량

            int shotPerDay; // 하루 총 백신 접종량
            int[] shotPerDayList = new int[3];
            double preventionRate; // 항체 생성률

            double mu_s, sigma_s;
            mu_s = 400000; // 백신 접종량 평균
            sigma_s = mu_s / 8;



            //// 기록 변수들 ////

            int day = 0; // 날짜 Counting
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

                Random rand = new Random();



                //// 감염 단계 ////
                numberOfContacts = (int)norm(mu_c, sigma_c); // 접촉자 수 선정
                numberOfContacts = (int)(numberOfContacts * (1 - Math.Pow(infectionsRate, 0.05)) * Math.Pow(Math.Max(1, day - 1500), 0.01) + 3);

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
                preventionRate = rand.Next(850, 950) / (double)1000;
                shotPerDay = (int)(norm(mu_s, sigma_s));

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
                // Console.WriteLine("백신 접종 : {0}, {1}", numberOfVaccinated, vaccinatedRate);
                // Console.WriteLine("최종 감염자 : {0}, {1}", numberOfInfections,infectionsRate);
                // Console.WriteLine("생존자: {0}", population);
            }
            //Console.WriteLine("최종 날짜 : {0}", day);
            //Console.WriteLine("백신 접종 : {0}, {1}", numberOfVaccinated, vaccinatedRate);
            //Console.WriteLine("최종 감염자 : {0}, {1}", numberOfInfections, infectionsRate);
            Console.WriteLine("생존자: {0}", population);

            return population;
        }

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int numberOfSon = 32;

            Parallel.For(0, numberOfSon, i =>
            {
                for (int j = 0; j < 10; j++)
                    FitnessCheck(0, null);
            });


            sw.Stop();

            long elapsedMilliseconds = sw.ElapsedMilliseconds;

            // millisecond -> second
            double elapsedSecond = (elapsedMilliseconds / 1000.0);
            Console.WriteLine("구동시간 : " + elapsedSecond + "초");
        }
    }

}
}
