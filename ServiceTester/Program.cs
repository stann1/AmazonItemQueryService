using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwsService;
using AwsService.Dtos;

namespace ServiceTester
{
    class Program
    {
        static void Main(string[] args)
        {
            IProductSearchService service = new ProductSearchService();
            string[] asins = new string[] { "B0186FESVC" };  // kindle touch

            SearchRequestDto requestObject = service.CreateDefaultSearchRequest(asins);
            SearchResultDto result = service.SearchForItems(requestObject);     // it is a good idea to try-catch this for ItemSearchRequestException

            if (!string.IsNullOrEmpty(result.SearchError))
            {
                Console.WriteLine("Error: {0}", result.SearchError);
            }
            else
            {
                if (result.ResultItems.Any())
                {
                    Console.WriteLine("Item title: {0}, price: {1}", result.ResultItems[0].Title, result.ResultItems[0].TopOfferPrice);
                }
                else
                {
                    Console.WriteLine("No matches found");
                }
            }
        }
    }
}
