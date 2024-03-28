namespace MoneyManager.Core
{
    public class MoneyValue : IEquatable<MoneyValue>
    {
        private readonly decimal value;

        private MoneyValue(decimal v) { value = v; } // Non-constructable

        public static implicit operator MoneyValue(decimal v) => new MoneyValue(v);

        public override bool Equals(object? obj) => obj is MoneyValue mv && Equals(mv);

        public override int GetHashCode() => value.GetHashCode(); // Since we only have one value, this should work

        public bool Equals(MoneyValue? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            return value == other.value;
        }

        public static bool operator ==(MoneyValue mv1, MoneyValue mv2) => ReferenceEquals(mv1, mv2) || mv1.Equals(mv2);

        public static bool operator !=(MoneyValue mv1, MoneyValue mv2) => !(mv1 == mv2);

        public static bool operator >(MoneyValue mv1, MoneyValue mv2) => mv1.value > mv2.value;

        public static bool operator <(MoneyValue mv1, MoneyValue mv2) => mv1.value < mv2.value;

        public static bool operator >=(MoneyValue mv1, MoneyValue mv2) => !(mv1 < mv2);

        public static bool operator <=(MoneyValue mv1, MoneyValue mv2) => !(mv1 > mv2);

        public static MoneyValue operator +(MoneyValue mv) => mv;

        public static MoneyValue operator -(MoneyValue mv) => -mv.value;

        public static MoneyValue operator +(MoneyValue mv1, MoneyValue mv2) => mv1.value + mv2.value;

        public static MoneyValue operator -(MoneyValue mv1, MoneyValue mv2) => mv1.value - mv2.value;

        public static MoneyValue operator *(MoneyValue mv1, decimal v2) => mv1.value * v2; // Only allow multiplication by decimals

        public static MoneyValue operator /(MoneyValue mv1, decimal v2) => mv1.value / v2; // Only allow division by decimals

        public override string ToString() => string.Format("{0:C}", value); // Return currency value when converted to string
    }
}
