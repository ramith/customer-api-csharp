using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IDbConnection _dbConnection;

    public CustomerController(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    [HttpGet("{accountId}")]
    public ActionResult<Customer> GetCustomer(string accountId)
    {
        _dbConnection.Open();
        var command = _dbConnection.CreateCommand();
        command.CommandText = "SELECT * FROM customer WHERE account_id = @accountId";
        command.Parameters.Add(new MySqlParameter("@accountId", accountId));

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var customer = new Customer
            {
                AccountId = reader["account_id"].ToString(),
                FirstName = reader["first_name"].ToString(),
                LastName = reader["last_name"].ToString(),
                KycStatus = reader["kyc_status"].ToString()
            };
            return Ok(customer);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet("cust/{accountId}")]
    public ActionResult<Customer> GetCust(string accountId)
    {
        Console.WriteLine("customer id: " + accountId);

        return new Customer
        {
            AccountId = accountId,
            FirstName = "John",
            LastName = "Doe",
            KycStatus = "PENDING"
        };
    }
}

public class Customer
{
    public string AccountId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string KycStatus { get; set; }
}
