using System;
using System.Collections.Generic;
using System.Text;

namespace Employees_payroll.Models
{
	[Serializable]
	public class Employee
    {
		public Employee()
		{

		}
		public Employee(string _firstName, string _lastName, string _role, decimal _rate, decimal _overRate)
		{
			firstName = _firstName;
			lastName = _lastName;
			role = _role;
			rate = _rate;
			overRate = _overRate;

			Reports = new List<PersonalReportModel>();

		}

		private string firstName;
		private string lastName;
		private string role;
		private decimal rate;
		private decimal overRate;
		public string FirstName
		{
			get { return firstName; }
		}

		public string LastName
		{
			get { return lastName; }
		}

		public string Role
		{
			get { return role; }
		}

		public decimal Rate
		{
			get { return rate; }
		}
		public decimal OverRate
		{
			get { return overRate; }
		}

		public List<PersonalReportModel> Reports { get; set; }

	}
}
