namespace Orfe.Examples.ResultExtensions
{
    public class ExampleFromPluralsightCourse
    {
        public string Promote(long id)
        {
            var gateway = new EmailGateway();

            return GetById(id)
                .ToResult("Customer with such Id is not found: " + id)
                .Ensure(customer => customer.CanBePromoted(), "The customer has the highest status possible")
                .Tap(customer => customer.Promote())
                .Bind(customer => gateway.SendPromotionNotification(customer.Email))
                .Finally(result => result.IsSuccess ? "Ok" : result.Error);
        }

        public Customer? GetById(long id)
        {
            return new Customer();
        }

        public class Customer
        {
            public string? Email { get; }

            public Customer()
            {
            }

            public bool CanBePromoted()
            {
                return true;
            }

            public void Promote()
            {
            }
        }

        private class EmailGateway
        {
            public Result<Unit,string> SendPromotionNotification(string? email)
            {
                return Result.Success();
            }
        }
    }
}
