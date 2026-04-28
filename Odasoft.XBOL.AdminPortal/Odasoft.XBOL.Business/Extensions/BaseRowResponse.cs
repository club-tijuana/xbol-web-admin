namespace Odasoft.XBOL.Business
{
    public partial class BaseRowResponse : IEquatable<BaseRowResponse>
    {
        public bool Equals(BaseRowResponse? obj)
        {
            if (obj is BaseRowResponse other)
            {
                return Id == other.Id;
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object? obj) => Equals(obj as BaseRowResponse);

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
