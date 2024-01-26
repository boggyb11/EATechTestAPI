using System.ComponentModel.DataAnnotations;

namespace EATechTestAPI.DTO
{
    public class CalculateDownstreamCustomersRequest
    {
        [Required]
        public Network Network { get; set; }
        [Required]
        public int SelectedNode {  get; set; }
    }

    public class Network
    {
        public List<Branch> Branches { get; set; }
        public List<Customer> Customers {  get; set; } 
    }

    public class Branch
    {
        public int StartNode { get; set; }
        public int EndNode { get; set; }
    }

    public class Customer
    {
        public int Node { get; set; }
        public int NumberOfCustomers { get; set; }
    }
}
