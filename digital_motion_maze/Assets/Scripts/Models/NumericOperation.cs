using System;

namespace fraction
{
    /// <summary>
    /// 做代数处理的静态类
    /// </summary>
    public static class NumericOperation
    {
        /// <summary>
        /// 求最大公约数的函数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static public int GCD(int a, int b)
        {
            //取得各自的绝对值
            a = Math.Abs(a);
            b = Math.Abs(b);
            if (a < b)
            {
                return GCD(b, a);//更换调用自己
            }
            while (b != 0)
            {
                //只要b不是零
                int r = a % b;//a除以b的余数
                a = b;
                b = r;
            }
            return a;
        }
        /// <summary>
        /// 求最小公倍数的函数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int LCM(int a, int b)
        {
            return a * b / GCD(a, b);
        }
    }
}
