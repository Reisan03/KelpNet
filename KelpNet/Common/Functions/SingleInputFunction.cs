﻿using System;

namespace KelpNet.Common.Functions
{
    [Serializable]
    public abstract class SingleInputFunction : Function
    {
        protected Func<NdArray, NdArray> SingleInputForward;
        protected Action<NdArray, NdArray> SingleOutputBackward;

        protected SingleInputFunction(string name) : base(name)
        {
        }

        public override NdArray Forward(params NdArray[] xs)
        {
            PrevInputs.Add(xs);
            xs[0].UseCount++;

            return SingleInputForward(xs[0]);
        }

        public override void Backward(NdArray y, params NdArray[] xs)
        {
            PrevInputs.RemoveAt(PrevInputs.Count - 1);
#if DEBUG
            if (xs == null || xs.Length != 1) throw new Exception("引数が正しくありません");
#endif
            BackwardCountUp();

            xs[0].UseCount--;
            SingleOutputBackward(y, xs[0]);
        }

        public override NdArray Predict(params NdArray[] xs)
        {
            return Predict(xs[0]);
        }

        //Predict専用メソッドを持つ関数のためのオーバーライド用
        public virtual NdArray Predict(NdArray input)
        {
            return SingleInputForward(input);
        }
    }
}
