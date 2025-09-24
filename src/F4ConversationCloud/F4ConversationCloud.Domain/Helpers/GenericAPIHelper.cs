using F4ConversationCloud.Domain.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace F4ConversationCloud.Domain.Helpers
{
    public static class GenericAPIHelper
    {
        public static APICallGenericResponse CallAPIGeneric<T>(APIRequestModel<T> RequestAPI)
        {
            APICallGenericResponse response = new APICallGenericResponse();
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                clientHandler.Proxy = null;
                var httpClientHandler = new HttpClientHandler() { Proxy = null };
                using (HttpClient client = new HttpClient(clientHandler))
                {
                    client.Timeout = Timeout.InfiniteTimeSpan;
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
                    if (RequestAPI.RequestType.ToLower() != "formpost" && RequestAPI.RequestType.ToLower() != "get" && RequestAPI.RequestType.ToLower() != "delete" && RequestAPI.RequestType.ToLower() != "deletebody")
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
                            response.Response = client.SendAsync(request).Result;
                        }
                        else if (RequestAPI.RequestType.ToLower() == "post")
                        {
                            if (RequestAPI.BodyType == "json")
                            {
                                response.Response = client.PostAsync(RequestAPI.EndPoint, ByteContent).Result;
                            }
                            else if (RequestAPI.BodyType == "xml")
                            {
                                string xmlPayload = GenericExtensions.SerializeToXml<T>(RequestAPI.RequestBody);
                                var httpContent = new StringContent(xmlPayload, Encoding.UTF8, "application/xml");

                                response.Response = client.PostAsync(RequestAPI.EndPoint, httpContent).Result;
                            }
                        }
                        else if (RequestAPI.RequestType.ToLower() == "put")
                        {
                            response.Response = client.PutAsync(RequestAPI.EndPoint, ByteContent).Result;
                        }
                    }
                    else if (RequestAPI.RequestType.ToLower() == "get")
                    {
                        response.Response = client.GetAsync(RequestAPI.EndPoint).Result;
                    }
                    else if (RequestAPI.RequestType.ToLower() == "delete")
                    {
                        response.Response = client.DeleteAsync(RequestAPI.EndPoint).Result;
                    }
                    else if (RequestAPI.RequestType.ToLower() == "deletebody")
                    {
                        var request = new HttpRequestMessage(HttpMethod.Delete, RequestAPI.EndPoint);
                        request.Content = new StringContent(JsonConvert.SerializeObject(RequestAPI.RequestBody), Encoding.UTF8, "application/json");
                        response.Response = client.SendAsync(request).Result;
                    }

                    response.Status = response.Response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = JsonConvert.SerializeObject(ex);
            }
            return response;
        }
    }
}
