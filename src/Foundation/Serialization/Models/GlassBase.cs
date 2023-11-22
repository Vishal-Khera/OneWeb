// Decompiled with JetBrains decompiler
// Type: GDT.Glass_Mapper.Classes.BaseTypes.GlassBase
// Assembly: GDT.Glass_Mapper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43BE4B5A-3494-4B9A-BC56-63D61AE13742
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\packages\dev.GDT.Glass_Mapper93.2.0.28\lib\GDT.Glass_Mapper.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Globalization;

namespace OneWeb.Foundation.Serialization.Models
{
    public abstract class GlassBase : IGlassBase
    {
        [SitecoreId] public virtual Guid Id { get; set; }

        [SitecoreInfo(SitecoreInfoType.Language)]
        public virtual Language Language { get; private set; }

        [SitecoreInfo(SitecoreInfoType.Version)]
        public virtual int Version { get; private set; }

        [SitecoreInfo(SitecoreInfoType.Url)] public virtual string Url { get; private set; }

        [SitecoreInfo(SitecoreInfoType.Url, UrlOptions = SitecoreInfoUrlOptions.LanguageEmbeddingAlways)]
        public virtual string UrlEmbedLang { get; private set; }

        [SitecoreInfo(SitecoreInfoType.Name)] public virtual string ItemName { get; private set; }

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string ItemDisplayName { get; }

        [SitecoreInfo(SitecoreInfoType.TemplateId)]
        public virtual Guid ItemTemplateId { get; private set; }

        [SitecoreInfo(SitecoreInfoType.TemplateName)]
        public virtual string ItemTemplateName { get; private set; }

        [SitecoreInfo(SitecoreInfoType.BaseTemplateIds)]
        public virtual IEnumerable<Guid> ItemBaseTemplateIds { get; private set; }

        [SitecoreField("__Created")] public virtual DateTime CreatedDate { get; private set; }

        [SitecoreField("__Updated")] public virtual DateTime UpdatedDate { get; private set; }

        [SitecoreChildren] public virtual IEnumerable<IGlassBase> BaseChildren { get; private set; }

        public IEnumerable<T> GetChildren<T>(Guid templateId, bool checkBaseTemplate = true) where T : GlassBase
        {
            var children = new List<T>();
            ISitecoreContext context = new SitecoreContext();
            if (BaseChildren == null)
                return children;
            if (checkBaseTemplate)
                children.AddRange(BaseChildren
                    .Where(item =>
                        item.ItemTemplateId == templateId || item.ItemBaseTemplateIds.Any(x => x.Equals(templateId)))
                    .Select(item => context.GetItem<T>(item.Id)));
            else
                children.AddRange(BaseChildren.Where(item => item.ItemTemplateId == templateId)
                    .Select(item => context.GetItem<T>(item.Id)));
            return children;
        }
    }
}