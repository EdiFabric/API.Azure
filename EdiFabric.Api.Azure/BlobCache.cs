using System.Text;

namespace EdiFabric.Api.Azure
{
    public class BlobCache
    {
        public static void Set()
        {
            try
            {
                var token = ReadTokenFromCache().Result;
                SerialKey.SetToken(token);

                //  Refresh token before expiration
                Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                //  Try one last time
                try
                {
                    var token = GetFromApi();
                    WriteTokenToCache(token).Wait();
                    SerialKey.SetToken(token);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine(ex2.ToString());
                    //  Contact support@edifabric.com for assistance
                    throw;
                }
            }
        }

        public static async Task LoadModels(IModelService modelService)
        {
            foreach (var blob in await BlobHelper.ListFromCache(Configuration.ContainerName))
            {
                if (blob.StartsWith("EdiNation") && blob.EndsWith(".dll"))
                {
                    var model = await BlobHelper.ReadFromCache(Configuration.ContainerName, blob);
                    await modelService.Load(Configuration.ApiKey, blob, model);
                }
            }
        }

        private static void Refresh()
        {
            try
            {
                //  Refresh the token two days before it expires
                if (SerialKey.DaysToExpiration < 3)
                    WriteTokenToCache(GetFromApi()).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //  If can't get a token a day before the current expires - throw an exception
                //  Otherwise keep trying
                if (SerialKey.DaysToExpiration <= 1)
                    throw;
            }
        }

        private static string GetFromApi()
        {
            int retries = 3;
            int index = 0;

            //  Try to get a token with retries
            while (index < retries)
            {
                try
                {
                    return SerialKey.GetToken(Configuration.ApiKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    index++;

                    if (index >= retries)
                        throw;
                }
            }

            throw new Exception("Can't get a token.");
        }

        private static async Task<string> ReadTokenFromCache()
        {
            var result = await BlobHelper.ReadFromCache(Configuration.ContainerName, Configuration.BlobName);
            return LoadString(result);
        }

        private static async Task WriteTokenToCache(string token)
        {
            await BlobHelper.WriteToCache(Configuration.ContainerName, Configuration.BlobName, LoadStream(token));
        }

        private static string LoadString(Stream stream)
        {
            return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
        }

        private static MemoryStream LoadStream(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value));
        }
    }
}
