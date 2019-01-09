﻿using System;

#if DOUBLE
using Real = System.Double;
namespace Double.KelpNet
#else
using Real = System.Single;
namespace KelpNet
#endif
{
    public class Cos : SingleInputFunction
    {
        private const string FUNCTION_NAME = "Cos";

        public Cos(string name = FUNCTION_NAME, string[] inputNames = null, string[] outputNames = null) : base(name, inputNames, outputNames)
        {
            SingleInputForward = ForwardCpu;
            SingleOutputBackward = BackwardCpu;
        }

        protected NdArray ForwardCpu(NdArray x)
        {
            Real[] resultData = new Real[x.Data.Length];

            for (int i = 0; i < resultData.Length; i++)
            {
                resultData[i] = (Real)Math.Cos(x.Data[i]);
            }

            return new NdArray(resultData, x.Shape, x.BatchCount, this);
        }

        protected void BackwardCpu(NdArray y, NdArray x)
        {
            for (int i = 0; i < y.Grad.Length; i++)
            {
                x.Grad[i] += (Real)Math.Sin(x.Data[i]) * -y.Grad[i];
            }
        }
    }
}
