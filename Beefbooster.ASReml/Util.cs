using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Beefbooster.BusinessLogic
{
    public static class Util
    {
        private const string VALID_STRAINS = "M1M2M3M4TX";

        public static int ZipFilesInFolder(string folderPath, string zipsFolderPath, string zipFileName, string strain)
        {
            int numberOfFiles = 0;

            if (!Directory.Exists(folderPath))
                throw new ApplicationException(string.Format("Cannot find folder '{0}'", folderPath));

            if (!Directory.Exists(zipsFolderPath))
                Directory.CreateDirectory(zipsFolderPath);

            // if it already exits, delete it
            var fiZip = new FileInfo(zipFileName);
            if (fiZip.Exists)
                fiZip.Delete();

            try
            {
                string[] filenames = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

                // 'using' statements gaurantee the stream is closed properly which is a big source
                // of problems otherwise.  Its exception safe as well which is great.

/*		    FileStream zipFileStream = File.Create(zipFileName);

		    Stream stream = zipFileStream;

		    var zipOutputStream = new ZipOutputStream(stream);*/

                using (var zipOutputStream = new ZipOutputStream(File.Create(zipFileName)))
                {
                    zipOutputStream.SetLevel(9); // 0 - store only to 9 - means best compression

                    //  note: Linux Gzip can't read it without setting this off
                    //zipOutputStream.UseZip64 = UseZip64.Off;

                    var buffer = new byte[4096];

                    foreach (string file in filenames)
                    {
                        numberOfFiles++;

                        // Using GetFileName makes the result compatible with XP
                        // as the resulting path is not absolute.
                        string fileName = Path.GetFileName(file);

                        if (fileName == null)
                            break;

                        string fileStrain = fileName.Substring(0, 2);

                        // only doing a single strain?
                        if (strain.ToLower() != "all")
                            if (strain != fileStrain)
                                continue;


                        string strPath = Path.Combine(strain, fileName);
                        var entry = new ZipEntry(strPath) {DateTime = DateTime.Now};

                        // Setup the entry data as required.

                        // Crc and size are handled by the library for seakable streams
                        // so no need to do them here.

                        // Could also use the last write time or similar for the file.
                        zipOutputStream.PutNextEntry(entry);

                        using (FileStream fs = File.OpenRead(file))
                        {
                            // Using a fixed size buffer here makes no noticeable difference for output
                            // but keeps a lid on memory usage.
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                zipOutputStream.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }

                    // Finish/Close arent needed strictly as the using statement does this automatically

                    // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                    // the created file would be invalid.
                    zipOutputStream.Finish();

                    // Close is important to wrap things up and unlock the file.
                    zipOutputStream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during zip file processing.  {0}", ex);
                // No need to rethrow the exception as for our purposes its handled.
            }

            return numberOfFiles;
        }

        public static bool ValidStrain(string strain)
        {
            if (strain == null)
                throw new ArgumentNullException();
            if (strain.Length != 2)
                throw new ArgumentException("Strain must 2 characters");
            string upStrain = strain.ToUpper();

            if (VALID_STRAINS.IndexOf(upStrain) >= 0)
                return true;
            throw new ArgumentException("Strain must be M1, M2, M3, M4 or TX");
        }

        public class BullSelectionParameters
        {
            private readonly bool _assistedBirthFlag;
            private readonly bool _fromHeiferFlag;
            private readonly int _maxBWT;
            private readonly int _maxBirthDateDay;
            private readonly int _maxBirthDateMonth;
            private readonly decimal _minADG;
            private readonly int _minBWT;
            private readonly string _strainCode;

            public BullSelectionParameters(string strainCode, int minBWT, int maxBWT,
                decimal minADG, int maxBirthDateMonth, int maxBirthDateDay,
                bool fromHeiferFlag, bool assistedBirthFlag)
            {
                _strainCode = strainCode;
                _minBWT = minBWT;
                _maxBWT = maxBWT;
                _minADG = minADG;
                _maxBirthDateMonth = maxBirthDateMonth;
                _maxBirthDateDay = maxBirthDateDay;
                _fromHeiferFlag = fromHeiferFlag;
                _assistedBirthFlag = assistedBirthFlag;
            }

            public string StrainCode
            {
                get { return _strainCode; }
            }

            public int MinBWT
            {
                get { return _minBWT; }
            }

            public int MaxBWT
            {
                get { return _maxBWT; }
            }

            public decimal MinADG
            {
                get { return _minADG; }
            }

            public int MaxBirthDateMonth
            {
                get { return _maxBirthDateMonth; }
            }

            public int MaxBirthDateDay
            {
                get { return _maxBirthDateDay; }
            }

            public bool FromHeiferFlag
            {
                get { return _fromHeiferFlag; }
            }

            public bool AssistedBirthFlag
            {
                get { return _assistedBirthFlag; }
            }
        }
    }
}