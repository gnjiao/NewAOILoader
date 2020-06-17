using BasicClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Main
{
    /// <summary>
    /// 通用的轴方向标定，主要是进行x,y,z,r的标定
    /// <para>目前想到的是，可以通过自动标定生成一个矩阵，用矩阵完成不同轴之间的映射</para>
    /// <para>4x4矩阵，example:轴本身是一维行向量，而映射矩阵是4*一维列向量</para>
    /// <para>如果x=-y,y=z,z=0,u=-u,则对应关系如下：</para>
    /// <para>[x,y,z,u] -- [0,-1,0,0][0,0,1,0][0,0,0,0][0,0,0,-1]</para>
    /// </summary>
    public class AxisDirectionService
    {
        #region singleton
        static readonly object _locker = new object();
        static AxisDirectionService _instance = null;
        public static AxisDirectionService GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                        _instance = new AxisDirectionService();
                }
            }
            return _instance;
        }

        private AxisDirectionService()
        {
            Load();
            if (AxisMap == null)
            {
                AxisMap = new double[4][];
                for (int i = 0; i < AxisMap.Length; ++i)
                {
                    AxisMap[i] = new double[4];
                }
            }
        }
        #endregion

        //const double offset = 0.1;
        const string FilePath = @"D:\Store\Custom\Map\AxisMap.btn";
        /// <summary>
        /// 轴坐标系数
        /// </summary>
        double[][] AxisMap { get; set; }

        /// <summary>
        /// 设置轴坐标系数
        /// </summary>
        /// <param name="axisNum"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetCo(AXIS axisNum, double[] value)
        {
            switch (axisNum)
            {
                case AXIS.X:
                case AXIS.Y:
                case AXIS.Z:
                    AxisMap[(int)axisNum] = value;
                    AxisMap[(int)axisNum][(int)AXIS.U] = 0;
                    break;
                case AXIS.U:
                    AxisMap[(int)axisNum][(int)AXIS.U] = value[(int)AXIS.U];
                    break;
            }
            return true;
        }

        public void Save()
        {
            //Serializer_Xml
            //    .Serialize<double[][]>(AxisMap, FilePath);
            string dir = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            try
            {
                BinaryFormatter writer = new BinaryFormatter();
                //创建流
                using (FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                {
                    //序列化写入
                    writer.Serialize(fs, AxisMap);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("Serializer", ex);
            }
        }

        private void Load()
        {
            if (!File.Exists(FilePath))
            {
                AxisMap = null;
                return;
            }                

            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                AxisMap = bf.Deserialize(fs) as double[][];
            }
            //AxisMap = Serializer_Xml.DeSerialize<double[][]>(FilePath);
        }

        public double[] GetValues(double[] delta)
        {
            if (delta.Length != 4) return new double[4];

            //return MatrixMult(delta, AxisMap);
            return MatrixMult1(delta, AxisMap);
        }

        #region matrix
        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="matrix1">矩阵1</param>
        /// <param name="matrix2">矩阵2</param>
        /// <returns>积</returns>
        private static double[][] MatrixMult(double[][] matrix1, double[][] matrix2)
        {
            //合法性检查
            if (MatrixCR(matrix1)[1] != MatrixCR(matrix2)[0])
            {
                throw new Exception("matrix1 的列数与 matrix2 的行数不想等");
            }
            //矩阵中没有元素的情况
            if (matrix1.Length == 0 || matrix2.Length == 0)
            {
                return new double[][] { };
            }
            //matrix1是m*n矩阵，matrix2是n*p矩阵，则result是m*p矩阵
            int m = matrix1.Length, n = matrix2.Length, p = matrix2[0].Length;
            double[][] result = new double[m][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new double[p];
            }
            //矩阵乘法：c[i,j]=Sigma(k=1→n,a[i,k]*b[k,j])
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    //对乘加法则
                    for (int k = 0; k < n; k++)
                    {
                        result[i][j] += (matrix1[i][k] * matrix2[k][j]);
                    }
                }
            }
            return result;
        }

        private static double[] MatrixMult(double[] matrix1, double[][] matrix2)
        {
            //合法性检查
            if (MatrixCR(matrix1)[1] != MatrixCR(matrix2)[0])
            {
                throw new Exception("matrix1 的列数与 matrix2 的行数不想等");
            }
            //矩阵中没有元素的情况
            if (matrix1.Length == 0 || matrix2.Length == 0)
            {
                return new double[] { };
            }
            //matrix1是m*n矩阵，matrix2是n*p矩阵，则result是m*p矩阵
            //matrix1是一维，所以m是1
            int m = 1/*matrix1.Length*/, n = matrix2.Length, p = matrix2[0].Length;
            double[] result = new double[n];
            //for (int i = 0; i < result.Length; i++)
            //{
            //    result[i] = new double[p];
            //}
            //矩阵乘法：c[i,j]=Sigma(k=1→n,a[i,k]*b[k,j])
            //for (int i = 0; i < m; i++)
            //{
                for (int j = 0; j < p; j++)
                {
                    //对乘加法则
                    for (int k = 0; k < n; k++)
                    {
                        result[j] += (matrix1[k] * matrix2[k][j]);
                    }
                }
            //}
            return result;
        }

        private static double[] MatrixMult1(double[] matrix1, double[][] matrix2)
        {
            //合法性检查
            if (MatrixCR(matrix1)[1] != MatrixCR(matrix2)[0])
            {
                throw new Exception("matrix1 的列数与 matrix2 的行数不想等");
            }
            //矩阵中没有元素的情况
            if (matrix1.Length == 0 || matrix2.Length == 0)
            {
                return new double[] { };
            }
            //matrix1是m*n矩阵，matrix2是n*p矩阵，则result是m*p矩阵
            //matrix1是一维，所以m是1
            int m = 1/*matrix1.Length*/, n = matrix2.Length, p = matrix2[0].Length;
            double[] result = new double[n];
            //for (int i = 0; i < result.Length; i++)
            //{
            //    result[i] = new double[p];
            //}
            //矩阵乘法：c[i,j]=Sigma(k=1→n,a[i,k]*b[k,j])
            //for (int i = 0; i < m; i++)
            //{
            result[0] = (matrix1[0] * matrix2[1][1] - matrix1[1] * matrix2[1][0]) /
                (matrix2[0][0] * matrix2[1][1] - matrix2[1][0] * matrix2[0][1]);

            result[1] = (matrix1[0] * matrix2[0][1] - matrix1[1] * matrix2[0][0]) /
                (matrix2[1][0] * matrix2[0][1] - matrix2[0][0] * matrix2[1][1]);
            //}
            //}
            return result;
        }

        /// <summary>
        /// 计算一个矩阵的行数和列数
        /// </summary>
        /// <param name="matrix">矩阵</param>
        /// <returns>数组：行数、列数</returns>
        private static int[] MatrixCR(double[][] matrix)
        {
            //接收到的参数不是矩阵则报异常
            if (!isMatrix(matrix))
            {
                throw new Exception("接收到的参数不是矩阵");
            }
            //空矩阵行数列数都为0
            if (!isMatrix(matrix) || matrix.Length == 0)
            {
                return new int[2] { 0, 0 };
            }
            return new int[2] { matrix.Length, matrix[0].Length };
        }

        private static int[] MatrixCR(double[] matrix)
        {
            return new int[2] { 1, matrix.Length };
        }

        /// <summary>
        /// 判断一个二维数组是否为矩阵
        /// </summary>
        /// <param name="matrix">二维数组</param>
        /// <returns>true:是矩阵 false:不是矩阵</returns>
        private static bool isMatrix(double[][] matrix)
        {
            //空矩阵是矩阵
            if (matrix.Length < 1) return true;
            //不同行列数如果不相等，则不是矩阵
            int count = matrix[0].Length;
            for (int i = 1; i < matrix.Length; i++)
            {
                if (matrix[i].Length != count)
                {
                    return false;
                }
            }
            //各行列数相等，则是矩阵
            return true;
        }
        #endregion
    }

    public enum AXIS
    {
        X,
        Y,
        Z,
        U
    }
}
