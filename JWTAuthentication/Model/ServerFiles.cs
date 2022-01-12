using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JWTAuthentication.Model.Enums;

namespace JWTAuthentication.Model
{
    public class ServerFiles : DbBase
    {
        public int ID { get; set; }
        public OwnerType OwnerType { get; set; }
        public int OwnerID { get; set; }
        public FileType FileType { get; set; }
        public string FilePath { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
