using System;

namespace Klika.Dinero.Model.Entities
{
    public interface IEntityBase
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}