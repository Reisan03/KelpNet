﻿using System;

#if DOUBLE
using Real = System.Double;
namespace Double.KelpNet
#else
using Real = System.Single;
namespace KelpNet
#endif
{
    //ネットワークの訓練を実行するクラス
    //主にArray->NdArrayの型変換を担う
    public class Trainer
    {
        //バッチで学習処理を行う
        public static Real Train(FunctionStack functionStack, Array[] input, Array[] teach, LossFunction lossFunction, bool isUpdate = true)
        {
            return Train(functionStack, NdArray.FromArrays(input), NdArray.FromArrays(teach), lossFunction, isUpdate);
        }

        //バッチで学習処理を行う
        public static Real Train(FunctionStack functionStack, NdArray input, NdArray teach, LossFunction lossFunction, bool isUpdate = true)
        {
            //結果の誤差保存用
            NdArray[] result = functionStack.Forward(input);
            Real sumLoss = lossFunction.Evaluate(result, teach);

            //Backwardのバッチを実行
            functionStack.Backward(result);

            //更新
            if (isUpdate)
            {
                functionStack.Update();
            }

            return sumLoss;
        }

        //精度測定
        public static Real Accuracy(FunctionStack functionStack, Array[] x, Array[] y)
        {
            return Accuracy(functionStack, NdArray.FromArrays(x), NdArray.FromArrays(y));
        }

        public static Real Accuracy(FunctionStack functionStack, NdArray x, NdArray y)
        {
            Real matchCount = 0;

            NdArray forwardResult = functionStack.Predict(x)[0];

            for (int b = 0; b < x.BatchCount; b++)
            {
                Real maxval = forwardResult.Data[b * forwardResult.Length];
                int maxindex = 0;

                for (int i = 0; i < forwardResult.Length; i++)
                {
                    if (maxval < forwardResult.Data[i + b * forwardResult.Length])
                    {
                        maxval = forwardResult.Data[i + b * forwardResult.Length];
                        maxindex = i;
                    }
                }

                if (maxindex == (int)y.Data[b * y.Length])
                {
                    matchCount++;
                }
            }

            return matchCount / x.BatchCount;
        }
    }
}
