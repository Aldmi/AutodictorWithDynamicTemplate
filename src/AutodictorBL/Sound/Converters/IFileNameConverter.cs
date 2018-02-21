namespace AutodictorBL.Sound.Converters
{
    public interface IFileNameConverter
    {
        string Convert(string name);
    }


    public class Omneo8CharacterFileNameConverter : IFileNameConverter
    {
        private const int MaxLength = 8;


        public string Convert(string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return str;

            var hash = str.GetHashCode().ToString();
            //if (hash.Length > MaxLength)
            //    hash = hash.Substring(0, MaxLength);

            return hash;
        }
    }

}