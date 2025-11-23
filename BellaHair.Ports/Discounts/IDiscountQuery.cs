namespace BellaHair.Ports.Discounts
{
    public interface IDiscountQuery
    {
        Task<BookingDiscountDTO?> FindBestDiscount(FindBestDiscountQuery query);
    }

    public record FindBestDiscountQuery(DateTime StartDateTime, Guid EmployeeId, Guid CustomerId, Guid TreatmentId);

    public record BookingDiscountDTO(string Name, decimal Amount);
}
