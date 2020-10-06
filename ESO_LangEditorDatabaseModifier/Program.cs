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
                var converter = new ConvertV2toV3();
                converter.Convertv2Tov3();

                Debug.WriteLine("Everthing is done!");
            }


        }
    }
}
