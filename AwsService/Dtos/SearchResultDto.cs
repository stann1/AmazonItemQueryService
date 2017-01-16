using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsService.Dtos
{
    public class SearchResultDto
    {
        public SearchResultDto()
        {
            ResultItems = new List<SearchResultItem>();
        }

        public List<SearchResultItem> ResultItems { get; set; }
        public string SearchError { get; set; }
    }
}
