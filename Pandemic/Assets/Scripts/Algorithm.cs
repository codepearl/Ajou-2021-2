using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
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

    public static void algo()
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

            ///�Ϸ� �ϰ� : ���� -> ��� -> ��� ���� -> ����

            Random rand = new Random();

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

        Console.WriteLine("���� ������ : {0}", numberOfInfections);
        Console.WriteLine("��� ���� : {0}", numberOfVaccinated);
        Console.WriteLine("������: {0}", population);
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
