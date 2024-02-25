using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FastUntility.Base
{
    /// <summary>
    /// post、get、put到url
    /// </summary>
    public static class BaseUrl
    {
        private static readonly Lazy<HttpClient> conn;

        static BaseUrl()
        {
            conn = new Lazy<HttpClient>(() => new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip }));
        }

        internal static HttpClient http
        {
            get
            {
                var http = conn.Value;
                http.DefaultRequestHeaders.Connection.Add("keep-alive");
                return http;
            }
        }


        #region get url(select)
        /// <summary>
        /// get url(select)
        /// </summary>
        public static string GetUrl(string url, int version = 1, int minor = 1, string mediaType = "application/json")
        {
            try
            {
                var handle = new HttpRequestMessage();
                handle.Version = new Version(version, minor);
                handle.Content = new StringContent("", Encoding.UTF8,mediaType);
                handle.Method = HttpMethod.Get;
                handle.RequestUri = new Uri(url);
                var response = http.SendAsync(handle).Result;
                handle.Content?.Dispose();
                handle.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {

                BaseLog.SaveLog(url + ":" + ex.ToString(), "GetUrl_exp");
                return null;
            }
        }
        #endregion

        #region post url(insert)
        /// <summary>
        /// post url(insert)
        /// </summary>
        public static string PostUrl(string url, Dictionary<string, object> dic, int version = 1, int minor = 1, string mediaType = "application/json")
        {
            try
            {
                var count = 0;
                foreach (var item in dic)
                {
                    if (url.Contains("?"))
                        url = string.Format("{0}&{1}={2}", url, item.Key, item.Value);
                    else
                    {
                        if (count == 0)
                            url = string.Format("{0}?{1}={2}", url, item.Key, item.Value);
                        else
                            url = string.Format("{0}&{1}={2}", url, item.Key, item.Value);
                    }
                    count++;
                }

                var handle = new HttpRequestMessage();
                handle.Version = new Version(version, minor);
                handle.Content = new StringContent("", Encoding.UTF8, mediaType);
                handle.Method = HttpMethod.Put;
                handle.RequestUri = new Uri(url);
                var response = http.SendAsync(handle).Result;
                handle.Content?.Dispose();
                handle.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => { BaseLog.SaveLog(url + ":" + ex.ToString(), "PostUrl_exp"); });
                return null;
            }
        }
        #endregion

        #region post content(insert)
        /// <summary>
        /// post content(insert)
        /// </summary>
        public static string PostContent(string url, string param, int version = 1, int minor = 1, string mediaType = "application/json")
        {
            try
            {
                var handle = new HttpRequestMessage();
                handle.Version = new Version(version, minor);
                handle.Content = new StringContent(param, Encoding.UTF8, mediaType);
                handle.Method = HttpMethod.Put;
                handle.RequestUri = new Uri(url);
                var response = http.SendAsync(handle).Result;
                handle.Content?.Dispose();
                handle.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => { BaseLog.SaveLog(url + ":" + ex.ToString(), "PostUrl_exp"); });
                return null;
            }
        }
        #endregion

        /// <summary>
        /// post form
        /// </summary>
        public static string PostForm(string url, List<KeyValuePair<string, string>> param)
        {
            try
            {
                var handle = new HttpRequestMessage();
                handle.Content = new FormUrlEncodedContent(param);
                handle.Method = HttpMethod.Post;
                handle.RequestUri = new Uri(url);
                var response = http.SendAsync(handle).Result;
                handle.Content?.Dispose();
                handle.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #region put url (update)
        /// <summary>
        /// put url(update)
        /// </summary>
        public static string PutUrl(string url, Dictionary<string, object> dic, int version = 1, int minor = 1, string mediaType = "application/json")
        {
            try
            {
                var count = 0;
                foreach (var item in dic)
                {
                    if (url.Contains("?"))
                        url = string.Format("{0}&{1}={2}", url, item.Key, item.Value);
                    else
                    {
                        if (count == 0)
                            url = string.Format("{0}?{1}={2}", url, item.Key, item.Value);
                        else
                            url = string.Format("{0}&{1}={2}", url, item.Key, item.Value);
                    }
                    count++;
                }

                var handle = new HttpRequestMessage();
                handle.Version = new Version(version, minor);
                handle.Content = new StringContent("", Encoding.UTF8, mediaType);
                handle.Method = HttpMethod.Put;
                handle.RequestUri = new Uri(url);

                var response = http.SendAsync(handle).Result;
                handle.Content?.Dispose();
                handle.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => { BaseLog.SaveLog(url + ":" + ex.ToString(), "PutUrl_exp"); });
                return null;
            }
        }
        #endregion

        /// <summary>
        /// Put form
        /// </summary>
        public static string PutForm(string url, List<KeyValuePair<string, string>> param)
        {
            try
            {
                var handle = new HttpRequestMessage();
                handle.Content = new FormUrlEncodedContent(param);
                handle.Method = HttpMethod.Put;
                handle.RequestUri = new Uri(url);
                var response = http.SendAsync(handle).Result;
                handle.Content?.Dispose();
                handle.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #region post soap
        /// <summary>
        /// post content(insert)
        /// </summary>
        public static string PostSoap(string url, string method, Dictionary<string, object> param, string Namespace = "http://tempuri.org/", int version = 1, int minor = 1)
        {
            try
            {
                var xml = new StringBuilder();
                xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xml.Append("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
                xml.Append("<soap:Header />");
                xml.Append("<soap:Body>");
                xml.AppendFormat("<{0} xmlns=\"{1}\">", method, Namespace);

                foreach (KeyValuePair<string, object> item in param)
                {
                    xml.AppendFormat("<{0}>{1}</{0}>", item.Key, item.Value.ToStr().Replace("<", "&lt;").Replace(">", "&gt;"));
                }

                xml.AppendFormat("</{0}>", method);
                xml.Append("</soap:Body>");
                xml.Append("</soap:Envelope>");

                var handle = new HttpRequestMessage();
                handle.Version = new Version(version, minor);
                handle.Content = new StringContent(xml.ToString(), Encoding.UTF8, "text/xml");
                handle.Method = HttpMethod.Post;
                handle.RequestUri = new Uri(url);
                var response = http.SendAsync(handle).Result;
                handle.Content?.Dispose();
                handle.Dispose();
                var result = response.Content.ReadAsStringAsync().Result;

                result = result.Replace("soap:Envelope", "Envelope");
                result = result.Replace("soap:Body", "Body");
                result = result.Replace(" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"", "");
                result = result.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                result = result.Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                result = result.Replace(" xmlns=\"http://openmas.chinamobile.com/pulgin\"", "");
                result = result.Replace(string.Format(" xmlns=\"{0}\"", Namespace), "");
                return BaseXml.GetXmlString(result, string.Format("Envelope/Body/{0}Response/{0}Result", method)).Replace("&lt;", "<").Replace("&gt;", ">");
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => { BaseLog.SaveLog(url + ":" + ex.ToString(), "PostSoap_exp"); });
                return null;
            }
        }
        #endregion

        #region delete url (delete)
        /// <summary>
        /// delete url (delete)
        /// </summary>
        public static string DeleteUrl(string url, int version = 1, int minor = 1, string mediaType = "application/json")
        {
            try
            {
                var handle = new HttpRequestMessage();
                handle.Version = new Version(version, minor);
                handle.Content = new StringContent("", Encoding.UTF8, mediaType);
                handle.Method = HttpMethod.Delete;
                handle.RequestUri = new Uri(url);
                var response = http.SendAsync(handle).Result;
                handle.Content?.Dispose();
                handle.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => { BaseLog.SaveLog(url + ":" + ex.ToString(), "PutUrl_exp"); });
                BaseLog.SaveLog(url + ":" + ex.ToString(), "DeleteUrl_exp");
                return null;
            }
        }
        #endregion

        /// <summary>
        /// Delete form
        /// </summary>
        public static string DeleteForm(string url, List<KeyValuePair<string, string>> param)
        {
            try
            {
                var handle = new HttpRequestMessage();
                handle.Content = new FormUrlEncodedContent(param);
                handle.Method = HttpMethod.Delete;
                handle.RequestUri = new Uri(url);
                var response = http.SendAsync(handle).Result;
                handle.Content?.Dispose();
                handle.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
