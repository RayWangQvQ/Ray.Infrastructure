using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ray.DDD
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}
