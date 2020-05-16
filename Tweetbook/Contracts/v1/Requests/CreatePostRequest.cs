﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Tweetbook.Contracts.v1.Requests
{
    public class CreatePostRequest
    {
        public string Name { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}