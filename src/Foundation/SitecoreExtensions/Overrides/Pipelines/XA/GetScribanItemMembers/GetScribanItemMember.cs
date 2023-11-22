namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GetScribanItemMembers
{
    public abstract class GetScribanItemMember
    {
        protected abstract string MemberName { get; }

        public void Process(GetScribanItemMembersPipelineArgs args)
        {
            if (!CanResolve(args.MemberName) && args.Mode != MemberMode.Name)
                return;
            Resolve(args);
            args.AbortPipeline();
        }

        protected bool CanResolve(string memberName)
        {
            return memberName == MemberName;
        }

        protected abstract void Resolve(GetScribanItemMembersPipelineArgs args);
    }
}