using System.ComponentModel.DataAnnotations;

namespace TTAServer
{
    public class CustomerTB
    {
        [Key]
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Phoneno { get; set; }

        public CustomerTB()
        {

        }

        public CustomerTB(int id, string name, string address, string country, string city, string phone)
        {
            this.CustomerID = id;
            this.Name = name;
            this.Address = address;
            this.Country = country;
            this.City = city;
            this.Phoneno = phone;
        }
    }
}
