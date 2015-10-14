using System;
using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.ARM.Shared
{
    public interface IEmployee : IDto, IIdentityObject
    {
        string EmpId { get; set; }
        string EmployeeFID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }

        DateTimeOffset JoiningDate { get; set; }
        DateTimeOffset RelivedOn { get; set; }
    }
}
