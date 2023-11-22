using OneWeb.Feature.Login.Models;
using Sitecore.Data.Items;

namespace OneWeb.Feature.Login.Repositories
{
    public interface IUserSearchBox
    {
        UserSearchBoxRenderingModel RenderUserSearchBox(Item item);
        UserLoginModel RenderUserLogin(Item datasourceItem);

        DownloadFormModel RenderDownloadForm(Item datasourceItem);
    }
}