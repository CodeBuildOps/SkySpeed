namespace SkySpeed.Contacts
{
    class ContactDetails
    {
        public int PassengerId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressPostal { get; set; }
        public string AddressTown { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string FullAddress { get; set; }

        public ContactDetails(int passengerId, string addressLine1, string addressLine2, string addressPostal, string addressTown, string addressState, string addressCountry, string mobile, string email)
        {
            PassengerId = passengerId;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            AddressPostal = addressPostal;
            AddressTown = addressTown;
            AddressState = addressState;
            AddressCountry = addressCountry;
            FullAddress = $"{AddressLine1} {AddressLine2} {AddressPostal} {AddressTown} {AddressState} {AddressCountry}";
            Mobile = mobile;
            Email = email;
        }
    }
}