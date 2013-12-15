using System;
using System.Globalization;
using System.Windows.Controls;
namespace IDKin.IM.Windows.Comps.PersonalInfo.Validation
{
	internal class RealNameValidationRule : ValidationRule
	{
		private string name;
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			ValidationResult result = new ValidationResult(true, null);
			if (value == null || (value is string && string.IsNullOrWhiteSpace(value.ToString())))
			{
				result = new ValidationResult(false, "真实姓名不能为空！");
			}
			return result;
		}
	}
}
