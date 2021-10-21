using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klika.Dinero.Model.Interfaces.EfRepo
{
    public interface ISoftDeleteableEntity
    {
        public bool IsDeleted { get; set; }
    }
}
