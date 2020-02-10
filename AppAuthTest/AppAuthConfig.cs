using System;
using System.Collections.Generic;
using System.Text;

namespace AppAuthTest
{
    class TestDbConfig
    {
        public string ConnectionString { get; set; }
        public string Query { get; set; }
    }
    class AppAuthConfig
    {
        public string TenantId { get; set; }
        public string Resource { get; set; }
        public string AzureServicesAuthConnectionString { get; set; }
        public string TestUrlGet { get; set; }
        public TestDbConfig TestDb { get; set; }
    }
}
