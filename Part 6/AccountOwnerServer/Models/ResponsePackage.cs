using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AccountOwnerServer.Models
{
    public class ResponsePackage
    {
        //internal class ExceptionResponse : IHttpActionResult
        //{
        //    public HttpRequestMessage Request { get; set; }

        //    public string ErrorMessage { get; set; }

        //    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        //    {
        //        var response = Request.CreateResponse(
        //                            HttpStatusCode.InternalServerError,
        //                            new ItemResponse<string>() { ErrorMessage = ErrorMessage });
        //        return Task.FromResult(response);
        //    }
        //}

        [KnownType(typeof(Owner))]
        [KnownType(typeof(Account))]

        public class ItemResponse<T> where T : class
        {
            /// <summary>
            /// Number of records returned
            /// </summary>
            public int RecordCountReturned { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public long? RecordCountTotal { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Next { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Previous { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Last { get; set; }

            /// <summary>
            /// 
            /// </summary>
            // You can use the 2 attributes below to ignore this field if empty when serializing to JSON
            //[DefaultValue("")]
            //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public string ErrorMessage { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public T Results { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public ItemResponse() { }

            internal ItemResponse(T result, int recordCountReturned, long? recordCountTotal, string nextLink, string previousLink, string lastLink, string errorMessage)
            {
                Results = result;
                RecordCountReturned = recordCountReturned;
                RecordCountTotal = recordCountTotal;
                Next = nextLink;
                Previous = previousLink;
                Last = lastLink;
                ErrorMessage = errorMessage;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [KnownType(typeof(Account))]
        [KnownType(typeof(Owner))]
        public class ArrayResponse<T> where T : class
        {
            /// <summary>
            /// Number of records returned
            /// </summary>
            public int RecordCountReturned { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public long? RecordCountTotal { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Next { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Previous { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Last { get; set; }

            /// <summary>
            /// 
            /// </summary>
            //[DefaultValue("")]
            //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public string ErrorMessage { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public IEnumerable<T> Results { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public ArrayResponse() { }

            internal ArrayResponse(IEnumerable<T> result, int recordCountReturned, long? recordCountTotal, string nextLink, string previousLink, string lastLink, string errorMessage)
            {
                Results = result;
                RecordCountReturned = recordCountReturned;
                RecordCountTotal = recordCountTotal;
                Next = nextLink;
                Previous = previousLink;
                Last = lastLink;
                ErrorMessage = errorMessage;
            }
        }
    }
}
