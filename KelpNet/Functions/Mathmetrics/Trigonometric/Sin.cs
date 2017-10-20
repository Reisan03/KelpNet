﻿using System;
using KelpNet.Common;
using KelpNet.Common.Functions;

namespace KelpNet.Functions.Mathmetrics.Trigonometric
{
    public class Sin : SingleInputFunction
    {
        private const string FUNCTION_NAME = "Sin";

        public Sin(string name = FUNCTION_NAME) : base(name)
        {
            SingleInputForward = ForwardCpu;
            SingleOutputBackward = BackwardCpu;
        }

        protected NdArray ForwardCpu(NdArray x)
        {
            Real[] resultData = new Real[x.Data.Length];

            for (int i = 0; i < resultData.Length; i++)
            {
                resultData[i] = (Real)Math.Sin(x.Data[i]);
            }

            return new NdArray(resultData, this);
        }

        protected void BackwardCpu(NdArray y, NdArray x)
        {
            for (int i = 0; i < y.Grad.Length; i++)
            {
                x.Grad[i] += (Real)Math.Cos(x.Data[i]) * y.Grad[i];
            }
        }
    }
}