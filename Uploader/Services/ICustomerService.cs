using System.Collections.Generic;
using System.Threading.Tasks;
using Uploader.Models;

namespace Uploader.Services
{
    public interface ICustomerService
    {
        public string GetCustomerIdByName(string customerName);
        public string GetAdminId();
        public Task<Response> CreateCustomer(Customer customer);
        public List<string> GetAllCustomerNames();
    }
}
