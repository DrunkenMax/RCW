using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCW
{
    static class Mod
    {
        const double c = 299792458;
        public static string Mass(double m)
        {
            string str = "";
            if (m < 1) str = Convert.ToString(Math.Round(m * 1000, 0)) + "г";
            if ((m >= 1) && (m < 10)) str = Convert.ToString(Math.Round(m, 2)) + "кг";
            if ((m >= 10) && (m < 100)) str = Convert.ToString(Math.Round(m, 1)) + "кг";
            if ((m >= 100) && (m < 1e3)) str = Convert.ToString(Math.Round(m, 0)) + "кг";
            if ((m >= 1e3) && (m < 1e4)) str = Convert.ToString(Math.Round(m / 1e3, 2)) + "т";
            if ((m >= 1e4) && (m < 1e5)) str = Convert.ToString(Math.Round(m / 1e3, 1)) + "т";
            if ((m >= 1e5) && (m < 1e6)) str = Convert.ToString(Math.Round(m / 1e3, 0)) + "т";
            if ((m >= 1e6) && (m < 1e7)) str = Convert.ToString(Math.Round(m / 1e6, 2)) + "тыс. т";
            if ((m >= 1e7) && (m < 1e8)) str = Convert.ToString(Math.Round(m / 1e6, 1)) + "тыс. т";
            if ((m >= 1e8) && (m < 1e9)) str = Convert.ToString(Math.Round(m / 1e6, 0)) + "тыс. т";
            return str;
        }
        public static string Temp(double t)
        {
            string str = "";
            if ((t > -100) && (t < 100))
                str += Convert.ToString(Math.Round(t, 1)) + "°C";
            else str += Convert.ToString(Math.Round(t)) + "°C";
            return str;
        }
        public static string Cons(double co)
        {
            string str = "";
            if (co < 1) str = Convert.ToString(Math.Round(co * 1000, 0)) + "г/с";
            if ((co >= 1) && (co < 10)) str = Convert.ToString(Math.Round(co, 2)) + "кг/с";
            if ((co >= 10) && (co < 100)) str = Convert.ToString(Math.Round(co, 1)) + "кг/с";
            if ((co >= 100) && (co < 1e3)) str = Convert.ToString(Math.Round(co, 0)) + "кг/с";
            if ((co >= 1e3) && (co < 1e4)) str = Convert.ToString(Math.Round(co / 1e3, 2)) + "т/с";
            if ((co >= 1e4) && (co < 1e5)) str = Convert.ToString(Math.Round(co / 1e3, 1)) + "т/с";
            if ((co >= 1e5) && (co < 1e6)) str = Convert.ToString(Math.Round(co / 1e3, 0)) + "т/с";
            if ((co >= 1e6) && (co < 1e7)) str = Convert.ToString(Math.Round(co / 1e6, 2)) + "тыс.т/с";
            if ((co >= 1e7) && (co < 1e8)) str = Convert.ToString(Math.Round(co / 1e6, 1)) + "тыс.т/с";
            if ((co >= 1e8) && (co < 1e9)) str = Convert.ToString(Math.Round(co / 1e6, 0)) + "тыс.т/с";
            return str;
        }
        public static string Dist(double d)
        {
            string str = "";
            if (d < 1) str = Convert.ToString(Math.Round(d * 100, 1)) + "см";
            if ((d >= 1) && (d < 10)) str = Convert.ToString(Math.Round(d, 2)) + "м";
            if ((d >= 10) && (d < 100)) str = Convert.ToString(Math.Round(d, 1)) + "м";
            if ((d >= 100) && (d < 1e3)) str = Convert.ToString(Math.Round(d, 0)) + "м";
            if ((d >= 1e3) && (d < 1e4)) str = Convert.ToString(Math.Round(d / 1e3, 2)) + "км";
            if ((d >= 1e4) && (d < 1e5)) str = Convert.ToString(Math.Round(d / 1e3, 1)) + "км";
            if ((d >= 1e5) && (d < 1e6)) str = Convert.ToString(Math.Round(d / 1e3, 0)) + "км";
            if ((d >= 1e6) && (d < 1e7)) str = Convert.ToString(Math.Round(d / 1e6, 2)) + "тыс. км";
            if ((d >= 1e7) && (d < 1e8)) str = Convert.ToString(Math.Round(d / 1e6, 1)) + "тыс. км";
            if ((d >= 1e8) && (d < 1e9)) str = Convert.ToString(Math.Round(d / 1e6, 0)) + "тыс. км";
            if (d >= 1e9)
            {
                double exp = Math.Round((d / c) % 1, 2);
                int tpi = Convert.ToInt32(Math.Truncate(d / c));
                if (tpi >= (24 * 3600))
                {
                    str += Convert.ToString(tpi / (24 * 3600)) + "св.д. ";
                    tpi = tpi - ((tpi / (24 * 3600)) * 24 * 3600);
                }
                if (tpi >= 3600)
                {
                    str += Convert.ToString(tpi / 3600) + "св.ч. ";
                    tpi = tpi - ((tpi / 3600) * 3600);
                }
                if (tpi >= 60)
                {
                    str += Convert.ToString(tpi / 60) + "св.м. ";
                    tpi = tpi - ((tpi / 60) * 60);
                }
                if (tpi >= 1)
                {
                    str += Convert.ToString(tpi + exp) + "св.c. ";
                }
            }
            return str;
        }
        public static string Time(double t)
        {
            string str = ""; int tpi = Convert.ToInt32(Math.Truncate(t));
            if (tpi >= (24 * 3600))
            {
                str += Convert.ToString(tpi / (24 * 3600)) + "д. ";
                tpi = tpi - ((tpi / (24 * 3600)) * (24 * 3600));
            }
            if (tpi >= 3600)
            {
                str += Convert.ToString(tpi / 3600) + "ч. ";
                tpi = tpi - ((tpi / 3600) * 3600);;
            }
            if (tpi >= 60)
            {
                str += Convert.ToString(tpi / 60) + "м. ";
                tpi = tpi - ((tpi / 60) * 60);
            }
            if (tpi >= 1)
            {
                str += Convert.ToString(tpi) + "c. ";
            }
            return str;
        }
        public static string Vel(double v)
        {
            string str = "";
            if ((v <= -1e8) && (v > -1e9)) str = Convert.ToString(Math.Round(v / 1e6, 0)) + "тыс.км/с";
            if ((v <= -1e7) && (v > -1e8)) str = Convert.ToString(Math.Round(v / 1e6, 1)) + "тыс.км/с";
            if ((v <= -1e6) && (v > -1e7)) str = Convert.ToString(Math.Round(v / 1e6, 2)) + "тыс.км/с";
            if ((v <= -1e5) && (v > -1e6)) str = Convert.ToString(Math.Round(v / 1e3, 0)) + "км/с";
            if ((v <= -1e4) && (v > -1e5)) str = Convert.ToString(Math.Round(v / 1e3, 1)) + "км/с";
            if ((v <= -1e3) && (v > -1e4)) str = Convert.ToString(Math.Round(v / 1e3, 2)) + "км/с";
            if ((v <= -100) && (v > -1e3)) str = Convert.ToString(Math.Round(v, 0)) + "м/с";
            if ((v <= -10) && (v > -100)) str = Convert.ToString(Math.Round(v, 1)) + "м/с";
            if ((v <= -1) && (v > -10)) str = Convert.ToString(Math.Round(v, 2)) + "м/c";
            if ((v < 1) && (v > -1)) str = Convert.ToString(Math.Round(v * 100, 1)) + "см/с";
            if ((v >= 1) && (v < 10)) str = Convert.ToString(Math.Round(v, 2)) + "м/c";
            if ((v >= 10) && (v < 100)) str = Convert.ToString(Math.Round(v, 1)) + "м/с";
            if ((v >= 100) && (v < 1e3)) str = Convert.ToString(Math.Round(v, 0)) + "м/с";
            if ((v >= 1e3) && (v < 1e4)) str = Convert.ToString(Math.Round(v / 1e3, 2)) + "км/с";
            if ((v >= 1e4) && (v < 1e5)) str = Convert.ToString(Math.Round(v / 1e3, 1)) + "км/с";
            if ((v >= 1e5) && (v < 1e6)) str = Convert.ToString(Math.Round(v / 1e3, 0)) + "км/с";
            if ((v >= 1e6) && (v < 1e7)) str = Convert.ToString(Math.Round(v / 1e6, 2)) + "тыс.км/с";
            if ((v >= 1e7) && (v < 1e8)) str = Convert.ToString(Math.Round(v / 1e6, 1)) + "тыс.км/с";
            if ((v >= 1e8) && (v < 1e9)) str = Convert.ToString(Math.Round(v / 1e6, 0)) + "тыс.км/с";
            return str;
        }
        public static string Acc(double a)
        {
            string str = "";
            if (a <= -1e8) str = Convert.ToString(Math.Round(a / 1e6, 0)) + "МН/кг";
            if ((a <= -1e7) && (a > -1e8)) str = Convert.ToString(Math.Round(a / 1e6, 1)) + "МН/кг";
            if ((a <= -1e6) && (a > -1e7)) str = Convert.ToString(Math.Round(a / 1e6, 2)) + "МН/кг";
            if ((a <= -1e5) && (a > -1e6)) str = Convert.ToString(Math.Round(a / 1e3, 0)) + "кН/кг";
            if ((a <= -1e4) && (a > -1e5)) str = Convert.ToString(Math.Round(a / 1e3, 1)) + "кН/кг";
            if ((a <= -1e3) && (a > -1e4)) str = Convert.ToString(Math.Round(a / 1e3, 2)) + "кН/кг";
            if ((a <= -100) && (a > -1e3)) str = Convert.ToString(Math.Round(a, 0)) + "Н/кг";
            if ((a <= -10) && (a > -100)) str = Convert.ToString(Math.Round(a, 1)) + "H/кг";
            if ((a <= -1) && (a > -10)) str = Convert.ToString(Math.Round(a, 2)) + "H/кг";
            if ((a < 1) && (a > -1)) str = Convert.ToString(Math.Round(a * 1000, 0)) + "мН/кг";
            if ((a >= 1) && (a < 10)) str = Convert.ToString(Math.Round(a, 2)) + "H/кг";
            if ((a >= 10) && (a < 100)) str = Convert.ToString(Math.Round(a, 1)) + "H/кг";
            if ((a >= 100) && (a < 1e3)) str = Convert.ToString(Math.Round(a, 0)) + "Н/кг";
            if ((a >= 1e3) && (a < 1e4)) str = Convert.ToString(Math.Round(a / 1e3, 2)) + "кН/кг";
            if ((a >= 1e4) && (a < 1e5)) str = Convert.ToString(Math.Round(a / 1e3, 1)) + "кН/кг";
            if ((a >= 1e5) && (a < 1e6)) str = Convert.ToString(Math.Round(a / 1e3, 0)) + "кН/кг";
            if ((a >= 1e6) && (a < 1e7)) str = Convert.ToString(Math.Round(a / 1e6, 2)) + "МН/кг";
            if ((a >= 1e7) && (a < 1e8)) str = Convert.ToString(Math.Round(a / 1e6, 1)) + "МН/кг";
            if (a >= 1e8) str = Convert.ToString(Math.Round(a / 1e6, 0)) + "МН/кг";
            return str;
        }
        public static string Hp(double h)
        {
            string str = "";
            str += Convert.ToString(Math.Round(h / 10, 1)) + "%";
            return str;
        }
        public static string OL(double o)
        {
            string str = "";
            str += Convert.ToString(Math.Round(o,2)) + "g";
            return str;
        }
    }
}
