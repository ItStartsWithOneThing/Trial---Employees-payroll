using Employees_payroll.Controllers;
using Employees_payroll.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Employees_payroll.Views
{
    class FreelancerView
    {
        public static void ShowFreelancerView(Employee user)
        {
            bool runMenu = true;

            Console.WriteLine($"Здравствуйте, {user.FirstName} {user.LastName}!");
            Console.WriteLine($"Ваша роль - {user.Role}");

            while (runMenu)
            {
                Console.WriteLine("Выберите желаемое действие:");
                Console.WriteLine("(1). Добавить часы работы");
                Console.WriteLine("(2). Просмотреть личный отчет за период");

                string option = Console.ReadLine().ToString();

                switch (option)
                {
                    case "1":
                        Console.WriteLine();
                        runMenu = FreelancerController.AddHours(user);
                        Thread.Sleep(15000);
                        Console.Clear();
                        break;
                    case "2":
                        Console.WriteLine();
                        runMenu = StaffMemberController.SelfReport(user);
                        Thread.Sleep(1500);
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine($"Неверный ввод. Вы ввели: {option}. Попробуйте ввести заново");
                        Console.Clear();
                        break;
                }

            }
        }
    }
}
