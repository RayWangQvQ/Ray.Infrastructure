using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ray.DDD
{
    public class DataFilterState
    {
        public bool IsEnabled { get; set; }

        public DataFilterState(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}
