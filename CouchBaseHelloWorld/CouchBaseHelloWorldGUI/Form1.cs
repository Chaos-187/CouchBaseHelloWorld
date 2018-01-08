using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Authentication;


namespace CouchBaseHelloWorldGUI
{
    public partial class Form1 : Form
    {
        Cluster cluster;

        public Form1()
        {
            InitializeComponent();

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://127.0.0.1:8091") }
            });

            var authenticator = new PasswordAuthenticator("Administrator", "password");
            cluster.Authenticate(authenticator);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var bucket = cluster.OpenBucket("TestBucket"))
            {
                var document = new Document<dynamic>
                {
                    Id = textBox1.Text,
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
                    MessageBox.Show("YAY");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var bucket = cluster.OpenBucket("TestBucket"))
            {
                var get = bucket.GetDocument<dynamic>(textBox1.Text);
                var document = get.Document;
                var msg = string.Format("{0} {1} {2}!", document.Id, document.Content.name ,document.Content.magic);
                MessageBox.Show(msg);
            }
        }
    }
}