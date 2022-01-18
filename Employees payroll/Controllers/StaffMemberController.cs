using Employees_payroll.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Employees_payroll.Controllers
{
    class StaffMemberController
    {
        public static DateTime InputDate()
        {
            DateTime date = default;
            bool dateValid = false;
            while(true)
            {
                Console.WriteLine("Введите дату в формате 'дд.мм.гг': ");

                dateValid = DateTime.TryParse(Console.ReadLine(), out date);

                if(dateValid && DateTime.Now >= date)
                {
                    return date;
                }

                Console.WriteLine("Неверный ввод!");
                Console.WriteLine("Попробуйте снова");
            }
        }
        public static int InputHours()
        {
            int hours = 0;
            while (true)
            {
                Console.WriteLine("Введите количество отработанных часов: ");
                bool result = int.TryParse(Console.ReadLine().ToString(), out hours);
                if (result)
                {
                    if (hours > 0 && hours < 13)
                    {
                        return hours;
                    }
                    Console.WriteLine("Количество часов не может быть менее 1 и более 12");
                }
                else
                {
                    Console.WriteLine("Неверный ввод, попробуйте еще раз");
                }
            }
        }
        public static decimal CountSalary(Employee emp, int hours)
        {
            decimal salary = default;
            if (hours > 160)
            {
                salary = (hours % 160 * emp.OverRate) + 160 * emp.Rate;
            }
            else
            {
                salary = hours * emp.Rate;
            }

            return salary;
        }
        public static bool AddHours(Employee currentEmp)
        {
            List<Employee> allEmployees;
            DateTime date = InputDate();
            int hours = InputHours();
            string description = Console.ReadLine().ToString();

            BinaryFormatter formatter = new BinaryFormatter();

            using(FileStream fs = new FileStream("AllEmployeesList.dat", FileMode.OpenOrCreate)) // Getting all Employees list
            {
                allEmployees = (List<Employee>)formatter.Deserialize(fs);
            }

            Employee targetEmp = allEmployees.Where(x => x.FirstName == currentEmp.FirstName && x.LastName == currentEmp.LastName).FirstOrDefault(); // Getting target Employee

            targetEmp.Reports.Add(new PersonalReportModel(date, hours, description)); // Adding new report

            using (FileStream fs = new FileStream("AllEmployeesList.dat", FileMode.OpenOrCreate)) // Saving changes
            {
                formatter.Serialize(fs, allEmployees);
            }

            bool result = true;
            while (true)
            {
                Console.WriteLine("Желаете посмотреть еще один отчет? Нажмите (Д)а для продолжения. (Н)ет для выхода на главный экран программы.");
                var i = Console.ReadKey().Key;
                Console.WriteLine();
                if (i == ConsoleKey.L) // Yes
                {
                    result = true;
                    break;
                }
                else if (i == ConsoleKey.Y) // No
                {
                    result = false;
                    break;
                }

                Console.WriteLine();
                Console.WriteLine("Неверный ввод. Повторите попытку");
                Thread.Sleep(1500);
                Console.Clear();
            }
            return result;
        }
        public static bool SelfReport(Employee currentEmp)
        {
            DateTime startingDate = default;
            DateTime endingDate = default;

            while(true)
            {
                startingDate = InputDate();
                Console.WriteLine();
                endingDate = InputDate();
                if(startingDate>endingDate)
                {
                    Console.WriteLine("Дата начала не может быть позже даты окончания!\nВведите заново");
                    continue;
                }
                break;
            }

            Console.WriteLine();

            Console.WriteLine($"Отчет по сотруднику: {currentEmp.FirstName} {currentEmp.LastName} за период за период с {startingDate} по {endingDate}");

            int totalHours = 0;
            decimal totalSalary = 0;

            var reports = currentEmp.Reports.Where(x => x.Date >= startingDate && x.Date <= endingDate).ToList();

            if (reports.Any())
            {
                Console.WriteLine();
                foreach (var i in reports)
                {
                    Console.WriteLine($"{i.Date}, {i.Hours} часов, {i.TaskDescription}");
                    totalHours += i.Hours;
                }
                totalSalary = CountSalary(currentEmp, totalHours);
            }

            else
            {
                Console.WriteLine("У сотрудника отсутствуют сведения об отработанных часах");
                Console.WriteLine("Нажмите кнопку Enter для продолжения");
                Console.ReadLine();
                return true;
            }

            Console.WriteLine($"Итого: {totalHours} часов, заработано: {totalSalary} руб");

            bool result = true;
            while (true)
            {
                Console.WriteLine("Желаете посмотреть еще один отчет? Нажмите (Д)а для продолжения. (Н)ет для выхода на главный экран программы.");
                var i = Console.ReadKey().Key;
                Console.WriteLine();
                if (i == ConsoleKey.L) // Yes
                {
                    result = true;
                    break;
                }
                else if (i == ConsoleKey.Y) // No
                {
                    result = false;
                    break;
                }

                Console.WriteLine();
                Console.WriteLine("Неверный ввод. Повторите попытку");
                Thread.Sleep(1000);
                Console.Clear();
            }
            return result;
        }
    }
}
