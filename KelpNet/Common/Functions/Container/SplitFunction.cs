﻿using System;

#if DOUBLE
using Real = System.Double;
namespace Double.KelpNet
#else
using Real = System.Single;
namespace KelpNet
#endif
{
    [Serializable]
    public class SplitFunction : MultiOutputFunction
    {
        const string FUNCTION_NAME = "SplitFunction";
        private readonly int _splitNum;

        public FunctionStack[] SplitedFunctions;

        public SplitFunction(int splitNum = 2, string name = FUNCTION_NAME, string[] inputNames = null, string[] outputNames = null) : base(name, inputNames, outputNames)
        {
            this._splitNum = splitNum;
            SplitedFunctions = new FunctionStack[splitNum];

            for (int i = 0; i < SplitedFunctions.Length; i++)
            {
                SplitedFunctions[i] = new FunctionStack(new Function[] { }, name + i, new[] { inputNames[0] }, new[] { outputNames[i] });
            }

            SingleInputForward = ForwardCpu;
            SingleOutputBackward = BackwardCpu;
        }

        private NdArray[] ForwardCpu(NdArray x)
        {
            NdArray[] result = new NdArray[_splitNum];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = SplitedFunctions[i].Forward(x)[0];
            }

            return result;
        }

        private void BackwardCpu(NdArray[] ys, NdArray x)
        {
        }

        public override NdArray[] Predict(params NdArray[] xs)
        {
            NdArray[] result = new NdArray[_splitNum];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = SplitedFunctions[i].Predict(xs[0])[0];
            }

            return result;
        }
    }
}
