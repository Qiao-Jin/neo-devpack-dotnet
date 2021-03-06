using System;
using System.Numerics;

namespace Neo.SmartContract.Framework
{
    public static class Helper
    {
        /// <summary>
        /// Converts byte to byte[] considering the byte as a BigInteger (0x00 at the end)
        /// </summary>
        [OpCode(OpCode.PUSH1)]
        [OpCode(OpCode.LEFT)]
        public extern static byte[] ToByteArray(this byte source);

        /// <summary>
        /// Converts sbyte to byte[].
        /// </summary>
        [OpCode(OpCode.CONVERT, StackItemType.Buffer)]
        public extern static byte[] ToByteArray(this sbyte source);

        /// <summary>
        /// Converts byte[] to BigInteger. No guarantees are assumed regarding BigInteger working range.
        /// If it's null it will return 0
        /// Examples: [0x0a] -> 10; [0x80] -> -128; [] -> 0; [0xff00] -> 255
        /// </summary>
        [OpCode(OpCode.DUP)]
        [OpCode(OpCode.ISNULL)]
        [OpCode(OpCode.JMPIFNOT, "0x05")]
        [OpCode(OpCode.PUSH0)]
        [OpCode(OpCode.SWAP)]
        [OpCode(OpCode.DROP)]
        [OpCode(OpCode.CONVERT, StackItemType.Integer)]
        public extern static BigInteger ToBigInteger(this byte[] source);
        //{
        //    if (value == null) return 0;
        //    return value.ToBigInteger();
        //}

        /// <summary>
        /// Converts string to byte[]. Examples: "hello" -> [0x68656c6c6f]; "" -> []; "Neo" -> [0x4e656f]
        /// </summary>
        [OpCode(OpCode.CONVERT, StackItemType.Buffer)]
        public extern static byte[] ToByteArray(this string source);

        /// <summary>
        /// Converts byte[] to string. Examples: [0x68656c6c6f] -> "hello"; [] -> ""; [0x4e656f] -> "Neo"
        /// </summary>
        [OpCode(OpCode.CONVERT, StackItemType.ByteString)]
        public extern static string ToByteString(this byte[] source);

        /// <summary>
        /// Returns true iff a <= x && x < b. Examples: x=5 a=5 b=15 is true; x=15 a=5 b=15 is false
        /// </summary>
        [OpCode(OpCode.WITHIN)]
        public extern static bool Within(this BigInteger x, BigInteger a, BigInteger b);

        /// <summary>
        /// Returns true iff a <= x && x < b. Examples: x=5 a=5 b=15 is true; x=15 a=5 b=15 is false
        /// </summary>
        [OpCode(OpCode.WITHIN)]
        public extern static bool Within(this int x, BigInteger a, BigInteger b);

        /// <summary>
        /// Converts and ensures parameter source is sbyte (range 0x00 to 0xff); faults otherwise.
        /// Examples: 255 -> fault; -128 -> [0x80]; 0 -> [0x00]; 10 -> [0x0a]; 127 -> [0x7f]; 128 -> fault
        /// ScriptAttribute: DUP SIZE PUSH1 NUMEQUAL ASSERT
        /// </summary>
        [Script(OpCode.DUP, OpCode.SIZE, OpCode.PUSH1, OpCode.NUMEQUAL, OpCode.ASSERT)]
        public extern static sbyte AsSbyte(this BigInteger source);
        //{
        //    Assert(source.AsByteArray().Length == 1);
        //    return (sbyte) source;
        //}

        /// <summary>
        /// Converts and ensures parameter source is sbyte (range 0x00 to 0xff); faults otherwise.
        /// Examples: 255 -> fault; -128 -> [0x80]; 0 -> [0x00]; 10 -> [0x0a]; 127 -> [0x7f]; 128 -> fault
        /// ScriptAttribute: DUP SIZE PUSH1 NUMEQUAL ASSERT
        /// </summary>
        [Script(OpCode.DUP, OpCode.SIZE, OpCode.PUSH1, OpCode.NUMEQUAL, OpCode.ASSERT)]
        public extern static sbyte AsSbyte(this int source);
        //{
        //    Assert(((BigInteger)source).AsByteArray().Length == 1);
        //    return (sbyte) source;
        //}

        /// <summary>
        /// Converts and ensures parameter source is byte (range 0x00 to 0xff); faults otherwise.
        /// Examples: 255 -> fault; -128 -> [0x80]; 0 -> [0x00]; 10 -> [0x0a]; 127 -> [0x7f]; 128 -> fault
        /// ScriptAttribute: DUP SIZE PUSH1 NUMEQUAL ASSERT
        /// </summary>
        [Script(OpCode.DUP, OpCode.SIZE, OpCode.PUSH1, OpCode.NUMEQUAL, OpCode.ASSERT)]
        public extern static byte AsByte(this BigInteger source);
        //{
        //    Assert(source.AsByteArray().Length == 1);
        //    return (byte) source;
        //}

        /// <summary>
        /// Converts and ensures parameter source is byte (range 0x00 to 0xff); faults otherwise.
        /// Examples: 255 -> fault; -128 -> [0x80]; 0 -> [0x00]; 10 -> [0x0a]; 127 -> [0x7f]; 128 -> fault
        /// ScriptAttribute: DUP SIZE PUSH1 NUMEQUAL ASSERT
        /// </summary>
        [Script(OpCode.DUP, OpCode.SIZE, OpCode.PUSH1, OpCode.NUMEQUAL, OpCode.ASSERT)]
        public extern static byte AsByte(this int source);
        //{
        //    Assert(((BigInteger)source).AsByteArray().Length == 1);
        //    return (byte) source;
        //}

        /// <summary>
        /// Converts parameter to sbyte from (big)integer range -128-255; faults if out-of-range.
        /// Examples: 256 -> fault; -1 -> -1 [0xff]; 255 -> -1 [0xff]; 0 -> 0 [0x00]; 10 -> 10 [0x0a]; 127 -> 127 [0x7f]; 128 -> -128 [0x80]
        /// </summary>
        public static sbyte ToSbyte(this BigInteger source)
        {
            if (source > 127)
                source -= 256;
            SmartContract.Assert(source.Within(-128, 128));
            return (sbyte)(source + 0);
        }

        /// <summary>
        /// Converts parameter to sbyte from (big)integer range -128-255; faults if out-of-range.
        /// Examples: 256 -> fault; -1 -> -1 [0xff]; 255 -> -1 [0xff]; 0 -> 0 [0x00]; 10 -> 10 [0x0a]; 127 -> 127 [0x7f]; 128 -> -128 [0x80]
        /// </summary>
        public static sbyte ToSbyte(this int source)
        {
            if (source > 127)
                source -= 256;
            SmartContract.Assert(source.Within(-128, 128));
            return (sbyte)(source + 0);
        }

        /// <summary>
        /// Converts parameter to byte from (big)integer range 0-255; faults if out-of-range.
        /// Examples: 256 -> fault; -1 -> fault; 255 -> -1 [0xff]; 0 -> 0 [0x00]; 10 -> 10 [0x0a]; 127 -> 127 [0x7f]; 128 -> -128 [0x80]
        /// </summary>
        public static byte ToByte(this BigInteger source)
        {
            SmartContract.Assert(source.Within(0, 256));
            if (source > 127)
                source -= 256;
            return (byte)(source + 0);
        }

        /// <summary>
        /// Converts parameter to byte from (big)integer range 0-255; faults if out-of-range.
        /// Examples: 256 -> fault; -1 -> fault; 255 -> -1 [0xff]; 0 -> 0 [0x00]; 10 -> 10 [0x0a]; 127 -> 127 [0x7f]; 128 -> -128 [0x80]
        /// </summary>
        public static byte ToByte(this int source)
        {
            SmartContract.Assert(source.Within(0, 256));
            if (source > 127)
                source -= 256;
            return (byte)(source + 0);
        }

        [OpCode(OpCode.CAT)]
        public extern static byte[] Concat(this byte[] first, byte[] second);

        [NonemitWithConvert(ConvertMethod.HexToBytes)]
        public extern static byte[] HexToBytes(this string hex, bool reverse = false);

        [OpCode(OpCode.SUBSTR)]
        public extern static byte[] Range(this byte[] source, int index, int count);

        /// <summary>
        /// Returns byte[] with first 'count' elements from 'source'. Faults if count < 0
        /// </summary>
        [OpCode(OpCode.LEFT)]
        public extern static byte[] Take(this byte[] source, int count);

        /// <summary>
        /// Returns byte[] with last 'count' elements from 'source'. Faults if count < 0
        /// </summary>
        [OpCode(OpCode.RIGHT)]
        public extern static byte[] Last(this byte[] source, int count);

        /// <summary>
        /// Returns a reversed copy of parameter 'source'.
        /// Example: [0a,0b,0c,0d,0e] -> [0e,0d,0c,0b,0a]
        /// </summary>
        [OpCode(OpCode.DUP)]
        [OpCode(OpCode.REVERSEITEMS)]
        public extern static byte[] Reverse(this Array source);

        [Script]
        public extern static Delegate ToDelegate(this byte[] source);

        /// <summary>
        /// ToScriptHash converts a base-58 Address to ScriptHash in little-endian byte array.
        /// Example: "AFsCjUGzicZmXQtWpwVt6hNeJTBwSipJMS".ToScriptHash() generates 0102030405060708090a0b0c0d0e0faabbccddee
        /// </summary>
        [NonemitWithConvert(ConvertMethod.ToScriptHash)]
        public extern static byte[] ToScriptHash(this string address);

        [NonemitWithConvert(ConvertMethod.ToBigInteger)]
        public extern static BigInteger ToBigInteger(this string text);

        [Syscall("System.Binary.Serialize")]
        public extern static byte[] Serialize(this object source);

        [Syscall("System.Binary.Deserialize")]
        public extern static object Deserialize(this byte[] source);
    }
}
