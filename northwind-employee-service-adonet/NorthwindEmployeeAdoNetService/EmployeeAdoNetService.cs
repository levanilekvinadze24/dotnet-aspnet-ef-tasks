using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace NorthwindEmployeeAdoNetService
{
    public sealed class EmployeeAdoNetService
    {
        private readonly DbProviderFactory _dbFactory;
        private readonly string _connectionString;

        public EmployeeAdoNetService(DbProviderFactory dbFactory, string connectionString)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            _connectionString = string.IsNullOrWhiteSpace(connectionString)
                ? throw new ArgumentException("Connection string cannot be null or whitespace.", nameof(connectionString))
                : connectionString;
        }

        public IList<Employee> GetEmployees()
        {
            var employees = new List<Employee>();

            try
            {
                using var connection = _dbFactory.CreateConnection();
                connection!.ConnectionString = _connectionString;
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Employees";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(ReadEmployee(reader));
                }

                return employees;
            }
            catch (Exception ex)
            {
                throw new EmployeeServiceException("Failed to retrieve employees.", ex);
            }
        }

        public Employee GetEmployee(long employeeId)
        {
            try
            {
                using var connection = _dbFactory.CreateConnection();
                connection!.ConnectionString = _connectionString;
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Employees WHERE EmployeeID = @id";

                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = employeeId;
                command.Parameters.Add(idParam);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadEmployee(reader);
                }

                throw new EmployeeServiceException("Employee not found.");
            }
            catch (Exception ex)
            {
                throw new EmployeeServiceException("Failed to retrieve employee.", ex);
            }
        }

        public long AddEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            try
            {
                using var connection = _dbFactory.CreateConnection();
                connection!.ConnectionString = _connectionString;
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Employees 
                    (FirstName, LastName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode,
                     Country, HomePhone, Extension, Notes, ReportsTo, PhotoPath) 
                    VALUES 
                    (@FirstName, @LastName, @Title, @TitleOfCourtesy, @BirthDate, @HireDate, @Address, @City, @Region, @PostalCode,
                     @Country, @HomePhone, @Extension, @Notes, @ReportsTo, @PhotoPath);
                    SELECT last_insert_rowid();";

                AddParameters(command, employee);

                var result = command.ExecuteScalar();
                return result is long id ? id : throw new EmployeeServiceException("Inserting an employee failed.");
            }
            catch (Exception ex)
            {
                throw new EmployeeServiceException("Failed to add employee.", ex);
            }
        }

        public void RemoveEmployee(long employeeId)
        {
            try
            {
                using var connection = _dbFactory.CreateConnection();
                connection!.ConnectionString = _connectionString;
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Employees WHERE EmployeeID = @id";

                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = employeeId;
                command.Parameters.Add(idParam);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new EmployeeServiceException("Failed to remove employee.", ex);
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            try
            {
                using var connection = _dbFactory.CreateConnection();
                connection!.ConnectionString = _connectionString;
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Employees SET 
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Title = @Title,
                        TitleOfCourtesy = @TitleOfCourtesy,
                        BirthDate = @BirthDate,
                        HireDate = @HireDate,
                        Address = @Address,
                        City = @City,
                        Region = @Region,
                        PostalCode = @PostalCode,
                        Country = @Country,
                        HomePhone = @HomePhone,
                        Extension = @Extension,
                        Notes = @Notes,
                        ReportsTo = @ReportsTo,
                        PhotoPath = @PhotoPath
                    WHERE EmployeeID = @Id";

                AddParameters(command, employee);

                var idParam = command.CreateParameter();
                idParam.ParameterName = "@Id";
                idParam.Value = employee.Id;
                command.Parameters.Add(idParam);

                int affectedRows = command.ExecuteNonQuery();
                if (affectedRows == 0)
                {
                    throw new EmployeeServiceException("Employee not found or not updated.");
                }
            }
            catch (Exception ex)
            {
                throw new EmployeeServiceException("Failed to update employee.", ex);
            }
        }

        private static void AddParameters(DbCommand command, Employee e)
        {
            command.Parameters.Add(CreateParameter(command, "@FirstName", e.FirstName));
            command.Parameters.Add(CreateParameter(command, "@LastName", e.LastName));
            command.Parameters.Add(CreateParameter(command, "@Title", e.Title));
            command.Parameters.Add(CreateParameter(command, "@TitleOfCourtesy", e.TitleOfCourtesy));
            command.Parameters.Add(CreateParameter(command, "@BirthDate", e.BirthDate));
            command.Parameters.Add(CreateParameter(command, "@HireDate", e.HireDate));
            command.Parameters.Add(CreateParameter(command, "@Address", e.Address));
            command.Parameters.Add(CreateParameter(command, "@City", e.City));
            command.Parameters.Add(CreateParameter(command, "@Region", e.Region));
            command.Parameters.Add(CreateParameter(command, "@PostalCode", e.PostalCode));
            command.Parameters.Add(CreateParameter(command, "@Country", e.Country));
            command.Parameters.Add(CreateParameter(command, "@HomePhone", e.HomePhone));
            command.Parameters.Add(CreateParameter(command, "@Extension", e.Extension));
            command.Parameters.Add(CreateParameter(command, "@Notes", e.Notes));
            command.Parameters.Add(CreateParameter(command, "@ReportsTo", e.ReportsTo));
            command.Parameters.Add(CreateParameter(command, "@PhotoPath", e.PhotoPath));
        }

        private static DbParameter CreateParameter(DbCommand command, string name, object? value)
        {
            var param = command.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            return param;
        }

        private static Employee ReadEmployee(IDataReader reader)
        {
            var id = reader.GetInt64(reader.GetOrdinal("EmployeeID"));
            var employee = new Employee(id)
            {
                FirstName = reader["FirstName"] as string ?? string.Empty,
                LastName = reader["LastName"] as string ?? string.Empty,
                Title = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : null,
                TitleOfCourtesy = reader["TitleOfCourtesy"] != DBNull.Value ? reader["TitleOfCourtesy"].ToString() : null,
                BirthDate = reader["BirthDate"] != DBNull.Value ? Convert.ToDateTime(reader["BirthDate"], CultureInfo.InvariantCulture) : null,
                HireDate = reader["HireDate"] != DBNull.Value ? Convert.ToDateTime(reader["HireDate"], CultureInfo.InvariantCulture) : null,
                Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
                City = reader["City"] != DBNull.Value ? reader["City"].ToString() : null,
                Region = reader["Region"] != DBNull.Value ? reader["Region"].ToString() : null,
                PostalCode = reader["PostalCode"] != DBNull.Value ? reader["PostalCode"].ToString() : null,
                Country = reader["Country"] != DBNull.Value ? reader["Country"].ToString() : null,
                HomePhone = reader["HomePhone"] != DBNull.Value ? reader["HomePhone"].ToString() : null,
                Extension = reader["Extension"] != DBNull.Value ? reader["Extension"].ToString() : null,
                Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : null,
                ReportsTo = reader["ReportsTo"] != DBNull.Value ? (long?)Convert.ToInt64(reader["ReportsTo"], CultureInfo.InvariantCulture) : null,
                PhotoPath = reader["PhotoPath"] != DBNull.Value ? reader["PhotoPath"].ToString() : null
            };

            return employee;
        }
    }
}
