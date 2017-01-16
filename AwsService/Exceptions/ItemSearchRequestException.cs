using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsService.Exceptions
{
    public class ItemSearchRequestException : Exception
    {
        public ItemSearchRequestException()
        {
        }

        public ItemSearchRequestException(string message) : base(message)
        {
        }

        public ItemSearchRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
