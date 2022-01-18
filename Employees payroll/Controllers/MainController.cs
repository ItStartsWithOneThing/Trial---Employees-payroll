using Employees_payroll.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Employees_payroll.Controllers
{
    public class MainController
    {
        public static Employee Authorization(string _firstName, string _lastName)
        {
            
            List<Employee> allEmployees = new List<Employee>();
            Employee CurrentUser;

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("AllEmployeesList.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length == 0) // first app entrance when Employees List is empty
                {
                    CurrentUser = new Employee("Константин", "Туркин", "Руководитель", 1250M, 125M);
                    allEmployees.Add(CurrentUser);
                    formatter.Serialize(fs, allEmployees);
                    if(_firstName == "Константин" && _lastName == "Туркин")
                    { 
                        return CurrentUser; 
                    }        
                    else
                    {
                        CurrentUser = null;
                        return CurrentUser;
                    }
                    
                }
                else
                {
                    allEmployees = (List<Employee>)formatter.Deserialize(fs);
                }
            }

            CurrentUser = allEmployees.FirstOrDefault<Employee>(x => x.FirstName == _firstName && x.LastName == _lastName);

            return CurrentUser;
        }
    }
}
