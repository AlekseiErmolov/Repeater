using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Repeater.Infrastructure.TranslateFacade.Entities;
using Repeater.Infrastructure.TranslateFacade.Helpers;
using Repeater.Infrastructure.TranslateFacade.Interfaces;
using Repeater.Interfaces;

namespace Repeater.Infrastructure.TranslateFacade.Classes
{
    public class TranslateEngine : ITranslateEngine
    {
        private readonly ILoggerWrap _logger;

        public TranslateEngine(ILoggerWrap logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Получает ключ
        /// </summary>
        public async Task<string> GetKey()
        {
            return await GetAuthenticateHeader();
        }

        /// <summary>
        ///     Делает запрос и получает переведенный текст
        /// </summary>
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
        private WebRequestModel BuildAuthentificateWebService()
        {
            var strRequestDetails =
                string.Format(
                    "grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",
                    HttpUtility.UrlEncode(ConfigurationManager.AppSettings["clientID"]),
                    HttpUtility.UrlEncode(ConfigurationManager.AppSettings["clientSecret"]));

            var webRequest = WebRequest.Create(ConfigurationManager.AppSettings["translateUrlGet"]);
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
        private WebRequestModel BuildTranslateRequest(string key, string txtToTranslate, string from, string to)
        {
            var uri = ConfigurationManager.AppSettings["translateUrl"] + HttpUtility.UrlEncode(txtToTranslate) +
                      $"&from={from}&to={to}";

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
        private async Task<string> GetResponse(WebRequestModel requestModel)
        {
            var answer = string.Empty;

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
    }
}