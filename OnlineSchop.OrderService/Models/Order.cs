using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSchop.OrderService.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        // Для PostgreSQL лучше использовать JsonB для хранения JSON
        [Column(TypeName = "jsonb")]
        public Dictionary<string, object> Metadata { get; set; }
    }

    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        
        public virtual Order Order { get; set; }
    }

    public enum OrderStatus
    {
        Created,
        Validated,
        Processing,
        Completed,
        Failed
    }
}