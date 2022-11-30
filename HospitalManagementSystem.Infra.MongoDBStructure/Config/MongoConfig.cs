using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Infra.MongoDBStructure.Config
{
    public class MongoConfig
    {
        public string ConnectionString { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string DbName { get; set; }
    }
}
