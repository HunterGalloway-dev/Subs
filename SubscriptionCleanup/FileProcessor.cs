using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace SubscriptionCleanup
{
    public class FileProcessor
    {
        private readonly ILogger<FileProcessor> _log;
        private readonly IConfiguration _config;

        public FileProcessor(ILogger<FileProcessor> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        /// <summary>
        /// Process directory list and gather subsequent files from those directories
        /// </summary>
        /// <param name="directories">List of directories</param>
        /// <returns>List of all files in directories</returns>
        public List<string> GatherFiles(List<string> directories)
        {
            List<string> files = new List<string>();
            List<string> directoryFiles;

            foreach(var dir in directories)
            {
                directoryFiles = GatherFilesFromDirectory(dir);
                files.AddRange(directoryFiles);
            }

            return files;
        }

        /// <summary>
        /// Given a directory path return the files in that directory
        /// 
        /// Added this method as a later implementation may want to look through other directories
        /// as well meaning a new implementation would be needed so decoupling code is the goal
        /// of this method
        /// </summary>
        /// <param name="Directory">Directory path</param>
        /// <returns>Files in directory</returns>
        public List<string> GatherFilesFromDirectory(string directory)
        {
            // Move search pattern to somewhere else
            if(Directory.Exists(directory))
            {

                return new List<string>(Directory.GetFiles(
                        directory,
                        _config.GetValue<string>("SearchPattern"),
                        (SearchOption)_config.GetValue<int>("SearchOption")));
            } else
            {
                _log.LogInformation("{directory} path does not exist.", directory);
                return new List<string>();
            }
        }
    }
}
