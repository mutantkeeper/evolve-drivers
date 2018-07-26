using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Evolver
{
   enum Decision: byte
    {
        Stay,
        Move,
        TurnRight,
        TurnLeft,
        TurnBack,
        TotalDecisions
    }

    enum Opcode : byte
    {
        NoOp = 0,
        // op, op1, op2, out_addr
        FirstOp,
        Add = FirstOp,
        Sub,
        Mul,
        Div,
        Mod,
        Or,
        Xor,
        Not,
        LShift,
        RShift,
        Cmp,
        LastOp = Cmp,

        Set, // dest, value, size
        Copy, // from, to, size
        GetGoal, // x_addr, y_addr
        GetPaneState, // x, y, result addr.
        // Jump,  // code_addr_16 (lo, hi)， cond

        NumOpcodes,
    }

    class Instruction
    {
        public Opcode op;
        public byte[] operands = new byte[3];
        public bool[] isAddr = new bool[3];

        public unsafe static Instruction FromBytes(byte* bytes)
        {
            Instruction instruction = new Instruction();
            var opcode = *bytes++;
            instruction.op = (Opcode)((opcode & 0x1F) % (int)Opcode.NumOpcodes);
            for (int i = 0; i < 3; ++i)
            {
                instruction.operands[i] = *bytes++;
                instruction.isAddr[i] = (opcode & (byte)(0x80 >> i)) != 0;
            }
            return instruction;
        }

        public unsafe static byte* FromStringToBytes(string text, byte* bytes)
        {
            const string pattern = @"[ \t]*([a-zA-Z]+)[ \t]+([0-9]+|\[[0-9]+\]),[ \t]*([0-9]+|\[[0-9]+\])[ \t]*\=\>[ \t]*([0-9]+|\[[0-9]+\])[ \t]*|\#.*";
            var groups = Regex.Match(text, pattern).Groups;
            var opcode = (int)(Opcode)Enum.Parse(typeof(Opcode), groups[1].Value);

            var operands = new string[] {
                groups[2].Value,
                groups[3].Value,
                groups[4].Value,
            };

            *bytes++ = (byte)opcode;

            for (int j = 0; j < 3; ++j)
            {
                if (operands[j][0] == '[')
                {
                    opcode |= 0x80 >> j;
                    operands[j] = operands[j].Substring(1, operands[j].Length - 2);
                }
                *bytes++ = byte.Parse(operands[j]);
            }
            return bytes;
        }

        public override string ToString()
        {
            var operandStrings = new string[3];
            for (int i = 0; i < 3; ++i)
            {
                operandStrings[i] = isAddr[i] ? string.Format("[{0}]", operands[i]) : operands[i].ToString();
            }
            return string.Format("{0}\t\t{1}, {2}\t\t => {3}\r\n", op.ToString(), operandStrings[0], operandStrings[1], operandStrings[2]);
        }

        public const int BYTES = 4;
    }
}
