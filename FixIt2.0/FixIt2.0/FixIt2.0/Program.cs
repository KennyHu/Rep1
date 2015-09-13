using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using FixIt2._0.SourceControlCommands;
using log4net;
using Microsoft.Practices.Unity;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace FixIt2._0
{
   static class Program
   {
      static void UnityInitiaze()
      {
         IUnityContainer unityContainer = new UnityContainer();
         unityContainer.RegisterType<ISourceControlCmd, P4Command>();
         sSrcControlCmd = unityContainer.Resolve<P4Command>();
      }

      static readonly ILog myILog = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      static ISourceControlCmd sSrcControlCmd;

      /// <summary>
      /// 应用程序的主入口点。
      /// </summary>
      [STAThread]
      static void Main()
      {  
         myILog.Info("Application start");

         UnityInitiaze();

         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new MainForm());

         myILog.Info("Application end");
      }
   }
}
