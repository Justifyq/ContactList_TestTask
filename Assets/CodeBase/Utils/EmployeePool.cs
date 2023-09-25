using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Utils
{
    public class EmployeePool
    {
        public Employee[][] Pool;

        public EmployeePool(IEnumerable<Employee> employees, int itemsInArray) => Split(employees.ToArray(), itemsInArray);

        private void Split(Employee[] employees, int itemsInArray)
        {
            bool needSplit = true;
            int currIndex = 0;

            List<Employee[]> arrays = new List<Employee[]>();

            while (needSplit)
            {
                var all = currIndex + itemsInArray >=  employees.Length;
                arrays.Add(employees[new Range(currIndex, all ? employees.Length : currIndex + itemsInArray)]);
                currIndex += itemsInArray;
                needSplit = !all;
            }

            Pool = arrays.ToArray();
        }
    }
}