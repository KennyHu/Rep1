using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixIt2._0.SourceControlCommands
{
   interface ISourceControlCmd
   {
      bool IsSourceControlEnable();

      ICollection<string> GetUsers();
      ICollection<string> GetSourceControlClients();
      void GetSourceControlInfo(out string user, out string host, out string client, out string clientRoot);

      void SwitchCurrUser(string user);
      void SwitchCurrClient(string client);
      void SetDefaultUser(string defaultUser);

      void CheckoutFile(string fileName);
      void CheckInFile(string fileName);
   }
}
