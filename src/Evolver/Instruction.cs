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

    enum JumpType: byte
    {
        Unconditional = 0,
        Unconditional2 = 1,
        IfZero = 2,
        IfNotZero = 3,
        IfPositive = 4,
        IfNegative = 5,
        IfZeroOrPositive = 6,
        IfZeroOrNegative = 7,
    }

    enum Opcode : byte
    {
        NoOp = 0,
        // op, op1, op2, out_addr
        Add,
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
        Set, // dest, value, size
        Copy, // from, to, size
        GetGoal, // x_addr, y_addr
        GetPaneState, // x, y, result addr.
        Jump,   // operands: 
                // first 2:
                //      IsRelative: 1 bit (if address is an offset to the current position)
                //      JumpType: 3 bits
                //      CodeAddress: 12 bits (at most 4096 instructions)
                // The last operand:
                //      Condition: must be address.
                // Limitation: 
                //      This doesn't allow mutants to jump to a computed address (loaded from memmory).
                //      To be considered in the future.
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
            if (instruction.op == Opcode.Jump)
                return new Jump(ref instruction);
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
            // TODO: Make Jump instructions more readable.
            var operandStrings = new string[3];
            for (int i = 0; i < 3; ++i)
            {
                operandStrings[i] = isAddr[i] ? string.Format("[{0}]", operands[i]) : operands[i].ToString();
            }
            return string.Format("{0}\t\t{1}, {2}\t\t => {3}\r\n", op.ToString(), operandStrings[0], operandStrings[1], operandStrings[2]);
        }

        public const int BYTES = 4;
    }

    class Jump: Instruction
    {
        public bool isRelative;
        public JumpType type;
        public UInt16 codeAddress;
        public byte conditionAddress
        {
            get
            {
                return operands[2];
            }
        }
        public Jump(ref Instruction instruction)
        {
            this.op = instruction.op;
            this.operands = instruction.operands;
            this.isAddr = instruction.isAddr;
            instruction = null;
            this.isRelative = (this.operands[0] & 0x80) != 0;
            this.type = (JumpType)((this.operands[0] >> 4) & 7);
            this.codeAddress = (UInt16)((((UInt16)this.operands[0] & 0xF) << 8) | this.operands[1]);
        }
    }
}
