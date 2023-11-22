namespace OneWeb.Foundation.Login.Repositories
{
    public interface IApiService
    {
        T Post<T>(string url, string parameters, string token = null);
        T Get<T>(string url,System.Uri requestUri, string parameters, string token = null);
        T Put<T>(string url, string parameters, string token = null);
        T Patch<T>(string url, string parameters, string token = null);
    }
}