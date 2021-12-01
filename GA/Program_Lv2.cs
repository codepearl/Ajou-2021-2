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

        static void Main(string[] args)
        {
            int population = 50000000;
            double youngRate = 0.167; // 영유아 비율 (10대 이하)
            double adultRate = 0.587; // 청장년 비율 (20대 ~ 50대)
            double oldRate = 1 - (youngRate + adultRate); // 노인 비율 (50대 이상)

            int youngPop = (int)(population * youngRate);
            int adultPop = (int)(population * adultRate);
            int oldPop = population - (youngPop + adultPop);

            double transmissionRate = 0.0089;
            double youngFatalityRate = 0.0001;      // 영유아 치명률
            double adultFatalityRate = 0.00104;     // 청장년 치명률
            double oldFatalityRate = 0.034796;      // 노인 치명률

            double vaccinatedRate = 0;
            double infectionsRate = 0;

            int youngdayInfects = 0;                // 영유아 하루 감염자 수
            int adultdayInfects = 0;                // 청장년 하루 감염자 수
            int olddayInfects = 0;                  // 노인 하루 감염자 수

            int numberOfVaccinated = 0;             // 백신 접종자 수
            int numberOfDeaths = 0;                 // 사망자 수
            int numberOfInfections = 14;            // 초기 감염자 숫자

            int day = 0;
            int endDay = 3650;                      // 끝나는 일수
            int[] dayList = new int[endDay];
            int[] infectedList = new int[endDay];
            int[] vaccinatedList = new int[endDay];
            int[] deadList = new int[endDay];
            int[] infectsPerDayList = new int[endDay];
            int threatingDay = 14;

            int numberOfContacts;

            int shotPerDay;
            double preventionRate;

            double mu_c, sigma_c, mu_s, sigma_s;

            mu_c = 30.0;
            sigma_c = 5.0;
            mu_s = 400000;
            sigma_s = 50000;

            double[,] dayVaccinatedList = new double[3, endDay]; // 날짜별 백신 접종 비율 리스트

            for (int i = 0; i < 3650; i++) // 날짜별 백신 접종 비율 배열 생성
            {
                Random rand = new Random();
                double youngVacRate = rand.Next(1000)/1000;
                double adultVacRate = rand.Next(1000 - (int)(youngVacRate * 1000)) / 1000;
                double oldVacRate = 1 - (youngVacRate + adultRate);

                dayVaccinatedList[0, i] = youngVacRate;
                dayVaccinatedList[1, i] = adultVacRate;
                dayVaccinatedList[2, i] = oldVacRate;
            }

            while (vaccinatedRate < 1 && numberOfInfections < population && day < endDay)
            {
                ///하루 일과 : 감염 -> 사망 -> 백신 접종 -> 집계

                Random rand = new Random();

                population = youngPop + adultPop + oldPop;
                youngRate = (double)(youngPop / population);
                adultRate = (double)(adultPop / population);
                oldRate = (double)(oldPop / population);

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

                youngdayInfects = (int)(numberOfInfections * numberOfContacts * transmissionRate * (1 - vaccinatedRate) + rand.Next(20, 31)) * youngRate;
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






            Console.WriteLine("최종 감염자 : {0}", numberOfInfections);
            Console.WriteLine("백신 접종 : {0}", numberOfVaccinated);
            Console.WriteLine("생존자: {0}", population);
        }
    }

}
