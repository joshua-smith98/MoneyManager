namespace MoneyManager.Core
{
    /// <summary>
    /// Numeric class representing any value of money. <see cref="ToString"/> automatically returns currency formatting. Implicitly casts from <see cref="decimal"/>.
    /// </summary>
    public class Money : IEquatable<Money>
    {
        private readonly decimal value;

        private Money(decimal v) { value = v; } // Non-constructable

        public static implicit operator Money(decimal v) => new Money(v);
        public static explicit operator decimal(Money mv) => mv.value;

        public override bool Equals(object? obj) => obj is Money mv && Equals(mv);

        public override int GetHashCode() => value.GetHashCode(); // Since we only wrap around value, this should work

        public bool Equals(Money? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            return value == other.value;
        }

        public static bool operator ==(Money mv1, Money mv2) => ReferenceEquals(mv1, mv2) || mv1.Equals(mv2);

        public static bool operator !=(Money mv1, Money mv2) => !(mv1 == mv2);

        public static bool operator >(Money mv1, Money mv2) => mv1.value > mv2.value;

        public static bool operator <(Money mv1, Money mv2) => mv1.value < mv2.value;

        public static bool operator >=(Money mv1, Money mv2) => !(mv1 < mv2); // me smorrt

        public static bool operator <=(Money mv1, Money mv2) => !(mv1 > mv2);

        public static Money operator +(Money mv) => mv;

        public static Money operator -(Money mv) => -mv.value;

        public static Money operator +(Money mv1, Money mv2) => mv1.value + mv2.value;

        public static Money operator -(Money mv1, Money mv2) => mv1.value - mv2.value;

        public static Money operator *(Money mv1, decimal v2) => mv1.value * v2; // Only allow multiplication by decimals

        public static Money operator /(Money mv1, decimal v2) => mv1.value / v2; // Only allow division by decimals

        public override string ToString() => string.Format("{0:C}", value); // Return currency value when converted to string
    }
}
