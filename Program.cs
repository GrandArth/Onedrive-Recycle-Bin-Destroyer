using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security;
using System.Diagnostics;

namespace RestoreRecyclebinSharepointItem
{
    class Program
    {
        static void Main(string[] args)
        {
            SecureString pwd = new SecureString();
            string Username = args[0];
            string password = args[1];
            string url = args[2];
            try
            {
                using (ClientContext ctx = new ClientContext(url))
                {
                    foreach (char c in password.ToArray())
                        pwd.AppendChar(c);
                    ctx.Credentials = new SharePointOnlineCredentials(Username, pwd);
					RecycleBinItemCollection recycleBinItems = ctx.Site.GetRecycleBinItems("", 2000,false, RecycleBinOrderBy.DeletedDate, RecycleBinItemState.SecondStageRecycleBin);
                    ctx.ExecuteQuery();
                    while (recycleBinItems != null) {
                        ctx.Load(recycleBinItems);
                        recycleBinItems.DeleteAll();
                        ctx.ExecuteQuery();
                        Console.WriteLine("items deleted");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            Console.ReadLine();
        }
    }
}
