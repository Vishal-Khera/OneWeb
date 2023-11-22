using OneWeb.Feature.Login.Models;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;

namespace OneWeb.Feature.Login.Repositories
{
    public class UserSearchBox : VariantsRepository, IUserSearchBox
    {
        public UserSearchBoxRenderingModel RenderUserSearchBox(Item datasource)
        {
            UserSearchBoxRenderingModel model = new UserSearchBoxRenderingModel();
            this.FillBaseProperties(model);
            model.EmptyTextMessage = datasource.GetFieldValue(OneWebUserSearchBox.Fields.EmptyTextMessage_FieldName);
            model.ShowUsersByDefault = datasource.GetFieldValue(OneWebUserSearchBox.Fields.ShowUsersByDefault_FieldName) == "1";
            model.PageSize = int.TryParse(datasource.GetFieldValue(OneWebUserSearchBox.Fields.PageSize_FieldName), out int pageSize) ? pageSize : 10;
            return model;
        }
        public UserLoginModel RenderUserLogin(Item datasource)
        {
            UserLoginModel model = new UserLoginModel();
            this.FillBaseProperties(model);
            model.Link = new Foundation.SitecoreExtensions.Models.LinkModel(datasource, OneWebUserLogin.Fields.Link_FieldName);
            model.ShowInPopup = datasource.GetFieldValue(OneWebUserLogin.Fields.ShowInPopup_FieldName) == "1";
            if (model.ShowInPopup)
            {
                Item formItem = datasource.GetReferencedItem(OneWebUserLogin.Fields.LoginFormLink_FieldName);
                model.FormID = formItem != null ? formItem.ID.ToString() : string.Empty; 
            }
            return model;
        }

        public DownloadFormModel RenderDownloadForm(Item datasource)
        {
            DownloadFormModel model = new DownloadFormModel();
            this.FillBaseProperties(model);
            model.Media = new Foundation.SitecoreExtensions.Models.MediaModel(datasource, BaseMedia.Fields.Media_FieldName).Url;
            model.Title = datasource.GetFieldValue(OneWebDownloadForm.Fields.Title);
            var settings = SiteExtensions.GetSettingsItem();
            Item formItem = settings.GetReferencedItem(SiteConfigurations.Fields.DownloadFormLink);
              model.FormId = formItem != null ? formItem.ID.ToString() : string.Empty;
            model.VerifyUserIdentity = settings.GetFieldValue(SiteConfigurations.Fields.VerifyUserIdentity_FieldName)=="1"?true:false;
            return model;
        }
    }
}