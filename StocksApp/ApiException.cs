using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp
{
    public class ApiException : Exception
    {
        public int ErrorCode { get; }
        public string ErrorText { get; }

        public ApiException(int errorCode, string errorText)
            : base(errorText)
        {
            ErrorCode = errorCode;
            ErrorText = errorText;
        }
    }

}
