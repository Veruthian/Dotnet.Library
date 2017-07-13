﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Soedeum.Dotnet.Library;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Numerics;
using Soedeum.Dotnet.Library.Text;
using Soedeum.Dotnet.Library.Text.Encodings;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "𠜎𠜱𠝹𠱓𠱸𠲖𠳏𠳕𠴕𠵼𠵿𠸎𠸏𠹷𠺝𠺢𠻗𠻹𠻺𠼭𠼮𠽌𠾴𠾼𠿪𡁜𡁯𡁵𡁶𡁻𡃁𡃉𡇙𢃇𢞵𢫕𢭃𢯊𢱑𢱕𢳂𢴈𢵌𢵧𢺳𣲷𤓓𤶸𤷪𥄫𦉘𦟌𦧲𦧺𧨾𨅝𨈇𨋢𨳊𨳍𨳒𩶘";

            var bs = Encoding.Unicode.GetBytes(s.Substring(0, 2));

            var bt = BitTwiddler.FromBytes(bs[0], bs[1], bs[2], bs[3]);

            var r = Utf16.Decoder.Process(bt, ByteOrder.LittleEndian);

            var c = CodePoint.FromString(s, 0);

            var d = Utf16.Encoder.Process(c, ByteOrder.BigEndian);

            var e = Utf16.Decoder.Process(d, ByteOrder.BigEndian);

            Pause();
        }

        // Comment
        static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            System.Console.WriteLine();
        }
    }
}