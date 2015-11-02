using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Zhuang.Data.SqlCommands.Store
{
    public class AssemblyResourceProvider : ISqlCommandStoreProvider
    {
        public AssemblyResourceProvider()
        {
            AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
            {

                if (!IsRecourceAssembly(args.LoadedAssembly))
                    return;

                var sqlCmds = GetSqlCommandsFromAssembly(args.LoadedAssembly);
                foreach (var sqlcmd in sqlCmds)
                {
                    SqlCommandRepository.Instance.AddOrReplaceSqlCommand(sqlcmd);
                }

            };
        }

        public IEnumerable<SqlCommand> GetSqlCommands()
        {
            List<SqlCommand> result = new List<SqlCommand>();

            var assemblies = Array.FindAll<Assembly>(AppDomain.CurrentDomain.GetAssemblies(), c =>
            {
                return IsRecourceAssembly(c);
            });

            foreach (Assembly assembly in assemblies)
            {
                result.AddRange(GetSqlCommandsFromAssembly(assembly));
            }

            return result;
        }

        public bool IsRecourceAssembly(Assembly assembly)
        {
            bool result = false;

            var att = ((SqlCommandResourceAttribute)Attribute.GetCustomAttribute(assembly, (typeof(SqlCommandResourceAttribute))));

            if (att != null)
            {
                result = true;
            }

            return result;
        }

        public static IEnumerable<SqlCommand> GetSqlCommandsFromAssembly(Assembly assembly)
        {
            List<SqlCommand> result = new List<SqlCommand>();

            var resourceNames = Array.FindAll<string>(assembly.GetManifestResourceNames(), c =>
            {
                //return c.EndsWith(ConfigFilesProvider.CONFIG_FILE_EXTENSION);
                return ConfigFilesProvider.IsValidConfigFileName(c);
            });

            foreach (string resourceName in resourceNames)
            {
                using (Stream manifestResourceStream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader streamReader = new StreamReader(manifestResourceStream))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        try
                        {
                            xmlDoc.Load(streamReader);
                            result.AddRange(ConfigFilesProvider.GetSqlCommandsFromXmlDoc(xmlDoc));
                        }
                        catch (Exception)
                        {

                        }

                    }
                }
            }

            return result;
        }
    }
}
