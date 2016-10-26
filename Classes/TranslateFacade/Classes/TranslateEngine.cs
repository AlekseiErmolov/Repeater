using Repeater.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Repeater.Classes.TranslateFacade
{
    public class TranslateEngine : ITranslateEngine
    {
        private const string strTranslatorAccessURI = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";

        ILoggerWrap _logger;

        public TranslateEngine(ILoggerWrap logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Получает ключ
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            return GetAuthenticateHeader();
        }

        /// <summary>
        ///     Делает запрос и получает переведенный текст
        /// </summary>
        /// <param name="txtToTranslate"></param>
        /// <returns></returns>
        public string TranslateText(string key, string txtToTranslate, string from, string to)
        {
            string result = string.Empty;

            WebRequestModel translateRequest = BuildTranslateRequest(key, txtToTranslate, from, to);
            string response = GetResponse(translateRequest);

            if (!string.IsNullOrEmpty(response))
            {
                var xTranslation = new XmlDocument();
                xTranslation.LoadXml(response);
                result = xTranslation.InnerText;
            }

            return result;
        }

        /// <summary>
        ///     Асинхронный запрос
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public static Task<string> MakeAsyncRequest(WebRequestModel requestModel)
        {
            var task = Task.Factory.FromAsync(
                requestModel.Request.BeginGetResponse,
                asyncResult => requestModel.Request.EndGetResponse(asyncResult),
                null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
        }

        /// <summary>
        ///     Билдит запрос для авторизации
        /// </summary>
        /// <returns></returns>
        private WebRequestModel BuildAuthentificateWebService()
        {
            var clientID = "5a4baa75-86e3-4523-b2bc-3bfcda5023fa";
            var clientSecret = "UnFTrIdP4RkbZBkJx5eTIJqP37yX0+maqhAsCzJ8cEk";

            string strRequestDetails =
                string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",
                    HttpUtility.UrlEncode(clientID),
                    HttpUtility.UrlEncode(clientSecret));

            WebRequest webRequest = WebRequest.Create(strTranslatorAccessURI);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            var bytes = Encoding.ASCII.GetBytes(strRequestDetails);

            var retval = new WebRequestModel
            {
                Request = webRequest,
                ByteArray = bytes
            };

            return retval;
        }

        /// <summary>
        ///     Формирует запрос для перевода
        /// </summary>
        /// <param name="txtToTranslate"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private WebRequestModel BuildTranslateRequest(string key, string txtToTranslate, string from, string to)
        {
            string uri =
                "http://api.microsofttranslator.com/v2/Http.svc/Translate?text="
                + HttpUtility.UrlEncode(txtToTranslate)
                + string.Format("&from={0}&to={1}", from, to);

            WebRequest translationWebRequest = WebRequest.Create(uri);
            translationWebRequest.Headers.Add("Authorization", key);

            var retval = new WebRequestModel
            {
                Request = translationWebRequest,
                ByteArray = null
            };

            return retval;
        }

        /// <summary>
        ///     Формирует заголовок
        /// </summary>
        /// <returns></returns>
        private string GetAuthenticateHeader()
        {
            WebRequestModel requestModel = BuildAuthentificateWebService();

            // Get the request stream.
            using (Stream dataStream = requestModel.Request.GetRequestStream())
            {
                dataStream.Write(requestModel.ByteArray, 0, requestModel.ByteArray.Length);
                dataStream.Close();
            }

            string response = GetResponse(requestModel);
            var token = JsonHelper.From<AdmAccessToken>(response);
            string headerValue = "Bearer " + token.access_token;

            return headerValue;
        }

        /// <summary>
        ///     Получение результат
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        private string GetResponse(WebRequestModel requestModel)
        {
            string answer = string.Empty;

            // Get the response.
            try
            {
                using (WebResponse response = requestModel.Request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(dataStream))
                        {
                            string responseFromServer = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(responseFromServer))
                            {
                                answer = responseFromServer;
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                string finalMessage = string.Empty;
                var message = GetResponceMessage(ex.Response);
                if (!string.IsNullOrEmpty(message))
                {
                    finalMessage = "Response is:\r\n" + message;
                }
                _logger.WriteError(string.IsNullOrEmpty(finalMessage) ? ex.Message : finalMessage);
            }

            return answer;
        }

        private string GetResponceMessage(WebResponse response)
        {
            string message = string.Empty;

            if (response != null)
            {
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    message = reader.ReadToEnd();
                }
            }

            return message;
        }

        /// <summary>
        ///     Читает ответ
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(responseStream))
                {
                    //Need to return this response 
                    string strContent = sr.ReadToEnd();
                    if (!string.IsNullOrEmpty(strContent))
                    {
                        var xTranslation = new XmlDocument();
                        xTranslation.LoadXml(strContent);
                        strContent = xTranslation.InnerText;
                    }
                    return strContent;
                }
            }
        }
    }
}