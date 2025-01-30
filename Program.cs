using _3CX_API_20;
using _3CX_API_20.Helpers;
using _3CX_API_20.Models;
using _3CX_API_20.Services;
using Microsoft.Extensions.Configuration;
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
        /*string basePath = "https://voice-3cx-devtest.3cx.pl:5001";
        string username = "123";
        string password = "IKP7nvjODlnFfkc8XUF7DnfN46PBoL3w";*/
        var configuration = new ConfigurationBuilder()
                   .SetBasePath(AppContext.BaseDirectory)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .Build();

        string basePath = configuration["ApiConfig:BasePath"];
        string username = configuration["ApiConfig:Username"];
        string password = configuration["ApiConfig:Password"];

        Console.WriteLine($"BasePath: {basePath}");
        Console.WriteLine($"Username: {username}");
        Console.WriteLine($"Password: {password}");

        var factory = new ApiConfigurationFactory(basePath, username, password);
        ApiConfiguration config = factory.CreateXAPIConfiguration();

        var httpClient = new HttpClient();
        var apiService = new ApiServices(config, httpClient);
        var usersApi = new UsersService(apiService, httpClient);
        var groupsService = new GroupsServices(apiService, httpClient);
        var usersService = new UsersService(apiService, httpClient);
        var queuesService = new QueueServices(apiService, httpClient);
        var customPromptService = new CustomPromptsServices(apiService, httpClient);
        try
        {

            
              string miasto;
                while (true)
                {
                    Console.WriteLine("Enter the shop (exactly 2 digits, value ≤ 30):");
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
                              Console.WriteLine("2.Usun userów z departamentu XX..NazwaSklepu, Dodaj userów XX2 do departamentu XX..NazwaSklepu, Nadaj uprawnienia członkom dep. nadaj uprawnienia userom XX5/6/7 do XX..NazwaSklepu");
                              Console.WriteLine("3.");
                              Console.WriteLine("4.");
                              Console.WriteLine("6 - Wyjście.");
                              Console.Write("Wybierz opcję (1-6): ");
                              string choice = Console.ReadLine();

                              switch (choice)
                              {
                                  case "1":
                                      string search = $"StartsWith(Number, '{miasto}') and (substring(Number, 2, 1) eq '5' or substring(Number, 2, 1) eq '6' or substring(Number, 2, 1) eq '7')";
                                      Console.WriteLine("");
                                      Console.WriteLine("Lista użytkowników któcyh będzie dotyczyć zmiana");
                                      var users = await GetListWithParams(usersApi, miasto, search);
                                      Console.WriteLine("");
                                      Console.WriteLine("Zmiana Rolu na użytkowniku. Rola User");
                                      await ChangeRoleOnUsers(users, usersService);
                                      // a.	Ustawi rolę: User użytkownikom XX[5/6/7]..
                                      Console.WriteLine("");
                                      string filtergroup = $"Name eq '{miasto}.Dial'";
                                      var groupID = await GetGroupIdByParams(groupsService, miasto,filtergroup);
                                      Console.WriteLine($"Group ID: {groupID}");
                                      await SetPrimaryGroupOnUser(usersService, users, groupID);
                                      //b.	Ustawi domyślny departament na XX.Dial dla użytkowników XX[567]..

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
                                          var listuserafterupdate = await GetListWithParams(usersApi, "", searchuser);
                                          await ChangePermissionsOnUsers(listuserafterupdate, usersService,groupsService, idGroup, true, true, true, true, true, true, false,false,false, false, false, true, false, true, true);
                                          //c.	nada członkom departamentu uprawnienia: Show My Calls, Presence, Calls, Operations

                                          string search2 = $"StartsWith(Number, '{miasto}') and (substring(Number, 2, 1) eq '5' or substring(Number, 2, 1) eq '6' or substring(Number, 2, 1) eq '7')";
                                          var listuser2 = await GetListWithParams(usersApi, "", search2);
                                          await ChangePermissionsOnUsersNoMembers(listuser2, usersService, idGroup,groupsService);
                                          //d.	nada użytkownikom XX[567] uprawnienia: Presence, Calls, Operations do departamentu XX..<nazwa sklepu> 

                                      break;
                                  case "3":
                                      string searchgr_mng = $"StartsWith(Name, '{miasto}.Zarzadzanie')";
                                      var idGroup_mng = await GetGroupIdByParams(groupsService, "", searchgr_mng);
                                      await DeleteAllFromGroup(idGroup_mng, groupsService);
                                      //a.	usunie wszystkich użytkowników z departamentów XX.Zarządzanie 

                                      string searchuser_mng = $"StartsWith(Number, '{miasto}') and (substring(Number, 2, 1) eq '3')";
                                      var listuser_mng = await GetListWithParams(usersApi, "", searchuser_mng);
                                      await AddMemberToGroup(listuser_mng, groupsService, idGroup_mng);
                                      //b.	doda użytkowników XX3.. do departamentów  XX.Zarządzanie
                                     var listuser_mng2 = await GetListWithParams(usersApi, "", searchuser_mng);
                                     await ChangePermissionsOnUsers(listuser_mng2, usersService,groupsService, idGroup_mng, true, true, true, true, true, true, false, false, false, false, false, true, false, true, true);
                                        //c.	nada członkom departamentu uprawnienia: Show My Calls, Presence, Calls, Operations

                                      string search2_mng = $"StartsWith(Number, '{miasto}') and (substring(Number, 2, 1) eq '5' or substring(Number, 2, 1) eq '6' or substring(Number, 2, 1) eq '7')";
                                      var listuser2_mng = await GetListWithParams(usersApi, "", search2_mng);
                                      await ChangePermissionsOnUsersNoMembers(listuser2_mng, usersService, idGroup_mng, groupsService);
                                      //d.	nada użytkownikom XX[567] uprawnienia: Presence, Calls, Operations do departamentu XX.Zarządzanie
                                       break;
                                  case "4":
                                      // 0-14 na 4 pozycji 0 lub 1
                                      string searchgr_4 = $"StartsWith(Name, '{miasto}.') and (substring(Name, 3, 1) eq '0' or substring(Name, 3, 1) eq '1')";
                                      var groupslist = await GetGroupsByParams(groupsService,miasto,searchgr_4);

                                        foreach (var group in groupslist.Value)
                                        {
                                       // string searchgr_mng = $"StartsWith(Name, '{miasto}.Zarzadzanie')";
                                        //var idGroup_4 = await GetGroupIdByParams(groupsService, "", group.Id.ToString());
                                            await DeleteAllFromGroup(group.Id, groupsService);
                                        }
                                      //a. usunie wszystkich użytkowników z departamentów XX.<końcówka numeru kolejki>.<nazwa sklepu>

                                      string search4 = $"StartsWith(Number, '{miasto}') and (substring(Number, 2, 1) eq '5' or substring(Number, 2, 1) eq '6' or substring(Number, 2, 1) eq '7')";
                                      var listuser4 = await GetListWithParams(usersApi, "10", search4);
                                      /*foreach (var groupperm in groupslist.Value)
                                        {
                                        await ChangePermissionsOnUsersNoMembers(listuser4, usersService, groupperm.Id, groupsService);
                                        // b.	nada użytkownikom XX[567] uprawnienia: Presence, Calls, Operations do departamentu XX.<końcówka numeru kolejki>.<nazwa sklepu>
                                        }   */
                                      await ChangePermissionsOnUsersNoMembersOnList(listuser4, usersService, groupslist.Value.Select(g => g.Id).ToList(), groupsService);


                                      string searchqueue = $"StartsWith(Number, '{miasto}1') and (substring(Number, 3, 1) eq '0' or substring(Number, 3, 1) eq '1' )";
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
              
            
 



           /* var a = await groupsService.ListMembersAsync(
                255,
                top:100,
                skip:0,
                expand: new HashSet<string> { "Rights" },
               select: new HashSet<string> { "Rights", "Number", "Id", "MemberName" });*/
            /*string filePath = @"C:\PJ.wav";

            try
            {
                var uploadResponse = await customPromptService.UploadCustomPromptAsync(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd podczas przesyłania pliku:");
                Console.WriteLine(ex.Message);
            }*/







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
            top: 30,
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

            /*  odkometuj jeśli chcesz mieć na konosli listę grup do których należy user
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
              }*/
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
            //Console.WriteLine("Add User to list");
        }
        foreach (var user in userUpdateList)
        {
            await usersService.UpdateUserAsync(user.Id, user);
            Console.WriteLine("Changed Role on User");
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
                Console.WriteLine("Departament który zostanie ustawiony jako domyślny");
                Console.WriteLine($"Group ID: {group.Id} , Name: {group.Name}");
                Console.WriteLine("");
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
    public static async Task<GroupsResponse> GetGroupsByParams(GroupsServices groupService, string miasto, string filtergroup)
    {
        var resultgroup = await groupService.ListGroupsAsync(
          filter: filtergroup,
          select: new HashSet<string> { "Id", "Name" }
            );

            foreach (var group in resultgroup.Value)
            {
                Console.WriteLine($"Group ID: {group.Id} , Name: {group.Name}");
            }
        return resultgroup;
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
        try
        {
            await groupsService.DeleteAllMembersFromGroup(groupId);
            Console.WriteLine("Usunięto wszytskich użytkowników z grupy");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
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

    public static async Task ChangePermissionsOnUsers(UsersResponse users, UsersService usersService, GroupsServices groupsServices, int groupId
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
                        var a = await groupsServices.ListMembersAsync(group.GroupId,
                           expand: new HashSet<string> { "Rights" },
                           select: new HashSet<string> { "Rights", "Number", "Id", "MemberName" });

                        var flags = RightsHelper.GetRightsFlagsForId(a, user.Number);
                        userUpdate.Groups.Add(new UserGroupUpdatePermission
                        {
                            GroupId = group.GroupId,
                            Rights = new RightsUpdatePermission
                            {

                                /*ShowMyCalls = group.Rights.ShowMyCalls,
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
                                ShowMyPresenceOutside = group.Rights.ShowMyPresenceOutside*/
                                ShowMyCalls = flags.ContainsKey("ShowMyCalls")
                                              ? flags["ShowMyCalls"]
                                              : group.GroupRights.ShowMyCalls,
                                CanBargeIn = flags.ContainsKey("CanBargeIn") ? flags["CanBargeIn"] : group.GroupRights.CanBargeIn,
                                AllowIVR = flags.ContainsKey("AllowIVR") ? flags["AllowIVR"] : group.GroupRights.AllowIVR,
                                AllowParking = flags.ContainsKey("AllowParking") ? flags["AllowParking"] : group.GroupRights.AllowParking,
                                CanIntercom = flags.ContainsKey("CanIntercom") ? flags["CanIntercom"] : group.GroupRights.CanIntercom,
                                CanSeeGroupCalls = flags.ContainsKey("CanSeeGroupCalls") ? flags["CanSeeGroupCalls"] : group.GroupRights.CanSeeGroupCalls,
                                PerformOperations = flags.ContainsKey("PerformOperations") ? flags["PerformOperations"] : group.GroupRights.PerformOperations,
                                CanSeeGroupMembers = flags.ContainsKey("CanSeeGroupMembers") ? flags["CanSeeGroupMembers"] : group.GroupRights.CanSeeGroupMembers,
                                AllowToChangePresence = flags.ContainsKey("AllowToChangePresence") ? flags["AllowToChangePresence"] : group.GroupRights.AllowToChangePresence,
                                AllowToManageCompanyBook = flags.ContainsKey("AllowToManageCompanyBook") ? flags["AllowToManageCompanyBook"] : group.GroupRights.AllowToManageCompanyBook,
                                AssignClearOperations = flags.ContainsKey("AssignClearOperations") ? flags["AssignClearOperations"] : group.GroupRights.AssignClearOperations,
                                CanSeeGroupRecordings = flags.ContainsKey("CanSeeGroupRecordings") ? flags["CanSeeGroupRecordings"] : group.GroupRights.CanSeeGroupRecordings,
                                Invalid = flags.ContainsKey("Invalid") ? flags["Invalid"] : group.GroupRights.Invalid,
                                ShowMyPresence = flags.ContainsKey("ShowMyPresence") ? flags["ShowMyPresence"] : group.GroupRights.ShowMyPresence,
                                ShowMyPresenceOutside = flags.ContainsKey("ShowMyPresenceOutside") ? flags["ShowMyPresenceOutside"] : group.GroupRights.ShowMyPresenceOutside,
                                RoleName = group.GroupRights.RoleName

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

    public static async Task ChangePermissionsOnUsersNoMembers(UsersResponse users, UsersService usersService, int groupId, GroupsServices groupsServices )
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
                        var a = await groupsServices.ListMembersAsync(group.GroupId,
                            expand: new HashSet<string> { "Rights" },
                            select: new HashSet<string> { "Rights", "Number","Id", "MemberName" });

                        var flags = RightsHelper.GetRightsFlagsForId(a,user.Number);

                        userUpdate.Groups.Add(new UserGroupUpdatePermission
                        {
                            GroupId = group.GroupId,
                            Rights = new RightsUpdatePermission
                            {//if jesli istneije to tamto a jeśłi nie to default

                                ShowMyCalls = flags.ContainsKey("ShowMyCalls")
                                              ? flags["ShowMyCalls"]
                                              : group.GroupRights.ShowMyCalls,
                                CanBargeIn = flags.ContainsKey("CanBargeIn")? flags["CanBargeIn"]: group.GroupRights.CanBargeIn,
                                AllowIVR = flags.ContainsKey("AllowIVR")? flags["AllowIVR"]: group.GroupRights.AllowIVR,
                                AllowParking = flags.ContainsKey("AllowParking") ? flags["AllowParking"] : group.GroupRights.AllowParking,
                                CanIntercom = flags.ContainsKey("CanIntercom") ? flags["CanIntercom"] : group.GroupRights.CanIntercom,
                                CanSeeGroupCalls = flags.ContainsKey("CanSeeGroupCalls") ? flags["CanSeeGroupCalls"] : group.GroupRights.CanSeeGroupCalls,
                                PerformOperations = flags.ContainsKey("PerformOperations") ? flags["PerformOperations"] : group.GroupRights.PerformOperations,
                                CanSeeGroupMembers = flags.ContainsKey("CanSeeGroupMembers") ? flags["CanSeeGroupMembers"] : group.GroupRights.CanSeeGroupMembers,
                                AllowToChangePresence = flags.ContainsKey("AllowToChangePresence") ? flags["AllowToChangePresence"] : group.GroupRights.AllowToChangePresence,
                                AllowToManageCompanyBook = flags.ContainsKey("AllowToManageCompanyBook") ? flags["AllowToManageCompanyBook"] : group.GroupRights.AllowToManageCompanyBook,
                                AssignClearOperations = flags.ContainsKey("AssignClearOperations") ? flags["AssignClearOperations"] : group.GroupRights.AssignClearOperations,
                                CanSeeGroupRecordings = flags.ContainsKey("CanSeeGroupRecordings") ? flags["CanSeeGroupRecordings"] : group.GroupRights.CanSeeGroupRecordings,
                                Invalid = flags.ContainsKey("Invalid") ? flags["Invalid"] : group.GroupRights.Invalid,
                                ShowMyPresence = flags.ContainsKey("ShowMyPresence") ? flags["ShowMyPresence"] : group.GroupRights.ShowMyPresence,
                                ShowMyPresenceOutside = flags.ContainsKey("ShowMyPresenceOutside") ? flags["ShowMyPresenceOutside"] : group.GroupRights.ShowMyPresenceOutside,
                                RoleName = group.GroupRights.RoleName

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
    public static async Task ChangePermissionsOnUsersNoMembersOnList(
    UsersResponse users,
    UsersService usersService,
    List<int> groupIds,
    GroupsServices groupsServices)
    {
        var userUpdateList = new List<UserUpdatePermissions>();

        foreach (var user in users.Value)
        {
            var userUpdate = new UserUpdatePermissions
            {
                Id = user.Id,
            };
            userUpdate.Groups = new List<UserGroupUpdatePermission>();

            var existingGroupIds = user.Groups?.Select(g => g.GroupId).ToHashSet() ?? new HashSet<int>();

            foreach (var groupId in groupIds)
            {
                if (existingGroupIds.Contains(groupId))
                {
                    // Aktualizacja istniejącej grupy
                    var group = user.Groups.First(g => g.GroupId == groupId);
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
                    // Dodanie nowej grupy
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
            }

            // Zachowanie istniejących grup, które nie są w podanej liście
            if (user.Groups != null)
            {
                foreach (var group in user.Groups)
                {
                    if (!groupIds.Contains(group.GroupId))
                    {
                        var a = await groupsServices.ListMembersAsync(group.GroupId,
                            expand: new HashSet<string> { "Rights" },
                            select: new HashSet<string> { "Rights", "Number", "Id", "MemberName" });

                        var flags = RightsHelper.GetRightsFlagsForId(a, user.Number);

                        userUpdate.Groups.Add(new UserGroupUpdatePermission
                        {
                            GroupId = group.GroupId,
                            Rights = new RightsUpdatePermission
                            {
                                ShowMyCalls = flags.ContainsKey("ShowMyCalls") ? flags["ShowMyCalls"] : group.GroupRights.ShowMyCalls,
                                CanBargeIn = flags.ContainsKey("CanBargeIn") ? flags["CanBargeIn"] : group.GroupRights.CanBargeIn,
                                AllowIVR = flags.ContainsKey("AllowIVR") ? flags["AllowIVR"] : group.GroupRights.AllowIVR,
                                AllowParking = flags.ContainsKey("AllowParking") ? flags["AllowParking"] : group.GroupRights.AllowParking,
                                CanIntercom = flags.ContainsKey("CanIntercom") ? flags["CanIntercom"] : group.GroupRights.CanIntercom,
                                CanSeeGroupCalls = flags.ContainsKey("CanSeeGroupCalls") ? flags["CanSeeGroupCalls"] : group.GroupRights.CanSeeGroupCalls,
                                PerformOperations = flags.ContainsKey("PerformOperations") ? flags["PerformOperations"] : group.GroupRights.PerformOperations,
                                CanSeeGroupMembers = flags.ContainsKey("CanSeeGroupMembers") ? flags["CanSeeGroupMembers"] : group.GroupRights.CanSeeGroupMembers,
                                AllowToChangePresence = flags.ContainsKey("AllowToChangePresence") ? flags["AllowToChangePresence"] : group.GroupRights.AllowToChangePresence,
                                AllowToManageCompanyBook = flags.ContainsKey("AllowToManageCompanyBook") ? flags["AllowToManageCompanyBook"] : group.GroupRights.AllowToManageCompanyBook,
                                AssignClearOperations = flags.ContainsKey("AssignClearOperations") ? flags["AssignClearOperations"] : group.GroupRights.AssignClearOperations,
                                CanSeeGroupRecordings = flags.ContainsKey("CanSeeGroupRecordings") ? flags["CanSeeGroupRecordings"] : group.GroupRights.CanSeeGroupRecordings,
                                Invalid = flags.ContainsKey("Invalid") ? flags["Invalid"] : group.GroupRights.Invalid,
                                ShowMyPresence = flags.ContainsKey("ShowMyPresence") ? flags["ShowMyPresence"] : group.GroupRights.ShowMyPresence,
                                ShowMyPresenceOutside = flags.ContainsKey("ShowMyPresenceOutside") ? flags["ShowMyPresenceOutside"] : group.GroupRights.ShowMyPresenceOutside,
                                RoleName = group.GroupRights.RoleName
                            }
                        });
                    }
                }
            }

            userUpdateList.Add(userUpdate);
        }

        foreach (var user in userUpdateList)
        {
            await usersService.UpdateUserAsync(user.Id, user);
            Console.WriteLine("Changed");
        }
    }


}


