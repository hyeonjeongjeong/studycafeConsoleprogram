using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace StudyCafeManager
{
    [Serializable]
    class PrivateSeat : ISeat
    {
        private string seatnum;
        private string status;
        public string SeatNum 
        {
            get { return seatnum; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public PrivateSeat(string seatnum)
        {
            this.seatnum = seatnum;
        }

        
    }
}
