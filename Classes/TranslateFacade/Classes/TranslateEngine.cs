using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Repeater.Classes.TranslateFacade.Classes;
using Repeater.Interfaces;

namespace Repeater.Classes.TranslateFacade
{
    public class TranslateEngine : ITranslateEngine
    {
        private const string strTranslatorAccessURI = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";

        private readonly ILoggerWrap _logger;

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
            var result = string.Empty;

            var translateRequest = BuildTranslateRequest(key, txtToTranslate, from, to);
            var response = GetResponse(translateRequest);

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
            var clientID = "Repeater2016";
            var clientSecret = "4ldwSHBcNftSnSNfgCu32Zi3XlFl02+q4TpocNCSaPE=";

            var strRequestDetails =
                string.Format(
                    "grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",
                    HttpUtility.UrlEncode(clientID),
                    HttpUtility.UrlEncode(clientSecret));

            var webRequest = WebRequest.Create(strTranslatorAccessURI);
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
            var uri =
                "http://api.microsofttranslator.com/v2/Http.svc/Translate?text="
                + HttpUtility.UrlEncode(txtToTranslate)
                + string.Format("&from={0}&to={1}", from, to);

            var translationWebRequest = WebRequest.Create(uri);
            translationWebRequest.ContentType = "application/xml; charset=utf-8";

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
            var requestModel = BuildAuthentificateWebService();

            // Get the request stream.
            using (var dataStream = requestModel.Request.GetRequestStream())
            {
                dataStream.Write(requestModel.ByteArray, 0, requestModel.ByteArray.Length);
                dataStream.Close();
            }

            var response = GetResponse(requestModel);
            var token = JsonHelper.From<AdmAccessToken>(response);
            var resultToken = "Bearer " + token.AccessToken;

            return resultToken;
        }

        /// <summary>
        ///     Получение результат
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        private string GetResponse(WebRequestModel requestModel)
        {
            var answer = string.Empty;

            // Get the response.
            try
            {
                using (var response = requestModel.Request.GetResponse())
                {
                    using (var dataStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(dataStream))
                        {
                            var responseFromServer = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(responseFromServer))
                                answer = responseFromServer;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                var finalMessage = string.Empty;
                var message = GetResponceMessage(ex.Response);
                if (!string.IsNullOrEmpty(message))
                    finalMessage = "Response is:\r\n" + message;
                _logger.WriteError(string.IsNullOrEmpty(finalMessage) ? ex.Message : finalMessage);
            }

            return answer;
        }

        private string GetResponceMessage(WebResponse response)
        {
            var message = string.Empty;

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
            using (var responseStream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(responseStream))
                {
                    //Need to return this response 
                    var strContent = sr.ReadToEnd();
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