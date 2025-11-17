using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Ports.Employees;

namespace BellaHair.Application
{
    class EmployeeCommandHandler : IEmployeeCommand
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly ITreatmentRepository _treatmentRepo;

        public EmployeeCommandHandler(IEmployeeRepository employeeRepo, ITreatmentRepository treatmentRepo)
        {
            _employeeRepo = employeeRepo;
            _treatmentRepo = treatmentRepo;
        }

        async Task IEmployeeCommand.CreateEmployeeCommand(CreateEmployeeCommand command)
        {
            var name = Name.FromStrings(command.FirstName, command.MiddleName, command.LastName);
            var email = Email.FromString(command.Email);
            var phoneNumber = PhoneNumber.FromString(command.PhoneNumber);
            var address = Address.Create(command.StreetName, command.City, command.StreetNumber, command.ZipCode, command.Floor);
            var treatments = await _treatmentRepo.Get(command.TreatmentIds);

            var employee = Employee.Create(name, email, phoneNumber, address, treatments);

            await _employeeRepo.AddAsync(employee);

            await _employeeRepo.SaveChangesAsync();
        }

        async Task IEmployeeCommand.DeleteEmployeeCommand(DeleteEmployeeCommand command)
        {
            var employee = await _employeeRepo.GetAsync(command.Id);

            _employeeRepo.Delete(employee);

            await _employeeRepo.SaveChangesAsync();
        }
    }
}
