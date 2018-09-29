using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MattCudaPhotography.DAL.CudaCache
{
    public class CudaCache
    {
        
        string rootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string fullFolderPath;
        int _baseDaysToCache = 14;

        private static CudaCache instance;

        /// <summary>
        /// Makes it a singleton class
        /// </summary>
        public static CudaCache Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CudaCache();

                }
                return instance;
            }
        }

        public void InitializeCudaCache(string folderName, int daysToCache = 14)
        {

            _baseDaysToCache = daysToCache;
            fullFolderPath = string.Concat(rootFolderPath, folderName);

            if (Directory.Exists(fullFolderPath) == false)
            {
                Directory.CreateDirectory(string.Concat(rootFolderPath, folderName));
            }

            if (! fullFolderPath.EndsWith(@"/"))
            {
                fullFolderPath = string.Concat(fullFolderPath, @"/");
            }
            
        }
    

        /// <summary>
        /// Saves a json file to the cache 
        /// </summary>
        /// <param name="fileName">The name of the file somefile.text</param>
        /// <param name="content">The json to write to the file</param>
        /// <returns>True if successful</returns>
        public bool SaveJsonAsync(string fileName, string content)
        {
            string fullPath = string.Concat(fullFolderPath, fileName);
            bool hasCacheExpired = false;

            if (File.Exists(fullPath))
            {
                hasCacheExpired = HasCacheExpired(fullPath);
                if (hasCacheExpired)
                {
                    File.Delete(fullPath);
                }

                File.WriteAllText(fullPath, content);

            }
            else
            {
                //create the cache file
                File.WriteAllText(fullPath, content);
            }

            return true;

        }

        /// <summary>
        /// Saves a byte array to a file
        /// It also does not support cache time limits
        /// </summary>
        /// <param name="fileName">The name of the file somefile.text</param>
        /// <param name="content">The byte array to save to the text file</param>
        /// <returns>True if successful</returns>
        public bool Save(string fileName, byte[] content)
        {

            string fullPath = string.Concat(fullFolderPath, fileName);
            File.WriteAllBytes(fullPath, content); 
            
            return true;

        }

        /// <summary>
        /// Reads all the bytes into an array
        /// </summary>
        /// <param name="fileName">The file name only with extension</param>
        /// <returns>A byte array of data from the file</returns>
        public byte[] Get(string fileName)
        {
            string fullPath = string.Concat(fullFolderPath, fileName);

            if (File.Exists(fullPath))
            {
                return File.ReadAllBytes(fullPath);
            }
            else
            {
                return null;
            }
        }

        public string GetJsonAsync(string fileName)
        {
            string fullPath = string.Concat(fullFolderPath, fileName);

            if (File.Exists(fullPath) == false)
            {
                return "";
            }
            else
            {
                return File.ReadAllText(fullPath);
            }
            
        }

        #region "Private Methods"

        private bool HasCacheExpired(string fullPath)
        {
            DateTime fileDate = File.GetCreationTime(fullPath);
            if (DateTime.Now.Subtract(fileDate).Days > 14)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion



    }
}
