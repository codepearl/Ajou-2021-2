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

            double transmissionRate = 0.0089;
            double fatalityRate = 0.000201;
            double vaccinatedRate = 0;
            double infectionsRate = 0;

            int dayInfects;
            int numberOfVaccinated = 0;
            int numberOfDeaths = 0;
            int numberOfInfections = 14;

            int day = 0;
            int[] dayList = new int[3650];
            int[] infectedList = new int[3650];
            int[] vaccinatedList = new int[3650];
            int[] deadList = new int[3650];
            int[] infectsPerDayList = new int[3650];
            int threatingDay = 14;

            int numberOfContacts;

            int shotPerDay;
            double preventionRate;

            double mu_c, sigma_c, mu_s, sigma_s;

            mu_c = 30.0;
            sigma_c = 5.0;
            mu_s = 400000;
            sigma_s = 50000;

            while (vaccinatedRate < 1 && numberOfInfections < population && day < 3650)
            {

                ///하루 일과 : 감염 -> 사망 -> 백신 접종 -> 집계

                Random rand = new Random();

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

            Console.WriteLine("최종 감염자 : {0}", numberOfInfections);
            Console.WriteLine("백신 접종 : {0}", numberOfVaccinated);
            Console.WriteLine("생존자: {0}", population);
        }
    }

}
