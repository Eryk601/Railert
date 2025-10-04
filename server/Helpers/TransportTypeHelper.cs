using server.Models;

namespace server.Helpers
{
    public static class TransportTypeHelper
    {
        public static string ToPolishName(this TransportType type)
        {
            return type switch
            {
                TransportType.Bus => "Autobus",
                TransportType.Tram => "Tramwaj",
                TransportType.Train => "Pociąg",
                TransportType.Metro => "Metro",
                _ => "Inny"
            };
        }
    }
}
