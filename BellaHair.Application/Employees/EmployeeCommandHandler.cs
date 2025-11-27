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

// Linnea

namespace BellaHair.Application.Employees
{
    public class EmployeeCommandHandler : IEmployeeCommand
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly ITreatmentRepository _treatmentRepo;
        private readonly IEmployeeFutureBookingsChecker _employeeFutureBookingsChecker;

        public EmployeeCommandHandler(IEmployeeRepository employeeRepo, ITreatmentRepository treatmentRepo, IEmployeeFutureBookingsChecker employeeFutureBookingsChecker)
        {
            _employeeRepo = employeeRepo;
            _treatmentRepo = treatmentRepo;
            _employeeFutureBookingsChecker = employeeFutureBookingsChecker;
        }

        public async Task UpdateEmployeeAsync(UpdateEmployeeCommand command)
        {
            var employeeToUpdate = await _employeeRepo.GetWithTreatmentsAsync(command.Id);

            var updatedName = Name.FromStrings(
                command.FirstName,
                command.LastName,
                command.MiddleName);

            var updatedAddress = Address.Create(
                command.StreetName,
                command.City,
                command.StreetNumber,
                command.ZipCode,
                command.Floor);

            var updatedTreatments = await _treatmentRepo.GetAsync(command.TreatmentIds);

            var updatedPhoneNumber = PhoneNumber.FromString(command.PhoneNumber);
            var updatedEmail = Email.FromString(command.Email);

            employeeToUpdate.Update(
                updatedName,
                updatedEmail,
                updatedPhoneNumber,
                updatedAddress,
                updatedTreatments);

            await _employeeRepo.SaveChangesAsync();
        }

        async Task IEmployeeCommand.CreateEmployeeCommand(CreateEmployeeCommand command)
        {
            var name = Name.FromStrings(command.FirstName, command.LastName, command.MiddleName);
            var email = Email.FromString(command.Email);
            var phoneNumber = PhoneNumber.FromString(command.PhoneNumber);
            var address = Address.Create(command.StreetName, command.City, command.StreetNumber, command.ZipCode, command.Floor);
            var treatments = await _treatmentRepo.GetAsync(command.TreatmentIds);

            var employee = Employee.Create(name, email, phoneNumber, address, treatments.ToList());

            await _employeeRepo.AddAsync(employee);

            await _employeeRepo.SaveChangesAsync();
        }

        async Task IEmployeeCommand.DeleteEmployeeCommand(DeleteEmployeeCommand command)
        {
            var employee = await _employeeRepo.GetAsync(command.Id);
            if (await _employeeFutureBookingsChecker.EmployeeHasFutureBookings(command.Id))
                throw new EmployeeException("Medarbejderen har fremtidige bookinger. Du er nødt til at fjerne dem, før du kan slette medarbejderen.");

            _employeeRepo.Delete(employee);

            await _employeeRepo.SaveChangesAsync();
        }
    }
}
