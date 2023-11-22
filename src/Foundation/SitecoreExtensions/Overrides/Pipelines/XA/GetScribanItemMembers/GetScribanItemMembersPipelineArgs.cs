using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GetScribanItemMembers
{
    [Serializable]
    public class GetScribanItemMembersPipelineArgs : PipelineArgs
    {
        public GetScribanItemMembersPipelineArgs()
        {
            Members = new List<string>();
        }

        protected GetScribanItemMembersPipelineArgs(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Members = new List<string>();
        }

        public MemberMode Mode { get; set; } = MemberMode.Value;

        public List<string> Members { get; set; }

        public string MemberName { get; set; }

        public object MemberValue { get; set; }

        public Item Item { get; set; }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            base.GetObjectData(info, context);
            info.AddValue("Mode", Mode);
            info.AddValue("MemberName", MemberName);
            info.AddValue("MemberValue", MemberValue);
            info.AddValue("Item", Item);
        }
    }
}