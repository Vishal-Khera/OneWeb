using OneWeb.Foundation.SitecoreExtensions.Extensions;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GetScribanItemMembers
{
    public class GetMediaUrl : GetScribanItemMember
    {
        protected override string MemberName => "media_url";

        protected override void Resolve(GetScribanItemMembersPipelineArgs args)
        {
            if (args.Mode == MemberMode.Name)
            {
                args.Members.Add(MemberName);
            }
            else
            {
                var membersPipelineArgs = args;
                var str = args.Item == null ? "#" : args.Item.GetMediaUrl();
                membersPipelineArgs.MemberValue = str;
            }
        }
    }
}