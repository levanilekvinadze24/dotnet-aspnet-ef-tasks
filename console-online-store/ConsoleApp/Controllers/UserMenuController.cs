using ConsoleMenu;
using ConsoleMenu.Builder;
using StoreBLL.Services;
using StoreDAL.Data;
using StoreDAL.Data.InitDataFactory;

namespace ConsoleApp;   // <- was ConsoleApp1

public enum UserRoles
{
    Guest,
    Administrator,
    RegistredCustomer,
}

public static class UserMenuController
{
    private static readonly Dictionary<UserRoles, Menu> RolesToMenu;
    private static int userId;
    private static UserRoles userRole;
    private static StoreDbContext context;

    static UserMenuController()
    {
        userId = 0;
        userRole = UserRoles.Guest;
        RolesToMenu = new Dictionary<UserRoles, Menu>();

        var factory = new StoreDbFactory(new TestDataFactory());
        context = factory.CreateContext();

        RolesToMenu.Add(UserRoles.Guest, new GuestMainMenu().Create(context));
        RolesToMenu.Add(UserRoles.RegistredCustomer, new UserMainMenu().Create(context));
        RolesToMenu.Add(UserRoles.Administrator, new AdminMainMenu().Create(context));
    }

    public static StoreDbContext Context => context;

    public static int CurrentUserId => userId;

    public static UserRoles CurrentUserRole => userRole;

    public static void Login()
    {
        Console.Write("Login: ");
        var login = Console.ReadLine() ?? string.Empty;
        Console.Write("Password: ");
        var password = Console.ReadLine() ?? string.Empty;

        var service = new UserService(context);
        var user = service.Authenticate(login, password);
        if (user is null)
        {
            Console.WriteLine("Invalid credentials.");
            return;
        }

        userId = user.Id;
        userRole = user.RoleName switch
        {
            "Admin" => UserRoles.Administrator,
            "Registered" => UserRoles.RegistredCustomer,
            _ => UserRoles.Guest
        };
    }

    public static void Logout()
    {
        userId = 0;
        userRole = UserRoles.Guest;
    }

    public static void Start()
    {
        ConsoleKey resKey;
        bool updateItems = true;
        do
        {
            resKey = RolesToMenu[userRole].RunOnce(ref updateItems);
        }
        while (resKey != ConsoleKey.Escape);
    }
}
