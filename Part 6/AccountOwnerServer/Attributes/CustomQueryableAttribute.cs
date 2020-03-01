using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using static AccountOwnerServer.Models.ResponsePackage;

namespace AccountOwnerServer.Attributes
{
    public class CustomQueryableAttribute : EnableQueryAttribute
    {
        public bool ForceInlineCount { get; private set; }
        public CustomQueryableAttribute(bool forceInlineCount = true, int pageSize = 1000)
        {
            ForceInlineCount = forceInlineCount;
            //Enables server paging by default
            if (PageSize == 0)
            {
                PageSize = pageSize;
            }
        }
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                throw actionExecutedContext.Exception; //Throw the exception so that the global handler will handle it
            }

            var requestUri = actionExecutedContext.HttpContext.Request.QueryString.Value;

            //Enables count by default if forced to do so by adding to query string
            if (ForceInlineCount && !actionExecutedContext.HttpContext.Request.Query.Any(c => c.Key == "$count"))
            {
                actionExecutedContext.HttpContext.Request.QueryString.Add("$count", "true");
            }
            actionExecutedContext.HttpContext.Request.Query.Count();

            var test = actionExecutedContext.HttpContext.Request.QueryString.Value;

            ////Need to remove non-OData queries from the url because if we send queries with the same query parameter the OnActionExecuted will fail.
            ////This scenario happens when you need to send a list via query strings. As an example, sending an array of strings for a variable named policies would look like:
            ////?policies=WCP0254231&policies=CPA0029880... Since the policies variable is in there twice the base.OnActionExecuted(actionExecutedContext) will fail with the error: 
            ////An item with the same key has already been added
            //var odataQueryParamaters = string.Join("&", actionExecutedContext.Request.GetQueryNameValuePairs().Where(kv => kv.Key.StartsWith("$")).Select(kv => $"{kv.Key}={kv.Value}"));
            ////The nonODataQueryParameters will be appended to the end of the next and last links. I also encode the values here to make sure things like an & are encoded for URL consumption
            //var nonODataQueryParameters = "&" + string.Join("&", actionExecutedContext.Request.GetQueryNameValuePairs().Where(kv => !kv.Key.StartsWith("$")).Select(kv => $"{kv.Key}={System.Web.HttpUtility.UrlEncode(kv.Value)}"));
            //var currentRequestUri = actionExecutedContext.Request.RequestUri;
            //var newRequestUri = $"{currentRequestUri.Scheme}://{currentRequestUri.Host}{currentRequestUri.AbsolutePath}?{odataQueryParamaters}";
            //actionExecutedContext.Request.RequestUri = new Uri(newRequestUri);

            //Let OData implementation handle everything
            base.OnActionExecuted(actionExecutedContext);


            ////Set back to original requestUri for some of the calculations below
            //actionExecutedContext.Request.RequestUri = new Uri(requestUri);

            //var odataOptions = actionExecutedContext.HttpContext.Request.ODataFeature();  //This is the secret sauce, really.
            //object responseObject;

            //bool responseIsValid = ResponseIsValid(actionExecutedContext.HttpContext.Response);
            //bool ContentValueIsValid = actionExecutedContext.HttpContext.Response.TryGetContentValue(out responseObject);

            //if (responseIsValid && ContentValueIsValid && responseObject is IQueryable)
            //{
            //    var data = ((IQueryable<object>)responseObject);

            //    var responsePackage = new ArrayResponse<object>(data,
            //                                                      data.Count(),
            //                                                      odataOptions.TotalCount,
            //                                                      (odataOptions.NextLink == null) ? string.Empty : $"{odataOptions.NextLink.PathAndQuery}{(nonODataQueryParameters.Length > 1 ? nonODataQueryParameters : string.Empty)}",
            //                                                      GetPreviousLink(actionExecutedContext.Request.RequestUri.PathAndQuery, data.Count()),
            //                                                      (odataOptions.NextLink == null) ? string.Empty : $"{GetLastLink(odataOptions.NextLink.PathAndQuery, odataOptions.TotalCount, data.Count())}{(nonODataQueryParameters.Length > 1 ? nonODataQueryParameters : string.Empty)}",
            //                                                      string.Empty);
            //    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(actionExecutedContext.Response.StatusCode, responsePackage);
            //}
            //else if (responseIsValid && ContentValueIsValid)
            //{
            //    var responsePackage = new ItemResponse<object>(responseObject,
            //                                                      1,
            //                                                      1,
            //                                                      string.Empty,
            //                                                      string.Empty,
            //                                                      string.Empty,
            //                                                      string.Empty);
            //    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(actionExecutedContext.Response.StatusCode, responsePackage);
            //}
            //else if (responseObject != null)
            //{
            //    string errorMessage = responseObject.ToString();
            //    if (responseObject is HttpError)
            //    {
            //        if (((HttpError)responseObject).Count > 1)
            //        {
            //            errorMessage = ((HttpError)responseObject).ElementAt(1).Value.ToString();
            //        }
            //        else
            //        {
            //            errorMessage = ((HttpError)responseObject).ExceptionMessage;
            //        }
            //    }
            //    var responsePackage = new ItemResponse<object>(null,
            //                                                      0,
            //                                                      0,
            //                                                      string.Empty,
            //                                                      string.Empty,
            //                                                      string.Empty,
            //                                                      errorMessage);
            //    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(actionExecutedContext.Response.StatusCode, responsePackage);
            //}
        }


        private string GetPreviousLink(string queriedUrl, int returnCount)
        {
            string previousLink = string.Empty;
            string pattern = @"\$skip=(\d+)";
            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);
            if (rgx.IsMatch(queriedUrl))
            {
                int skipCount = Convert.ToInt32(rgx.Match(queriedUrl).Groups[1].Value);
                int prevSkipCount = skipCount >= returnCount ? skipCount - returnCount : 0;
                if (skipCount > 0 && prevSkipCount > 0)
                {
                    previousLink = rgx.Replace(queriedUrl, $"$skip={prevSkipCount}");
                }
                else if (skipCount > 0 && prevSkipCount == 0)
                {
                    previousLink = rgx.Replace(queriedUrl, string.Empty).TrimEnd('&');
                }
            }
            return previousLink;
        }

        private string GetLastLink(string queriedUrl, long? totalResultsCount, int returnCount)
        {
            string pattern = @"\$skip=(\d+)";
            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);

            return rgx.Replace(queriedUrl, "$skip=" + (totalResultsCount - returnCount));
        }

        private bool ResponseIsValid(HttpResponse response)
        {
            return response.IsSuccessStatusCode();
        }
    }
}
