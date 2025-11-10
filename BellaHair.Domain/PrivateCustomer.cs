namespace BellaHair.Domain
{
    public class PrivateCustomer : EntityBase
    {
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
