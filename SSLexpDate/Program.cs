using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Sockets;
using System.Net.Security;



namespace SSLexpDate
{
    class Program
    {
        static void Main(string[] args)
        {
            bool differenceInDays = false;
            string domainPath = "";
            string[] strArguments = Environment.GetCommandLineArgs();
            if (strArguments.Length > 1)
            {
                foreach (string arg in strArguments)
                {
                    if (arg.ToLower()=="-d")
                    {
                        differenceInDays = true;
                    }
                    else 
                    {
                        domainPath = arg.Trim();
                    }
                }
            }
            else
            {
                Console.WriteLine("Error keys. SSLexpDate.exe [-d] domainname");
            }
            try {
            var expirationDate = GetCertificateExpirationDate(domainPath, 443);
            if (differenceInDays)
            {
                    Console.WriteLine((expirationDate - DateTime.Now).Days.ToString());
            }
            else
            {
                    Console.WriteLine(expirationDate.ToString());
            };

           
            }
            catch
            {
                Console.WriteLine("Error connect ssl: "+ domainPath);
            }

            
            //Console.ReadLine(); //for test

        }

        static  DateTime GetCertificateExpirationDate(string host, int port)
        {
            TcpClient client = new TcpClient(host, port);

            X509Certificate2 x509 = null;
            SslStream sslStream = new SslStream(client.GetStream(), false,
                delegate (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslError)
                {
                    x509 = new X509Certificate2(cert);
                    return true;
                });
            sslStream.AuthenticateAsClient(host);
            client.Close();
            return x509.NotAfter;
        }
    }

}
