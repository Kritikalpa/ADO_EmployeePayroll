﻿using System;

namespace EmployeePayrollService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Employee Payroll");
            EmployeeModel employeeModel = new EmployeeModel();
            EmployeeRepo employeeRepo = new EmployeeRepo();

            employeeModel.EmployeeName = "Purple";
            employeeModel.PhoneNumber = "3216549870";
            employeeModel.Address = "Alaska";
            employeeModel.Department = "Marketing";
            employeeModel.Gender = 'F';
            employeeModel.BasicPay = 280000;
            employeeModel.Deductions = 4357.54;
            employeeModel.TaxablePay = 3256.35;
            employeeModel.Tax = 756.23;
            employeeModel.NetPay = 273234.23;
            employeeModel.StartDate = Convert.ToDateTime("2020-10-13");

            employeeRepo.addEmployee(employeeModel);


            employeeRepo.getAllEmployee();
        }
    }
}
