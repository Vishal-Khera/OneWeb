using OneWeb.Foundation.Serialization;
using Sitecore.Data.Items;
using Sitecore.Data.Validators;
using System;
using System.Runtime.Serialization;

namespace OneWeb.Foundation.SitecoreExtensions.Fields.Validators
{
    public class EndDateValidator : StandardValidator
	{
		public override string Name => "End Date Validator";

		public EndDateValidator()
		{

		}
		public EndDateValidator(SerializationInfo info, StreamingContext context)
		: base(info, context)
		{
		}
		protected override ValidatorResult Evaluate()
		{
			string value = ControlValidationValue;
			if (string.IsNullOrEmpty(value))
			{
				return ValidatorResult.Valid;
			}
			Item item = this.GetItem();
			DateTime startDate = Sitecore.DateUtil.IsoDateToDateTime(item.Fields[BaseDuration.Fields.StartDate_FieldName].Value);
			DateTime endDate = Sitecore.DateUtil.IsoDateToDateTime(ControlValidationValue);
			if (endDate >= startDate)
				return ValidatorResult.Valid;
			Text = $"End Date can not be less than Start Date. Please choose a valid Date";
			return GetMaxValidatorResult();
		}

		protected override ValidatorResult GetMaxValidatorResult()
		{
			return GetFailedResult(ValidatorResult.FatalError);
		}
	}
}