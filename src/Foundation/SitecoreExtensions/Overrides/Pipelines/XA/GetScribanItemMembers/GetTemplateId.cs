namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GetScribanItemMembers
{
    public class GetTemplateId : GetScribanItemMember
    {
        protected override string MemberName => "template_id";

        protected override void Resolve(GetScribanItemMembersPipelineArgs args)
        {
            if (args.Mode == MemberMode.Name)
                args.Members.Add(MemberName);
            else
                args.MemberValue = args.Item.TemplateID.ToShortID();
        }
    }
}