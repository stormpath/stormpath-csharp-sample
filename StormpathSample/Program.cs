using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using RestSharp;

namespace StormpathSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
                    ? Environment.GetEnvironmentVariable("HOME")
                    : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            // The creation of the client assumes that the file is located in $HOME/Stormpath/apiKey.properties
            var client = createClient(homePath + "/Stormpath/apiKey.properties");
            List<Account> accounts = parseAccountCSV("accounts_to_import.csv");

            //In Stormpath, an "Application" represents your real world application.  
            //Directories and groups can be assigned to it in order to define its user base
            Console.WriteLine("Creating application...");
            var request = new RestRequest(Method.POST);

            // In Stormpath, users and groups are stored in a directory.  Directories can be shared between applications.
            // Adding 'createDirectory=true' to create an application with a default
            // directory as account store, and group and account location.
            request.Resource = "applications?createDirectory=true";
            // The name of the application needs to be unique for the tenant, otherwise the Stormpath REST API will return an error.
            string appName = "The Name for My App " + createUniqueString();
            var application = new Application();
            application.name = appName;
            // Setting the status because the serializer of the client will send a value when it's null
            application.status = "enabled";
            // The only way to make the calls work with this library (RestSharp)
            // is by setting the request format first and then the body :/
            // We do this because it is necessary that the Content-type is
            // set to  application/json in the request header.
            request.RequestFormat = DataFormat.Json;
            request.AddBody(application);
            application = client.Execute<Application>(request);
            Console.WriteLine("  Created application with href '" + application.href + "' and name '" + application.name + "'");

            //Now that we've created an application, let's import some users from a CSV.  
            //You can also important from any type of datasource but we're using a CSV to keep it simple
            Console.WriteLine("Creating accounts from csv file...");
            List<Account> createdAccounts = new List<Account>();
            foreach(Account account in accounts)
            {
                //for every account in the CSV, we create an account in your Stormpath Directory.  
                //for simplicity, we're using your application's default directory.
                request = new RestRequest(Method.POST);
                request.Resource = "/accounts";
                request.RequestFormat = DataFormat.Json;
                request.AddBody(account);
                var createdAccount = client.Execute<Account>(request, application.href);
                Console.WriteLine("  Created account with href '" + createdAccount.href + "' and email '" + createdAccount.email + "'");

                //Once they're in Stormpath, we're also inserting the user account into a List that we're going to use later to test Stormpath
                createdAccounts.Add(createdAccount);
            }

            //Now that we've imported our user accounts from a CSV and into Stormpath, we're going to retrieve one 
            //from that List<Accounts> we just created to test Stormpath
            var accountToRetrieve = createdAccounts[1];
            Console.WriteLine("Retrieving account with email " + accountToRetrieve.email + "...");
            request = new RestRequest(Method.GET);
            accountToRetrieve = client.Execute<Account>(request, accountToRetrieve.href);
            Console.WriteLine("  Account with href " + accountToRetrieve.href + " and full name " + accountToRetrieve.fullName + " has been successfully retrieved!");

            //Next, we'll authenticate a user account
            var accountToAttemptLogin = accounts[0];
            Console.WriteLine("Attempting to authenticate account with email " + accountToAttemptLogin.email + "...");
            var loginAttempt = new BasicLoginAttempt();
            var authStr = accountToAttemptLogin.email + ":" + accountToAttemptLogin.password;
            //Username and password need to be base64 encoded before they are sent to Stormpath
            loginAttempt.value = Convert.ToBase64String(Encoding.UTF8.GetBytes(authStr));
            request = new RestRequest(Method.POST);
            // Adding 'expand=account' to make the REST API return the whole account, instead of just the href
            request.Resource = "/loginAttempts?expand=account";
            request.RequestFormat = DataFormat.Json;
            request.AddBody(loginAttempt);
            var authResult = client.Execute<AuthenticationResult>(request, application.href);
            var authenticatedAccount = authResult.account;
            Console.WriteLine("  Account with href " + authenticatedAccount.href + " has been successfully authenticated!");

            //Now, we're going to show you how you would export data from Stormpath.  To keep it simple, we're 
            //just printing to the console.  You can instead insert into a DB or into a CSV

            //First, we'll pull all the accounts your application has access to
            Console.WriteLine("Getting application accounts...");
            request = new RestRequest(Method.GET);
            // Adding 'expand=directory' to make the REST API return the while directory, instead of just the href.
            // In this request, only the first 25 resources (accounts in this case) will be returned since we're
            // not paginating the results for this sample code.
            var accountList = client.Execute<AccountList>(request, application.href + "/accounts?expand=directory");

            //Now that we've retrieved them from Stormpath, let's print them
            foreach(Account account in accountList.items)
            {
                Console.WriteLine("  Account href : " + account.href);
                Console.WriteLine("  Account full name: " + account.fullName);
                Console.WriteLine("  Account email: " + account.email);
                Console.WriteLine("  Account directory: " + account.directory.name);
            }

            //Next, let's create a Group in Stormpath
            Console.WriteLine("Creating a group...");
            var group = new Group();
            group.name = "New Group";
            group.status = "enabled";
            request = new RestRequest(Method.POST);
            request.Resource = "/groups";
            request.RequestFormat = DataFormat.Json;
            request.AddBody(group);
            group = client.Execute<Group>(request, application.href);
            Console.WriteLine("  Group with href " + group.href + " and name " + group.name + " has been successfully created!");

            //Then assign a user account to the group
            Console.WriteLine("Assigning group with name " + group.name + " to account with full name " + authenticatedAccount.fullName + "...");
            var groupMembership = new GroupMembership();
            groupMembership.account = authenticatedAccount;
            groupMembership.group = group;
            request = new RestRequest(Method.POST);
            request.Resource = "/groupMemberships";
            request.RequestFormat = DataFormat.Json;
            request.AddBody(groupMembership);
            groupMembership = client.Execute<GroupMembership>(request);
            Console.WriteLine("  Group membersip with href " + groupMembership.href + " has been successfully created!");

            //if you want to clean up your work in Stormpath after the above has run, you can use this code below

            /*
            Console.WriteLine("Deleting directory...");
            request = new RestRequest(Method.DELETE);
            client.Execute<object>(request, group.directory.href);
            Console.WriteLine("  Directory Deleted!");

            Console.WriteLine("Deleting application...");
            request = new RestRequest(Method.DELETE);
            client.Execute<object>(request, application.href);
            Console.WriteLine("  Application Deleted!");
            */

            Console.WriteLine("");
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

        }

        /*
         * This method assumes that the file is in the following format:
         * 
         * apiKey.id = THEAPIKEYIDVALUE
         * apiKey.secret = THEAPIKEYSECRETVALUE
         * 
         */
        static Client createClient(string apiKeyFilePath)
        {
            var data = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines(apiKeyFilePath))
                data.Add(row.Split('=')[0].Trim(), string.Join("=", row.Split('=').Skip(1).ToArray()).Trim());

            return new Client(data["apiKey.id"], data["apiKey.secret"]);
        }

        static List<Account> parseAccountCSV(string path)
        {
            List<Account> parsedData = new List<Account>();
           
            using (StreamReader readFile = new StreamReader(path))
            {
                string line;
                string[] row;

                while ((line = readFile.ReadLine()) != null)
                {
                    row = line.Split(',');
                    var account = new Account();
                    account.givenName = row[0];
                    account.surname = row[1];
                    account.email = row[2];
                    account.password = row[3];
                    // Setting the status because the library (RestSharp) sends the fields with 'null' when values are not set.
                    account.status = "enabled";
                    parsedData.Add(account);
                }
            }

            return parsedData;
        }

        static string createUniqueString()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString;
        }
    }
}
