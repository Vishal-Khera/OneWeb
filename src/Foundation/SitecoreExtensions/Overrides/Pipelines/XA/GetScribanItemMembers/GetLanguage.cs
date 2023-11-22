namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GetScribanItemMembers
{
    public class GetLanguage : GetScribanItemMember
    {
        protected override string MemberName => "language";

        protected override void Resolve(GetScribanItemMembersPipelineArgs args)
        {
            if (args.Mode == MemberMode.Name)
                args.Members.Add(MemberName);
            else
                args.MemberValue = args.Item.Language.Name;
        }
    }
}