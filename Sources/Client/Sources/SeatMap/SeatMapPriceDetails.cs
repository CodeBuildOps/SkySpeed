namespace SkySpeed.SeatMap
{
    class SeatMapPriceDetails
    {
        public int PassengerId { get; set; }
        public string Seat { get; set; }
        public double SeatPrice { get; set; }

        public SeatMapPriceDetails(int passengerId, string seat, double seatPrice)
        {
            PassengerId = passengerId;
            Seat = seat;
            SeatPrice = seatPrice;
        }
    }
}