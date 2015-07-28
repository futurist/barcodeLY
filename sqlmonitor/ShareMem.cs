using System;

using System.Text;

using OpenNETCF.IO;

using OpenNETCF.Threading;

using System.Diagnostics;

using System.Reflection;

using System.IO;



namespace sqlmonitor
{
    class ShareMem
    {
        public const string SharedMapName = "MMF_PEER_NAME";

        public const int MaxMapSize = 1024*1024;

        public const string SharedMutexName = "MMF_PEER_MUTEX";



        private static MemoryMappedFile m_mmf;

        private static NamedMutex m_mutex= new NamedMutex(false, SharedMutexName);

        // first 4 bytes store string length, so dataLength is dataBuffer.length - 4
        private static byte[] dataBuffer = new byte[MaxMapSize];


        public static void createMemFile(string input)
        {
            

            // grab the mutex
            m_mmf = MemoryMappedFile.CreateInMemoryMap(SharedMapName, MaxMapSize);

            if (!m_mutex.WaitOne(5000, false))
            {
                m_mmf.Close();
                m_mmf = MemoryMappedFile.CreateInMemoryMap(SharedMapName, MaxMapSize);

                Debug.WriteLine("Unable to acquire mutex.  create Abandoned");
                //return;
            }

            
            // String to byte array
            byte[] x = Encoding.UTF8.GetBytes(input);
            byte[] y = { 0, 0, 0, 0, 0, 0, 0, 0 };

            var z = new byte[x.Length + y.Length];
            x.CopyTo(z, 0);
            y.CopyTo(z, x.Length);

            Buffer.BlockCopy(z, 0, dataBuffer, 0, z.Length);

            // write the packet at the start

            m_mmf.Seek(0, System.IO.SeekOrigin.Begin);
            m_mmf.Write(z, 0, z.Length);


            // release the mutex
            m_mutex.ReleaseMutex();

        }

        public static string readMemFile() 
        {
            m_mmf = MemoryMappedFile.CreateInMemoryMap(SharedMapName, MaxMapSize);
            // grab the mutex to prevent concurrency issues
            if (!m_mutex.WaitOne(1000, false))
            {
                Debug.WriteLine("Unable to acquire mutex.  read Abandoned");
                return "@ERROR";
            }

            // read from the start
            m_mmf.Seek(0, System.IO.SeekOrigin.Begin);


            // get the length
            m_mmf.Read(dataBuffer, 0, MaxMapSize);

            int i;
            for (i = 0; i < MaxMapSize; ++i)
            {
                if (dataBuffer[i] == 0)
                {
                    if ((dataBuffer[i + 1] | dataBuffer[i + 2] | dataBuffer[i + 3] |
                        dataBuffer[i + 4] | dataBuffer[i + 5] | dataBuffer[i + 6] | dataBuffer[i + 7]) == 0)
                        break;
                }
            }

            string received = Encoding.UTF8.GetString(dataBuffer, 0, i);


            // release the mutex so any other clients can receive
            m_mutex.ReleaseMutex();


            Debug.WriteLine("Received: " + received);

            return received;

        }



        public static void closeMemFile()
        {
            m_mmf.Close();
        }

    }
}
