using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Models
{
    public class CustomerEmployeeViewModel
    {
        public EmployeesModel Employee { get; set; }
        public List<CustomersModel> Customers { get; set; }
    }
}
