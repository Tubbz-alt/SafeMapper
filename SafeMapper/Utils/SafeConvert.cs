﻿namespace SafeMapper.Utils
{
    using System;
    using System.Data.SqlTypes;
    using System.Globalization;

    public class SafeConvert
    {
        private const double MaxDecimalAsDouble = (double)decimal.MaxValue;
        private const double MinDecimalAsDouble = (double)decimal.MinValue;

        public static byte ToByte(string value)
        {
            ulong result = ToUInt64(value);
            return (result <= byte.MaxValue) ? (byte)result : (byte)0;
        }

        public static sbyte ToSByte(string value)
        {
            long result = ToInt64(value);
            return (result <= sbyte.MaxValue && result >= sbyte.MinValue) ? (sbyte)result : (sbyte)0;
        }

        public static short ToInt16(string value)
        {
            long result = ToInt64(value);
            return (result <= short.MaxValue && result >= short.MinValue) ? (short)result : (short)0;
        }

        public static ushort ToUInt16(string value)
        {
            ulong result = ToUInt64(value);
            return (result <= ushort.MaxValue) ? (ushort)result : (ushort)0;
        }

        public static uint ToUInt32(string value)
        {
            ulong result = ToUInt64(value);
            return (result <= uint.MaxValue) ? (uint)result : 0;
        }

        public static ulong ToUInt64(string value)
        {
            ulong result = 0;
            if (value[0] == '-')
            {
                return 0;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if ((value[i] >= 48) && (value[i] <= 57))
                {
                    result = (10 * result) + (value[i] - 48uL);
                }
                else
                {
                    result = 0;
                    break;
                }
            }

            return result;
        }

        public static long ToInt64(string value)
        {
            long result = 0;
            bool neg = value[0] == '-';
            for (int i = neg ? 1 : 0; i < value.Length; i++)
            {
                if ((value[i] >= 48) && (value[i] <= 57))
                {
                    result = (10 * result) + (value[i] - 48);
                }
                else
                {
                    result = 0;
                    break;
                }
            }

            return neg ? result * -1 : result;
        }

        public static int ToInt32(long value)
        {
            return (value < int.MinValue || value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(uint value)
        {
            return (value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(ulong value)
        {
            return (value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(double value)
        {
            if (value >= 0)
            {
                if (value < 2147483647.5)
                {
                    int result = (int)value;
                    double dif = value - result;
                    if (dif > 0.5 || (dif == 0.5 && (result & 1) != 0))
                    {
                        result++;
                    }

                    return result;
                }
            }
            else
            {
                if (value >= -2147483648.5)
                {
                    int result = (int)value;
                    double dif = value - result;
                    if (dif < -0.5 || (dif == -0.5 && (result & 1) != 0))
                    {
                        result--;
                    }

                    return result;
                }
            }

            return 0;
        }

        public static int ToInt32(decimal value)
        {
            return (value < int.MinValue || value > int.MaxValue) ? 0 : Convert.ToInt32(value);
        }

        public static int ToInt32(string value)
        {
            long result = ToInt64(value);
            return (result <= int.MaxValue && result >= int.MinValue) ? (int)result : 0;
        }

        public static decimal ToDecimal(string s)
        {
            decimal d;
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out d) ? d : 0m;
        }

        public static decimal ToDecimal(string s, IFormatProvider provider)
        {
            decimal d;
            return decimal.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out d) ? d : 0m;
        }

        public static double ToDouble(string s)
        {
            double d;
            return double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out d) ? d : 0d;
        }

        public static double ToDouble(string s, IFormatProvider provider)
        {
            double d;
            return double.TryParse(s, NumberStyles.Number, provider, out d) ? d : 0d;
        }

        public static decimal ToDecimal(double d)
        {
            return d == MaxDecimalAsDouble ? Decimal.MaxValue
                 : d == MinDecimalAsDouble ? Decimal.MinValue
                 : d > MaxDecimalAsDouble ? 0m
                 : d < MinDecimalAsDouble ? 0m
                 : (decimal)d;
        }

        public static Guid ToGuid(string s)
        {
            Guid g;
            return Guid.TryParse(s, out g) ? g : Guid.Empty;
        }

        public static DateTime ToDateTime(string s, IFormatProvider provider)
        {
            DateTime d;
            return DateTime.TryParse(s, provider, DateTimeStyles.None, out d) ? d : DateTime.MinValue;
        }

        public static DateTime ToDateTime(SqlDateTime sqldate)
        {
            return sqldate.Value;
        }

        public static SqlDateTime ToSqlDateTime(DateTime date)
        {
            return date < (DateTime)SqlDateTime.MinValue ? SqlDateTime.MinValue : date;
        }

        public static bool ToBoolean(string s)
        {
            bool b;
            return bool.TryParse(s, out b) ? b : false;
        }
    }
}