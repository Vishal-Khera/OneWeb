namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GetScribanItemMembers
{
    public class GetPath : GetScribanItemMember
    {
        protected override string MemberName => "path";

        protected override void Resolve(GetScribanItemMembersPipelineArgs args)
        {
            if (args.Mode == MemberMode.Name)
                args.Members.Add(MemberName);
            else
                args.MemberValue = args.Item.Paths.FullPath;
        }
    }
}