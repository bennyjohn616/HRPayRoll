using Amazon;
using Amazon.Runtime;
using Amazon.S3;  
using Amazon.S3.Transfer;
using System;
using TraceError;

namespace PayrollBO.AWS
{
    public class AmazonUploader
    {
        public bool sendMyFileToS3(System.IO.Stream localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3)
        {
            try
            {

                //var credentials = new AppConfigAWSCredentials();
                Amazon.Runtime.AWSCredentials credentials = new Amazon.Runtime.StoredProfileAWSCredentials("default");
                IAmazonS3 client = new AmazonS3Client(credentials,RegionEndpoint.APSouth1);
                TransferUtility utility = new TransferUtility(client);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

                if (subDirectoryInBucket == "" || subDirectoryInBucket == null)
                {
                    request.BucketName = bucketName; //no subdirectory just bucket name  
                }
                else
                {   // subdirectory and bucket name  
                    request.BucketName = bucketName + @"/" + subDirectoryInBucket;
                }
                request.Key = fileNameInS3; //file name up in S3  
                request.InputStream = localFilePath;
                utility.Upload(request); //commensing the transfer
            }
            catch(AmazonS3Exception aex)
            {
                ErrorLog.Log(aex);
                return false;
            }
            catch(Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }

            return true; //indicate that the file was sent  
        }
    }
}