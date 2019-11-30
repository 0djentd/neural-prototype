using System.Collections.Generic;
using System;
namespace ConsoleApp4
{
    class Data
    {
        private List<double[]> dataTrainingCategorizedIn = new List<double[]>();
        private List<double[]> dataTrainingCategorizedOut = new List<double[]>();
        private List<double[]> dataTrainingCategorizedError = new List<double[]>();
        public void AddDataType(double[] inData, double[] outData)
        {
            dataTrainingCategorizedIn.Add(outData);
            dataTrainingCategorizedOut.Add(outData);
            double[] errorData = new double[outData.Length];
            dataTrainingCategorizedError.Add(errorData);
        } 
        public void UpdateDataOut(double[] outData, double[] outError, int type)
        {
            dataTrainingCategorizedOut[type] = outData;
            dataTrainingCategorizedError[type] = outError;
        }
        public void showData()
        {
            Console.WriteLine("===============");
            for(int i = 0; i < dataTrainingCategorizedOut.Count; i++)
            {
                Console.WriteLine(dataTrainingCategorizedIn[i] + ":" + dataTrainingCategorizedOut[i]+":"+dataTrainingCategorizedError);
            }
            Console.WriteLine("===============");
        }
    }
}