namespace Odasoft.XBOL.Business
{
    public partial class EventCategoryResult : IEquatable<EventCategoryResult>
    {
        public bool Equals(EventCategoryResult? obj)
        {
            if (obj is EventCategoryResult other)
            {
                return Id == other.Id;
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object? obj) => Equals(obj as EventCategoryResult);

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
