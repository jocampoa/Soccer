namespace Soccer.Models
{
    using SQLite.Net.Attributes;

    public class Parameter
    {
        [PrimaryKey, AutoIncrement]
        public int ParameterId { get; set; }

        public string UrlAPI { get; set; }

        public string URLBackend { get; set; }

        public string Option { get; set; }

        public override int GetHashCode()
        {
            return ParameterId;
        }
    }
}
