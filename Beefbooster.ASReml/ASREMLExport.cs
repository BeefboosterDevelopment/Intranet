using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Beefbooster.BusinessLogic;
using Beefbooster.DataAccessLibrary.Domain;

namespace Beefbooster.ASREML
{
    /// <summary>
    ///     Summary description for ASReml_Export
    /// </summary>
    [Serializable]
    public class ASREMLExport
    {
        private readonly string _bullConnectionString;
        private readonly string _cowCalfConnectionString;
        private readonly string _rootFolder;
        private readonly string _zipsFolder;

        public ASREMLExport(string rootFolder, string cowCalfConnectionString,
            string bullConnectionString)
        {
            _cowCalfConnectionString = cowCalfConnectionString;
            _bullConnectionString = bullConnectionString;
            _rootFolder = rootFolder;

            // where to store the zip files...
            _zipsFolder = Path.Combine(_rootFolder, DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
            _zipsFolder = Path.Combine(_zipsFolder, "ZipFiles\\");

            // where to store the data files...
            _rootFolder = Path.Combine(_rootFolder, DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
            _rootFolder = Path.Combine(_rootFolder, "Export\\");
        }

        private string RootFolderPath
        {
            get { return _rootFolder; }
        }

        private string ZipsFolderPath
        {
            get { return _zipsFolder; }
        }

        private string DeriveStrainFolder(string strain)
        {
            string strainPath = Path.Combine(RootFolderPath, strain + @"\");
            if (!Directory.Exists(strainPath))
                Directory.CreateDirectory(strainPath);
            return strainPath;
        }

        public string CreateZip(string strainToExport)
        {
            if (strainToExport.ToLower() == "all")
            {
                List<BBStrain> strainList = new StrainHelper().GetStrains();
                foreach (BBStrain strain in strainList)
                    ExportSingleStrain(strain.StrainCode);
            }
            else
            {
                ExportSingleStrain(strainToExport);
            }
            return MakeZipFile(strainToExport);
        }

        private void ExportSingleStrain(string strain)
        {
            var expStrain = new ASREMLExportSingleStrain(strain, DeriveStrainFolder(strain), _cowCalfConnectionString,
                _bullConnectionString);
            expStrain.Export();
        }

        private string MakeZipFile(string strain)
        {
            string zipFileName = Path.Combine(ZipsFolderPath,
                string.Format("ASReml_StrainData_{0}_{1}_{2}_{3}.zip", DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.Now.Day, strain.ToUpper()));

            var fiZip = new FileInfo(zipFileName);

            if (fiZip.Exists)
                fiZip.Delete();

            Util.ZipFilesInFolder(RootFolderPath, ZipsFolderPath, zipFileName, strain);

            return zipFileName;
        }
    }
}