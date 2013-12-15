using System;
using System.Globalization;
using System.Windows.Controls;
namespace IDKin.IM.Windows.Comps.PersonalInfo.Validation
{
	internal class BirthdayValidationRule : ValidationRule
	{
		private string birthday;
		public string Birthday
		{
			get
			{
				return this.birthday;
			}
			set
			{
				this.birthday = value;
			}
		}
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			ValidationResult result = new ValidationResult(true, null);
			this.birthday = (value ?? string.Empty).ToString();
			if (this.birthday == null || string.IsNullOrWhiteSpace(this.birthday))
			{
				result = new ValidationResult(false, "生日不能为空！");
			}
			return result;
		}
	}
}
