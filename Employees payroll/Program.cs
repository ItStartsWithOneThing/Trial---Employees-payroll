using Employees_payroll.Controllers;
using Employees_payroll.Models;
using Employees_payroll.Views;
using System;
using System.Threading.Tasks;

namespace Employees_payroll
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isWorking = true;

            Console.WriteLine("Приветствую Вас!");

            while (isWorking)
            {
                Console.Write("Введите имя пользователя: ");
                var _firstName = Console.ReadLine().ToString();
                Console.Write("Введите фамилию пользователя: ");
                var _lastName = Console.ReadLine().ToString();

               
                Employee CurrentUser;

                CurrentUser = MainController.Authorization(_firstName, _lastName);
                if (CurrentUser == null)
                {
                    Console.WriteLine("Ошибка! Пользователь с такими данными не найден. Введите данные заново.");
                    continue;
                }


                switch (CurrentUser.Role)
                {
                    case "Руководитель":
                        ManagerView.ShowManagerView(CurrentUser);
                        isWorking = false;
                        break;
                    case "Штатный сотрудник":
                        StaffMemberView.ShowStaffMemberView(CurrentUser);
                        isWorking = false;
                        break;
                    case "Фрилансер":
                        isWorking = false;
                        break;
                }
            }
            Console.Read();
        }
    }
}
