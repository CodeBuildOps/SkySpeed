namespace SkySpeed.SeatMap
{
    internal class SeatMapPriceDetails
    {
        public string Seat { get; set; }
        public double SeatPrice { get; set; }

        public SeatMapPriceDetails(string seat, double seatPrice)
        {
            Seat = seat;
            SeatPrice = seatPrice;
        }
    }
}