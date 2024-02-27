namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code
{
    public struct Time
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }


        public static implicit operator Time(string value)
        {
            string[] split = value.Split(':');
            return new Time
            {
                Hours = split[0].ToInt32(),
                Minutes = split[1].ToInt32()
            };
        }

        
    }
}