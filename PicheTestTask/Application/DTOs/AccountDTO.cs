using System.ComponentModel.DataAnnotations;

namespace PicheTestTask.Domain.Models
{
    public class AccountDTO
    {
        public string AccountNumber { get; set; }
        public string OwnerName { get; set; }
        public decimal Balance { get; set; }
    }
}
