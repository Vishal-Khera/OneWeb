// Decompiled with JetBrains decompiler
// Type: Sitecore.ExperienceForms.Processing.Actions.RedirectToPage
// Assembly: Sitecore.ExperienceForms, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A1E6B42A-5C54-47F7-AA35-9DBD85993A83
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.ExperienceForms.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using OneWeb.Feature.Form.Model;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.Links;
using Sitecore.Text;


namespace OneWeb.Feature.Form.SubmitActions
{
    public class SendData : SubmitActionBase<SendDataModel>
    {
        public SendData(ISubmitActionData submitActionData)
            : base(submitActionData)
        {
        }
        private static HttpClient GetHttpClient(string url, string token = null)
        {
            var client = new HttpClient { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Accept.Clear();
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
        public static async Task PostData(string url, Dictionary<string, string> data)
        {
            using (var client = new HttpClient())
            {
                var res = client.PostAsync(url, new FormUrlEncodedContent(data)).Result;
                var content = await res.Content.ReadAsStringAsync();

                string postData = string.Empty;
                foreach (var field in data)
                {
                    postData += field.Key + "=" + HttpUtility.UrlEncode(field.Value, Encoding.UTF8) + "&";
                }

                Sitecore.Diagnostics.Log.Warn("Dkm form response " + res.StatusCode + "Postdata " + postData, typeof(SendData));
            }
        }
        public object PostAsync<T>(string url, string urlParameters)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var data = Encoding.ASCII.GetBytes(urlParameters);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                Sitecore.Diagnostics.Log.Warn("DKM Events CRM responset " + response.StatusCode + " for " + url + "?" + urlParameters, typeof(SendData));

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var json = response.StatusDescription;
                    var result = JsonConvert.DeserializeObject<T>(json);
                    return result;
                }
                return response;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }
        private static string GetValue(IViewModel field)
        {
            if (field == null)
            { return default(string); }


            if ((field as StringInputViewModel) != null)
            {
                return (string)(object)(field as StringInputViewModel).Value;
            }

            if (field is ListViewModel)
            {
                var listField = (ListViewModel)field;
                var array = listField?.Value?.ToArray();
                if (array == null)
                {
                    return string.Empty;
                }
                return String.Join(",", array);
            }

            if (field is DateViewModel)
            {
                var dateField = (DateViewModel)field;
                return dateField.Value.HasValue ? dateField.Value.Value.ToShortDateString() : string.Empty;
            }

            if (field is NumberViewModel)
            {
                var numberField = (NumberViewModel)field;
                return numberField.Value.HasValue ? numberField.Value.ToString() : string.Empty;
            }

            if (field is TextViewModel)
            {
                var textField = (TextViewModel)field;
                return (string)(object)textField.Text;
            }

            if (field is CheckBoxViewModel)
            {
                var checkbox = (CheckBoxListViewModel)field;
                return (string)(object)checkbox.Value;
            }


            return default(string);
        }
        private static Dictionary<string, string> FormFieldsToDictionary(IList<IViewModel> fields, string formField)
        {
            Dictionary<string, string> fielDictionary = new Dictionary<string, string>();
            foreach (var field in fields)
            {
                var value = field.GetType().GetProperty("Value")?.GetValue(field, null);
                if (value != null)
                {
                    if (value.GetType() == typeof(String) || value.GetType() == typeof(Boolean) || value.GetType() == typeof(Int32))
                    {
                        var fieldId = "fxb_" + formField + "_Fields_" + Guid.Parse(field.ItemId) + "__Value";
                        fielDictionary.Add(fieldId, value.ToString());
                    }
                    else
                    {
                        var fieldId = "fxb_" + formField + "Fields_Index_" + Guid.Parse(field.ItemId);
                        var array = value as dynamic;
                        fielDictionary.Add(fieldId, array[0]);
                    }
                }

            }
            return fielDictionary;
        }
        protected override bool Execute(SendDataModel data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull((object)formSubmitContext, nameof(formSubmitContext));
            if (data == null || (string.IsNullOrWhiteSpace(data.ExternalLink)) && !(data.InternalItemId != Guid.Empty))
                return false;

            string postData = string.Empty;
            var fieldsDictionary = FormFieldsToDictionary(formSubmitContext.Fields, data.Anchor);
            var url = data.ExternalLink;

            PostData(url, fieldsDictionary);
            return true;
        }
    }
}