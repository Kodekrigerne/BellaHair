namespace BellaHair.Ports.Discounts
{
    //Dennis
    /// <summary>
    /// Exposes general/common queries related to discounts
    /// </summary>
    /// <remarks>
    /// Do not use this for queries related to specific types of discounts
    /// </remarks>
    public interface IDiscountQuery
    {
        Task<BookingDiscountDTO?> FindBestDiscount(FindBestDiscountQuery query);
    }

    public record FindBestDiscountQuery(DateTime StartDateTime, Guid EmployeeId, Guid CustomerId, Guid TreatmentId, bool IncludeBirthdayDiscount,Guid? BookingId = null);

    public record BookingDiscountDTO(string Name, decimal Amount, DiscountTypeDTO Type);
}
