﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;

namespace EmployeePayrollService
{
    class EmployeeRepo
    {
        public static string connectionString = "Data Source=(LocalDb)\\testServer;Initial Catalog=payroll_services;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);

        public void getAllEmployee()
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                using (this.connection)
                {
                    string query = @"SELECT id,name,phone_number,address,department,gender,
                                    basic_pay,deductions,taxable_pay,tax,net_pay,start_Date 
                                    FROM employee_payroll";

                    SqlCommand cmd = new SqlCommand(query, this.connection);

                    this.connection.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
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

                    this.connection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                this.connection.Close();
            }
        }

        public bool addEmployee(EmployeeModel employeeModel)
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("SpAddEmployeeDetails", this.connection);
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
                    this.connection.Open();
                    var result = command.ExecuteNonQuery();
                    this.connection.Close();
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
                this.connection.Close();
            }
        }
    }
}
