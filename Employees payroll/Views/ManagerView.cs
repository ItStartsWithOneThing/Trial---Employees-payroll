using Employees_payroll.Controllers;
using Employees_payroll.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Employees_payroll.Views
{
    class ManagerView
    {
        public static void ShowManagerView(Employee user)
        {
            bool runMenu = true;

            Console.WriteLine($"Здравствуйте, {user.FirstName} {user.LastName}!");
            Console.WriteLine($"Ваша роль - {user.Role}");

            while (runMenu)
            {
                Console.WriteLine("Выберите желаемое действие:");
                Console.WriteLine("(1). Добавить сотрудника");
                Console.WriteLine("(2). Просмотреть отчет по всем сотрудникам");
                Console.WriteLine("(3). Просмотреть отчет по конкретному сотруднику");
                Console.WriteLine("(4). Добавить часы работы");
                Console.WriteLine("(5). Выход из программы");

                string option = Console.ReadLine().ToString();

                switch (option)
                {
                    case "1":
                        Console.WriteLine();
                        var newGuy = ManagerController.CreateNewEmployee();
                        ManagerController.AddNewEmployee(newGuy);
                        Thread.Sleep(1500);
                        Console.Clear();
                        break;
                    case "2":
                        Console.WriteLine();
                        runMenu = ManagerController.CommonReport();
                        Thread.Sleep(1500);
                        Console.Clear();
                        break;
                    case "3":
                        Console.WriteLine();
                        runMenu = ManagerController.PersonalReport();
                        Thread.Sleep(1500);
                        Console.Clear();
                        break;
                    case "4":
                        Console.WriteLine();
                        runMenu = ManagerController.AddHours();
                        Thread.Sleep(1500);
                        Console.Clear();
                        break;
                    case "5":
                        Console.WriteLine();
                        runMenu = false;
                        break;
                    default:
                        Console.WriteLine();
                        Console.Clear();
                        Console.WriteLine("Неверный ввод. Попробуйте ввести заново");
                        break;
                }
            }
        }
    }
}
