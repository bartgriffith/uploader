using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uploader.Models;

namespace Uploader.Services
{
    public class CustomerService : ICustomerService
    {
        const string storageconn = "DefaultEndpointsProtocol=https;AccountName=coastalphotostore;AccountKey=iPNAxTeNSwqjYTSnAldHJ7QriXwNycPWE+EMNJquC2I+zdx6nPsNnsxdmyBPjWc/b9DHhL940yIktsfBJxZCJw==;EndpointSuffix=core.windows.net";
        const string table = "Customer";
        const string partitionkey = "name";
        const string rowkey = "email";

        private CloudTable _customerTable;

        public CustomerService()
        {
            var storageacct = CloudStorageAccount.Parse(storageconn);
            var tblclient = storageacct.CreateCloudTableClient(new TableClientConfiguration());
            _customerTable = tblclient.GetTableReference(table);
        }

        public string GetAdminId()
        {
            return "123456789";
        }

        public string GetCustomerIdByName(string customerName)
        {
            var adminId = GetAdminId();
            var tableQuery = new TableQuery<CustomerEntity>().Select(new List<string> { "CustomerId" }).Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, adminId),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, customerName))).Take(1);
            var customerResult = _customerTable.ExecuteQuery(tableQuery);

            return customerResult.FirstOrDefault()?.CustomerId;
        }

        public List<string> GetAllCustomerNames()
        {
            var adminId = GetAdminId();
            var tableQuery = new TableQuery<CustomerEntity>().Select(new List<string> { "RowKey" }).Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, adminId));
            var customerResult = _customerTable.ExecuteQuery(tableQuery);

            var response = new List<string>();
            foreach (CustomerEntity customer in customerResult)
            {
                response.Add(customer.RowKey);
            }

            return response;
        }

        public async Task<Response> CreateCustomer(Customer customer)
        {
            try
            {
                var customerEntity = new CustomerEntity(customer.Name, GetAdminId());
                customerEntity.Phone = customer.Phone;
                customerEntity.Password = customer.Password;

                var insertCustomerOperation = TableOperation.InsertOrMerge(customerEntity);
                var result = await _customerTable.ExecuteAsync(insertCustomerOperation);

                return new Response();
            }
            catch (Exception e)
            {
                return new Response
                {
                    ErrorsOccurred = true
                };
            }
            
        }
    }
}
