using BellaHair.Ports.Bookings;
using BellaHair.Ports.Discounts;
using BellaHair.Ports.PrivateCustomers;
using BellaHair.Ports.Treatments;
using BellaHair.Presentation.WebUI.Components.Pages;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BlazorTestProject1
{
    public class FrontPageTest : BunitContext
    {
        [Fact]
        public void CampaginCountShowsCorrect()
        {
            // Arrange
            Services.AddSingleton<ICampaignDiscountQuery>(new FakeCampaignQuery());
            Services.AddSingleton<IPrivateCustomerQuery>(new FakeCustomerQuery());
            Services.AddSingleton<IBookingQuery>(new FakeBookingQuery());
            Services.AddSingleton<ITreatmentQuery>(new FakeTreatmentQuery());
            Services.AddSingleton<ISnackbar>(new FakeSnackbar());

            // Act & Assert
            var cut = Render<Home>();
            cut.Markup.Contains("10 kunder");
        }
    }

    public class FakeCampaignQuery : ICampaignDiscountQuery
    {
        async Task<int> ICampaignDiscountQuery.GetActiveCountAsync()
        {
            return 5;
        }

        async Task<List<CampaignDiscountDTO>> ICampaignDiscountQuery.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        async Task<int> ICampaignDiscountQuery.GetCountAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class FakeCustomerQuery : IPrivateCustomerQuery
    {
        async Task<int> IPrivateCustomerQuery.GetCountAsync()
        {
            return 10;
        }

        Task<int> IPrivateCustomerQuery.GetCustomerCountAsync(string? search)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<PrivateCustomerDTO>> IPrivateCustomerQuery.GetCustomersPaginatedAsync(int skip, int take, string? search)
        {
            throw new NotImplementedException();
        }

        Task<PrivateCustomerDTO> IPrivateCustomerQuery.GetPrivateCustomerAsync(GetPrivateCustomerQuery query)
        {
            throw new NotImplementedException();
        }

        Task<List<PrivateCustomerDTO>> IPrivateCustomerQuery.GetPrivateCustomersAsync()
        {
            throw new NotImplementedException();
        }

        Task<bool> IPrivateCustomerQuery.PCFutureBookingsCheck(Guid id)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeBookingQuery : IBookingQuery
    {
        Task<bool> IBookingQuery.BookingHasOverlap(BookingIsAvailableQuery query)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<BookingDTO>> IBookingQuery.GetAllNewAsync(string? search)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<BookingDTO>> IBookingQuery.GetAllOldAsync(string? search)
        {
            throw new NotImplementedException();
        }

        async Task<int> IBookingQuery.GetAllTodayCountAsync()
        {
            return 20;
        }

        Task<IEnumerable<BookingCalendarDTO>> IBookingQuery.GetAllWithinPeriodOnEmployee(DateTime startDateTime, DateTime endDateTime, Guid employeeId)
        {
            throw new NotImplementedException();
        }

        async Task<int> IBookingQuery.GetNewCountAsync(string? search)
        {
            return 20;
        }

        Task<IEnumerable<BookingDTO>> IBookingQuery.GetNewPaginatedAsync(int skip, int take, string? search)
        {
            throw new NotImplementedException();
        }

        Task<int> IBookingQuery.GetOldCountAsync(string? search)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<BookingDTO>> IBookingQuery.GetOldPaginatedAsync(int skip, int take, string? search)
        {
            throw new NotImplementedException();
        }

        Task<BookingWithRelationsDTO> IBookingQuery.GetWithRelationsAsync(GetWithRelationsQuery query)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeTreatmentQuery : ITreatmentQuery
    {
        Task<List<TreatmentDTO>> ITreatmentQuery.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<TreatmentDTO> ITreatmentQuery.GetAsync(GetQuery query)
        {
            throw new NotImplementedException();
        }

        async Task<int> ITreatmentQuery.GetCountAsync()
        {
            return 50;
        }
    }

    public class FakeSnackbar : ISnackbar
    {
        IEnumerable<Snackbar> ISnackbar.ShownSnackbars => throw new NotImplementedException();

        SnackbarConfiguration ISnackbar.Configuration => throw new NotImplementedException();

        event Action? ISnackbar.OnSnackbarsUpdated
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        Snackbar? ISnackbar.Add(string message, Severity severity, Action<SnackbarOptions>? configure, string? key)
        {
            throw new NotImplementedException();
        }

        Snackbar? ISnackbar.Add(MarkupString message, Severity severity, Action<SnackbarOptions>? configure, string? key)
        {
            throw new NotImplementedException();
        }

        Snackbar? ISnackbar.Add(RenderFragment message, Severity severity, Action<SnackbarOptions>? configure, string? key)
        {
            throw new NotImplementedException();
        }

        Snackbar? ISnackbar.Add<[DynamicallyAccessedMembers((DynamicallyAccessedMemberTypes)(-1))] T>(Dictionary<string, object>? componentParameters, Severity severity, Action<SnackbarOptions>? configure, string? key)
        {
            throw new NotImplementedException();
        }

        void ISnackbar.Clear()
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        void ISnackbar.Remove(Snackbar snackbar)
        {
            throw new NotImplementedException();
        }

        void ISnackbar.RemoveByKey(string key)
        {
            throw new NotImplementedException();
        }
    }
}
