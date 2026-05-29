namespace Odasoft.XBOL.Business
{
    public partial class AdminEventCategoryResult : IEquatable<AdminEventCategoryResult>
    {
        public bool Equals(AdminEventCategoryResult? obj)
        {
            if (obj is AdminEventCategoryResult other)
            {
                return Id == other.Id;
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object? obj) => Equals(obj as AdminEventCategoryResult);

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
