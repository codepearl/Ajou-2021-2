using System;

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

            for (int i = 0; i < 3650; i++) // 날짜별 백신 접종 비율 배열 생성
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

            double vaccinatedFatalityRate = 1 - 0.98; // 백신 맞았을 경우에 치명률

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

            while (vaccinatedRate < 1 && numberOfInfections < population && day < endDay)
            {
                // 하루 일과 : 집계-> 감염 -> 사망 -> 백신 접종 -> 마감

                Random rand = new Random();

                numberOfInfections = 0;
                numberOfVaccinated = 0;
                for (int i = 0; i < 3; i++)
                {
                    numberOfInfections += numberofInfectionsList[i] + numberofVaccinatedInfectionsList[i]; // 총 감염자 수 불러오기
                    numberOfVaccinated += numberOfVaccinatedList[i]; // 총 백신 접종량 불러오기
                }

                infectionsRate = (numberOfInfections / (double)population);
                vaccinatedRate = (numberOfVaccinated / (double)population);



                //// 감염 단계 ////
                numberOfContacts = (int)norm(mu_c, sigma_c); // 접촉자 수 선정
                numberOfContacts = (int)(numberOfContacts * (1 - Math.Pow(infectionsRate, 0.1)) * Math.Pow(Math.Max(1, day - 1500), 0.2) + 3);
                // print(numberOfContacts, infectionsRate, numberOfInfections)
                // 코로나가 심해질수록 밖에 안나감, 3명은 무조건 만남
                // 1500일 이후 위드코로나 진행 (만나는 사람 증가로 인한 2차 팬데믹 유도)

                for (int i = 0; i < 3; i++)
                {
                    dayinfectsList[i] = (int)((numberOfInfections * numberOfContacts * transmissionRate * (1 - vaccinatedRate)) * popRateList[i]);
                    dayinfectsList[i] += (int)(rand.Next(20, 31) * popRateList[i] * (1 - vaccinatedRate));
                    dayVaccinatedInfectsList[i] = (int)((numberOfInfections * numberOfContacts * transmissionRate * vaccinatedTransRate * vaccinatedRate) * popRateList[i]);
                    dayVaccinatedInfectsList[i] += (int)(rand.Next(20, 31) * popRateList[i] * vaccinatedRate);

                    // 감염자로 인한 전파 + 외부 유입 (20~30)
                    numberofInfectionsList[i] += dayinfectsList[i]; // 연령별 총 감염자수 추가
                    numberofVaccinatedInfectionsList[i] += dayVaccinatedInfectsList[i];
                }



                //// 사망 단계 ////
                numberOfInfections = 0;
                for (int i = 0; i < 3; i++)
                {
                    numberOfInfections += numberofInfectionsList[i] + numberofVaccinatedInfectionsList[i]; // 감염 단계 후 총 감염자 수 다시 불러오기
                    dayDeathsList[i] = (int)(numberofInfectionsList[i] * fatalityRateList[i]);
                    popList[i] -= dayDeathsList[i];
                    numberofInfectionsList[i] -= dayDeathsList[i];

                    dayDeathsList[i] = (int)(numberofVaccinatedInfectionsList[i] * fatalityRateList[i] * vaccinatedFatalityRate);
                    popList[i] -= dayDeathsList[i];
                    numberofInfectionsList[i] -= numberofVaccinatedInfectionsList[i];

                }







                //// 백신 접종 단계 ////
                preventionRate = norm(0.9, 0.05);
                shotPerDay = (int)(norm(mu_s, sigma_s));

                for (int i = 0; i < 3; i++)
                {
                    shotPerDayList[i] = (int)(shotPerDay * VaccineScheduleList[i, day]);
                    numberOfVaccinatedList[i] += (int)(shotPerDayList[i] * preventionRate * (1 - vaccinatedRate) * Math.Max(infectionsRate, 0.01));
                    // 확진자가 많아질수록 백신 희망자가 늘어남
                    // 백신 접종률이 높아질수록 안이한 생각에 맞지 않으려 함
                    // 0.01%의 국민은 무조건 하루에 백신을 맞추려고 함.
                    int changingnumber = (int)(numberOfInfections * (shotPerDayList[i] / (popList[i] - numberOfVaccinatedList[i])));
                    numberofInfectionsList[i] -= changingnumber;
                    numberofVaccinatedInfectionsList[i] += changingnumber;
                }



                //// 집계 단계 ////

                population = popList[0] + popList[1] + popList[2]; // 인구 집계

                for (int i = 0; i < 3; i++)
                    popRateList[i] = (double)popList[i] / (double)population;

                dayRecord[day] = day; // 날짜 기록

                for (int i = 0; i < 3; i++)
                {
                    infectsPerDayList[i, day] = dayinfectsList[i]; // 하루 미접종자 감염자 수 기록
                    vaccinatedinfectsPerDayList[i, day] = dayVaccinatedInfectsList[i]; // 하루 접종자 감염자 수 기록
                    infectedRecord[i, day] = numberofInfectionsList[i]; // 총 감염자 수 기록

                    deadRecord[i, day] = startPop - popList[i]; // 사망자 기록
                }


                for (int i = 0; i < 3; i++)
                {
                    numberOfVaccinated += numberOfVaccinatedList[i]; // 총 백신 접종량 집계
                    vaccinatedRecord[i, day] = numberOfVaccinatedList[i]; // 백신 접종자 수 기록

                    popRateList[i] = popList[i] / (double)population; // 인구 비율 집계
                }

                day++; // 하루 일과 마침

                Console.WriteLine("{0} : {1}", day, population);

            }
            Console.WriteLine("최종 날짜 : {0}", day);
            Console.WriteLine("최종 감염자 : {0}", numberOfInfections);
            Console.WriteLine("백신 접종 : {0}", numberOfVaccinated);
            Console.WriteLine("생존자: {0}", population);

            return population;
        }

        static void Main(string[] args)
        {
            FitnessCheck(0, null);
        }
    }

}
