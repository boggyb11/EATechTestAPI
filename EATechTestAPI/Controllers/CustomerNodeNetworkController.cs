using EATechTestAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EATechTestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerNodeNetworkController : ControllerBase
    {

        [HttpPost(Name = "CalculateDownstreamCustomers")]
        public CalculateDownstreamCustomersResponse CalculateDownstreamCustomers(CalculateDownstreamCustomersRequest request)
        {
            List<Branch> downStreamBranches = [];

            var initialBranches = FindConnectingBranches(request.SelectedNode, request.Network);

            downStreamBranches.AddRange(initialBranches);

            downStreamBranches.AddRange(IterateBranches(initialBranches, request.Network));

            int totalCustomers = 0;

            totalCustomers += GetCustomersForNode(request.Network, request.SelectedNode);

            var uniqueEndNodes = downStreamBranches.Select(b => b.EndNode).Distinct().ToList();
            foreach (var node in uniqueEndNodes)
            {
                totalCustomers += GetCustomersForNode(request.Network, node);
            }

            return new CalculateDownstreamCustomersResponse() { TotalDownstreamCustomers = totalCustomers };
        }

        private int GetCustomersForNode(Network network, int node) => network.Customers.Where(x => x.Node == node).ToList().Sum(x => x.NumberOfCustomers);

        private List<Branch> FindConnectingBranches (int startingNode, Network network) => network.Branches.Where(x => x.StartNode == startingNode).ToList();

        private List<Branch> IterateBranches(List<Branch> startingBranches, Network network)
        {
            List<Branch> downStreamBranches = [];

            foreach (var branch in startingBranches)
            {
                var connectingBranches = FindConnectingBranches(branch.EndNode, network);
                if (connectingBranches.Any())
                {
                    downStreamBranches.AddRange(connectingBranches);

                    downStreamBranches.AddRange(IterateBranches(connectingBranches, network));
                }
            }

            return downStreamBranches;
        }
    }
}
