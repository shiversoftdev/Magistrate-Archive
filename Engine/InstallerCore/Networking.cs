using System.IO.Compression;
using System.Threading.Tasks;
using System.IO;
/* Network authoritative commands: (todo but also temporary)

    FAIL(id) => Fail and disable a check
    ENABLE(id) => Enable a check. If it was previously failed, the state will still be disabled
    DISABLE(id) => Stop a check from ticking. This does not fail the tick, so if failing was intended, use FAIL(id)
    REPORT(id) => Force the state of the check onto the top of the queue

*/


namespace Engine.Installer.Core
{
    /// <summary>
    /// All networking interfaces and components that require networking
    /// </summary>
    public static class Networking
    {
        private const string ZipPrefix = "csse";
        private const string ZipSuffix = "20190";
        private const string ZipURLFormatter = "http://www.shiversoft.net/csse-public/{0}_pub_{1}.zip"; //
        private const string ZipName = "engine.zip";
        


        internal static string ProjectURL
        {
            get
            {
                return string.Format(ZipURLFormatter, ZipPrefix, ZipSuffix);
            }
        }

        /// <summary>
        /// Download the engine to the path specified
        /// </summary>
        /// <param name="path">The path to download the engine to</param>
        /// <returns></returns>
        internal static async Task<bool> DownloadEngine(string path)
        {
            try
            {
                byte attempts = 0;
                while(Directory.Exists(path) && attempts < 20)
                {
                    Directory.Delete(path, true);
                    await Task.Delay(100);
                    attempts++;
                }
                Directory.CreateDirectory(path);
            }
            catch
            {
                return false;
            }
            try
            {
                bool result = await DownloadResource(ProjectURL, path, ZipName);
                if (!result)
                    return false;
                ZipFile.ExtractToDirectory(Path.Combine(path, ZipName), path);
            }
            catch
            {
                //report an error to the installer
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Download a zip to the specified path
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> DownloadZip(string url, string path, string zipname)
        {
            try
            {
                if (File.Exists(Path.Combine(path, zipname)))
                    File.Delete(Path.Combine(path, zipname));

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch
            {
                return false;
            }
            try
            {
                bool result = await DownloadResource(url, path, zipname);
                if (!result)
                    return false;
                ZipFile.ExtractToDirectory(Path.Combine(path, zipname), path);
                if (File.Exists(Path.Combine(path, zipname)))
                    File.Delete(Path.Combine(path, zipname));
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Download an internet resource
        /// </summary>
        /// <param name="URL">The url to try to access</param>
        /// <param name="outdir">The directory to write the file to</param>
        /// <param name="outname">The name of the file to write</param>
        /// <returns>The result of the operation</returns>
        public static async Task<bool> DownloadResource(string URL, string outdir, string outname)
        {
            try
            {
                byte[] FileData;
                using (var client = new System.Net.Http.HttpClient())
                {
                    FileData = await client.GetByteArrayAsync(URL);
                }

                if(!Directory.Exists(outdir))
                {
                    Directory.CreateDirectory(outdir);
                }

                File.WriteAllBytes(Path.Combine(outdir, outname), FileData);
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}
