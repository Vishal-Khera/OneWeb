using Sitecore.XA.Foundation.Abstractions;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GetScribanItemMembers
{
    public class GetHasLayout : GetScribanItemMember
    {
        protected readonly IContext Context;

        public GetHasLayout(IContext context)
        {
            Context = context;
        }

        protected override string MemberName => "has_layout";

        protected override void Resolve(GetScribanItemMembersPipelineArgs args)
        {
            if (args.Mode == MemberMode.Name)
            {
                args.Members.Add(MemberName);
            }
            else
            {
                var layout = args.Item.Visualization.GetLayout(Context.Device);
                args.MemberValue = layout != null;
            }
        }
    }
}