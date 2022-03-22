using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace StudyCafeManager
{
    [Serializable]
    abstract class Person
    {
        protected string name;
        protected string email;
        protected string pw;

        #region<Property>
        public string Name
        {
            get { return name; }
        }
        public string Email
        {
            get { return email; }
        }
        public string PW
        {
            get { return pw; }
        }
        #endregion
    }
}
