using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Config
{
    public class MongoConnection
    {
        public string ConnString;
        public MongoConnection(string connString)
        {
            ConnString = connString;
        }
    }
}
