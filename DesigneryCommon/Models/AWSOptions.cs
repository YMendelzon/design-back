﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCommon.Models
{
    public class AWSOptions
    {
        public string SecretKey { get; set; }
        public string AccessKey { get; set; }
        public string Region { get; set; }
        public string BucketName { get; set; }
    }

}
