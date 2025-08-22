using Microsoft.EntityFrameworkCore;
using PicheTestTask.Application;
using PicheTestTask.Data;
using Xunit;

namespace PicheTestTask.Tests
{
    // Unit tests for AccountService
    public class AccountServiceTests
    {
        // Helper method: creates AccountService with InMemory EF Core context
        private AccountService GetService(out Context context)
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new Context(options);
            return new AccountService(context);
        }

        // test for account creation
        [Fact]
        public async Task CreateAccount_ShouldCreate_WithInitialBalance()
        {
            var service = GetService(out var context);

            var acc = await service.CreateAccount("Alex");

            Assert.NotNull(acc);
            Assert.Equal("Alex", acc.OwnerName);
            Assert.Equal(100, acc.Balance);
            Assert.Equal(16, acc.AccountNumber.Length);
        }

        // test for deposit operation
        [Fact]
        public async Task Deposit_ShouldIncreaseBalance()
        {
            var service = GetService(out var context);
            var acc = await service.CreateAccount("Test");

            var result = await service.Deposit(acc.AccountNumber, 50);

            Assert.True(result);
            Assert.Equal(150, context.Accounts.First().Balance);
        }

        // test for withdraw operation
        [Fact]
        public async Task Withdraw_ShouldDecreaseBalance()
        {
            var service = GetService(out var context);
            var acc = await service.CreateAccount("Test");

            var result = await service.Withdraw(acc.AccountNumber, 50);

            Assert.True(result);
            Assert.Equal(50, context.Accounts.First().Balance);
        }

        // test for withdraw with insufficient funds
        [Fact]
        public async Task Withdraw_ShouldFail_WhenInsufficientFunds()
        {
            var service = GetService(out var context);
            var acc = await service.CreateAccount("Test");

            var result = await service.Withdraw(acc.AccountNumber, 200);

            Assert.False(result);
        }

        // test for transfer operation
        [Fact] 
        public async Task Transfer_ShouldMoveFundsBetweenAccounts()
        {
            var service = GetService(out var context);
            var acc1 = await service.CreateAccount("Alice");
            var acc2 = await service.CreateAccount("Bob");

            var result = await service.Transfer(acc1.AccountNumber, acc2.AccountNumber, 50);

            Assert.True(result);
            Assert.Equal(50, context.Accounts.First(a => a.OwnerName == "Alice").Balance);
            Assert.Equal(150, context.Accounts.First(a => a.OwnerName == "Bob").Balance);
        }
    }
}
