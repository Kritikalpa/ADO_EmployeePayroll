using EmployeePayrollService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeePayrollTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GivenSalaryDetails_AbleToUpdateDetails()
        {
            EmployeeRepo employeeRepo = new EmployeeRepo();
            SalaryUpdateModel updateModel = new SalaryUpdateModel()
            {
                EmployeeId = 101,
                EmployeeName = "Cyan",
                EmployeeSalary = 1500.34
            };

            double EmpSalary = employeeRepo.UpdateEmployeeSalary(updateModel);

            Assert.AreEqual(updateModel.EmployeeSalary, EmpSalary);
        }
    }
}
