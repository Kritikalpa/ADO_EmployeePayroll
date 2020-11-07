using EmployeePayrollService.Model.SalaryModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EmployeePayrollService
{
    public class EmployeeRepo
    {
        public static string connectionString = "Data Source=(LocalDb)\\testServer;Initial Catalog=payroll_services;Integrated Security=True";

        public void getAllEmployee(int choice)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                using (connection)
                {
                    string query = "";
                    switch (choice)
                    {
                        case 1:
                            query = @"SELECT id,name,phone_number,address,department,gender,
                                    basic_pay,deductions,taxable_pay,tax,net_pay,start_Date 
                                    FROM employee_payroll";
                            break;

                        case 2:
                            query = @"SELECT id,name,phone_number,address,department,gender,
                                    basic_pay,deductions,taxable_pay,tax,net_pay,start_Date 
                                    FROM employee_payroll WHERE start_Date between CAST('2020-10-27' AS DATE) and GETDATE()";
                            break;

                        case 3:
                            query = @"SELECT SUM(net_pay), MAX(net_pay), MIN(net_pay), 
                                    AVG(net_pay), COUNT(name), gender FROM employee_payroll GROUP BY gender";
                            break;

                        default:
                            break;
                    }



                    SqlCommand cmd = new SqlCommand(query, connection);

                    connection.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows && choice == 3)
                    {
                        while (dr.Read())
                        {
                            Console.WriteLine("Sum : {0}, Avg : {1}, Max : {2}, Min : {3}, Count : {4}, Gender : {5}", dr.GetDecimal(0), dr.GetDecimal(1), dr.GetDecimal(2), dr.GetDecimal(3), dr.GetInt32(4), Convert.ToChar(dr.GetString(5)));
                            Console.WriteLine("\n");
                        }

                    }
                    else if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            employeeModel.EmployeeID = dr.GetInt32(0);
                            employeeModel.EmployeeName = dr.GetString(1);
                            employeeModel.PhoneNumber = dr.GetString(2);
                            employeeModel.Address = dr.GetString(3);
                            employeeModel.Department = dr.GetString(4);
                            employeeModel.Gender = Convert.ToChar(dr.GetString(5));
                            employeeModel.BasicPay = Convert.ToDouble(dr.GetDecimal(6));
                            employeeModel.Deductions = Convert.ToDouble(dr.GetDecimal(7));
                            employeeModel.TaxablePay = Convert.ToDouble(dr.GetDecimal(8));
                            employeeModel.Tax = Convert.ToDouble(dr.GetDecimal(9));
                            employeeModel.NetPay = Convert.ToDouble(dr.GetDecimal(10));
                            employeeModel.StartDate = dr.GetDateTime(11);

                            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", employeeModel.EmployeeID, employeeModel.EmployeeName, employeeModel.Address, employeeModel.Department, employeeModel.BasicPay);
                            Console.WriteLine("\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found");
                    }

                    dr.Close();

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool addEmployee(EmployeeModel employeeModel)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand("SpAddEmployeeDetails", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@name", employeeModel.EmployeeName);
                    command.Parameters.AddWithValue("@phone_number", employeeModel.PhoneNumber);
                    command.Parameters.AddWithValue("@address", employeeModel.Address);
                    command.Parameters.AddWithValue("@department", employeeModel.Department);
                    command.Parameters.AddWithValue("@gender", employeeModel.Gender);
                    command.Parameters.AddWithValue("@basic_pay", employeeModel.BasicPay);
                    command.Parameters.AddWithValue("@deductions", employeeModel.Deductions);
                    command.Parameters.AddWithValue("@taxable_pay", employeeModel.TaxablePay);
                    command.Parameters.AddWithValue("@tax", employeeModel.Tax);
                    command.Parameters.AddWithValue("@net_pay", employeeModel.NetPay);
                    command.Parameters.AddWithValue("@start_Date", employeeModel.StartDate);
                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public double UpdateEmployeeSalary(SalaryUpdateModel salaryUpdateModel)
        {
            SqlConnection SalaryConnection = new SqlConnection(connectionString);
            double salary = 0;
            try
            {
                using (SalaryConnection)
                {
                    SalaryDetailModel displayModel = new SalaryDetailModel();
                    SqlCommand command = new SqlCommand("spUpdateEmployeeSalary", SalaryConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", salaryUpdateModel.EmployeeId);
                    command.Parameters.AddWithValue("@name", salaryUpdateModel.EmployeeName);
                    command.Parameters.AddWithValue("@salary", salaryUpdateModel.EmployeeSalary);
                    SalaryConnection.Open();

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            displayModel.EmployeeId = dr.GetInt32(0);
                            displayModel.EmployeeName = dr.GetString(1);
                            displayModel.SalaryId = dr.GetInt32(2);
                            displayModel.EmployeeSalary = Convert.ToDouble(dr.GetDecimal(3));


                            Console.WriteLine("{0}, {1}, {2}, {3}", displayModel.EmployeeId, displayModel.EmployeeName, displayModel.SalaryId, displayModel.EmployeeSalary);
                            Console.WriteLine("\n");

                            salary = displayModel.EmployeeSalary;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found");
                    }

                    dr.Close();

                    SalaryConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                SalaryConnection.Close();
            }
            return salary;
        }
    }
}
