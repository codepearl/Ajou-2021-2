using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace 알고리즘
{
    class Program
    {
        public class dayVaccinating
        {
            public double[] howOldAreYou = new double[3];

            public dayVaccinating()
            {
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

        public static wrapper[] selection(wrapper[] w, int select) // 서재은 작품
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

        public static double norm(double mean, double stdDev)
        {
            Random rand = new Random(); //reuse this if you are generating many
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random public doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                         mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
            return randNormal;
        }

        public static dayVaccinatingList FirstVaccinatingCreate(int endDay)
        {
            // GA의 기본적인 기본 //
            ... (532줄 남음)
접기
Program_Lv2_local.cs
28KB


using System;
            using System.Diagnostics;
            using System.Threading;
            using System.Threading.Tasks;

namespace 알고리즘
    {
        class Program
        {
            public class dayVaccinating
            {
                public double[] howOldAreYou = new double[3];

                public dayVaccinating()
                {
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

            public static wrapper[] selection(wrapper[] w, int select) // 서재은 작품
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

            public static double norm(double mean, double stdDev)
            {
                Random rand = new Random(); //reuse this if you are generating many
                double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random public doubles
                double u2 = 1.0 - rand.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                             Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                double randNormal =
                             mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
                return randNormal;
            }

            public static dayVaccinatingList FirstVaccinatingCreate(int endDay)
            {
                // GA의 기본적인 기본 //



                dayVaccinatingList VaccineScheduleList = new dayVaccinatingList(); // 날짜별 백신 접종 비율 리스트

                for (int i = 0; i < endDay; i++) // 날짜별 백신 접종 비율 배열 생성
                {
                    if (i < 150)
                    {
                        VaccineScheduleList.a[i].howOldAreYou[0] = 0;
                        VaccineScheduleList.a[i].howOldAreYou[1] = 0;
                        VaccineScheduleList.a[i].howOldAreYou[2] = 0;
                    }
                    else
                    {
                        Random rand = new Random();
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

            public static dayVaccinatingList[] Mutation(wrapper[] topTeer, int endDay)
            {
                dayVaccinatingList[] childrenList;
                childrenList = new dayVaccinatingList[topTeer.Length];

                for (int i = 0; i < childrenList.Length; i++)
                {
                    childrenList[i] = new dayVaccinatingList();

                    for (int day = 0; day < endDay; day++)
                    {
                        childrenList[i].a[day].howOldAreYou[0] = topTeer[i].d.a[day].howOldAreYou[1];           // 첫 번째(0번째) 날의 청장년 배분률을 미성년 배분률로
                        childrenList[i].a[day].howOldAreYou[1] = topTeer[i].d.a[day].howOldAreYou[2];           // 첫 번째(0번째) 날의 노인 배분률을 청장년 배분률로
                        childrenList[i].a[day].howOldAreYou[2] = topTeer[i].d.a[day].howOldAreYou[0];           // 첫 번째(0번째) 날의 미성년 배분률을 노인 배분률로
                    }

                }
                return childrenList;
            }

            public static dayVaccinatingList[] CrossOver(wrapper[] topTeer, int endDay)
            {
                dayVaccinatingList[] childrenList;            //crossover를 통해 만들어낼 24개의 자식 개체
                childrenList = new dayVaccinatingList[24];

                for (int i = 0; i < 24; i++)
                    childrenList[i] = new dayVaccinatingList();
                int o = 0;          // dayVaccinatingList를 0부터 23까지 넣기 위한 index
                while (o < 24)
                {
                    for (int j = 0; j < 4; j++)         // 부친 index
                    {
                        for (int k = 0; k < j; k++)         // 모친 index
                        {
                            if (j != k)         // 부친과 모친은 다른 사람이어야 함
                            {
                                int m = 0;          // dayVaccinating을 0부터 endDay-1까지 넣기 위한 index
                                while (m < endDay)
                                {
                                    childrenList[o].a[m] = topTeer[j].d.a[m];         // 아들은 부친을 3만큼 닮음
                                    childrenList[o].a[m + 1] = topTeer[j].d.a[m + 1];
                                    childrenList[o].a[m + 2] = topTeer[j].d.a[m + 2];
                                    childrenList[o].a[m + 3] = topTeer[k].d.a[m + 3];         // 아들은 모친을 2만큼 닮음
                                    childrenList[o].a[m + 4] = topTeer[k].d.a[m + 4];

                                    childrenList[o + 1].a[m] = topTeer[k].d.a[m];         // 딸은 모친을 3만큼 닮음
                                    childrenList[o + 1].a[m + 1] = topTeer[k].d.a[m + 1];
                                    childrenList[o + 1].a[m + 2] = topTeer[k].d.a[m + 2];
                                    childrenList[o + 1].a[m + 3] = topTeer[j].d.a[m + 3];         // 딸은 부친을 2만큼 닮음
                                    childrenList[o + 1].a[m + 4] = topTeer[j].d.a[m + 4];

                                    m = m + 5;
                                }
                                o = o + 2;
                            }
                        }
                    }
                }
                return childrenList;            // 24개 자식 개체들 반환
            }



            public static int FitnessCheck(dayVaccinatingList d)
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
                mu_c = 100.0; // 하루 접촉자 수 평균
                sigma_c = mu_c / 5;




                //// 바이러스 관련 설정 변수 ////

                double transmissionRate = 0.0089; // 감염률
                double vaccinatedTransRate = 0.2; // 백신 맞았을 경우에 감염률

                double[] fatalityRateList = new double[3]; // 각 인구별 치명률 배열

                fatalityRateList[0] = 0.0001; // 미성년자 치명률 0.0001
                fatalityRateList[1] = 0.00104; // 청장년 치명률 0.00104
                fatalityRateList[2] = 0.034796; // 노인 치명률 0.034796

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

                double vaccineCost = 73.375;
                double mu_s, sigma_s;

                mu_s = popList[1] * vaccineCost; // 백신 접종량 평균
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







                //// 실제 Simulation 시작 ////

                while (vaccinatedRate < 1 & numberOfInfections < population & day < endDay)
                {
                    // 하루 일과 : 집계-> 감염 -> 사망 -> 백신 접종 -> 마감
                    // Console.WriteLine("");
                    // Console.WriteLine("날짜 : " + day);

                    Random rand = new Random();

                    mu_s = (popList[1] - numberofInfectionsList[1] - numberofVaccinatedInfectionsList[1]) * vaccineCost; // 백신 접종량 평균
                    sigma_s = mu_s / 8;

                    //// 감염 단계 ////
                    numberOfContacts = (int)norm(mu_c, sigma_c); // 접촉자 수 선정
                    numberOfContacts = (int)(numberOfContacts * (1 - Math.Pow(infectionsRate, 0.05)) * Math.Pow(Math.Max(1, day - 1500), 0.01) + 3);

                    // prpublic int(numberOfContacts, infectionsRate, numberOfInfections)
                    // 코로나가 심해질수록 밖에 안나감, 3명은 무조건 만남
                    // 1500일 이후 위드코로나 진행 (만나는 사람 증가로 인한 2차 팬데믹 유도)

                    // Console.Write("감염자 : ");
                    for (int i = 0; i < 3; i++)
                    {
                        dayinfectsList[i] = (int)((numberOfInfections * (double)numberOfContacts * transmissionRate * (1 - vaccinatedRate) * (1 - infectionsRate)) * popRateList[i]);
                        dayinfectsList[i] += (int)((double)rand.Next(20, 31) * popRateList[i] * (1 - vaccinatedRate));
                        dayinfectsList[i] = Math.Abs(dayinfectsList[i]);
                        dayVaccinatedInfectsList[i] = (int)((numberOfInfections * (double)numberOfContacts * transmissionRate * vaccinatedTransRate * vaccinatedRate * (1 - infectionsRate)) * popRateList[i]);
                        dayVaccinatedInfectsList[i] += (int)(rand.Next(20, 31) * popRateList[i] * vaccinatedRate);
                        dayVaccinatedInfectsList[i] = Math.Abs(dayVaccinatedInfectsList[i]);

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

                        // 확진자가 많아질수록 백신 희망자가 늘어남
                        // 백신 접종률이 높아질수록 안이한 생각에 맞지 않으려 함
                        // 0.01%의 국민은 무조건 하루에 백신을 맞추려고 함.
                        int changingnumber = 0;
                        if ((popList[i] - numberOfVaccinatedList[i]) > 0)
                            changingnumber = (int)(numberofInfectionsList[i] / (double)(popList[i] - numberOfVaccinatedList[i]) * reallyshot);

                        if (changingnumber > numberofInfectionsList[i])
                            changingnumber = numberofInfectionsList[i];

                        numberofInfectionsList[i] -= changingnumber;
                        numberofVaccinatedInfectionsList[i] += changingnumber;
                        int dayCount = 1;

                        for (int j = changingnumber; j < 0; j = j - rand.Next(0, Math.Max(0, (int)j)))
                        {

                            if (dayCount == threatingDay - 1 || dayCount == day)
                            {
                                dayCount = 0;
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

                int endDay = 3650;
                int numberOfSelect = 4;
                int numberOfMutation = 1;
                int targetGeneration = 30;

                wrapper[] children;
                children = new wrapper[28];

                dayVaccinatingList[] childrenByCrossOver;
                dayVaccinatingList[] childrenByMutation;

                wrapper[] topteer;
                wrapper[] saving;

                saving = new wrapper[numberOfSelect * targetGeneration];

                for (int generation = 0; generation < targetGeneration; generation++)
                {
                    Console.WriteLine("세대수 : {0}", generation);
                    Parallel.For(0, children.Length, i =>
                    {
                        if (generation == 0)
                        {
                            children[i] = new wrapper();
                            dayVaccinatingList Solution;
                            Solution = FirstVaccinatingCreate(endDay);

                            children[i].d = Solution;
                            children[i].population = FitnessCheck(Solution);
                        }
                        else
                            children[i].population = FitnessCheck(children[i].d);
                    });

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

                wrapper[] winner;

                winner = selection(children, 3);

                Console.WriteLine("1위 생존자 수 : {0}", winner[0].population);
                Console.WriteLine("1위 사망자 수 : {0}", (50000000 - winner[0].population));
                Console.WriteLine("2위 생존자 수 : {0}", winner[1].population);
                Console.WriteLine("2위 사망자 수 : {0}", (50000000 - winner[1].population));
                Console.WriteLine("3위 생존자 수 : {0}", winner[2].population);
                Console.WriteLine("3위 사망자 수 : {0}", (50000000 - winner[2].population));


                sw.Stop();

                long elapsedMilliseconds = sw.ElapsedMilliseconds;

                // millisecond -> second
                double elapsedSecond = (elapsedMilliseconds / 1000.0);
                Console.WriteLine("구동시간 : " + elapsedSecond + "초");
            }
        }
    }