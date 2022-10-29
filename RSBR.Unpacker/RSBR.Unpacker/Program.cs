using System;
using System.IO;

namespace RSBR.Unpacker
{
    class Program
    {
        private static String m_Title = "Run Sackboy! Run! PAK (OBB) Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    RSBR.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of PAK (OBB) file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    RSBR.Unpacker E:\\Games\\RSBR\\main.25.com.playstation.runsackboyrun.obb D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_PakFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_PakFile))
            {
                Utils.iSetError("[ERROR]: Input PAK file -> " + m_PakFile + " <- does not exist");
                return;
            }

            PakUnpack.iDoIt(m_PakFile, m_Output);
        }
    }
}
