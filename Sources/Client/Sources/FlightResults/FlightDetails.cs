namespace SkySpeed.FlightResults
{
    class FlightDetails
    {
        public int SelectedFlightIndex { get; set; }
        public string Designator { get; set; }
        public string FlightNumber { get; set; }
        public string DayAndDate { get; set; }
        public string Sector { get; set; }
        public string DepartArrival { get; set; }
        public string Stop { get; set; }
        public string SeatsLeft { get; set; }
        public string Fare { get; set; }
        public string Duration { get; set; }

        public FlightDetails(int selectedFlightIndex, string designator, string flightNumber, string dayAndDate, string sector, string departArrival, string stop, string seatsLeft, string fare, string duration)
        {
            SelectedFlightIndex = selectedFlightIndex;
            Designator = designator;
            FlightNumber = flightNumber;
            DayAndDate = dayAndDate;
            Sector = sector;
            DepartArrival = departArrival;
            Stop = stop;
            SeatsLeft = seatsLeft;
            Fare = fare;
            Duration = duration;
        }
    }
}