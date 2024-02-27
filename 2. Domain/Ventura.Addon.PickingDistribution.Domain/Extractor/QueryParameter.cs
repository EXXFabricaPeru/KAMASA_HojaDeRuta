namespace Exxis.Addon.HojadeRutaAGuia.Domain.Extractor
{
    public struct QueryParameter
    {
        public string Name { get; }
        public Types Type { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public QueryParameter(string rawStructure) : this()
        {
            var type = string.Empty;
            var typePhase = false;
            foreach (char c in rawStructure)
            {
                if (c == '[' || c == ']')
                    typePhase = !typePhase;
                else if (typePhase)
                    type += c;
                else
                    Name += c;
            }

            Type = type == nameof(Types.Field) ? Types.Field : Types.Excel;
        }

        public enum Types
        {
            Excel,
            Field
        }
    }


}