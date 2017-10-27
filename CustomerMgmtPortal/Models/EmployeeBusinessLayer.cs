using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomerMgmtPortal.DAL;
using System.Data.Entity;

namespace CustomerMgmtPortal.Models
{
    public class EmployeeBusinessLayer
    {
        readonly SalesERPDAL dalObj = new SalesERPDAL();
        public List<Employee> GetEmployees()
        {            
            return dalObj.Employees.ToList();            
        }

        public Employee SaveEmployee(Employee e)
        {
            dalObj.Employees.Add(e);
            dalObj.SaveChanges();
            return e;
        }

        public Employee FindEmployee(int id)
        {
            return dalObj.Employees.Find(id);
        }

        public Employee EditEmployee(Employee emp)
        {
            dalObj.Entry(emp).State = EntityState.Modified;
            dalObj.SaveChanges();
            return emp;
        }

        public void DeleteEmployee(int id)
        {
            Employee emp = dalObj.Employees.Find(id);
            if(emp != null)
            { 
              dalObj.Employees.Remove(emp);
              dalObj.SaveChanges();
            }
        }        

        public UserStatus IsValidUser(UserDetails uDetail)
        {
            if (uDetail.UserName == "admin" && uDetail.Password == "password")
                return UserStatus.AuthenticatedAdmin;
            else if (uDetail.UserName == "Manoj" && uDetail.Password == "password")
                return UserStatus.AuthenticatedUser;
            else
                return UserStatus.NonAuthenticatedUser;
        }
    }
}