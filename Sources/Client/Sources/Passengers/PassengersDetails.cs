using SkySpeed.Payments;

namespace SkySpeed.Passengers
{
    class PassengersDetails
    {
        // From PassengerPage
        public int PassengerId { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DOB { get; set; }
        public string Country { get; set; }

        // From ContactsPage
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressPostal { get; set; }
        public string AddressTown { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string FullAddress { get; set; }

        // From SeatMapPage
        public string Seat { get; set; }
        public double SeatPrice { get; set; }

        // From PaymentPage
        public string PaymentMethod { get; set; }
        public string Amount { get; set; }

        // From PaymentPage
        public PaymentDetails PaymentDetailsObject { get; set; }

        public PassengersDetails(int passengerId, string type, string gender, string title, string firstName, string middleName, string lastName, string dob, string country,
            string addressLine1 = null, string addressLine2 = null, string addressPostal = null, string addressTown = null, string addressState = null, string addressCountry = null, string mobile = null, string email = null,
            string seat = null, double seatPrice = 0)
        {
            // For PassengerPage
            PassengerId = passengerId;
            Type = type;
            Gender = gender;
            Title = title;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            FullName = string.IsNullOrEmpty(MiddleName) ? $"{Title} {FirstName} {LastName}" : $"{Title} {FirstName} {MiddleName} {LastName}";
            DOB = dob;
            Country = country;

            // For ContactsPage
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            AddressPostal = addressPostal;
            AddressTown = addressTown;
            AddressState = addressState;
            AddressCountry = addressCountry;
            FullAddress = $"{AddressLine1} {AddressLine2} {AddressPostal} {AddressTown} {AddressState} {AddressCountry}";
            Mobile = mobile;
            Email = email;

            // For SeatMapPage
            Seat = seat;
            SeatPrice = seatPrice;
        }

        // For PaymentPage
        public PassengersDetails(string paymentMethod, string cardNumber, string amount, string expirationMonth, string expirationYear, string cardHolderName)
        {
            PaymentMethod = paymentMethod;
            Amount = amount;
            PaymentDetailsObject = new PaymentDetails(PaymentMethod, cardNumber, Amount, expirationMonth, expirationYear, cardHolderName);
        }
    }
}
