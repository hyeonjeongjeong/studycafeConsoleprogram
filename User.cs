using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace StudyCafeManager
{
    [Serializable]
    class User : Person
    {
        public User(string name, string email, string pw)
        {
            this.name = name;
            this.email = email;
            this.pw = pw;
        }

        public override string ToString()
        {
            return $"회원 이름 : {name}, 회원 이메일 : {email}"; 
        }
    }
}
