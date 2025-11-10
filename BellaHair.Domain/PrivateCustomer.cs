namespace BellaHair.Domain
{
    public class PrivateCustomer : EntityBase
    {
        //Udregn denne ud fra mængden af fuldførte bookings, get => Bookings.Count ??
        public int Visits { get; private set; }

        private PrivateCustomer()
        {
            Id = Guid.NewGuid();
        }

        public static PrivateCustomer Create()
        {
            return new PrivateCustomer();
        }
    }
}
