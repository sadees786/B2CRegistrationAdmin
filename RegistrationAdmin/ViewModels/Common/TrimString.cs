using RegistrationAdmin.Models.Interfaces;

namespace RegistrationAdmin.ViewModels.Common
{
    public class TrimString : IEmptyCheck
    {
        public TrimString(string s)
        {
            _value = s?.Trim();
        }

        private readonly string _value;

        public static implicit operator string(TrimString t) => t?._value;
        public static implicit operator TrimString(string s) => s != null ? new TrimString(s) : null;

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return _value.Equals(obj as string);
        }

        public override string ToString() => _value;

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(_value);
        }
    }
}
