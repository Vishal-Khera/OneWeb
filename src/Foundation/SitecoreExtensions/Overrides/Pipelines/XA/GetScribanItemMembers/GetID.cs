namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GetScribanItemMembers
{
    public class GetID : GetScribanItemMember
    {
        protected override string MemberName => "id";

        protected override void Resolve(GetScribanItemMembersPipelineArgs args)
        {
            if (args.Mode == MemberMode.Name)
                args.Members.Add(MemberName);
            else
                args.MemberValue = args.Item.ID;
        }
    }
}