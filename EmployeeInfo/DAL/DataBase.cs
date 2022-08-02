using System;
using EmployeeInfo.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmployeeInfo.DAL
{
    public class DataBase
    {
        private SqlConnection _sqlConnection;

        public DataBase()
        {
            _sqlConnection = new SqlConnection("Data Source = .; Initial Catalog = EmployeeInfo; Integrated Security = true;");
        }

        #region Read functions
        public async Task<bool> HasEmployeeById(int id)
        {
            await _sqlConnection.OpenAsync();

            using SqlCommand sqlCommand = new SqlCommand("Select Top(1) 1 From Employees Where Id = @id", _sqlConnection);
            sqlCommand.Parameters.Add(new SqlParameter("@id", id));

            using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            bool hasEmployee = sqlDataReader.HasRows;

            await _sqlConnection.CloseAsync();

            return hasEmployee;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            await _sqlConnection.OpenAsync();

            using SqlCommand sqlCommand = new SqlCommand("Select * From Employees Where Id = @id", _sqlConnection);
            sqlCommand.Parameters.Add(new SqlParameter("@id", id));

            using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            Employee employee = default;

            while (sqlDataReader.Read())
            {
                employee = new Employee()
                {
                    EmployeeId = (int)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Surname = (string)sqlDataReader["Surname"],
                    JobStartDate = sqlDataReader.GetFieldValue<DateTime>(3),
                    Position = (string)sqlDataReader["Position"],
                    WageRate = (double)sqlDataReader["WageRate"],
                    MonthlyTotalTime = (int)sqlDataReader["MonthlyTotalTime"],
                };
            }

            await _sqlConnection.CloseAsync();

            return employee;
        }

        public async Task<List<Worktime>> GetWorktimesByEmployeeId(int employeeId)
        {
            await _sqlConnection.OpenAsync();

            using SqlCommand sqlCommand = new SqlCommand("Select * From WorkTimes Where EmployeeId = @employeeId", _sqlConnection);
            sqlCommand.Parameters.Add(new SqlParameter("@employeeId", employeeId));

            using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            List<Worktime> worktimes = new List<Worktime>();

            while (sqlDataReader.Read())
            {
                worktimes.Add(new Worktime()
                {
                    Id = (int)sqlDataReader["Id"],
                    WeekDay = (int)sqlDataReader["WeekDay"],
                    EntryHour = (int)sqlDataReader["EntryHour"],
                    EntryMinute = (int)sqlDataReader["EntryMinute"],
                    DepartureHour = (int)sqlDataReader["DepartureHour"],
                    DepartureMinute = (int)sqlDataReader["DepartureMinute"],
                    Date = (DateTime)sqlDataReader["Date"],
                });
            }

            _sqlConnection.Close();

            return worktimes;
        }

        public async Task<List<Employee>> GetEmployeesByPosition(string position)
        {
            await _sqlConnection.OpenAsync();

            using SqlCommand sqlCommand = new SqlCommand($"Select * From Employees Where Position like N'%{position}%' order by WageRate desc", _sqlConnection);
            //sqlCommand.Parameters.Add(new SqlParameter("@position", position));

            using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            List<Employee> employees = new List<Employee>();

            while (sqlDataReader.Read())
            {
                employees.Add(new Employee()
                {
                    EmployeeId = (int)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Surname = (string)sqlDataReader["Surname"],
                    JobStartDate = sqlDataReader.GetFieldValue<DateTime>(3),
                    Position = (string)sqlDataReader["Position"],
                    WageRate = (double)sqlDataReader["WageRate"],
                    MonthlyTotalTime = (int)sqlDataReader["MonthlyTotalTime"],
                });
            }

            await _sqlConnection.CloseAsync();

            return employees;
        }

        public async Task<List<Employee>> GetEmployeesAnnualy()
        {
            await _sqlConnection.OpenAsync();

            using SqlCommand sqlCommand = new SqlCommand($"Select * From Employees", _sqlConnection);

            using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            List<Employee> employees = new List<Employee>();

            while (sqlDataReader.Read())
            {
                employees.Add(new Employee()
                {
                    EmployeeId = (int)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Surname = (string)sqlDataReader["Surname"],
                    JobStartDate = sqlDataReader.GetFieldValue<DateTime>(3),
                    Position = (string)sqlDataReader["Position"],
                    WageRate = (double)sqlDataReader["WageRate"],
                    MonthlyTotalTime = (int)sqlDataReader["MonthlyTotalTime"],
                });
            }

            await _sqlConnection.CloseAsync();

            return employees;
        }

        public async Task<List<LateEmployee>> GetLateEmployees()
        {
            await _sqlConnection.OpenAsync();

            using SqlCommand sqlCommand = new SqlCommand($"Select EmployeeId, Count(EmployeeId) As Count From WorkTimes Where Year(Date) = {DateTime.Now.Year} and Month(Date) = {DateTime.Now.AddMonths(-1).Month} and ((EntryHour = 9 and EntryMinute > 0) or EntryHour > 9) Group By EmployeeId", _sqlConnection);

            using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            List<LateEmployee> lateEmployees = new List<LateEmployee>();

            while (sqlDataReader.Read())
            {
                lateEmployees.Add(new LateEmployee()
                {
                    EmployeeId = (int)sqlDataReader["EmployeeId"],
                    Count = (int)sqlDataReader["Count"],
                });
            }

            await _sqlConnection.CloseAsync();

            return lateEmployees;
        }

        public async Task<List<Worktime>> GetEmployeesByDay(DateTime dateTime)
        {
            await _sqlConnection.OpenAsync();

            using SqlCommand sqlCommand = new SqlCommand($"Select Employees.Id, Name, Surname, EntryHour, EntryMinute, DepartureHour, DepartureMinute From WorkTimes Inner Join Employees On Employees.Id = EmployeeId Where Date = '{dateTime:yyyy.MM.dd}'", _sqlConnection);
            //sqlCommand.Parameters.Add(new SqlParameter("@date", dateTime.ToString("yyyy.MM.dd")));

            using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            List<Worktime> worktimes = new List<Worktime>();

            while (sqlDataReader.Read())
            {
                worktimes.Add(new Worktime()
                {
                    EmployeeId = (int)sqlDataReader["Id"],
                    EntryHour = (int)sqlDataReader["EntryHour"],
                    EntryMinute = (int)sqlDataReader["EntryMinute"],
                    DepartureHour = (int)sqlDataReader["DepartureHour"],
                    DepartureMinute = (int)sqlDataReader["DepartureMinute"],
                    Employee = new Employee()
                    {
                        Name = (string)sqlDataReader["Name"],
                        Surname = (string)sqlDataReader["Surname"]
                    }
                });
            }

            await _sqlConnection.CloseAsync();

            return worktimes;
        }
        #endregion

        #region Add, Update or Delete functions
        public async Task<int> AddEmployee(Employee employee)
        {
            _sqlConnection.Open();

            using SqlCommand sqlCommand = new SqlCommand($"Insert Into Employees Values('{employee.Name}', '{employee.Surname}', '{employee.JobStartDate:yyyy.MM.dd}', '{employee.Position}', {employee.WageRate})", _sqlConnection);

            int employeeId = sqlCommand.ExecuteNonQuery();

            _sqlConnection.Close();

            return employeeId;
        }

        public async Task<int> UpdateEmployee(Employee employee)
        {
            _sqlConnection.Open();

            using SqlCommand sqlCommand = new SqlCommand($"Update Employees Set Name = '{employee.Name}', Surname = '{employee.Surname}', WageRate = {employee.WageRate.ToString().Replace(',', '.')} Where Id = {employee.EmployeeId};", _sqlConnection);

            int employeeId = sqlCommand.ExecuteNonQuery();

            _sqlConnection.Close();

            return employeeId;
        }

        public async Task<int> AddWorkTime(Worktime worktime)
        {
            _sqlConnection.Open();

            int dayOfWeek = (int)worktime.Date.DayOfWeek;

            using SqlCommand sqlCommand = new SqlCommand($"Insert Into WorkTimes Values({dayOfWeek}, {worktime.EntryHour}, {worktime.EntryMinute}, {worktime.DepartureHour}, {worktime.DepartureMinute}, {worktime.Date:yyyy-MM-dd}, {worktime.EmployeeId})", _sqlConnection);

            int workTimeId = sqlCommand.ExecuteNonQuery();

            _sqlConnection.Close();

            return workTimeId;
        }

        public async Task<int> DeleteEmployeeAndWorkTimes(int employeeId)
        {
            await _sqlConnection.OpenAsync();

            using SqlCommand workTimeDeleteCommand = new SqlCommand($"Delete From WorkTimes Where EmployeeId = @employeeId", _sqlConnection);
            workTimeDeleteCommand.Parameters.Add(new SqlParameter("@employeeId", employeeId));

            int workTimeResult = await workTimeDeleteCommand.ExecuteNonQueryAsync();

            using SqlCommand employeeDeleteCommand = new SqlCommand($"Delete From Employees Where Id = @id", _sqlConnection);
            employeeDeleteCommand.Parameters.Add(new SqlParameter("@id", employeeId));

            int employeeResult = await employeeDeleteCommand.ExecuteNonQueryAsync();

            await _sqlConnection.CloseAsync();

            return workTimeResult + employeeResult;
        }
        #endregion
    }
}
