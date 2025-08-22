using Microsoft.AspNetCore.Mvc;
using PicheTestTask.Application;
using PicheTestTask.Domain.Models;

namespace PicheTestTask.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        // GET /account/{accountNumber}
        // Get account details by account number
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<AccountDTO>> GetAccountByNumber(string accountNumber)
        {
            var account = await _accountService.GetAccountByNumber(accountNumber);
            if (account == null)
                return NotFound($"Account with number {accountNumber} not found.");

            return Ok(account);
        }

        // POST /account
        // Create a new account
        [HttpPost]
        public async Task<ActionResult> CreateAccount([FromBody] string ownerName)
        {
            var account = await _accountService.CreateAccount(ownerName);
            return Ok(account);
        }

        // GET /account
        // Get list of all accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccounts();
            return Ok(accounts);
        }

        // POST /account/{accountNumber}/deposit?amount=100
        // Deposit funds into account
        [HttpPost("{accountNumber}/deposit")]
        public async Task<ActionResult> Deposit(string accountNumber, [FromQuery] decimal amount)
        {
            var result = await _accountService.Deposit(accountNumber, amount);
            return result
                ? Ok($"Deposited {amount} to {accountNumber}")
                : BadRequest("Deposit failed");                 
        }

        // POST /account/{accountNumber}/withdraw?amount=50
        // Withdraw funds from account
        [HttpPost("{accountNumber}/withdraw")]
        public async Task<ActionResult> Withdraw(string accountNumber, [FromQuery] decimal amount)
        {
            var result = await _accountService.Withdraw(accountNumber, amount);
            return result
                ? Ok($"Withdrew {amount} from {accountNumber}")
                : BadRequest("Withdraw failed");
        }

        // POST /account/transfer?from=123&to=456&amount=100
        // Transfer funds between two accounts
        [HttpPost("transfer")]
        public async Task<ActionResult> Transfer([FromQuery] string from, [FromQuery] string to, [FromQuery] decimal amount)
        {
            var result = await _accountService.Transfer(from, to, amount);
            return result
                ? Ok($"Transferred {amount} from {from} to {to}")
                : BadRequest("Transfer failed");
        }

    }

}
