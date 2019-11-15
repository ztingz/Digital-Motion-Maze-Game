using System;

namespace fraction
{
    /// <summary>
    /// 管理分数的类
    /// </summary>
    public class Fraction : IComparable
    {
        #region 成员变量
        private int numerator;//分子
        private int denominator;//分母
        #endregion

        #region 属性
        /// <summary>
        /// 分子
        /// </summary>
        public int Numerator
        {
            get
            {
                return numerator;
            }
            //set
            //{
            //    numerator = value;
            //}
        }
        /// <summary>
        /// 分母
        /// </summary>
        public int Denominator
        {
            get
            {
                return denominator;
            }
            //set
            //{
            //    denominator = value;
            //}
        }
        /// <summary>
        /// 小数值
        /// </summary>
        public double DecimalValue
        {
            get
            {
                double result = (double)numerator / (double)denominator;
                return result;
            }
        }
        #endregion

        #region 构造
        /// <summary>
        /// 管理分数的类(产生0)
        /// </summary>
        public Fraction()
        {
            this.numerator = 0;
            this.denominator = 1;
            this.CreateIrreducibleFration();
            this.minus();
        }
        /// <summary>
        /// 管理分数等级(以已约分数存储)
        /// </summary>
        /// <param name="numerator">分子</param>
        /// <param name="denominator">分母</param>
        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                //分母为0则表示异常
                throw new ArgumentException("分母被指定为0");
            }
            this.numerator = numerator;
            this.denominator = denominator;
            this.CreateIrreducibleFration();
            this.minus();
        }
        /// <summary>
        /// 把包含小数的数值转换成分数的类
        /// </summary>
        /// <param name="x"></param>
        public Fraction(double x)
        {
            //求出输入的数值的小数点以下的位数
            string strTemp = x.ToString();
            strTemp = strTemp.Substring(strTemp.IndexOf(".") + 1);
            int number = strTemp.Length;
            //
            this.denominator = (int)Math.Pow(10, number);
            this.numerator = (int)(x * Math.Pow(10, number));
            this.CreateIrreducibleFration();
            this.minus();
        }
        /// <summary>
        /// 把整数变成分数形式的类(x/1)
        /// </summary>
        /// <param name="x"></param>
        public Fraction(int x)
        {
            this.numerator = x;
            this.denominator = 1;
            this.CreateIrreducibleFration();
            this.minus();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}/{1}", numerator, denominator);
            //return base.ToString();
        }

        /// <summary>
        /// 修正为既约分数的方法
        /// </summary>
        private void CreateIrreducibleFration()
        {
            int d = NumericOperation.GCD(numerator, denominator);
            numerator = numerator / d;
            denominator = denominator / d;
        }
        /// <summary>
        /// 调整负号位置的方法
        /// </summary>
        private void minus()
        {
            if ((denominator < 0 && numerator < 0) || (denominator < 0))
            {
                numerator *= -1;
                denominator *= -1;
            }
        }
        #endregion

        #region 静态方法(运算符关系)
        /// <summary>
        /// 分数の絶対値を返す
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Fraction ABS(Fraction x)
        {
            return new Fraction(Math.Abs(x.Numerator), Math.Abs(x.Denominator));
        }
        #region 四則演算
        /// <summary>
        /// 加算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Fraction operator +(Fraction x, Fraction y)
        {
            //両分母の最小公倍数をとる
            int lc = NumericOperation.LCM(x.denominator, y.denominator);
            //分母が異なるなら最小公倍数/分母を分子にかける
            if (x.denominator != y.denominator)
            {
                x.numerator *= (lc / x.denominator);
                y.numerator *= (lc / y.denominator);
            }
            Fraction result = new Fraction(x.numerator + y.numerator, lc);
            return result;
        }
        /// <summary>
        /// 加算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Fraction operator +(Fraction x, int y)
        {
            return x + new Fraction(y);
        }
        /// <summary>
        /// 減算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Fraction operator -(Fraction x, Fraction y)
        {
            //両分母の最小公倍数をとる
            int lc = NumericOperation.LCM(x.denominator, y.denominator);
            //分母が異なるなら最小公倍数/分母を分子にかける
            if (x.denominator != y.denominator)
            {
                x.numerator *= (lc / x.denominator);
                y.numerator *= (lc / y.denominator);
            }
            Fraction result = new Fraction(x.numerator - y.numerator, lc);
            return result;
        }
        /// <summary>
        /// 減算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Fraction operator -(Fraction x, int y)
        {
            return x - new Fraction(y);
        }
        /// <summary>
        /// 乗算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Fraction operator *(Fraction x, Fraction y)
        {
            return new Fraction(x.numerator * y.numerator, x.denominator * y.denominator);
        }
        /// <summary>
        /// 乗算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Fraction operator *(Fraction x, int y)
        {
            return new Fraction(x.numerator * y, x.denominator);
        }
        /// <summary>
        /// 除算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Fraction operator /(Fraction x, Fraction y)
        {
            if (y.numerator == 0)
            {
                //分母が0なら例外を吐く
                throw new ArgumentException("割る数に0が指定されました");
            }
            return new Fraction(x.numerator * y.denominator, x.denominator * y.numerator);
        }
        /// <summary>
        /// 除算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Fraction operator /(Fraction x, int y)
        {
            if (y == 0)
            {
                //分母が0なら例外を吐く
                throw new ArgumentException("分母に0が指定されました");
            }
            return new Fraction(x.numerator, x.denominator * y);
        }
        #endregion

        #region 比較演算
        /// <summary>
        /// objと自分自身が等価のときはtrueを返す
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            //objがnullか、型が違うときは、等価でない
            if ((object)obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }
            Fraction c = (Fraction)obj;
            return (this.numerator == c.numerator) && (this.denominator == c.denominator);
            //return base.Equals(obj);
        }
        /// <summary>
        /// Equalがtrueを返すときに同じ値を返す
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            //XOR
            return this.numerator ^ this.denominator.GetHashCode();
            //return base.GetHashCode();
        }
        /// <summary>
        /// 自分自身がotherより小さいときはマイナスの数、大きいときはプラスの数、
        /// 同じときは0を返す
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            //nullより大きい
            if (obj == null)
            {
                return 1;
            }
            //違う型とは比較できない
            if (this.GetType() != obj.GetType())
            {
                throw new ArgumentException("別の型とは比較できません。", "obj");
            }
            //単純な減算の結果を返す
            return this.DecimalValue.CompareTo(((Fraction)obj).DecimalValue);
        }

        /// <summary>
        /// 比較演算子(==)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(Fraction x, Fraction y)
        {
            //nullの確認
            if ((object)x == null)
            {
                return ((object)y == null);
            }
            if ((object)y == null)
            {
                return false;
            }
            //Equalメソッドを呼び出す
            return x.Equals(y);
        }
        /// <summary>
        /// 比較演算子(!=)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(Fraction x, Fraction y)
        {
            return !(x == y);
        }
        /// <summary>
        /// 比較（<）
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <(Fraction x, Fraction y)
        {
            //nullの確認
            if ((object)x == null || (object)y == null)
            {
                throw new ArgumentException();
            }
            //CompareToメソッド
            //Fraction temp = x - y;

            return ((x - y).numerator < 0);
        }
        /// <summary>
        /// 比較（>）
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >(Fraction x, Fraction y)
        {
            return (y < x);
        }
        /// <summary>
        /// 比較（<=）
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <=(Fraction x, Fraction y)
        {
            //nullの確認
            if ((object)x == null || (object)y == null)
            {
                throw new ArgumentException();
            }
            //CompareToメソッド
            return ((x - y).numerator <= 0);
        }
        /// <summary>
        /// 比較（>=）
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >=(Fraction x, Fraction y)
        {
            return (y <= x);
        }
        #endregion
        #endregion
    }
}
