using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using OneWeb.Foundation.Search.Models;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Fields;
using Sitecore.XA.Foundation.Search.Attributes;
using Sitecore.XA.Foundation.Search.Pipelines.ResolveSearchQueryTokens;

namespace OneWeb.Foundation.Search.Pipelines.ResolveSearchQueryTokens
{
    public class OneWebFieldBoost : ResolveSearchQueryTokensProcessor
    {
        protected string TokenPart { get; } = nameof(OneWebFieldBoost);
        [SxaTokenKey]
        protected override string TokenKey => FormattableString.Invariant(FormattableStringFactory.Create("{0}|FieldName|Boost", (object)this.TokenPart));

        public override void Process(ResolveSearchQueryTokensEventArgs args)
        {
            if (args.ContextItem == null)
                return;
            for (int index = 0; index < args.Models.Count; ++index)
            {
                var model = args.Models[index];
                if (model.Type.Equals("sxa") && ContainsToken(model))
                {
                    var groups = Regex.Match(model.Value, @"^OneWebFieldBoost\|(.*)\|(.*)").Groups;
                    var fieldName = string.Empty;
                    var fieldBoost = 1f;
                    if (!string.IsNullOrWhiteSpace(groups[1]?.Value))
                    {
                        fieldName = groups[1].Value;
                    }
                    if (!string.IsNullOrWhiteSpace(groups[2]?.Value))
                    {
                        fieldBoost = float.TryParse(groups[2].Value, out var boost) ? boost : 1f;
                    }

                    args.Models.Insert(index, BuildModel(fieldName, fieldBoost, model.Operation));
                    args.Models.Remove(model);
                }
            }
        }

        protected virtual SearchStringModel BuildModel(
          string fieldName,
          float boost,
          string operation)
        {
            return new SearchStringModel("ow_field_boost", FormattableString.Invariant(FormattableStringFactory.Create("{0}|{1}", (object)fieldName.ToLowerInvariant(), boost)))
            {
                Operation = operation
            };
        }

        protected override bool ContainsToken(SearchStringModel m) => Regex.Match(m.Value, @"^OneWebFieldBoost\|(.*)").Success;
    }
}
