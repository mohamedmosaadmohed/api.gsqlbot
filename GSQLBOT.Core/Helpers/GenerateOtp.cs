using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSQLBOT.Core.Helpers
{
    public static class OtpGenerator
    {
        static Random random = new Random();
        public static string GenerateOtp()=> random.Next(100000, 999999).ToString();   
    }

}
