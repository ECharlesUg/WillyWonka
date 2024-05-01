using System;
using System.Collections.Generic;

namespace WillyWonka
{
    class Customer
    {
        // declares variables needed for the class
        public int custNum { get; set; }
        public int OompWaitTime { get; set; }
        public int LoompaWaitTime { get; set; }
        public int arriveNum { get; set; }

        public Customer(int n, int arrivalTime, int oompaWaitTime, int loompaWaitTime)
        {
            custNum = n;
            arriveNum = arrivalTime;
            OompWaitTime = oompaWaitTime;
            LoompaWaitTime = loompaWaitTime;
        }
    }

    class Program
    {
        static Queue<Customer> GenerateEntranceLine()
        {
            // calculates ncessaryinformation for customers and stores the infor in the Queue
            Queue<Customer> customers = new Queue<Customer>();
            Random rnd = new Random();
            int nextArriveTime = 0;
            while (nextArriveTime < 600)
            {
                int arrivalTime = rnd.Next(5, 11);
                nextArriveTime += arrivalTime;

                if (nextArriveTime < 600)
                {
                    // generates wait time
                    // oompa takes 7 to 14 mins
                    int oompWaitTime = rnd.Next(7, 15);
                    // loompa takes 5 to 8 mins
                    int loompaWaitTime = rnd.Next(5, 9);
                    Customer newCustomer = new Customer(customers.Count + 1, nextArriveTime, oompWaitTime, loompaWaitTime);
                    customers.Enqueue(newCustomer);
                }
            }

            return customers;
        }

        static void Main(string[] args)
        {
            Queue<Customer> entrance = GenerateEntranceLine();
            Queue<Customer> oompa = new Queue<Customer>();
            Queue<Customer> loompa = new Queue<Customer>();
            Dictionary<int, int> LoompaLine = new Dictionary<int, int>();
            Dictionary<int, int> OompaLine = new Dictionary<int, int>();

            int servedCustomers = 0;
            // used to count the second up till 600 seconds
            for (int i = 0; i <= 600; i++)
            {
                Console.Write($"Time: {i}, ");

                if (entrance.Count > 0 && entrance.Peek().arriveNum == i)
                {
                    Customer newCust = entrance.Dequeue();
                    oompa.Enqueue(newCust);
                    OompaLine[newCust.custNum] = newCust.OompWaitTime;
                }

                // Iterate over Oompa Queue
                Console.Write($"Entrance Line Front: {(entrance.Count > 0 ? $"Customer #{entrance.Peek().custNum}" : "None")}, ");
                Console.Write($"Oompa's line currently has {oompa.Count} customer(s)");
                for (int j = 0; j < oompa.Count; j++)
                {
                    Customer cust = oompa.ElementAt(j);
                    if (OompaLine[cust.custNum] == 0)
                    {
                        loompa.Enqueue(cust);
                        LoompaLine[cust.custNum] = cust.LoompaWaitTime;
                        oompa.Dequeue();
                        Console.Write($" with Customer #{cust.custNum} in front with a remaining time of 0 minute(s).");
                    }
                    else
                    {
                        Console.Write($" with Customer #{cust.custNum} in front which they need to wait for {cust.OompWaitTime} minute(s)");
                        OompaLine[cust.custNum]--;
                    }
                }

                // Iterate over Loompa queue
                Console.Write($" .Loompa's line currently has {loompa.Count} customer(s)");
                for (int j = 0; j < loompa.Count; j++)
                {
                    Customer cust = loompa.ElementAt(j);
                    if (LoompaLine[cust.custNum] == 0)
                    {
                        loompa.Dequeue();
                        servedCustomers++;
                        Console.Write($" with Customer #{cust.custNum} in front with a remaining time of 0 minute(s).");
                    }
                    else
                    {
                        Console.Write($" with Customer #{cust.custNum} in front with a remaining time of {cust.LoompaWaitTime} minute(s).");
                        LoompaLine[cust.custNum]--;
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
            }
            int totalOompaWaitTime = 0;
            int totalLoompaWaitTime = 0;
            // calculates total wait time for oompa and loompa
            foreach (var customer in oompa)
            {
                totalOompaWaitTime += customer.OompWaitTime;
            }

            foreach (var customer in loompa)
            {
                totalLoompaWaitTime += customer.LoompaWaitTime;
            }
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"The Store is closed to new customers. There are {oompa.Count} customer(s) in Oompa's line and {loompa.Count} customer(s) in the checkout line.");
            Console.WriteLine($"So far, we have served {servedCustomers} customers!");
            Console.WriteLine($"We need to stay open for {totalOompaWaitTime + totalLoompaWaitTime} minutes to finish processing our customers' orders...");
        }

    }
}
