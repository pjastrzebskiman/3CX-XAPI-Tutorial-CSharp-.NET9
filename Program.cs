using _3CX_API_20;
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        string basePath = "https://voice-3cx-devtest.3cx.pl:5001";
        string username = "123";
        string password = "IKP7nvjODlnFfkc8XUF7DnfN46PBoL3w";

        var factory = new ApiConfigurationFactory(basePath, username, password);
        ApiConfiguration config = factory.CreateXAPIConfiguration();


        var usersApi = new UserApi(config);
        try
        {
            var users = await usersApi.ListUserAsync(
                top: 20,
                filter: "contains(Lastname, 'Kurek')",
                select: new HashSet<string> { "FirstName", "LastName", "EmailAddress", "Id" }
            );
            foreach (var user in users.Value)
            {
                Console.WriteLine($"ID: {user.Id}, Name: {user.FirstName} {user.LastName}, Email: {user.EmailAddress}");
                await usersApi.SendWelcomeEmailAsync(user.Id);
                 Console.WriteLine($"Wysłano e-mail powitalny do użytkownika o ID  {user.Id}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd: {ex.Message}");
        }
    }
    }
