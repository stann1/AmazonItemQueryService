using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwsService.Dtos;

namespace AwsService
{
    public interface IProductSearchService
    {
        SearchRequestDto CreateDefaultSearchRequest(string[] asins);
        SearchResultDto SearchForItems(SearchRequestDto searchRequest);
    }
}
