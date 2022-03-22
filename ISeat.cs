using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace StudyCafeManager
{
    interface ISeat
    {
        string SeatNum { get; }
        string Status { get; set; }

    }
}
