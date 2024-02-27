namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities
{
    public class ElementTuple<T>
    {
        public T Id { get; }
        public T Description { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        private ElementTuple(T id, T description)
        {
            Id = id;
            Description = description;
        }

        public static ElementTuple<T> MakeTuple(T id, T description)
        {
            return new ElementTuple<T>(id, description);
        }
    }
}