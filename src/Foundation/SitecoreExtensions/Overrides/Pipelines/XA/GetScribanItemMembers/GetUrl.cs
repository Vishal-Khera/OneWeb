using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Multisite.Services;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GetScribanItemMembers
{
    public class GetUrl : GetScribanItemMember
    {
        protected readonly ILinkProviderService LinkProviderService;

        public GetUrl(ILinkProviderService linkProviderService)
        {
            LinkProviderService = linkProviderService;
        }

        protected override string MemberName => "url";

        protected override void Resolve(GetScribanItemMembersPipelineArgs args)
        {
            if (args.Mode == MemberMode.Name)
                args.Members.Add(MemberName);
            else if (args.Item == null)
                args.MemberValue = "#";
            else
                args.MemberValue = args.Item.GetItemUrl();
        }
    }
}