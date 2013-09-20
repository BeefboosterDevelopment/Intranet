using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Beefbooster.BusinessLogic
{

    public static class Util
    {
        const string VALID_STRAINS = "M1M2M3M4TX";

    public static int ZipFilesInFolder(string folderPath, string zipsFolderPath, string zipFileName)
    {
        int numberOfFiles = 0;

        if (!Directory.Exists(folderPath))
            throw new ApplicationException(string.Format("Cannot find folder '{0}'", folderPath));

        if (!Directory.Exists(zipsFolderPath))
            Directory.CreateDirectory(zipsFolderPath);

        // if it already exits, delete it
        FileInfo fiZip = new FileInfo(zipFileName);
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
    
            using (var zipOutputStream = new ZipOutputStream(File.Create(zipFileName))) {
                zipOutputStream.SetLevel(9); // 0 - store only to 9 - means best compression

                //  note: Linux Gzip can't read it without setting this off
                //zipOutputStream.UseZip64 = UseZip64.Off;

                byte[] buffer = new byte[4096];

				foreach (string file in filenames) {

                    numberOfFiles++;

					// Using GetFileName makes the result compatible with XP
					// as the resulting path is not absolute.
				    string fileName = Path.GetFileName(file);
                    string strain = fileName.Substring(0, 2);
                    string strPath = Path.Combine(strain, fileName);

					ZipEntry entry = new ZipEntry(strPath);
    					
					// Setup the entry data as required.
    					
					// Crc and size are handled by the library for seakable streams
					// so no need to do them here.

					// Could also use the last write time or similar for the file.
					entry.DateTime = DateTime.Now;
                    zipOutputStream.PutNextEntry(entry);
    					
					using ( FileStream fs = File.OpenRead(file) ) {
    		
						// Using a fixed size buffer here makes no noticeable difference for output
						// but keeps a lid on memory usage.
						int sourceBytes;
						do {
							sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            zipOutputStream.Write(buffer, 0, sourceBytes);
						} while ( sourceBytes > 0 );
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
		catch(Exception ex)
		{
			Console.WriteLine("Exception during processing {0}", ex);    			
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
        private string _strainCode;
        public string StrainCode { get { return _strainCode; } }

        private int _minBWT;
        public int MinBWT { get { return _minBWT; } }

        private int _maxBWT;
        public int MaxBWT { get { return _maxBWT; } }

        private decimal _minADG;
        public decimal MinADG { get { return _minADG; } }

        private int _maxBirthDateMonth;
        public int MaxBirthDateMonth { get { return _maxBirthDateMonth; } }

        private int _maxBirthDateDay;
        public int MaxBirthDateDay { get { return _maxBirthDateDay; } }

        private bool _fromHeiferFlag;
        public bool FromHeiferFlag { get { return _fromHeiferFlag; } }

        private bool _assistedBirthFlag;
        public bool AssistedBirthFlag { get { return _assistedBirthFlag; } }
    }
    }
}
