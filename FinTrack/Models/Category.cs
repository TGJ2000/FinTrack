using System.Data.SqlTypes;

namespace FinTrack.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { set; get; } = false;
    }
}
