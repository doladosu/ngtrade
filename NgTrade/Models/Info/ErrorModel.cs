using System;

namespace NgTrade.Models.Info
{
    public class ErrorModel
    {
        public int HttpStatusCode { get; set; }
        public Exception Exception { get; set; }
    }
}