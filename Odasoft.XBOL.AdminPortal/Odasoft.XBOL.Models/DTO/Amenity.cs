namespace Odasoft.XBOL.Models.DTO
{
    public class Amenity : IEquatable<Amenity>
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string IconIdentifier { get; set; } = "";

        public bool Equals(Amenity? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Amenity);
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
