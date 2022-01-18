using Employees_payroll.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Employees_payroll.Controllers
{
    public class ManagerController
    {
        public static int InputHours()
        {
            int hours = 0;
            while(true)
            {
                Console.WriteLine("Введите количество отработанных часов: ");
                bool result = int.TryParse(Console.ReadLine().ToString(), out hours);
                if(result)
                {
                    if(hours > 0 && hours < 13)
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
        public static bool AddHours()
        {
            Console.WriteLine("Необходимо ввести имя и фамилию сотрудника, которому вы добавляете отработанные часы");

            string firstName;
            string lastName;
            DateTime date;
            int hours;
            string description;

            List<Employee> allEmployees;
            Employee emp;

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("AllEmployeesList.dat", FileMode.OpenOrCreate))
            {
                allEmployees = (List<Employee>)formatter.Deserialize(fs);
            }

            while (true)
            {
                firstName = InputName(1);
                if (allEmployees.Where(x => x.FirstName == firstName).Any())
                {
                    break;
                }

                Console.WriteLine("Такого сотрудника не существует, либо проверьте правильность ввода Имени");
                Console.WriteLine();
            }
            while (true)
            {
                lastName = InputName(2);
                if (allEmployees.Where(x => x.FirstName == firstName && x.LastName == lastName).Any())
                {
                    break;
                }

                Console.WriteLine("Такого сотрудника не существует, либо проверьте правильность ввода Фамилии");
                Console.WriteLine();
            }

            emp = allEmployees.Where(x => x.FirstName == firstName && x.LastName == lastName).FirstOrDefault();

            Console.WriteLine();

            date = InputDate(3);
            Console.WriteLine();
            hours = InputHours();
            Console.WriteLine();
            Console.WriteLine("Опишите выполненную работу");
            description = Console.ReadLine().ToString();

            emp.Reports.Add(new PersonalReportModel(date, hours, description));

            using (FileStream fs = new FileStream("AllEmployeesList.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, allEmployees);
            }

            Console.WriteLine();

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
                Thread.Sleep(2000);
                Console.Clear();
            }

            return result;
        }

        public static bool PersonalReport()
        {
            
            List<Employee> allEmployees;
            string inputFirstName;
            string inputLastName;
            Employee emp;
            DateTime startingDate = default;
            DateTime endingDate = default;

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("AllEmployeesList.dat", FileMode.OpenOrCreate))
            {
                allEmployees = (List<Employee>)formatter.Deserialize(fs);
            }


            while (true)
            {
                inputFirstName = InputName(1);
                if(allEmployees.Where(x=>x.FirstName == inputFirstName).Any())
                {
                    break;
                }

                Console.WriteLine("Такого сотрудника не существует, либо проверьте правильность ввода Имени");
            }
            while (true)
            {
                inputLastName = InputName(2);
                if (allEmployees.Where(x => x.FirstName == inputFirstName && x.LastName == inputLastName).Any())
                {
                    break;
                }

                Console.WriteLine("Такого сотрудника не существует, либо проверьте правильность ввода Фамилии");
            }

            emp = allEmployees.Where(x => x.FirstName == inputFirstName && x.LastName == inputLastName).FirstOrDefault();

            Console.WriteLine();

            startingDate = InputDate(1);
            endingDate = InputDate(2);

            Console.WriteLine();

            int totalHours = 0;
            decimal totalSalary = 0;

            var reports = emp.Reports.Where(x => x.Date >= startingDate && x.Date <= endingDate).ToList();

            if (reports.Any()) 
            {
                Console.WriteLine($"Отчет по сотруднику: {emp.FirstName} {emp.LastName} за период за период с {startingDate} по {endingDate}");
                Console.WriteLine();
                foreach (var i in reports)
                {
                    Console.WriteLine($"{i.Date}, {i.Hours} часов, {i.TaskDescription}");
                    totalHours += i.Hours;
                }
                totalSalary = CountSalary(emp, totalHours);
                Console.WriteLine($"Итого: {totalHours} часов, заработано: {totalSalary} руб");
            }

            else
            {
                Console.WriteLine("У сотрудника отсутствуют сведения об отработанных часах за данный период");
                Console.WriteLine("Нажмите кнопку Enter для продолжения");
                Console.ReadLine();
                return true;
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

        public static decimal CountSalary(Employee emp, int hours)
        {
            decimal salary = default;
            if(hours > 160 && emp.Role != "Фрилансер")
            {
                salary = ((hours - 160) * emp.OverRate) + 160 * emp.Rate;
            }
            else
            {
                salary = hours * emp.Rate;
            }

            return salary;
        }

        public static DateTime InputDate(int number)
        {
            DateTime startingDate = default;
            DateTime endingDate = default;
            DateTime date = default;

            switch(number)
            {
                case 1:
                    bool startDateNotValid = true;
                    while (startDateNotValid) // Input Start Date
                    {
                        Console.WriteLine("Введите дату начала в формате 'дд.мм.гг': ");
                        try
                        {
                            startingDate = Convert.ToDateTime(Console.ReadLine());
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                        finally
                        {
                            if (startingDate == default || DateTime.Now < startingDate)
                            {
                                Console.WriteLine("Попробуйте снова");
                            }
                            else
                            {
                                startDateNotValid = false;
                            }
                        }
                    }
                    return startingDate;

                case 2:
                    bool endtDateNotValid = true;
                    while (endtDateNotValid) // Input End Date
                    {
                        Console.WriteLine("Введите дату окончания 'дд.мм.гг': ");
                        try
                        {
                            endingDate = Convert.ToDateTime(Console.ReadLine());
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                        finally
                        {
                            if (endingDate == default || DateTime.Now < endingDate)
                            {
                                Console.WriteLine("Попробуйте снова");
                            }
                            else
                            {
                                endtDateNotValid = false;
                            }
                        }
                    }
                    return endingDate;
                case 3:
                    bool dateNotValid = true;
                    while (dateNotValid) // Input Start Date
                    {
                        Console.WriteLine("Введите дату в формате 'дд.мм.гг': ");
                        try
                        {
                            date = Convert.ToDateTime(Console.ReadLine());
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                        finally
                        {
                            if (date == default || DateTime.Now < date)
                            {
                                Console.WriteLine("Попробуйте снова");
                            }
                            else
                            {
                                dateNotValid = false;
                            }
                        }
                    }
                    return date;

                default: return new DateTime();
            }
        }
       
        public static string InputName(int number)
        {
            string pattern = @"[а-я]{2,16}";
            string firstName = null;
            string lastName = null;
            bool nameIsMatch = false;

            switch (number)
            {
                case 1:
                    while (true) // Input First Name
                    {
                        Console.WriteLine("Введите имя: ");
                        firstName = Console.ReadLine().ToString().ToLower();
                        nameIsMatch = Regex.IsMatch(firstName, pattern, RegexOptions.IgnoreCase);
                        if (nameIsMatch)
                        {
                            firstName = string.Concat(firstName[0].ToString().ToUpper(), firstName.Substring(1)); // Making first letter Upper Case
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Используйте только кириллицу! Минимальное кол-во символов - 2, максимальное - 16");
                        }
                    }
                    return firstName;
                case 2:
                    while (true) // Input Last Name
                    {
                        Console.WriteLine("Введите фамилию: ");
                        lastName = Console.ReadLine().ToString().ToLower();
                        nameIsMatch = Regex.IsMatch(lastName, pattern, RegexOptions.IgnoreCase);
                        if (nameIsMatch)
                        {
                            lastName = string.Concat(lastName[0].ToString().ToUpper(), lastName.Substring(1)); // Making first letter Upper Case
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Используйте только кириллицу! Минимальное кол-во символов - 2, максимальное - 16");
                        }
                    }
                    return lastName;
                default: return "Ошибка ввода имени";
            }
        }

        public static bool CommonReport()
        {
            bool result = true;
            List<Employee> allEmployees;

            DateTime startingDate = default;
            DateTime endingDate = default;

            while (true)
            {
                startingDate = InputDate(1);
                Console.WriteLine();
                endingDate = InputDate(2);

                if(startingDate<=endingDate)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Дата начала - {startingDate}, Дата окончания - {endingDate}\nДата начала не может быть позже, чем дата окончания!");
                }
            }


            
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("AllEmployeesList.dat", FileMode.OpenOrCreate))
            {
                allEmployees = (List<Employee>)formatter.Deserialize(fs);
            }

            Console.WriteLine();
            Console.WriteLine($"Отчет за период с {startingDate} по {endingDate}");

            int allHours = 0;
            decimal allSalary = 0M;

            foreach(var emp in allEmployees)
            {
                string name = emp.FirstName + " " + emp.LastName;
                int hours = emp.Reports.Where(x => startingDate <= x.Date && x.Date <= endingDate).Sum(x => x.Hours);// Counting Summ of HOURS
                decimal salary = CountSalary(emp, hours);
                Console.WriteLine($"{name} отработал {hours} часов и заработал за период {salary} рублей.");
                allHours += hours;
                allSalary += salary;
            }
            Console.WriteLine($"Всего часов отработано за период {allHours}, сумма к выплате {allSalary} рублей.");
            Console.WriteLine();
            
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



        public static void AddNewEmployee(Employee newEmpl) 
        {
            List<Employee> allEmployees;

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("AllEmployeesList.dat", FileMode.OpenOrCreate))
            {
                allEmployees = (List<Employee>)formatter.Deserialize(fs);
            }

            bool alreadyExist = allEmployees.Where(x => x.FirstName == newEmpl.FirstName && x.LastName == newEmpl.LastName).Any();

            if(alreadyExist)
            {
                Console.WriteLine("Такой сотрудник уже существует");
                Console.WriteLine("Нажмите клавишу Enter для продолжения");
                Console.ReadLine();
                return;
            }

            allEmployees.Add(newEmpl);

            using (FileStream fs = new FileStream("AllEmployeesList.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, allEmployees);
            }

            Console.WriteLine("Сотрудник успешно добавлен");
        }


        public static Employee CreateNewEmployee()
        {

            bool roleIsNotValid = true;
            string firstName = null;
            string lastName = null;
            string role = null;
            decimal rate = 0;
            decimal overRate = 0;

            while (roleIsNotValid)
            {
                Console.WriteLine("Добавление нового сотрудника");
                Console.WriteLine();

                firstName = InputName(1);
                Console.WriteLine();
                lastName = InputName(2);
                Console.WriteLine();

                Console.WriteLine("Выберете роль работника:\n(1). Руководитель\n(2). Штатный сотрудник\n(3). Фрилансер");
                Console.WriteLine();

                int option = 0;

                try // Can use "if else" construction with Int32.TryParse Method instead of "try..catch..finnally" construction for more performance
                {
                    option = Convert.ToInt32(Console.ReadLine());
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Неправильный ввод! {ex.Message} \nИспользуйте клавиши: 1, 2, 3");
                }
                finally
                {
                    if (option.GetType() == typeof(Int32) && (option < 1 && option > 3))
                    {
                        option = 4;
                    }
                    else if(option.GetType() != typeof(Int32))
                    {
                        option = default;
                    }
                    
                }
                switch (option)
                {
                    case 1:
                        role = "Руководитель";
                        rate = 1250;
                        overRate = 2125;
                        roleIsNotValid = false;
                        break;
                    case 2:
                        role = "Штатный сотрудник";
                        rate = 750;
                        overRate = 1500;
                        roleIsNotValid = false;
                        break;
                    case 3:
                        role = "Фрилансер";
                        rate = 1000;
                        overRate = 0;
                        roleIsNotValid = false;
                        break;
                    default:
                        break;
                }
            }

            return new Employee(firstName, lastName, role, rate, overRate);
        }

    }
}
