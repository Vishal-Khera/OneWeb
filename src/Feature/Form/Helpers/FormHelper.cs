using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.Diagnostics;
using OneWeb.Feature.Form.Constants;
using OneWeb.Foundation.SitecoreExtensions;

namespace OneWeb.Feature.Form.Helpers
{
    public class FormHelper
    {
        public static string FetchReferenceNumber(FormSubmitContext formSubmitContext, Constant.ReferenceNumberAccessType accessType)
        {
            return GetReferenceNumber(formSubmitContext.FormId.ToString(), accessType);
        }

        public static void SetValue(IViewModel postedField, string value)
        {
            Assert.ArgumentNotNull((object)postedField, "postedField");
            var valueField = postedField as IValueField;
            var property = postedField.GetType().GetProperty("Value");
            property.SetValue((object)postedField, (object)value);
        }

        private static string GetReferenceNumber(string formId, Constant.ReferenceNumberAccessType accessType)
        {
            var contextSiteName = Context.Site.Name?.ToLower();
            Sitecore.Data.Database db = Sitecore.Context.Database;
            using (var connection = new SqlConnection(Sitecore.Configuration.Settings.GetConnectionString("oneweb")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                if (db.Name == "master")
                {
                    switch (accessType)
                    {
                        case Constant.ReferenceNumberAccessType.IncrementAndLock:
                            command = new SqlCommand("sp_SXA_FetchAndLockRMANumber_CM", connection);
                            break;
                        case Constant.ReferenceNumberAccessType.Read:
                            command = new SqlCommand("sp_SXA_FetchRMANumber_CM", connection);
                            break;
                        case Constant.ReferenceNumberAccessType.ReadAndRelease:
                            command = new SqlCommand("sp_SXA_ReleaseRMANumber_CM", connection);
                            break;
                    }
                }
                else
                {
                    switch (accessType)
                    {
                        case Constant.ReferenceNumberAccessType.IncrementAndLock:
                            command = new SqlCommand("sp_SXA_FetchAndLockRMANumber", connection);
                            break;
                        case Constant.ReferenceNumberAccessType.Read:
                            command = new SqlCommand("sp_SXA_FetchRMANumber", connection);
                            break;
                        case Constant.ReferenceNumberAccessType.ReadAndRelease:
                            command = new SqlCommand("sp_SXA_ReleaseRMANumber", connection);
                            break;
                    }
                }
                using (command)
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@SiteName", SqlDbType.VarChar).Value = contextSiteName;
                    command.Parameters.Add("@FormId", SqlDbType.VarChar).Value = formId;
                    var returnParameter = command.Parameters.Add("RetVal", SqlDbType.VarChar);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    command.ExecuteNonQuery();
                    connection.Close();
                    return returnParameter.Value.ToString();
                }
            }
        }             
    }
}

