using Scriban;
using Sitecore.XA.Foundation.Mvc.Models;

namespace OneWeb.Foundation.SitecoreExtensions.Services
{
    public interface IScribanTemplateRenderer
    {
        TemplateContext GetTemplateContext(
            bool isControlEditable,
            RenderingWebEditingParams webEditingParameters);

        Template Parse(string template, string name);

        string Render(Template template, TemplateContext context);
    }
}