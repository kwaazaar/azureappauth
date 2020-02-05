﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AppAuthTest
{
    class AppAuthConfig
    {
        public string TenantId { get; set; }
        public string Resource { get; set; }
        public string AzureCliConnString { get; set; }
        public string ClientCredentialsConnString { get; set; }
        public string MSIConnString { get; set; }
        public string TestUrlGet { get; set; }
    }
}
