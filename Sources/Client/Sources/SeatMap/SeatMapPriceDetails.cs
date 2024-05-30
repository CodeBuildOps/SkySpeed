namespace SkySpeed.SeatMap
{
    class SeatMapPriceDetails
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