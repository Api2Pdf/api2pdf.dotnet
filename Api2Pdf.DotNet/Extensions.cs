using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Api2PdfLibrary.Extensions
{
    public static class HttpClientExtensions
    {
        public static T PostPdfRequest<T>(this HttpClient httpClient, string url, object obj)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var content = new StringContent(JsonConvert.SerializeObject(obj, serializerSettings));
            return JsonConvert.DeserializeObject<T>(httpClient.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result);
        }

        public static async Task<T> PostPdfRequestAsync<T>(this HttpClient httpClient, string url, object obj)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var content = new StringContent(JsonConvert.SerializeObject(obj, serializerSettings));
            var response = await httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        
        public static T DeletePdfRequest<T>(this HttpClient httpClient, string url)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return JsonConvert.DeserializeObject<T>(httpClient.DeleteAsync(url).Result.Content.ReadAsStringAsync().Result);
        }
        
        public static async Task<T> DeletePdfRequestAsync<T>(this HttpClient httpClient, string url)
        {
            var response = await httpClient.DeleteAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}
