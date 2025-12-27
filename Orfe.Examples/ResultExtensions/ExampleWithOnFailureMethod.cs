using System.Threading.Tasks;

namespace Orfe.Examples.ResultExtensions
{
    public class ExampleWithTapErrorMethod
    {
        public string TapError_non_async(int customerId, decimal moneyAmount)
        {
            var paymentGateway = new PaymentGateway();
            var database = new Database();

            return GetById(customerId)
                .ToResult("Customer with such Id is not found: " + customerId)
                .Tap(customer => customer.AddBalance(moneyAmount))
                .Check(customer => paymentGateway.ChargePayment(customer, moneyAmount))
                .Bind(customer => database.Save(customer)
                        .TapError(() => paymentGateway.RollbackLastTransaction()))
                .Finally(result => result.IsSuccess ? "OK" : result.Error);
        }

        private Customer? GetById(long id)
        {
            return new Customer();
        }

        private class Customer
        {
            public void AddBalance(decimal moneyAmount)
            {

            }
        }

        private class PaymentGateway
        {
            public Result<Unit,string> ChargePayment(Customer customer, decimal moneyAmount)
            {
                return Result.Success();
            }

            public void RollbackLastTransaction()
            {

            }

            public Task RollbackLastTransactionAsync()
            {
                return Task.FromResult(1);
            }
        }

        private class Database
        {
            public Result<Unit,string> Save(Customer customer)
            {
                return Result.Success();
            }
        }
    }
}
