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
    public abstract class SingleInputFunction : Function
    {
        protected Func<NdArray, NdArray> SingleInputForward;
        protected Action<NdArray, NdArray> SingleOutputBackward;

        protected SingleInputFunction(string name, string[] inputNames = null, string[] outputNames = null) : base(name, inputNames, outputNames)
        {
        }

        public override NdArray[] Forward(params NdArray[] xs)
        {
            PrevInputs.Add(xs);
            xs[0].UseCount++;

            return new[] { SingleInputForward(xs[0]) };
        }

        public override void Backward(params NdArray[] ys)
        {
            NdArray[] xs = PrevInputs[PrevInputs.Count - 1];
            PrevInputs.RemoveAt(PrevInputs.Count - 1);

#if DEBUG
            if (xs == null || xs.Length != 1) throw new Exception("引数が正しくありません");
#endif
            BackwardCountUp();

            xs[0].UseCount--;
            SingleOutputBackward(ys[0], xs[0]);
        }

        public override NdArray[] Predict(params NdArray[] xs)
        {
            return new[] { Predict(xs[0]) };
        }

        //Predict専用メソッドを持つ関数のためのオーバーライド用
        public virtual NdArray Predict(NdArray input)
        {
            return SingleInputForward(input);
        }
    }
}
