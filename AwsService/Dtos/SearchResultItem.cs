using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsService.Dtos
{
    public class SearchResultItem
    {
        public string Asin { get; set; }
        public string Title { get; set; }
        public string ListingPrice { get; set; }
        public IEnumerable<SearchResultItemOffer> ItemOffers { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string TopOfferPrice { get; set; }
        public string TopOfferPriceUsedItem { get; set; }
        public string ErrorMessage { get; set; }

        public SearchResultItem()
        {
            ItemOffers = new List<SearchResultItemOffer>();
        }

    }
}
