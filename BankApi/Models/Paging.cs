using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Models
{
    public class Paging
    {
        private const int _maxItemPerPage = 30;
        private int _itemsPerPage;
        public int Page { get; set; } = 1;
        public int ItemsPerPage
        {
            get => _itemsPerPage;
            set => _itemsPerPage = value > _maxItemPerPage ? _itemsPerPage : value;
        }
    }
}
