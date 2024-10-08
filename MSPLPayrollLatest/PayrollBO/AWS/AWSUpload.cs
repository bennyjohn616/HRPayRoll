using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Threading.Tasks;
using TraceError;

namespace PayrollBO.AWS
{

    public class AWSUpload
    {
        public static string awsAccessKeyId = "AKIAUAQYHWR3HJBU6Y5Q";
        public static string awsSecretAccessKey = "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";
        public static RegionEndpoint bucketRegion = RegionEndpoint.APSouth1;
        public static IAmazonS3 s3Client;
        public static string bucketName = "*** provide bucket name ***";
        public static string keyName = "*** provide a name for the uploaded object ***";
        public static string filePath = "*** provide the full path name of the file to upload ***";
        // Specify your bucket region (an example region is shown).
        public static string errmsg;


        public static void Main()
        {
            s3Client = new AmazonS3Client(awsAccessKeyId,awsSecretAccessKey,bucketRegion);
            errmsg = "";
            SendFile();
        }

        public static void SendFile()
        {
            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
                request.BucketName = bucketName;
                request.Key = keyName;
                request.FilePath = filePath;
                fileTransferUtility.Upload(request); 
                errmsg = "";

            }
            catch (AmazonS3Exception e)
            {
                errmsg = e.ToString();
                ErrorLog.Log(e);
            }
            catch (Exception e)
            {
                errmsg = e.ToString();
                ErrorLog.Log(e);
            }

        }
    }
}
