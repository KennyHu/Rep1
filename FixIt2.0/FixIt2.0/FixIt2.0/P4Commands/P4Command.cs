using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixIt2._0.SourceControlCommands
{
   public class P4Command : ISourceControlCmd
   {
      // const 
      private const string P4 = "p4";
      private const string EDIT = " edit ";
      private const string REVERT = " revert ";
      private const string CMD = "cmd.exe";

      private const string P4_RegressionFolder = "regression";

      public bool IsSourceControlEnable()
      {
         return false;
      }

      public ICollection<string> GetUsers()
      {
         throw new NotImplementedException("");
      }

      public ICollection<string> GetSourceControlClients()
      {
         throw new NotImplementedException("");
      }

      public void GetSourceControlInfo(out string user, out string host, out string client, out string clientRoot)
      {
         throw new NotImplementedException("");
      }

      public void SwitchCurrUser(string user)
      {
         throw new NotImplementedException("");
      }

      public void SwitchCurrClient(string client)
      {
         throw new NotImplementedException("");
      }

      public void SetDefaultUser(string defaultUser)
      {
         throw new NotImplementedException("");
      }

      public void CheckoutFile(string fileName)
      {
         throw new NotImplementedException("");
      }

      public void CheckInFile(string fileName)
      {
         throw new NotImplementedException("");
      }
   }
}
