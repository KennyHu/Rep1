using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFF
{
   class Program
   {
      private static string progressBar = ".\\|/";
      private static string CFF_MAP_FILE_NAME = "917CEE4B-0CFF-417E-810D-9435574FED63.cff";
      private static string CFF_MAP_FILE_NAME_BAK = "917CEE4B-0BAC-417E-810D-9435574FED63.cfb";

      private static string cffMapFileFullName = "";
      private static string cffMapFileBakFullName = "";
      private static bool bIsRecover = false;

      private static Dictionary<string, string> cffFileMap = new Dictionary<string, string>();

      static void Main(string[] args)
      {
         if(args.Length != 1)
         {
            Console.WriteLine("The usage is 'CFF.exe folderName'.");
            return;
         }

         string destFolder = args[0];
         if (!Directory.Exists(destFolder))
         {
            Console.WriteLine("The destination folder " + destFolder + " does not exist.");
            return;
         }

         cffMapFileFullName = Path.Combine(destFolder, CFF_MAP_FILE_NAME);
         cffMapFileBakFullName = Path.Combine(destFolder, CFF_MAP_FILE_NAME_BAK);

         if (File.Exists(cffMapFileFullName))
         {
            bIsRecover = true;
            cffFileImplement(destFolder);
            cffFolderImplement(destFolder);
         }
         else
         {
            bIsRecover = false;
            cffFolderImplement(destFolder);
            cffFileImplement(destFolder);
         }
      }

      static void cffFileImplement(string destFolder)
      {
         try
         {
            if (bIsRecover)
            {
               initCffMapFromFile(cffMapFileFullName);
               changeFiles(cffFileMap, true);
            }
            else
            {
               initCffMap(destFolder, false);
               changeFiles(cffFileMap, false);

               FileStream fs = File.Open(cffMapFileFullName, FileMode.Append); ;
               using (StreamWriter sw = getStreamWriter(fs))
               {
                  foreach (var key in cffFileMap.Keys)
                  {
                     string content = key + "=" + cffFileMap[key];
                     sw.WriteLine(content);
                  }
               }
            }
         }
         catch (Exception e)
         {
            deleteMapFile();
            Console.WriteLine("Exception raised: " + e.ToString());
         }
      }

      static void cffFolderImplement(string destFolder)
      {  
         try
         {
            if (bIsRecover)
            {
               initCffMapFromFile(cffMapFileFullName);

               changeFolders(cffFileMap, true);
               deleteMapFile();
            }
            else
            {
               initCffMap(destFolder, true);
               changeFolders(cffFileMap, false);

               FileStream fs = File.Create(cffMapFileFullName);
               using (StreamWriter sw = getStreamWriter(fs))
               {
                  foreach (var key in cffFileMap.Keys)
                  {
                     string content = key + "=" + cffFileMap[key];
                     sw.WriteLine(content);
                  }
               }
            }
         }
         catch (Exception e)
         {
            deleteMapFile();
            Console.WriteLine("Exception raised: " + e.ToString());
         }
         finally
         {
            cffFileMap.Clear();
         }
      }

      #region helpers

      static void deleteMapFile()
      {
         if (File.Exists(cffMapFileFullName))
         {
            File.Copy(cffMapFileFullName, cffMapFileBakFullName, true);
            File.Delete(cffMapFileFullName);
         }
      }

      static void changeFiles(Dictionary<string, string> mapFileName, bool keyIsSource)
      {
         foreach (var key in cffFileMap.Keys)
         {
            string fileNameOnly = Path.GetFileName(key);
            if (fileNameOnly.Length > 0)
            {
               string folder = Path.GetDirectoryName(key);
               string fileName = Path.Combine(folder, cffFileMap[key]);

               if (keyIsSource)
                  File.Move(key, fileName);
               else
                  File.Move(fileName, key);
            }
         }
      }

      static void changeFolders(Dictionary<string, string> mapFolderName, bool keyIsSource)
      {
         IEnumerable<string> keys = null;
         if (keyIsSource)
            keys = cffFileMap.Keys;
         else
            keys = cffFileMap.Keys.Reverse();

         foreach (var key in keys)
         {
            string fileNameOnly = Path.GetFileName(key);
            if (fileNameOnly.Length == 0)
            {
               if (keyIsSource)
                  Directory.Move(key, cffFileMap[key]);
               else
                  Directory.Move(cffFileMap[key], key);
            }
         }
      }

      static void initCffMap(string destFolder, bool folderOnly)
      {
         string[] subDir = Directory.GetDirectories(destFolder);

         foreach (var dirs in subDir)
         {
            Console.WriteLine("...");
            
            if (folderOnly)
            {
               string key = generateCffMapKey(destFolder, false);
               while(cffFileMap.ContainsKey(key))
                  key = generateCffMapKey(destFolder, false);
               cffFileMap[key] = dirs + "\\";
            }

            initCffMap(dirs, folderOnly);
         }
         if (!folderOnly)
         {
            string[] files = Directory.GetFiles(destFolder);
            int cnt = 0;
            foreach (var file in files)
            {
               if (!file.ToLower().Equals(cffMapFileFullName.ToLower()))
               {
                  string key = generateCffMapKey(Path.GetDirectoryName(file), true);
                  while (cffFileMap.ContainsKey(key))
                     key = generateCffMapKey(Path.GetDirectoryName(file), true);
                  cffFileMap[key] = Path.GetFileName(file);
                  Console.Write(progressBar[cnt++]);
                  Console.CursorLeft = 1;
                  cnt = (cnt == progressBar.Length) ? cnt % progressBar.Length : cnt;
               }
            }
            Console.WriteLine("");
         }
      }

      static void initCffMapFromFile(string file)
      {
         using (StreamReader sr = getStreamReader(file))
         {
            do
            {
               string line = sr.ReadLine();
               string[] contents = line.Split('=');

               cffFileMap[contents[0]] = contents[1];
            } while (!sr.EndOfStream);
         }
      }

      /// <summary>
      /// get Stream Reader by File Name
      /// </summary>
      /// <param name="fileName">message</param>
      private static StreamReader getStreamReader(String fileName)
      {
         return new System.IO.StreamReader(fileName, Encoding.Default);
      }

      /// <summary>
      /// close Stream Reader by FileStream
      /// </summary>
      /// <param name="streamReader">message</param>
      private static void closeStreamReader(StreamReader streamReader)
      {
         streamReader.Close();
         streamReader.Dispose();
      }

      /// <summary>
      /// get Stream Writer by FileStream
      /// </summary>
      /// <param name="fileStream">message</param>
      private static StreamWriter getStreamWriter(FileStream fileStream)
      {
         return new System.IO.StreamWriter(fileStream, Encoding.Default);
      }

      /// <summary>
      /// close Stream Reader by FileStream
      /// </summary>
      /// <param name="streamWriter">message</param>
      private static void closeStreamWriter(StreamWriter streamWriter)
      {
         streamWriter.Close();
         streamWriter.Dispose();
      }

      /// <summary>
      /// Generate a 16 chars guid, such as 49f949d735f5c79e
      /// </summary>
      /// <returns></returns>
      private static string generateId()
      {
         long i = 1;
         foreach (byte b in Guid.NewGuid().ToByteArray())
         {
            i *= ((int)b + 1);
         }
         return string.Format("{0:x}", i - DateTime.Now.Ticks);
      }

      private static string generateCffMapKey(string folder, bool isFile)
      {
         string newOne = generateId();
         string key = Path.Combine(folder, newOne.ToString());

         if(isFile)
            key += ".cff";
         else
            key += "\\";

         return key;
      }

      #endregion
   }
}
