using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Models
{
    public class UserData
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(60)]
        public string FirstName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string PersonalId { get; set; }
        
        public string MobileNumber { get; set; }
        [Required]
        [MaxLength(60)]
        public string LastName { get; set; }
        [Required]
        public string Sex { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        [Required]
        public List<Account> AccountNumbers { get; set; }
    }
}
