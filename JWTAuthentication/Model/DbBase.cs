using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Model
{
    public abstract class DbBase
    {
        public DateTime ModifiedDate { get; set; }
        public int ModifiedByUserID { get; set; }
    }
}
