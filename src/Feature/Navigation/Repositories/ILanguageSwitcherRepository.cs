using OneWeb.Feature.Navigation.Models;
using Sitecore.Data.Items;

namespace OneWeb.Feature.Navigation.Repositories
{
    public interface ILanguageSwitcherRepository
    {
        LanguageSwitcherModel GetSiteLanguages(Item datasourceItem);
    }
}