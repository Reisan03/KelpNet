﻿using System.IO;
using System.Runtime.Serialization;

namespace KelpNet.Tools
{
    public class ModelIO
    {
        public static void Save(FunctionStack functionStack, string fileName)
        {
            NetDataContractSerializer bf = new NetDataContractSerializer();

            using (Stream stream = File.OpenWrite(fileName))
            {
                bf.Serialize(stream, functionStack);
            }
        }

        public static FunctionStack Load(string fileName)
        {
            NetDataContractSerializer bf = new NetDataContractSerializer();
            FunctionStack result;

            using (Stream stream = File.OpenRead(fileName))
            {
                result = (FunctionStack)bf.Deserialize(stream);
            }

            foreach (Function function in result.Functions)
            {
                function.ResetState();

                for (int i = 0; i < function.Optimizers.Length; i++)
                {
                    function.Optimizers[i].ResetParams();
                }
            }

            return result;
        }
    }
}
