// Decompiled with JetBrains decompiler
// Type: GDT.Glass_Mapper.Classes.BaseTypes.IGlassBase
// Assembly: GDT.Glass_Mapper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43BE4B5A-3494-4B9A-BC56-63D61AE13742
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\packages\dev.GDT.Glass_Mapper93.2.0.28\lib\GDT.Glass_Mapper.dll

using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Globalization;

namespace OneWeb.Foundation.Serialization.Models
{
    public interface IGlassBase
    {
        [SitecoreId] Guid Id { get; }

        [SitecoreInfo(SitecoreInfoType.Language)]
        Language Language { get; }

        [SitecoreInfo(SitecoreInfoType.Version)]
        int Version { get; }

        [SitecoreInfo(SitecoreInfoType.Url)] string Url { get; }

        [SitecoreInfo(SitecoreInfoType.Url, UrlOptions = SitecoreInfoUrlOptions.LanguageEmbeddingAlways)]
        string UrlEmbedLang { get; }

        [SitecoreInfo(SitecoreInfoType.Name)] string ItemName { get; }

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        string ItemDisplayName { get; }

        [SitecoreInfo(SitecoreInfoType.TemplateId)]
        Guid ItemTemplateId { get; }

        [SitecoreInfo(SitecoreInfoType.TemplateName)]
        string ItemTemplateName { get; }

        [SitecoreInfo(SitecoreInfoType.BaseTemplateIds)]
        IEnumerable<Guid> ItemBaseTemplateIds { get; }

        [SitecoreField("__Created")] DateTime CreatedDate { get; }

        [SitecoreField("__Updated")] DateTime UpdatedDate { get; }

        [SitecoreChildren] IEnumerable<IGlassBase> BaseChildren { get; }

        IEnumerable<T> GetChildren<T>(Guid templateId, bool checkBaseTemplate = true) where T : GlassBase;
    }
}