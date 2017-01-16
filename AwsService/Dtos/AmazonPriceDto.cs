using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsService.Dtos
{
    public class AmazonPriceDto
    {
        public decimal AmazonPrice { get; set; }
        public decimal AmazonPriceInBgn { get; set; }
        public decimal AmazonPriceUsed { get; set; }
        public decimal AmazonPriceUsedInBgn { get; set; }
        public bool IsValid { get; set; }
    }
}
