using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using Zhuang.Data.Common;

namespace Zhuang.Data.SqlCommands.Store
{
    public class ConfigFilesProvider : ISqlCommandStoreProvider
    {
        internal const string CONFIG_FILE_EXTENSION = ".config";

        private FileSystemWatcher _watcher;

        public string BasePath
        {
            get
            {
                string basePath = ConfigurationManager.AppSettings[AppSettingsKey.SqlCommandsBasePath];
                return basePath == null ? @".\App_Config\SqlCommands" : basePath;
            }
        }

        public ConfigFilesProvider()
        {
            if (!Directory.Exists(Path.GetFullPath(BasePath)))
            {
                Directory.CreateDirectory(Path.GetFullPath(BasePath));
            }

            _watcher = new FileSystemWatcher(Path.GetFullPath(BasePath), "*" + CONFIG_FILE_EXTENSION);
            _watcher.IncludeSubdirectories = true;
            // Add event handlers.
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.Created += new FileSystemEventHandler(OnChanged);
            _watcher.Deleted += new FileSystemEventHandler(OnChanged);
            _watcher.Renamed += new RenamedEventHandler(OnRenamed);
        }

        public void EnableFileSystemWatcher()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void DisableFileSystemWatcher()
        {
            _watcher.EnableRaisingEvents = false;
        }

        // Define the event handlers. 
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            //Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            HandleChanged();
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            //Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        private void HandleChanged()
        {
            var sqlCmds = GetSqlCommands();
            foreach (var sqlcmd in sqlCmds)
            {
                SqlCommandRepository.Instance.AddOrReplaceSqlCommand(sqlcmd);
            }
        }

        public IEnumerable<SqlCommand> GetSqlCommands()
        {
            List<SqlCommand> result = new List<SqlCommand>();

            DirectoryInfo dirRoot = new DirectoryInfo(Path.GetFullPath(BasePath));
            if (!dirRoot.Exists)
                dirRoot.Create();

            RecursiveGetSqlCommands(dirRoot, result);

            return result;
        }

        private void RecursiveGetSqlCommands(DirectoryInfo dir, List<SqlCommand> sqlcmds)
        {
            var files = dir.GetFiles();

            foreach (var file in files)
            {
                sqlcmds.AddRange(GetSqlCommandsFromFile(file));
            }

            var dirs = dir.GetDirectories();
            if (dirs.Length != 0)
            {
                foreach (var tempDir in dirs)
                {
                    RecursiveGetSqlCommands(tempDir, sqlcmds);
                }
            }
            else
            {
                return;
            }

        }

        private IEnumerable<SqlCommand> GetSqlCommandsFromFile(FileInfo file)
        {
            IList<SqlCommand> result = new List<SqlCommand>();

            if (file.Extension.ToLower() != CONFIG_FILE_EXTENSION)
                return result;

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(file.FullName);
            }
            catch (Exception)
            {
                return result;
            }

            result = (List<SqlCommand>)GetSqlCommandsFromXmlDoc(xmlDoc);

            return result;
        }

        public static IEnumerable<SqlCommand> GetSqlCommandsFromXmlDoc(XmlDocument xmlDoc)
        {
            IList<SqlCommand> result = new List<SqlCommand>();

            var commandsNode = xmlDoc.SelectSingleNode("commands");
            if (commandsNode == null)
                return result;

            var commandNodes = commandsNode.SelectNodes("command");

            foreach (XmlNode commandNode in commandNodes)
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Key = commandNode.Attributes["key"] == null ? "" : commandNode.Attributes["key"].Value;
                sqlCmd.Text = commandNode.InnerText;

                result.Add(sqlCmd);
            }

            return result;
        }
    }
}
