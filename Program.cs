using _3CX_API_20;
using _3CX_API_20.Models;
using _3CX_API_20.Services;
using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static _3CX_API_20.Models.UserUpdatePermissions;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Program
{
    public static async Task Main(string[] args)
    {
        string basePath = "https://URL:PORT";
        string username = "UERNAMEAPI";
        string password = "Password";

        var factory = new ApiConfigurationFactory(basePath, username, password);
        ApiConfiguration config = factory.CreateXAPIConfiguration();

        var httpClient = new HttpClient();
        var apiService = new ApiServices(config, httpClient);
        var usersApi = new UsersService(apiService, httpClient);
        var groupsService = new GroupsServices(apiService, httpClient);
        var usersService = new UsersService(apiService, httpClient);
        var queuesService = new QueueServices(apiService, httpClient);
        try
        {
             string miasto;
              while (true)
              {
                  Console.WriteLine("Enter the city (exactly 2 digits, value ≤ 30):");
                  miasto = Console.ReadLine()?.Trim();

                  if (!string.IsNullOrEmpty(miasto) &&
                      miasto.Length == 2 &&
                      int.TryParse(miasto, out int miastoNumber) &&
                      miastoNumber <= 30)
                  {
                      break;
                  }
                  else
                  {
                      Console.WriteLine("Invalid data. Make sure you enter exactly 2 digits and the value does not exceed 30.");
                  }
              }

              Console.WriteLine($"City entered correctly: {miasto}");

            
                        bool exit = false;
                        while (!exit)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("=== Główne Menu ===");
                            Console.WriteLine("1.Ustaw Role user i główny departament na XX.Dial");
                            Console.WriteLine("2.");
                            Console.WriteLine("3. Wyjdź");
                            Console.Write("Wybierz opcję (1-6): ");
                            string choice = Console.ReadLine();

                            switch (choice)
                            {
                                case "1":
                                    string search = $"StartsWith(Number, '{miasto}') and (substring(Number, 2, 1) eq '5' or substring(Number, 2, 1) eq '6' or substring(Number, 2, 1) eq '7')";
                                    var users = await GetListWithParams(usersApi, miasto, search);
                                    await ChangeRoleOnUsers(users, usersService);
                                    string filtergroup = $"Name eq '{miasto}.Dial'";
                                    var groupID = await GetGroupIdByParams(groupsService, miasto,filtergroup);
                                    Console.WriteLine($"Group ID: {groupID}");
                                    await SetPrimaryGroupOnUser(usersService, users, groupID);

                                    break;
                                case "2":

                                        string searchgr = $"StartsWith(Name, '{miasto}..')";
                                        var idGroup = await GetGroupIdByParams(groupsService, "", searchgr);
                                        await DeleteAllFromGroup(idGroup, groupsService);
                                        //a.	usunie wszystkich użytkowników z departamentów XX..<nazwa sklepu>

                                        string searchuser = $"StartsWith(Number, '{miasto}') and (substring(Number, 2, 1) eq '2')";
                                        var listuser = await GetListWithParams(usersApi, "", searchuser);
                                        await AddMemberToGroup(listuser, groupsService, idGroup);
                                        //b.	doda użytkowników XX2.. do departamentów XX..<nazwa sklepu>
                                        await ChangePermissionsOnUsers(listuser, usersService, idGroup, true, true, true, true, true, true, false,false,false, false, false, true, false, true, true);
                                        //c.	nada członkom departamentu uprawnienia: Show My Calls, Presence, Calls, Operations

                                        string search2 = $"StartsWith(Number, '{miasto}') and (substring(Number, 2, 1) eq '5' or substring(Number, 2, 1) eq '6' or substring(Number, 2, 1) eq '7')";
                                        var listuser2 = await GetListWithParams(usersApi, "", search2);
                                        await ChangePermissionsOnUsersNoMembers(listuser2, usersService, idGroup);
                                        //d.	nada użytkownikom XX[567] uprawnienia: Presence, Calls, Operations do departamentu XX..<nazwa sklepu> 

                                    break;
                                case "3":
                                    string searchgr_mng = $"StartsWith(Name, '{miasto}.Zarządzanie')";
                                    var idGroup_mng = await GetGroupIdByParams(groupsService, "", searchgr_mng);
                                    await DeleteAllFromGroup(idGroup_mng, groupsService);
                                    //a.	usunie wszystkich użytkowników z departamentów XX.Zarządzanie 

                                    string searchuser_mng = $"StartsWith(Number, '{miasto}') and (substring(Number, 2, 1) eq '3')";
                                    var listuser_mng = await GetListWithParams(usersApi, "", searchgr_mng);
                                    await AddMemberToGroup(listuser_mng, groupsService, idGroup_mng);
                                    //b.	doda użytkowników XX3.. do departamentów  XX.Zarządzanie

                                    await ChangePermissionsOnUsers(listuser_mng, usersService, idGroup_mng, true, true, true, true, true, true, false, false, false, false, false, true, false, true, true);
                                    //c.	nada członkom departamentu uprawnienia: Show My Calls, Presence, Calls, Operations

                                    string search2_mng = $"StartsWith(Number, '{miasto}') and (substring(Number, 2, 1) eq '5' or substring(Number, 2, 1) eq '6' or substring(Number, 2, 1) eq '7')";
                                    var listuser2_mng = await GetListWithParams(usersApi, "", search2_mng);
                                    await ChangePermissionsOnUsersNoMembers(listuser2_mng, usersService, idGroup_mng);
                                    //d.	nada użytkownikom XX[567] uprawnienia: Presence, Calls, Operations do departamentu XX.Zarządzanie

                                     break;
                                case "4":
                                    Console.WriteLine("Podaj końcówke numeru kolejki");
                                    string kolejka = Console.ReadLine().ToString();
                                    string searchgr_4 = $"StartsWith(Name, '{10}.{kolejka}.')";
                                    var idGroup_4 = await GetGroupIdByParams(groupsService, "", searchgr_4);
                                    await DeleteAllFromGroup(idGroup_4, groupsService);
                                    //a. usunie wszystkich użytkowników z departamentów XX.<końcówka numeru kolejki>.<nazwa sklepu>

                                    string search4 = $"StartsWith(Number, '{10}') and (substring(Number, 2, 1) eq '5' or substring(Number, 2, 1) eq '6' or substring(Number, 2, 1) eq '7')";
                                    var listuser4 = await GetListWithParams(usersApi, "10", search4);
                                    await ChangePermissionsOnUsersNoMembers(listuser4, usersService, idGroup_4);
                                    // b.	nada użytkownikom XX[567] uprawnienia: Presence, Calls, Operations do departamentu XX.<końcówka numeru kolejki>.<nazwa sklepu>
                                   
                                    string searchqueue = $"StartsWith(Numner, '{miasto}1') and (substring(Numner, 3, 1) eq '0' or substring(Numner, 3, 1) eq '1' )";
                                    var queues = await GetIdByParams(queuesService, searchqueue);

                                    foreach (var queue in queues.Value)
                                    {
                                        Console.WriteLine($"ID: {queue.Id}, Name: {queue.Name}");
                                        await queuesService.DeleteAllAgentsFromQueue(queue.Id);
                                    }

                                    // c.usunie użytkowników z kolejki XX1<końcówka numeru kolejki>
                                    break;

                                case "6":
                                    exit = true;
                                    Console.WriteLine("Zakończono program. Naciśnij dowolny klawisz...");
                                    Console.ReadKey();
                                    break;
                                default:
                                    Console.WriteLine("Nieprawidłowy wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                                    Console.ReadKey();
                                    break;
                            }
                        }


            




            Console.WriteLine("Koniec");




            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }



    }


    public static async Task<QueuesResponse> GetIdByParams(QueueServices queueServices, string search)
    {
        var queues = await queueServices.ListQueueAsync(
            top: 10,
            filter: search
        );
        foreach (var queue in queues.Value)
        {
            Console.WriteLine($"ID: {queue.Id}, Name: {queue.Name}");
        }
        return queues;
    }

    public static async Task<UsersResponse> GetListWithParams(UsersService usersApi, string miasto, string search)
    {
       // string search = $"StartsWith(Number, '{miasto}') and (substring(Number, 2, 1) eq '5' or substring(Number, 2, 1) eq '6' or substring(Number, 2, 1) eq '7')";

        var users = await usersApi.ListUserAsync(
            top: 100,
            filter: search,
            expand: new HashSet<string> { "Groups($expand=Rights,GroupRights)" }
        );

        foreach (var user in users.Value)
        {
            Console.WriteLine(
                $"ID: {user.Id}, Name: {user.FirstName} {user.LastName}, Email: {user.EmailAddress}, Group {user.PrimaryGroupId} Number {user.Number}"
            );

            if (user.Groups != null && user.Groups.Count > 0)
            {
                Console.WriteLine("  Belongs to groups:");
                foreach (var group in user.Groups)
                {
                    Console.WriteLine($"    -> {group.Name} {group.Id}");
                }
            }
            else
            {
                Console.WriteLine("  (No groups assigned or expand returned no groups.)");
            }
        }
        return users;
    }
    public static async Task ChangeRoleOnUsers(UsersResponse users, UsersService usersService)
    {
        var userUpdateList = new List<UsersUpdate>();
        foreach (var user in users.Value)
        {
            var userUpdate = new UsersUpdate
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            userUpdate.Groups = new List<UserGroupUpdate>();

            if (user.Groups != null && user.Groups.Count > 1)
            {
                foreach (var group in user.Groups)
                {
                    userUpdate.Groups.Add(new UserGroupUpdate
                    {
                        GroupId = group.GroupId,
                        Rights = new RightsUpdate
                        {
                            RoleName = RoleNameEnum.Users
                        }
                    });
                }
            }
            else if (user.Groups != null && user.Groups.Count == 1)
            {
                userUpdate.Groups.Add(new UserGroupUpdate
                {
                    GroupId = user.Groups[0].GroupId,
                    Rights = new RightsUpdate
                    {
                        RoleName = RoleNameEnum.Users
                    }
                });

            }
            else
            {
                Console.WriteLine($"User {user.Id} havent groups");
            }
            userUpdateList.Add(userUpdate);
            Console.WriteLine("Add User to list");
        }
        foreach (var user in userUpdateList)
        {
            await usersService.UpdateUserAsync(user.Id, user);
            Console.WriteLine("Changed");
        }
    }

    public static async Task<int> GetGroupIdByParams(GroupsServices groupService, string miasto, string filtergroup)
    {
        int groupID = 0;
        //string filtergroup = $"Name eq '{miasto}.Dial'";
        var resultgroup = await groupService.ListGroupsAsync(
            filter: filtergroup,
          select: new HashSet<string> { "Id", "Name" }
            );


        if (resultgroup.Value.Count > 1)
        {
            Console.WriteLine("Jest kilka takich departamentów");
            foreach (var group in resultgroup.Value)
            {
                Console.WriteLine($"Group ID: {group.Id} , Name: {group.Name}");
            }
        }
        else if (resultgroup.Value.Count == 0)
        {
            Console.WriteLine("Nie ma takiego departamentu");
        }
        else
        {
            foreach (var group in resultgroup.Value)
            {
                Console.WriteLine($"Group ID: {group.Id} , Name: {group.Name}");
                groupID = group.Id;
            }
        }
        return groupID;
    }

    public static async Task SetPrimaryGroupOnUser(UsersService usersService, UsersResponse users, int groupID)
    {
        try
        {
            if (groupID == 0)
            {
                Console.WriteLine("Group ID is 0");
                return;
            }

            foreach (var user in users.Value)
            {
                var userUpdate = new UsersUpdateMainDepartment
                {
                    Id = user.Id,
                    PrimaryGroupId = groupID
                };
                try
                {
                    await usersService.UpdateUserAsync(user.Id, userUpdate);
                    Console.WriteLine($"User {user.Id} Number {user.Number} has been assigned to the group {groupID}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Sprawdź czy User ma tan departament na liscie swoich departamentów !!!   Error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }

   /* public static async Task <List<int>> GetGroupsIdListFromParams(GroupsServices groupsService, string miasto, string search)
    {
       // string search = $"StartsWith(Name, '{miasto}..')";
        var a = await groupsService.ListGroupsAsync(
            top: 50,
            filter: search,
            expand: new HashSet<string> { "Members" },
            select: new HashSet<string> { "Id", "Name" }
        );
        List <int> id = new List<int>();
        foreach (var group in a.Value)
        {
            Console.WriteLine($"ID: {group.Id}, Name: {group.Name}");
            id.Add(group.Id);
        }
        return id;
    }*/

    public static async Task DeleteAllFromGroup(int groupId , GroupsServices groupsService )
    {
       
        await groupsService.DeleteAllMembersFromGroup(groupId);
    }

    public static async Task AddMemberToGroup(UsersResponse listuser,GroupsServices groupsService, int groupId)
    {
        var membersUpdate = new GroupMembersUpdate
        {
            Members = new List<MemberUpdate>()
        };

        foreach (var user in listuser.Value)
        {
            if (!string.IsNullOrWhiteSpace(user.Number))
            {
                membersUpdate.Members.Add(new MemberUpdate { Number = user.Number });
            }
        }
        await groupsService.UpdateGroupAsync(groupId, membersUpdate);
    }

    public static async Task ChangePermissionsOnUsers(UsersResponse users, UsersService usersService, int groupId
   ,bool AllowIVR
   ,bool PerformOperations
   ,bool AllowParking
   ,bool CanIntercom
   ,bool CanSeeGroupCalls 
   ,bool CanSeeGroupMembers
   ,bool AllowToChangePresence 
   ,bool AllowToManageCompanyBook
   ,bool AssignClearOperations
   ,bool CanSeeGroupRecordings
   ,bool Invalid
   ,bool ShowMyCalls
   ,bool CanBargeIn 
   ,bool ShowMyPresence 
   ,bool ShowMyPresenceOutside)
     {
        var userUpdateList = new List<UserUpdatePermissions>();

        foreach (var user in users.Value)
        {
            var userUpdate = new UserUpdatePermissions
            {
                Id = user.Id,
            };
            userUpdate.Groups = new List<UserGroupUpdatePermission>();

            if (user.Groups != null && user.Groups.Count != 0)
            {
                foreach (var group in user.Groups)
                {
                    if (group.GroupId != groupId)
                    {
                        userUpdate.Groups.Add(new UserGroupUpdatePermission
                        {
                            GroupId = group.GroupId,
                            Rights = new RightsUpdatePermission
                            {

                                ShowMyCalls = group.Rights.ShowMyCalls,
                                CanBargeIn = group.Rights.CanBargeIn,
                                AllowIVR = group.Rights.AllowIVR,
                                AllowParking = group.Rights.AllowParking,
                                CanIntercom = group.Rights.CanIntercom,
                                CanSeeGroupCalls = group.Rights.CanSeeGroupCalls,
                                PerformOperations = group.Rights.CanSeeGroupCalls,
                                RoleName = group.Rights.RoleName,
                                CanSeeGroupMembers = group.Rights.CanSeeGroupMembers,
                                AllowToChangePresence = group.Rights.AllowToChangePresence,
                                AllowToManageCompanyBook = group.Rights.AllowToManageCompanyBook,
                                AssignClearOperations = group.Rights.AssignClearOperations,
                                CanSeeGroupRecordings = group.Rights.CanSeeGroupRecordings,
                                Invalid = group.Rights.Invalid,
                                ShowMyPresence = group.Rights.ShowMyPresence,
                                ShowMyPresenceOutside = group.Rights.ShowMyPresenceOutside

                            }
                        });
                    }
                    else
                    {
                        userUpdate.Groups.Add(new UserGroupUpdatePermission
                        {
                            GroupId = group.GroupId,
                            Rights = new RightsUpdatePermission
                            {

                                ShowMyCalls = ShowMyCalls,
                                CanBargeIn = CanBargeIn,
                                AllowIVR = AllowIVR,
                                AllowParking = AllowParking,
                                CanIntercom = CanIntercom,
                                CanSeeGroupCalls = CanSeeGroupCalls,
                                PerformOperations = PerformOperations,
                                CanSeeGroupMembers = CanSeeGroupMembers,
                                AllowToChangePresence = AllowToChangePresence,
                                AllowToManageCompanyBook = AllowToManageCompanyBook,
                                AssignClearOperations = AssignClearOperations,
                                CanSeeGroupRecordings = CanSeeGroupRecordings,
                                Invalid = Invalid,
                                ShowMyPresence = ShowMyPresence,
                                ShowMyPresenceOutside = ShowMyPresenceOutside,
                                RoleName = group.Rights.RoleName

                            }
                        });
                    }
                }
            }
            else
            {
                Console.WriteLine($"User {user.Id} havent groups");
            }
            userUpdateList.Add(userUpdate);
            Console.WriteLine("Add User to list");
        }
        foreach (var user in userUpdateList)
        {
            await usersService.UpdateUserAsync(user.Id, user);
            Console.WriteLine("Changed");
        }
    }

    public static async Task ChangePermissionsOnUsersNoMembers(UsersResponse users, UsersService usersService, int groupId )
    {
        var userUpdateList = new List<UserUpdatePermissions>();

        foreach (var user in users.Value)
        {
            var userUpdate = new UserUpdatePermissions
            {
                Id = user.Id,
            };
            userUpdate.Groups = new List<UserGroupUpdatePermission>();
            bool hasGroupId = false;

            if (user.Groups != null && user.Groups.Count != 0)
            {
                foreach (var group in user.Groups)
                {
                    if (group.GroupId == groupId)
                    {
                        hasGroupId = true;
                        userUpdate.Groups.Add(new UserGroupUpdatePermission
                        {
                            GroupId = group.GroupId,
                            Rights = new RightsUpdatePermission
                            {

                                
                                AllowIVR = true,
                                AllowParking = true,
                                CanIntercom = true,
                                CanSeeGroupCalls = true,
                                PerformOperations = true,
                                CanSeeGroupMembers = true,
                                RoleName = "observers"

                            }
                        });
                    }
                    else
                    {
                        userUpdate.Groups.Add(new UserGroupUpdatePermission
                        {
                            GroupId = group.GroupId,
                            Rights = new RightsUpdatePermission
                            {

                                ShowMyCalls = group.Rights.ShowMyCalls,
                                CanBargeIn = group.Rights.CanBargeIn,
                                AllowIVR = group.Rights.AllowIVR,
                                AllowParking = group.Rights.AllowParking,
                                CanIntercom = group.Rights.CanIntercom,
                                CanSeeGroupCalls = group.Rights.CanSeeGroupCalls,
                                PerformOperations = group.Rights.PerformOperations,
                                CanSeeGroupMembers = group.Rights.CanSeeGroupMembers,
                                AllowToChangePresence = group.Rights.AllowToChangePresence,
                                AllowToManageCompanyBook = group.Rights.AllowToManageCompanyBook,
                                AssignClearOperations = group.Rights.AssignClearOperations,
                                CanSeeGroupRecordings = group.Rights.CanSeeGroupRecordings,
                                Invalid = group.Rights.Invalid,
                                ShowMyPresence = group.Rights.ShowMyPresence,
                                ShowMyPresenceOutside = group.Rights.ShowMyPresenceOutside,
                                RoleName = group.Rights.RoleName

                            }
                        });
                    }
                }
            }
            else
            {
                Console.WriteLine($"User {user.Id} havent groups");
            }
            if (!hasGroupId)
            {
                userUpdate.Groups.Add(new UserGroupUpdatePermission
                {
                    GroupId = groupId,
                    Rights = new RightsUpdatePermission
                    {
                        AllowIVR = true,
                        AllowParking = true,
                        CanIntercom = true,
                        CanSeeGroupCalls = true,
                        PerformOperations = true,
                        CanSeeGroupMembers = true,
                        RoleName = "observers"
                    }
                });

                Console.WriteLine($"Dodano grupę o ID {groupId} do użytkownika {user.Id}.");
            }
            userUpdateList.Add(userUpdate);


            Console.WriteLine("Add User to list");
        }
        foreach (var user in userUpdateList)
        {
            await usersService.UpdateUserAsync(user.Id, user);
            Console.WriteLine("Changed");
        }
    }


}


