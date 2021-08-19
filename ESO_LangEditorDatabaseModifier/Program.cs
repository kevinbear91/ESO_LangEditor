using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;

namespace ESO_LangEditorDatabaseModifier
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press C to Convert v2 to v3.");

            ConsoleKeyInfo input = Console.ReadKey(true);

            if (input.Key == ConsoleKey.C)
            {
                //new CleanEnInZh();
                var cleaner = new CleanEnInZhOnServer();
                cleaner.ProcessData();
                //converter.SeedServerByV4SqliteDatabase();

                //var converter = new ConvertV3toV4();
                //converter.Convertv3Tov4();

                Debug.WriteLine("Everthing is done!");
            }


        }
    }
}
