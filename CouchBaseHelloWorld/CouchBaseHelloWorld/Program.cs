using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Authentication;

namespace CouchBaseHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {

            var cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://127.0.0.1:8091") }
            });

            var authenticator = new PasswordAuthenticator("Administrator", "password");
            cluster.Authenticate(authenticator);
            

            using (var bucket = cluster.OpenBucket("TestBucket"))
            {
                var document = new Document<dynamic>
                {
                    Id = "1",
                    Content = new
                    {
                        name = "Couchbase",
                        color = "Red",
                        magic = "works",
                        test = "yay"
                    }
                };

                var upsert = bucket.Upsert(document);
                if (upsert.Success)
                {
                    var get = bucket.GetDocument<dynamic>(document.Id);
                    document = get.Document;
                    var msg = string.Format("{0} {1}!", document.Id, document.Content.name);
                    Console.WriteLine(msg);
                }
                Console.Read();
            }




        }
    }
}
