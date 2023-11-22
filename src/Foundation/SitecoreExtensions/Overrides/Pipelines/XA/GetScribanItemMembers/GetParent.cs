namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GetScribanItemMembers
{
    public class GetParent : GetScribanItemMember
    {
        protected override string MemberName => "parent";

        protected override void Resolve(GetScribanItemMembersPipelineArgs args)
        {
            if (args.Mode == MemberMode.Name)
                args.Members.Add(MemberName);
            else
                args.MemberValue = args.Item.Parent;
        }
    }
}