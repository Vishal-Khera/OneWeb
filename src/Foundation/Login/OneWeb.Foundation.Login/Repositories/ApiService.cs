using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace OneWeb.Foundation.Login.Repositories
{
    public class ApiService : IApiService
    {
        private static HttpClient GetHttpClient(string url, string token = null)
        {
            var client = new HttpClient { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Accept.Clear();
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
        public async Task<T> GetAsync<T>(string url,System.Uri requestUri  , string SecretKey, string token = null)
        {
            try
            {
                using (var client = GetHttpClient(url, token))
                {
                    HttpResponseMessage response =   client.GetAsync(requestUri).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(json);
                        return result;
                    }
                    return default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }
        public async Task<T> PostAsync<T>(string url, string urlParameters, string token = null)
        {
            try
            {
                var data = new StringContent(urlParameters, Encoding.UTF8, "application/json");
                using (var client = GetHttpClient(url, token))
                {
                    HttpResponseMessage response =  client.PostAsync(url, data).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var json =  await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(json);
                        return result;
                    }
                    return default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }
        public async Task<T> PutAsync<T>(string url, string urlParameters, string token = null)
        {
            try
            {
                var data = new StringContent(urlParameters, Encoding.UTF8, "application/json");
                using (var client = GetHttpClient(url, token))
                {
                    HttpResponseMessage response = client.PutAsync(url, data).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(json);
                        return result;
                    }
                    return default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }
        public async Task<T> PatchAsync<T>(string url, string urlParameters, string token = null)
        {
            try
            {
                var data = new StringContent(urlParameters, Encoding.UTF8, "application/json");
                using (var client = GetHttpClient(url, token))
                {
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
                    request.Content = data;
                    

                    HttpResponseMessage response = client.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(json);
                        return result;
                    }
                    return default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }
        public T Post<T>(string url, string parameters, string token = null)
        {
            var response = PostAsync<T>(url, parameters, token);
            if(response != null)
            {
                var result = response.Result;
                return result;
            }
            return default;
        }
        public T Get<T>(string url,System.Uri requestUri, string parameters, string token = null)
        {
            var response = GetAsync<T>(url,requestUri, parameters, token);
            if (response != null)
            {
                var result = response.Result;
                return result;
            }
            return default;
        }
        public T Put<T>(string url, string parameters, string token = null)
        {
            var response = PutAsync<T>(url, parameters, token);
            if (response != null)
            {
                var result = response.Result;
                return result;
            }
            return default;
        }
        public T Patch<T>(string url, string parameters, string token = null)
        {
            var response = PatchAsync<T>(url, parameters, token);
            if (response != null)
            {
                var result = response.Result;
                return result;
            }
            return default;
        }
    }
}