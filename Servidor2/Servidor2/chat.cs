using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Servidor2
{
    class Chat
    {
        private TcpListener server;
        private TcpClient client = new TcpClient();
        private IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Any, 8000);
        private List<Connection> list = new List<Connection>();

        Connection conection;


        //llamar a las librerias  
        private struct Connection
        {
            public NetworkStream stream;
            public StreamWriter sw;
            public StreamReader sr;
            public string nick;
        }

        public Chat()
        {
            Inicio();
        }

        //arranca servidor
        public void Inicio()
        {

            Console.WriteLine("Servidor hay voy!");
            server = new TcpListener(ipendpoint);
            server.Start();

            while (true)
            {
                client = server.AcceptTcpClient();

                conection = new Connection();
                conection.stream = client.GetStream();
                conection.sr = new StreamReader(conection.stream);
                conection.sw = new StreamWriter(conection.stream);

                conection.nick = conection.sr.ReadLine();

                list.Add(conection);
                Console.WriteLine(conection.nick + " Espere un poco.");



                Thread t = new Thread(Escuchar_conexion);

                t.Start();
            }


        }

        void Escuchar_conexion()
        {
            //se escribira un mensaje por ´parte del cliente y nua vez enviado se va a borrar
            Connection hcon = conection;

            do
            {
                try
                {
                    string tmp = hcon.sr.ReadLine();
                    Console.WriteLine(hcon.nick + ": " + tmp);
                    foreach (Connection c in list)
                    {
                        try
                        {
                            c.sw.WriteLine(hcon.nick + ": " + tmp);
                            c.sw.Flush();
                        }
                        catch
                        {
                        }
                    }
                }
                //en caso de que exista un error
                catch
                {
                    list.Remove(hcon);
                    Console.WriteLine(conection.nick + "No hay sistema");
                    break;
                }
            } while (true);
        }

    }
}

    
