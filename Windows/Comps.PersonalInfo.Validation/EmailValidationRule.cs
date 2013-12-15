using IDKin.IM.Windows.Util;
using System;
using System.Globalization;
using System.Windows.Controls;
namespace IDKin.IM.Windows.Comps.PersonalInfo.Validation
{
	internal class EmailValidationRule : ValidationRule
	{
		private string email;
		public string Email
		{
			get
			{
				return this.email;
			}
			set
			{
				this.email = value;
			}
		}
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			ValidationResult result = new ValidationResult(true, null);
			this.email = (value ?? string.Empty).ToString();
			if (this.email == null || string.IsNullOrWhiteSpace(this.email))
			{
				result = new ValidationResult(false, "Email不能为空！");
			}
			else
			{
				if (!ValidationUtil.IsValidEmail(this.email))
				{
					result = new ValidationResult(false, "Email格式不正确，请重新输入！");
				}
			}
			return result;
		}
	}
}
