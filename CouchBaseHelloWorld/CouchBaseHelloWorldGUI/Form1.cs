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
using Newtonsoft.Json;


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

            var authenticator = new PasswordAuthenticator("<USERNAME>", "<PASSWORD>");
            cluster.Authenticate(authenticator);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var bucket = cluster.OpenBucket("TestBucket"))
            {
                var numbers = new numbers()
                {
                    one = 1,
                    two = 2
                };

                var couchitem = new couchitem()
                {

                    name = "KYLE",
                    color = System.Drawing.Color.Red,
                    magic = "test",
                    test = numbers

                };

                var document = new Document<couchitem>
                {
                    Id = textBox1.Text,
                    Content = couchitem
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

                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(document.Content))
                {
                    if (property.GetValue(document.Content) is Newtonsoft.Json.Linq.JArray)
                    {
                        textBox2.Text += String.Format("{0} : {1}", property.Name, Environment.NewLine);
                        foreach (var item in property.GetValue(document.Content) as Newtonsoft.Json.Linq.JArray)
                        {
                            foreach (PropertyDescriptor property2 in TypeDescriptor.GetProperties(item))
                            {
                               
                                textBox2.Text += String.Format("         {0} : {1}{2}", property2.Name, Convert.ToString(property2.GetValue(item)), Environment.NewLine);
                            }
                        }
                    }
                    else
                    {
                        textBox2.Text += String.Format("{0} : {1}{2}", property.Name, Convert.ToString(property.GetValue(document.Content)), Environment.NewLine);
                    }


                }


                //var msg = string.Format("{0} {1} {2} {3}!", document.Id, document.Content.name ,document.Content.magic, document.Content.color);
                // MessageBox.Show(msg);
            }
        }
    }
}