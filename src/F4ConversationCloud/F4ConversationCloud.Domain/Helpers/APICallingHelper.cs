
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using System.Xml;
using Newtonsoft.Json;
using F4ConversationCloud.Domain.Extension;
namespace F4ConversationCloud.Domain.Helpers
{
    public class APIRequestModel<T>
    {
        public string RequestType { get; set; }
        public Dictionary<string, string> OAuths { get; set; } = new Dictionary<string, string>();
        public string EndPoint { get; set; }
        public T RequestBody { get; set; }
        public string APIType { get; set; }
        public Dictionary<string, string> QuerryStringParameters { get; set; }
        public string APIKey { get; set; } // Name of API
        public bool AddLogs { get; set; } = true;
        public bool IsExternalAPI { get; set; }
        public string BodyType { get; set; }
    }
    public static class APICallingHelper
    {
        /// <summary>
        /// T will be the Class Type of Response.
        /// U will be the Class Type of Request Body
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="EndPoint"></param>
        /// <param name="MethodType"></param>
        /// <param name="RequestBody"></param>
        /// <param name="OAUths"></param>
        /// <param name="APIType"></param>
        /// <param name="QuerryStringParameters"></param>
        /// <param name="AddLogs"></param>
        /// <returns></returns>
        public static T BindMainAPIRequestModel<T, U>(string EndPoint, string MethodType, U RequestBody, Dictionary<string, string> OAUths, string APIType, Dictionary<string, string> QuerryStringParameters, bool AddLogs = true, bool IsExternalAPI = false, string BodyType = "json")
        {
            APIRequestModel<U> model = new APIRequestModel<U>
            {
                OAuths = OAUths,
                APIType = APIType,
                EndPoint = EndPoint,
                QuerryStringParameters = QuerryStringParameters,
                RequestBody = RequestBody,
                RequestType = MethodType,
                AddLogs = AddLogs,
                IsExternalAPI = IsExternalAPI,
                BodyType = BodyType
            };

            return CallAPI<T, U>(model);
        }

        /// <summary>
        /// T will the response class from API
        /// U will the request class to API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="RequestAPI"></param>
        /// <returns></returns>
        public static T CallAPI<T, U>(APIRequestModel<U> RequestAPI)
        {
            string APIResponse = string.Empty;
            HttpResponseMessage APIResponseMessage = new HttpResponseMessage();

            T ReturnResponse;
            if (typeof(T) == typeof(string))
            {
                ReturnResponse = (T)(object)string.Empty;  // Explicit cast from string to T
            }
            else
            {
                ReturnResponse = (T)Activator.CreateInstance(typeof(T));  // Standard instantiation for other types
            }

            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                clientHandler.Proxy = null;
                var httpClientHandler = new HttpClientHandler() { Proxy = null };
                using (HttpClient client = new HttpClient(clientHandler))
                {
                    client.BaseAddress = new Uri(RequestAPI.EndPoint);
                    if (RequestAPI.QuerryStringParameters != null && RequestAPI.QuerryStringParameters.Count > 0)
                    {
                        RequestAPI.EndPoint += "?"; //string.Format(RequestAPI.EndPoint + "?{0}", HttpUtility.UrlEncode(string.Join("&", RequestAPI.QuerryStringParameters.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)))));
                        int QueryCounter = 0;
                        foreach (var queries in RequestAPI.QuerryStringParameters)
                        {
                            if (QueryCounter > 0)
                            {
                                RequestAPI.EndPoint += "&";
                            }
                            RequestAPI.EndPoint += queries.Key + "=" + HttpUtility.UrlEncode(queries.Value);
                            QueryCounter++;
                        }
                    }
                    if (RequestAPI.OAuths != null && RequestAPI.OAuths.Count > 0)
                    {
                        foreach (var oauth in RequestAPI.OAuths)
                        {
                            client.DefaultRequestHeaders.Add(oauth.Key, oauth.Value);
                        }
                    }
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    if (RequestAPI.RequestType.ToLower() != "get" && RequestAPI.RequestType.ToLower() != "delete")
                    {
                        ByteArrayContent ByteContent = null;
                        if (RequestAPI.RequestBody != null)
                        {
                            var ContentBody = JsonConvert.SerializeObject(RequestAPI.RequestBody);
                            var buffer = System.Text.Encoding.UTF8.GetBytes(ContentBody);
                            ByteContent = new ByteArrayContent(buffer);
                            ByteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        }
                        if (RequestAPI.RequestType.ToLower() == "patch")
                        {
                            var method = new HttpMethod("PATCH");

                            var request = new HttpRequestMessage(method, RequestAPI.EndPoint)
                            {
                                Content = ByteContent
                            };
                            APIResponseMessage = client.SendAsync(request).Result;
                        }
                        else if (RequestAPI.RequestType.ToLower() == "post")
                        {
                            if (RequestAPI.BodyType == "json")
                            {
                                APIResponseMessage = client.PostAsync(RequestAPI.EndPoint, ByteContent).Result;
                            }
                            else if (RequestAPI.BodyType == "xml")
                            {
                                string xmlPayload = GenericExtensions.SerializeToXml<U>(RequestAPI.RequestBody);
                                var httpContent = new StringContent(xmlPayload, Encoding.Unicode, "application/xml");

                                APIResponseMessage = client.PostAsync(RequestAPI.EndPoint, httpContent).Result;
                            }
                        }
                        else if (RequestAPI.RequestType.ToLower() == "put")
                        {
                            APIResponseMessage = client.PutAsync(RequestAPI.EndPoint, ByteContent).Result;
                        }
                    }
                    else if (RequestAPI.RequestType.ToLower() == "get")
                    {
                        APIResponseMessage = client.GetAsync(RequestAPI.EndPoint).Result;
                    }
                    else if (RequestAPI.RequestType.ToLower() == "delete")
                    {
                        APIResponseMessage = client.DeleteAsync(RequestAPI.EndPoint).Result;
                    }

                    if (APIResponseMessage == null || (APIResponseMessage.StatusCode != HttpStatusCode.OK && APIResponseMessage.StatusCode != HttpStatusCode.Created && APIResponseMessage.StatusCode != HttpStatusCode.NoContent))
                    {
                        if (!RequestAPI.IsExternalAPI)
                        {
                            throw new Exception("Invalid Response from " + RequestAPI.APIType + " API");
                        }
                    }
                    APIResponse = APIResponseMessage.Content.ReadAsStringAsync().Result;
                }

                if (typeof(T) == typeof(string))
                {
                    ReturnResponse = (T)(object)APIResponse;
                }
                else
                {
                    ReturnResponse = JsonConvert.DeserializeObject<T>(APIResponse);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return ReturnResponse;
        }
    }
}
