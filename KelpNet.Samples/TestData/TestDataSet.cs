﻿using KelpNet.Common;

namespace KelpNet.Samples.TestData
{
    public class TestDataSet
    {
        public NdArray Data;
        public NdArray Label;

        public TestDataSet(NdArray data, NdArray label)
        {
            this.Data = data;
            this.Label = label;
        }
    }
}
