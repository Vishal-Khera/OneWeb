// Decompiled with JetBrains decompiler
// Type: OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext.AddTranslateFunction
// Assembly: Sitecore.XA.Foundation.Scriban, Version=6.10.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11AE1DEC-A15D-4D6E-9649-838ED84316F4
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.XA.Foundation.Scriban.dll

using Scriban.Runtime;
using Sitecore.Abstractions;
using Sitecore.Globalization;
using Sitecore.XA.Foundation.Abstractions;
using Sitecore.XA.Foundation.Multisite;
using Sitecore.XA.Foundation.SitecoreExtensions.Interfaces;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddTranslateFunction : IGenerateScribanContextProcessor
    {
        protected readonly BaseLanguageManager BaseLanguageManager;
        protected readonly IContext Context;
        protected readonly IMultisiteContext MultisiteContext;
        protected readonly IPageContext PageContext;

        public AddTranslateFunction(
            IContext context,
            IMultisiteContext multisiteContext,
            BaseLanguageManager baseLanguageManager,
            IPageContext pageContext)
        {
            Context = context;
            BaseLanguageManager = baseLanguageManager;
            MultisiteContext = multisiteContext;
            PageContext = pageContext;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Add("o_language", Context.Language);
            args.GlobalScriptObject.Import("sc_translate", new TranslateText(TranslateTextImpl));
        }

        public string TranslateTextImpl(string key, string languageName = null)
        {
            var language = string.IsNullOrEmpty(languageName)
                ? Context.Language
                : BaseLanguageManager.GetLanguage(languageName);
            return Translate.TextByLanguage(GetDictionaryDomain().Name, TranslateOptions.Default, key, language, key,
                null);
        }

        protected virtual DictionaryDomain GetDictionaryDomain()
        {
            var domainDefinition = "";
            DictionaryDomain domain;
            return !DictionaryDomain.TryParse(domainDefinition, Context.Database, out domain)
                ? DictionaryDomain.GetDefaultDomain(Context.Database)
                : domain;
        }

        private delegate string TranslateText(string translationItemName, string language = null);
    }
}