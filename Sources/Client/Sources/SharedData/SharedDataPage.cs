using System.Windows.Controls;

namespace SharedData
{
    public class SharedDataPage
    {
        public static int NumberOfPassengers { get; set; }
        public static int NumberOfADT { get; set; }
        public static int NumberOfCHD { get; set; }
        public static DataGrid PassengersDetailsGrid { get; set; }
        public static object FlightDetails { get; set; }
    }
}