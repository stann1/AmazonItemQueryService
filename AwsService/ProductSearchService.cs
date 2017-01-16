using System;
using System.Collections.Generic;
using System.Linq;
using AwsService.AwsProductSearchService;
using AwsService.Dtos;
using AwsService.Exceptions;
using AwsService.WcfServiceAddons;

namespace AwsService
{
    public class ProductSearchService : IProductSearchService
    {
        private const string AWS_ACCESS_KEY_ID = ""; // your aws service access key
        private const string AWS_SECRET_KEY = ""; // your secret key
        private const string DESTINATION = "webservices.amazon.co.uk";
        private const string ASSOCIATE_TAG = ""; // your associate tag

        private readonly AWSECommerceServicePortTypeClient _client;
        
        public ProductSearchService()
        {
            _client = new AWSECommerceServicePortTypeClient("AWSECommerceServicePortUK");
            _client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(AWS_ACCESS_KEY_ID, AWS_SECRET_KEY));
        }

        public SearchRequestDto CreateDefaultSearchRequest(string[] asins)
        {
            SearchRequestDto searchRequest = new SearchRequestDto()
            {
                ItemLookupIds = asins,
                ItemLookupRequestIdType = ItemLookupRequestIdType.ASIN,
                ResponseGroups = new ResponseGroup[] { ResponseGroup.ItemAttributes, ResponseGroup.Images, ResponseGroup.Offers },
                SearchType = SearchType.ItemLookup
            };

            return searchRequest;
        }

        public SearchResultDto SearchForItems(SearchRequestDto searchRequest)
        {
            if (searchRequest.SearchType != SearchType.ItemLookup)
            {
                return new SearchResultDto();
            }
            
            ItemLookupResponse response = GetLookupResponse(searchRequest.ItemLookupRequestIdType,
                searchRequest.ItemLookupIds,
                searchRequest.ResponseGroups.Select(r => r.ToString()).ToArray());

            if (response.OperationRequest.Errors != null)
            {
                throw new ItemSearchRequestException(response.OperationRequest.Errors[0].Message);
            }

            SearchResultDto searchResult = MapSearchResponseToDto(response);

            return searchResult;
        }

        private SearchResultDto MapSearchResponseToDto(ItemLookupResponse response)
        {
            SearchResultDto searchResult = new SearchResultDto();
            searchResult.ResultItems = new List<SearchResultItem>();

            // save error due to invalid search requests
            Items responseResult = response.Items[0];
            if (responseResult.Request.Errors != null && responseResult.Request.Errors.Any())
            {
                searchResult.SearchError = responseResult.Request.Errors.First().Message;
                return searchResult;
            }

            // itterate over the search result items
            if (responseResult.Item == null)
            {
                searchResult.SearchError = "Query returned no response items";
                return searchResult;
            }

            foreach (var itemListing in responseResult.Item)
            {
                SearchResultItem resultItem = new SearchResultItem();

                if (itemListing.Errors != null && itemListing.Errors.Any())
                {
                    resultItem.ErrorMessage = itemListing.Errors.First().Message;
                    searchResult.ResultItems.Add(resultItem);
                    continue;
                }

                try
                {
                    MapItemLookupToItemResultDto(resultItem, itemListing);

                }
                catch (NullReferenceException nex)
                {
                    resultItem.ErrorMessage = string.Format("Mapping failed for amazon item {0}. {1}", itemListing.ASIN, nex.Message);
                }

                searchResult.ResultItems.Add(resultItem);
            }

            return searchResult;
        }

        private void MapItemLookupToItemResultDto(SearchResultItem resultItem, Item itemListing)
        {
            resultItem.Asin = itemListing.ASIN;
            resultItem.Title = itemListing.ItemAttributes.Title;
            if (itemListing.ItemAttributes.ListPrice != null)
            {
                resultItem.ListingPrice = itemListing.ItemAttributes.ListPrice.FormattedPrice;
            }
            if (itemListing.Offers != null && itemListing.Offers.Offer.Any())
            {
                string topOfferPrice = itemListing.OfferSummary.LowestNewPrice !=null ? itemListing.OfferSummary.LowestNewPrice.FormattedPrice : "n/a";
                resultItem.TopOfferPrice = topOfferPrice;
                string topOfferUsedItem = itemListing.OfferSummary.LowestUsedPrice != null ? itemListing.OfferSummary.LowestUsedPrice.FormattedPrice : "n/a";
                resultItem.TopOfferPriceUsedItem = topOfferUsedItem;
            }

            resultItem.ImageUrl = itemListing.MediumImage.URL;
            resultItem.Url = itemListing.DetailPageURL;
        }

        private ItemLookupResponse GetLookupResponse(ItemLookupRequestIdType idType, string[] itemIds, string[] responseGroups)
        {
            ItemLookup search = new ItemLookup();
            search.AssociateTag = ASSOCIATE_TAG;
            search.AWSAccessKeyId = AWS_ACCESS_KEY_ID;

            // Create a request object
            ItemLookupRequest request = new ItemLookupRequest();

            request.ResponseGroup = responseGroups;
            request.IdType = idType;
            request.ItemId = itemIds;

            // Set the request on the search wrapper
            search.Request = new ItemLookupRequest[] { request };

            //Send the request and store the response
            ItemLookupResponse response = _client.ItemLookup(search);
            return response;
        }
    }
}
