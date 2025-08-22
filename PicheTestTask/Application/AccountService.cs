using Microsoft.EntityFrameworkCore;
using PicheTestTask.Data;
using PicheTestTask.Domain.Models;
using System.Security.Cryptography;

namespace PicheTestTask.Application
{
    // Service for handling account endpoints
    public class AccountService
    {
        private readonly Context _context;

        public AccountService(Context context)
        {
            _context = context;
        }

        // Create a new account with default balance and generated account number
        public async Task<AccountDTO> CreateAccount(string ownerName)
        {
            var account = new Account
            {
                OwnerName = ownerName,
                Balance = 100, // default initial balance
                AccountNumber = GenerateAccountNumber()
            };

            _context.Accounts.Add(account);

            await _context.SaveChangesAsync();

            return new AccountDTO
            {
                AccountNumber = account.AccountNumber,
                OwnerName = account.OwnerName,
                Balance = account.Balance
            };
        }

        // Generate random 16-digit account number
        private static string GenerateAccountNumber()
        {
            var rng = RandomNumberGenerator.Create(); // cryptographically secure RNG
            var bytes = new byte[16];
            rng.GetBytes(bytes);

            var digits = new char[16];
            for (int i = 0; i < 16; i++)
            {
                digits[i] = (char)('0' + (bytes[i] % 10)); // map each byte to digit 0–9
            }

            return new string(digits); // build string of 16 digits
        }

        // Get account details by account number
        public async Task<AccountDTO?> GetAccountByNumber(string accountNumber)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                return null;
            }

            return new AccountDTO
            {
                AccountNumber = account.AccountNumber,
                OwnerName = account.OwnerName,
                Balance = account.Balance
            };
        }

        // Get list of all accounts
        public async Task<IEnumerable<AccountDTO>> GetAllAccounts()
        {
            return await _context.Accounts
                .Select(a => new AccountDTO
                {
                    AccountNumber = a.AccountNumber,
                    OwnerName = a.OwnerName,
                    Balance = a.Balance
                })
                .ToListAsync();
        }

        // Deposit money into an account
        public async Task<bool> Deposit(string accountNumber, decimal amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null) return false;

            account.Balance += amount;
            await _context.SaveChangesAsync();
            return true;
        }

        // Withdraw money from an account
        public async Task<bool> Withdraw(string accountNumber, decimal amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null || account.Balance < amount)
            {
                return false;
            }

            account.Balance -= amount;
            await _context.SaveChangesAsync();
            return true;
        }

        // Transfer money between two accounts
        public async Task<bool> Transfer(string fromAccountNumber, string toAccountNumber, decimal amount)
        {
            if (amount <= 0 || fromAccountNumber == toAccountNumber) return false;

            var from = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == fromAccountNumber);

            var to = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == toAccountNumber);

            if (from == null || to == null || from.Balance < amount) return false;

            from.Balance -= amount;
            to.Balance += amount;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
