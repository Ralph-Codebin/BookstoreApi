﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_api.Requests
{
    public class DUpdateSubscriptiontRequest
    {
        public int id { get; set; }
        public int state { get; set; }
    }
}
