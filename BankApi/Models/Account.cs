using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BankApi.Models
{
    public class Account
    {
        public int Id { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public UserData User { get; set; }



    }
}
