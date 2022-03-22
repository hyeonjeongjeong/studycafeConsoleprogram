using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace StudyCafeManager
{
    [Serializable]
    class Room : ISeat
    {
        private string status;
        private string seatnum;
        public string SeatNum
        {
            get { return seatnum; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public Room(string seatnum)
        {
            this.seatnum = seatnum;
        }
    }
}
