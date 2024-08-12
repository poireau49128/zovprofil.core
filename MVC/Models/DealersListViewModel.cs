using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;

namespace MVC.Models
{
    public class DealersListViewModel
    {
        public IEnumerable<Dealer> Dealers { get; set; }
        public SelectList Country { get; set; }
        public SelectList City { get; set; }
    }
}
