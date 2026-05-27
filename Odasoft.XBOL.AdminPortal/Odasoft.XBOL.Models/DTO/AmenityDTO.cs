namespace Odasoft.XBOL.Models.DTO
{
    public class AmenityDTO : IEquatable<AmenityDTO>
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string IconIdentifier { get; set; } = "";

        public bool Equals(AmenityDTO? other)
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
            return Equals(obj as AmenityDTO);
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
