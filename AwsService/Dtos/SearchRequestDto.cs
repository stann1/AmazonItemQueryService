using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwsService.AwsProductSearchService;

namespace AwsService.Dtos
{
    public class SearchRequestDto
    {
        public SearchType SearchType { get; set; }

        public ItemLookupRequestIdType ItemLookupRequestIdType { get; set; }

        public string[] ItemLookupIds { get; set; }

        public ResponseGroup[] ResponseGroups { get; set; }
    }
}
