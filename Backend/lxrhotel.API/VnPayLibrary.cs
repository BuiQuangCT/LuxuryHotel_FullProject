using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace lxrhotel.API
{
    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value)) _requestData.Add(key, value);
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value)) _responseData.Add(key, value);
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            var data = new StringBuilder();
            foreach (var kv in _requestData)
                if (!string.IsNullOrEmpty(kv.Value))
                    data.Append(Uri.EscapeDataString(kv.Key) + "=" + Uri.EscapeDataString(kv.Value) + "&");

    var signData = string.Empty;
    if (data.Length > 0) 
    {
        signData = data.ToString().Remove(data.Length - 1);
        baseUrl += "?" + signData;
    }

    var vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);

    baseUrl += (data.Length > 0 ? "&" : "?") + "vnp_SecureHash=" + vnp_SecureHash;

    return baseUrl;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var data = new StringBuilder();
            foreach (var kv in _responseData)
                if (!string.IsNullOrEmpty(kv.Value) && kv.Key != "vnp_SecureHash")
                    data.Append(Uri.EscapeDataString(kv.Key) + "=" + Uri.EscapeDataString(kv.Value) + "&");

    // Remove the last '&'
    var checkSumData = string.Empty;
    if (data.Length > 0)
    {
        checkSumData = data.ToString().Remove(data.Length - 1);
    }
    
    var myChecksum = HmacSHA512(secretKey, checkSumData);
    return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        public string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}