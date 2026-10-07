// Copyright (c) 2024, Awessets

using System;

namespace MergeIt.SimpleDI
{
    public class IntroduceAttribute : Attribute
    {
        private string _key;
        
        public IntroduceAttribute(string key = "")
        {
            _key = key;
        }
    }
}