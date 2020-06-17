using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public abstract class ValidateModelBase : ObservableObject, IDataErrorInfo
    {
        private readonly Dictionary<string, string> _dataErrors = new Dictionary<string, string>();

        public bool IsValidate => _dataErrors.Count <= 0;

        public string Error => null;

        public string this[string colName]
        {
            get
            {
                ValidationContext vc = new ValidationContext(this, null, null)
                {
                    MemberName = colName
                };
                var res = new List<ValidationResult>();
                var result = Validator.TryValidateProperty(
                    this.GetType().GetProperty(colName).GetValue(this, null), vc, res);
                if (res.Count > 0)
                {
                    AddDic(_dataErrors, vc.MemberName);
                    string msg = string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                    return msg;
                }
                RemoveDic(_dataErrors, vc.MemberName);
                return null;
            }
        }

        private void RemoveDic(Dictionary<string, string> dic, string dicKey)
        {
            dic.Remove(dicKey);
        }

        private void AddDic(Dictionary<string, string> dic, string dicKey)
        {
            if (!dic.ContainsKey(dicKey))
                dic.Add(dicKey, "");
        }
    }
}
