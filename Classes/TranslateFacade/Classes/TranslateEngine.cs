using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Repeater.Classes.TranslateFacade.Interfaces;
using Repeater.Interfaces;

namespace Repeater.Classes.TranslateFacade.Classes
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
        public async Task<string> GetKey()
        {
            return await GetAuthenticateHeader();
        }

        /// <summary>
        ///     Делает запрос и получает переведенный текст
        /// </summary>
        /// <param name="txtToTranslate"></param>
        /// <returns></returns>
        public async Task<string> TranslateText(string key, string txtToTranslate, string from, string to)
        {
            var result = string.Empty;

            var translateRequest = BuildTranslateRequest(key, txtToTranslate, from, to);
            var response = await GetResponse(translateRequest);

            if (!string.IsNullOrEmpty(response))
            {
                var xTranslation = new XmlDocument();
                xTranslation.LoadXml(response);
                result = xTranslation.InnerText;
            }

            return result;
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
        private async Task<string> GetAuthenticateHeader()
        {
            var requestModel = BuildAuthentificateWebService();

            // Get the request stream.
            using (var dataStream = await requestModel.Request.GetRequestStreamAsync())
            {
                dataStream.Write(requestModel.ByteArray, 0, requestModel.ByteArray.Length);
            }

            var response = await GetResponse(requestModel);
            var token = JsonHelper.From<AdmAccessToken>(response);
            var resultToken = "Bearer " + token.AccessToken;

            return resultToken;
        }

        /// <summary>
        ///     Получение результат
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        private async Task<string> GetResponse(WebRequestModel requestModel)
        {
            var answer = string.Empty;

            // Get the response.
            try
            {
                using (var response = await requestModel.Request.GetResponseAsync())
                using (var dataStream = response.GetResponseStream())
                using (var reader = new StreamReader(dataStream))
                {
                    var responseFromServer = await reader.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(responseFromServer))
                        answer = responseFromServer;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }

            return answer;
        }

        /// <summary>
        ///     Читает ответ
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<string> ReadStreamFromResponse(WebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            using (var sr = new StreamReader(responseStream))
            {
                //Need to return this response 
                var strContent = await sr.ReadToEndAsync();
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