using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_teamwork.Helpers
{
    public class Helper
    {
        public static string GenerateSlug(string name)
        {
            return name.ToLower().Replace(" ", "-");
        }
    }
}