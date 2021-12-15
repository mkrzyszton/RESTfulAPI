using System.ComponentModel.DataAnnotations;

namespace Products.Models
{
    public class Products
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price{ get; set; }
        public int Amount { get; set; }
    }
}
