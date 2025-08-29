

namespace PayDayExplosion.Domain.Data.Common
{
    public abstract class Base
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }



    }
}
