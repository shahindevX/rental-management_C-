using Rental_Management_1295498.Entities;
using Rental_Management_1295498.Enums;
using Rental_Management_1295498.Factories;
using Rental_Management_1295498.Manager;
using Rental_Management_1295498.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental_Management_1295498
{
    internal class Program
    {
        static VehicleRepositories repo = new VehicleRepositories();

        static void Main(string[] args)
        {
            try
            {
                DoTask();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void DoTask()
        {
            {
                var customer1 = new RentalMaster
                {
                    CustomerName = "Shahin Khan",
                    Contact = "01812557487",
                    Items = new List<Vehicle>
                 {
                 new Vehicle { VType = VehicleType.Car, Days = 4 }
                 }
                };

                var customer2 = new RentalMaster
                {
                    CustomerName = "Imran Mia",
                    Contact = "01822334455",
                    Items = new List<Vehicle>
                {
               new Vehicle { VType = VehicleType.Bike, Kilometers = 10 }
                }
                };


                new RentalFactory(customer1).Process();
                new RentalFactory(customer2).Process();

                repo.Save(customer1);
                repo.Save(customer2);
            }

            Console.WriteLine("------Rental Management-------");
            Console.WriteLine();


            while (true)
            {
                Console.WriteLine("Select Operation Type");
                Console.WriteLine("\n| 1.Create | 2.View | 3.Update | 4.Delete | 5.Exit");
                Console.WriteLine();
                Console.Write("Enter Choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;
                if (choice == 5) break;

                switch (choice)
                {
                    case 1: Create(); break;
                    case 2: View(); break;
                    case 3: Update(); break;
                    case 4: Delete(); break;
                    default:
                        Console.WriteLine("Invalid Selection");
                        break;
                }
            }
        }

        static void Create()
        {
            RentalMaster m = new RentalMaster();
            Console.Write("Customer Name: "); m.CustomerName = Console.ReadLine();
            Console.Write("Contact Number: "); m.Contact = Console.ReadLine();

            AddVehicles(m);
            new RentalFactory(m).Process();
            repo.Save(m);
            Console.WriteLine("Rental Processed and Saved Successfully!");
            View();
        }

        static void View()
        {
            var list = repo.GetAll();


            Console.WriteLine("\n" + new string('=', 120));
            Console.WriteLine("\t\t\t\t\t      RENTAL LIST");
            Console.WriteLine(new string('=', 120));


            string fmt = "| {0,-5} | {1,-15} | {2,-15} | {3,-12} | {4,-10} | {5,-12} | {6,-12} | {7,-15} |";


            Console.WriteLine(fmt, "ID", "Name", "Contact", "Vehicle", "Usage", "Net Rent", "Discount", "Grand Total");
            Console.WriteLine(new string('-', 120));

            foreach (var r in list)
            {
                bool isFirst = true;

                foreach (var v in r.Items)
                {

                    IVehicleManager manager;
                    if (v.VType == VehicleType.Car)
                    {
                        manager = new CarManager();
                    }
                    else
                    {
                        manager = new BikeManager();
                    }


                    double grossRent = manager.GetTotalRent(v);
                    double discount = manager.GetDiscount(v);
                    double netRent = grossRent - discount;


                    string usage = v.VType == VehicleType.Car ? $"{v.Days} Days" : $"{v.Kilometers} KM";


                    Console.WriteLine(fmt,
                        isFirst ? r.RentalId.ToString() : "",
                        isFirst ? r.CustomerName : "",
                        isFirst ? r.Contact : "",
                        v.VType.ToString(),
                        usage,
                        netRent.ToString("N2"),
                        discount.ToString("N2"),
                        "");

                    isFirst = false;
                }


                Console.WriteLine(fmt, "", "", "", "", "", "", "", r.GrandTotal.ToString("N2"));
                Console.WriteLine(new string('-', 120));
            }
        }

        static void Update()
        {
            Console.Write("Enter ID to Update: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;
            var m = repo.GetById(id);
            if (m == null)
            {
                Console.WriteLine("Record not found!");
                return;
            }

            Console.Write("New Name: "); m.CustomerName = Console.ReadLine();
            Console.Write("New Contact: "); m.Contact = Console.ReadLine();

            m.Items.Clear();
            AddVehicles(m);
            new RentalFactory(m).Process();
            repo.Update(m);
            Console.WriteLine("Record updated successfully!");
            View();
        }

        static void Delete()
        {
            Console.Write("Enter ID to Delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;
            repo.Delete(id);
            Console.WriteLine("Record removed from system.");
            View();
        }

        static void AddVehicles(RentalMaster m)
        {
            Console.Write("Vehicle Type (1:Car, 2:Bike): ");
            VehicleType v = (VehicleType)int.Parse(Console.ReadLine());
            Vehicle item = new Vehicle { VType = v };

            if (v == VehicleType.Car)
            {
                Console.Write("Days: ");
                item.Days = int.Parse(Console.ReadLine());
            }
            else
            {
                Console.Write("Kilometers: ");
                item.Kilometers = double.Parse(Console.ReadLine());
            }

            m.Items.Add(item);
        }
    }
}
