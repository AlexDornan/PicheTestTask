using System.ComponentModel.DataAnnotations;

namespace PicheTestTask.Domain.Models
{
    public class Account
    {
        [Required]
        public int AccountId {  get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string OwnerName { get; set; }
        [Required]
        public decimal Balance { get; set; }
    }
}
