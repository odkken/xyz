using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZ
{
    public static class StaticUtil
    {
        public static double Clamp(this double d, double min, double max)
        {
            if (d < min)
                return min;
            if (d > max)
                return max;
            return d;
        }
        public static float Clamp(this float d, float min, float max)
        {
            if (d < min)
                return min;
            if (d > max)
                return max;
            return d;
        }
    }
}
