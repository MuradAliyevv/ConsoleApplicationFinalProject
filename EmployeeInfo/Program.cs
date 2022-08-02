using System;
using EmployeeInfo.DAL;
using EmployeeInfo.Models;
using System.Collections.Generic;
using EmployeeInfo.Helpers;

namespace EmployeeInfo
{
    class Program
    {
        private static readonly DataBase dataBase = new DataBase();

        static void Main(string[] args)
        {
            ChooseProgram();
            ExecuteProgram();
        }

        #region Main methods
        private static void ChooseProgram()
        {
            Console.Clear();

            Console.WriteLine("PLease choose Program (A/B): ");
        }
        private static void ChooseAOperation()
        {
            Console.Clear();

            Console.WriteLine("Please choose below operations:\n" +
                "1. Show Employee Information\n" +
                "2. Show Montly Information of Employee\n" +
                "3. Show Employee by Position\n" +
                "4. Show Yearly Statistics of Employee\n" +
                "5. Show Delayed Employee\n" +
                "6. Show Employee Information by WeekDay\n" +
                "For return to Main menu please click Backspace");
        }
        private static void ChooseBOperation()
        {
            Console.Clear();

            Console.WriteLine("Please choose below operations:\n" +
                "1. Add New Employee\n" +
                "2. Update Employee's Information\n" +
                "3. Add Daily Work Info\n" +
                "4. Delete Employee from Database\n" +
                "5. Backing up Database\n" +
                "For return to Main menu please click Backspace");
        }

        private static void ExecuteProgram()
        {
            ConsoleKey consoleKey = Console.ReadKey().Key;

            switch (consoleKey)
            {
                case ConsoleKey.A:
                    ChooseAOperation();
                    ExecuteAOperation();
                    break;
                case ConsoleKey.B:
                    ChooseBOperation();
                    ExecuteBOperation();
                    break;
                case ConsoleKey.Backspace:
                    ChooseProgram();
                    break;
            }
        }
        private static void ExecuteAOperation()
        {
            ConsoleKey consoleKey = Console.ReadKey().Key;

            switch (consoleKey)
            {
                case ConsoleKey.D1:
                    ShowEmployeeInformation();
                    break;
                case ConsoleKey.D2:
                    ShowMonthlyWorkInformation();
                    break;
                case ConsoleKey.D3:
                    ShowEmployeeByPosition();
                    break;
                case ConsoleKey.D4:
                    ShowEmployeesAnnualy();
                    break;
                case ConsoleKey.D5:
                    ShowLateEmployees();
                    break;
                case ConsoleKey.D6:
                    ShowEmployeesByDay();
                    break;
                case ConsoleKey.Backspace:
                    ChooseProgram();
                    ExecuteProgram();
                    break;
            }
        }
        private static void ExecuteBOperation()
        {
            ConsoleKey consoleKey = Console.ReadKey().Key;

            switch (consoleKey)
            {
                case ConsoleKey.D1:
                    AddEmployee();
                    break;
                case ConsoleKey.D2:
                    UpdateEmployee();
                    break;
                case ConsoleKey.D3:
                    AddWorkTime();
                    break;
                case ConsoleKey.D4:
                    DeleteWorkTime();
                    break;
                case ConsoleKey.D5:
                    break;
                case ConsoleKey.Backspace:
                    ChooseProgram();
                    ExecuteProgram();
                    break;
            }
        }

        private static void ReturnMenuOfProgramA()
        {
            Console.WriteLine("For return to Menu of Program A please click Backspace");

            ConsoleKey consoleKey = Console.ReadKey().Key;

            switch (consoleKey)
            {
                case ConsoleKey.Backspace:
                    ChooseAOperation();
                    ExecuteAOperation();
                    break;
            }
        }
        private static void ReturnMenuOfProgramB()
        {
            Console.WriteLine("For return to Menu of Program B please click Backspace");

            ConsoleKey consoleKey = Console.ReadKey().Key;

            switch (consoleKey)
            {
                case ConsoleKey.Backspace:
                    ChooseBOperation();
                    ExecuteBOperation();
                    break;
            }
        }
        #endregion

        #region A Operaions
        private static void ShowEmployeeInformation()
        {
            Console.Clear();

            Employee employee = default;

            while (employee is null)
            {
                Console.Write("Please enter Employee ID: ");

                if (int.TryParse(Console.ReadLine(), out int employeeId))
                {
                    employee = dataBase.GetEmployeeById(employeeId).Result;

                    if (employee is null)
                        Console.WriteLine($"Employee not found which id is {employeeId}");
                    else
                    {
                        Console.WriteLine(employee.ToString());

                        ReturnMenuOfProgramA();
                    }
                }
                else
                {
                    Console.WriteLine("Please enter only number");
                }
            }
        }
        private static void ShowMonthlyWorkInformation()
        {
            Console.Clear();

            Employee employee = default;

            while (employee is null)
            {
                Console.Write("Please enter Employee ID: ");

                if (int.TryParse(Console.ReadLine(), out int employeeId))
                {
                    employee = dataBase.GetEmployeeById(employeeId).Result;

                    if (employee is null)
                        Console.WriteLine($"Employee not found which id is {employeeId}");
                    else
                    {
                        List<Worktime> workTimes = dataBase.GetWorktimesByEmployeeId(employeeId).Result;

                        int totalWorkHour = 0;
                        int totalWorkMinute = 0;

                        foreach (Worktime workTime in workTimes)
                        {
                            totalWorkHour += workTime.DepartureHour - workTime.EntryHour;
                            totalWorkMinute += workTime.DepartureMinute - workTime.EntryMinute;
                        }

                        if (totalWorkMinute >= 60)
                        {
                            totalWorkHour += totalWorkMinute / 60;
                            totalWorkMinute %= 60;
                        }

                        float totalWorkTime = (float)Math.Round(totalWorkHour + ((float)totalWorkMinute / 60), 2);

                        double monthlySalary = Math.Round(employee.WageRate * 10 * totalWorkTime, 2); ;

                        Console.WriteLine($"{employee.EmployeeId}     {employee.Name} {employee.Surname}     {totalWorkHour} hour {totalWorkMinute} minute     {monthlySalary}");

                        Console.WriteLine("--------------------------------------------------");

                        foreach (Worktime workTime in workTimes)
                            Console.WriteLine($"{workTime.Date.Day:00}     {workTime.EntryHour:00}:{workTime.EntryMinute:00}     {workTime.DepartureHour:00}:{workTime.DepartureMinute:00}");

                        ReturnMenuOfProgramA();
                    }
                }
                else
                    Console.WriteLine("Please enter only number");
            }
        }
        private static void ShowEmployeeByPosition()
        {
            Console.Clear();

            List<Employee> employees = new List<Employee>();

            while (employees.Count == 0)
            {
                Console.Write("Please enter Position: ");

                string postion = Console.ReadLine();

                employees = dataBase.GetEmployeesByPosition(postion).Result;

                if (employees is null)
                    Console.WriteLine($"{postion} Position not found");
                else
                {
                    int counter = 0;
                    foreach (Employee employee in employees)
                        Console.WriteLine($"{++counter}     {employee.EmployeeId}     {employee.Name} {employee.Surname}     {employee.JobStartDate.ToShortDateString()}     {employee.WageRate}");

                    ReturnMenuOfProgramA();
                }
            }
        }
        private static void ShowEmployeesAnnualy()
        {
            Console.Clear();

            List<Employee> employees = dataBase.GetEmployeesAnnualy().Result;

            int employeeCountBefore2000 = 0;

            foreach (Employee employee in employees)
                if (employee.JobStartDate.Year < 2000)
                    employeeCountBefore2000++;

            Console.WriteLine($"Before 2000     {employeeCountBefore2000}     {(((float)employeeCountBefore2000 / employees.Count) * 100):00.00}");

            for (int i = 2000; i <= DateTime.Now.Year; i++)
            {
                int employeeCountCurrentYear = 0;

                foreach (Employee employee in employees)
                    if (employee.JobStartDate.Year == i)
                        employeeCountCurrentYear++;

                Console.WriteLine($"{i}     {employeeCountCurrentYear}     {(((float)employeeCountCurrentYear / employees.Count) * 100):00.00}");
            }

            Console.WriteLine($"Total     {employees.Count}");

            ReturnMenuOfProgramA();
        }
        private static void ShowLateEmployees()
        {
            Console.Clear();

            List<LateEmployee> lateEmployees = dataBase.GetLateEmployees().Result;

            Console.WriteLine("Person Id     Delay Count");

            foreach (LateEmployee lateEmployee in lateEmployees)
                Console.WriteLine($"{lateEmployee.EmployeeId}             {lateEmployee.Count}");

            ReturnMenuOfProgramA();
        }
        private static void ShowEmployeesByDay()
        {
            Console.Clear();

            DateTime dateTime = default;

            while (dateTime == default)
            {
                Console.Write("Please enter Date (day.month.year): ");

                if (DateTime.TryParse(Console.ReadLine(), out dateTime))
                {
                    List<Worktime> workTimes = dataBase.GetEmployeesByDay(dateTime).Result;

                    foreach (Worktime workTime in workTimes)
                    {
                        Console.WriteLine($"{workTime.EmployeeId}     {workTime.Employee.Name} {workTime.Employee.Surname}     {workTime.EntryHour:00}:{workTime.EntryMinute:00}     {workTime.DepartureHour:00}:{workTime.DepartureMinute:00}     {workTime.DepartureHour - workTime.EntryHour} hour {workTime.DepartureMinute - workTime.EntryMinute} minute");
                    }

                    ReturnMenuOfProgramA();
                }
            }
        }
        #endregion

        #region B Operations
        private static void AddEmployee()
        {
            Console.Clear();

            string name = default;

            while (name is null)
            {
                Console.WriteLine("Please enter Employee's name: ");

                string value = Console.ReadLine();

                if (ModelValidator.CheckNullAndMaxLength(value, 25)) name = value;
            }

            string surname = default;

            while (surname is null)
            {
                Console.WriteLine("Please enter Employee's surname: ");

                string value = Console.ReadLine();

                if (ModelValidator.CheckNullAndMaxLength(value, 25)) surname = value;
            }

            DateTime jobStartDate = default;

            while (jobStartDate == default)
            {
                Console.WriteLine("Please enter Start Date (day.month.year): ");

                if (DateTime.TryParse(Console.ReadLine(), out jobStartDate))
                    if (jobStartDate > DateTime.Now) jobStartDate = default;
            }

            string position = default;

            while (position is null)
            {
                Console.WriteLine("Please enter Employee's Position: ");

                string value = Console.ReadLine();

                if (ModelValidator.CheckNullAndMaxLength(value, 15)) position = value;
            }

            double wageRate = default;

            while (wageRate == default)
            {
                Console.WriteLine("Please enter Employee's Wage Rate (like 1.25): ");

                if (!double.TryParse(Console.ReadLine().Replace('.', ','), out wageRate) || double.IsNegative(wageRate))
                    wageRate = default;
            }

            Employee employee = new Employee()
            {
                Name = name,
                Surname = surname,
                JobStartDate = jobStartDate,
                Position = position,
                WageRate = wageRate
            };

            int result = dataBase.AddEmployee(employee).Result;

            if (result > 0)
                Console.WriteLine($"Employee added");
            else
                Console.WriteLine("Something went wrong");

            ReturnMenuOfProgramB();
        }
        private static void UpdateEmployee()
        {
            Console.Clear();

            Employee employee = default;

            while (employee is null)
            {
                Console.Write("Please enter Employee ID: ");

                if (int.TryParse(Console.ReadLine(), out int employeeId))
                {
                    employee = dataBase.GetEmployeeById(employeeId).Result;

                    if (employee is null)
                        Console.WriteLine($"Employee not found which id is {employeeId}");
                    else
                        Console.WriteLine(employee.ToString());
                }
                else
                    Console.WriteLine("Please enter only number");
            }

            string name = default;

            while (name is null)
            {
                Console.WriteLine("Please enter Employee's new name: ");

                string value = Console.ReadLine();

                if (ModelValidator.CheckNullAndMaxLength(value, 25)) name = value;
            }

            string surname = default;

            while (surname is null)
            {
                Console.WriteLine("Please enter Employee's new surname: ");

                string value = Console.ReadLine();

                if (ModelValidator.CheckNullAndMaxLength(value, 25)) surname = value;
            }

            double wageRate = default;

            while (wageRate == default)
            {
                Console.WriteLine("Please enter Employee's Wage Rate (like 1.25): ");

                if (!double.TryParse(Console.ReadLine().Replace('.', ','), out wageRate) || double.IsNegative(wageRate))
                    wageRate = default;
            }

            employee.Name = name;
            employee.Surname = surname;
            employee.WageRate = wageRate;

            int result = dataBase.UpdateEmployee(employee).Result;

            if (result > 0)
                Console.WriteLine($"Employee updated");
            else
                Console.WriteLine("Something went wrong");

            ReturnMenuOfProgramB();
        }
        private static void AddWorkTime()
        {
            Console.Clear();

            DateTime dateTime = default;

            while (dateTime == default)
            {
                Console.Write("Please enter Date (day.month.year): ");

                if (DateTime.TryParse(Console.ReadLine(), out dateTime))
                    if (dateTime > DateTime.Now)
                        dateTime = default;
            }

            bool hasEmployee = default;

            int employeeId = default;

            while (!hasEmployee)
            {
                Console.Write("Please enter Employee ID: ");

                if (int.TryParse(Console.ReadLine(), out employeeId))
                {
                    hasEmployee = dataBase.HasEmployeeById(employeeId).Result;

                    if (!hasEmployee)
                    {
                        employeeId = default;
                        Console.WriteLine($"Employee not found which id is {employeeId}");
                    }
                }
                else
                    Console.WriteLine("Please enter only number");
            }

            int entryHour = default;

            while (entryHour == default)
            {
                Console.WriteLine("Please enter Entry Hour: ");

                if (!int.TryParse(Console.ReadLine(), out entryHour) || !ModelValidator.CheckHour(entryHour))
                    entryHour = default;
            }

            int entryMinute = default;

            while (entryMinute == default)
            {
                Console.WriteLine("Please enter Entry Minute: ");

                if (!int.TryParse(Console.ReadLine(), out entryMinute) || !ModelValidator.CheckMinute(entryMinute))
                    entryMinute = default;
            }

            int departureHour = default;

            while (departureHour == default)
            {
                Console.WriteLine("Please enter Departure Hour: ");

                if (!int.TryParse(Console.ReadLine(), out departureHour) || !ModelValidator.CheckHour(departureHour))
                    departureHour = default;
            }

            int departureMinute = default;

            while (departureMinute == default)
            {
                Console.WriteLine("Please enter Entry Minute: ");

                if (!int.TryParse(Console.ReadLine(), out departureMinute) || !ModelValidator.CheckMinute(departureMinute))
                    departureMinute = default;
            }

            Worktime worktime = new Worktime()
            {
                EntryHour = entryHour,
                EntryMinute = entryMinute,
                DepartureHour = departureHour,
                DepartureMinute = departureMinute,
                Date = dateTime,
                EmployeeId = employeeId
            };

            int result = dataBase.AddWorkTime(worktime).Result;

            if (result > 0)
                Console.WriteLine($"Work Time added");
            else
                Console.WriteLine("Something went wrong");

            ReturnMenuOfProgramB();
        }
        private static void DeleteWorkTime()
        {
            Console.Clear();

            bool hasEmployee = default;

            int employeeId = default;

            while (!hasEmployee)
            {
                Console.Write("Please enter Employee ID: ");

                if (int.TryParse(Console.ReadLine(), out employeeId))
                {
                    hasEmployee = dataBase.HasEmployeeById(employeeId).Result;

                    if (!hasEmployee)
                    {
                        employeeId = default;
                        Console.WriteLine($"Employee not found which id is {employeeId}");
                    }
                }
                else
                    Console.WriteLine("Please enter only number");
            }

            int result = dataBase.DeleteEmployeeAndWorkTimes(employeeId).Result;

            Console.WriteLine("Employee and Work Times deleted");

            ReturnMenuOfProgramB();
        }
        #endregion
    }
}