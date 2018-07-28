using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evolver
{
    public class Mutant
    {
        private int numInstructions;

        public int Id { get; private set; }
        private byte[] bytes;
        private Instruction[] instructions;

        public Mutant(int id, byte[] bytes)
        {
            Id = id;
            this.bytes = bytes;
            numInstructions = bytes.Length / Instruction.BYTES;
            ReadInstructions();
        }

        private void ReadInstructions()
        {
            instructions = new Instruction[numInstructions];
            unsafe
            {
                fixed (byte* ptr = bytes)
                {
                    int i = 0;
                    byte* cur = ptr;
                    byte* end = ptr + numInstructions * Instruction.BYTES;
                    while (cur < end)
                    {
                        instructions[i++] = Instruction.FromBytes(cur);
                        cur += 4;
                    }
                }
            }
        }

        public static Mutant CreateMutantWithText(TextReader reader, int numBytes)
        {
            string line = reader.ReadLine();
            int id = int.Parse(line);
            byte[] bytes = new byte[numBytes];
            var numInstructions = numBytes / 4;
            unsafe
            {
                fixed (byte* ptr = bytes)
                {
                    var cur = ptr;
                    for (int i = 0; i < numInstructions; ++i)
                    {
                        line = reader.ReadLine();
                        try
                        {
                            cur = Instruction.FromStringToBytes(line, cur);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(string.Format("Failed at line {0}: {1}", i + 2, e.Message));
                        }
                    }
                }
            }
            return new Mutant(id, bytes);
        }

        internal string ReadableInstructions()
        {
            StringBuilder text = new StringBuilder();
            foreach (var instruction in instructions)
            {
                text.AppendFormat("{0}\n", instruction.ToString());
            }
            return text.ToString();
        }
    
        internal Decision MakeDecision(Map map, Car car, sbyte[] memory, int instructionsLimit)
        {
            var relativeGoal = car.RelativeGoal();
            memory[0] = 0;
            memory[1] = relativeGoal.X;
            memory[2] = relativeGoal.Y;
            memory[3] = map.Width;
            memory[4] = map.Height;
            memory[5] = (sbyte)map[car.RelativeLeft()];
            memory[6] = (sbyte)map[car.RelativeUp()];
            memory[7] = (sbyte)map[car.RelativeRight()];
            memory[8] = (sbyte)map[car.RelativeDown()];
            memory[9] = (sbyte)map[car.RelativePosition(car.Position.Left().Up())];
            memory[10] = (sbyte)map[car.RelativePosition(car.Position.Up().Right())];
            memory[11] = (sbyte)map[car.RelativePosition(car.Position.Right().Down())];
            memory[12] = (sbyte)map[car.RelativePosition(car.Position.Down().Left())];

            // The last 128 bytes are not cleared so the mutant can use them to persist data for next turn.
            for (int i = 13; i < 128; ++i)
                memory[i] = 0;

            int instructionsExecuted = 0;
            for (int instructionAddress = 0;
                instructionAddress < instructions.Length && instructionsExecuted < instructionsLimit;
                ++instructionAddress, ++instructionsExecuted)
            {
                var instruction = instructions[instructionAddress];

                if (instruction.op == Opcode.Jump)
                {
                    Jump jump = instruction as Jump;
                    bool conditionMatched = jump.type == JumpType.Unconditional || jump.type == JumpType.Unconditional2;
                    if (!conditionMatched)
                    {
                        sbyte cond = memory[jump.conditionAddress];
                        switch (jump.type)
                        {
                            case JumpType.IfZero:
                                conditionMatched = cond == 0;
                                break;
                            case JumpType.IfNotZero:
                                conditionMatched = cond != 0;
                                break;
                            case JumpType.IfPositive:
                                conditionMatched = cond > 0;
                                break;
                            case JumpType.IfNegative:
                                conditionMatched = cond < 0;
                                break;
                            case JumpType.IfZeroOrPositive:
                                conditionMatched = cond >= 0;
                                break;
                            case JumpType.IfZeroOrNegative:
                                conditionMatched = cond <= 0;
                                break;
                        }
                        if (!conditionMatched)
                            continue;
                    }
                    int address = jump.codeAddress;
                    if (jump.isRelative)
                    {
                        address = (Int16)jump.codeAddress + instructionAddress;
                    }
                    // We are strict on Jumps.
                    if (address < 0 || address >= instructions.Length)
                        continue;
                    instructionAddress = address - 1;
                    continue;
                }

                var operands = new sbyte[3];
                for (int i = 0; i < 3; ++i)
                {
                    operands[i] = instruction.isAddr[i] ? memory[instruction.operands[i]] : (sbyte)instruction.operands[i];
                }
                       
                switch (instruction.op)
                {
                    case Opcode.NoOp:
                        break;
                    case Opcode.Add:
                        memory[(byte)operands[2]] = (sbyte)(operands[0] + operands[1]);
                        break;
                    case Opcode.Sub:
                        memory[(byte)operands[2]] = (sbyte)(operands[0] - operands[1]);
                        break;
                    case Opcode.Mul:
                        memory[(byte)operands[2]] = (sbyte)(operands[0] * operands[1]);
                        break;
                    case Opcode.Div:
                        if (operands[1] != 0)
                            memory[(byte)operands[2]] = (sbyte)(operands[0] / operands[1]);
                        else
                            memory[(byte)operands[2]] = (sbyte)-1;
                        break;
                    case Opcode.Mod:
                        if (operands[1] != 0)
                            memory[(byte)operands[2]] = (sbyte)(operands[0] % operands[1]);
                        else
                            memory[(byte)operands[2]] = (sbyte)-1;
                        break;
                    case Opcode.Or:
                        memory[(byte)operands[2]] = (sbyte)(operands[0] | operands[1]);
                        break;
                    case Opcode.Xor:
                        memory[(byte)operands[2]] = (sbyte)(operands[0] ^ operands[1]);
                        break;
                    case Opcode.Not:
                        memory[(byte)operands[2]] = (sbyte)~operands[0];
                        break;
                    case Opcode.LShift:
                        memory[(byte)operands[2]] = (sbyte)(operands[0] << operands[1]);
                        break;
                    case Opcode.RShift:
                        memory[(byte)operands[2]] = (sbyte)(operands[0] >> operands[1]);
                        break;
                    case Opcode.Cmp:
                        memory[(byte)operands[2]] = (sbyte)(operands[0] > operands[1] ? 1 : operands[0] < operands[1] ? -1 : 0);
                        break;
                    case Opcode.Set: // dest, value, size
                        {
                            var size = Math.Min((byte)operands[2], 256 - (byte)operands[0]);
                            for (var i = 0; i < size; ++i)
                                memory[(byte)operands[0] + i] = operands[1];
                        }
                        break;
                    case Opcode.Copy: // from, to, size
                        {
                            var size = Math.Min(Math.Min((byte)operands[2], 256 - (byte)operands[0]), 256 - (byte)operands[1]);
                            for (var i = 0; i < size; ++i)
                                memory[(byte)operands[1] + i] = memory[(byte)operands[0] + i];
                        }
                        break;
                    case Opcode.GetGoal: // x_addr, y_addr
                        memory[(byte)operands[0]] = relativeGoal.X;
                        memory[(byte)operands[1]] = relativeGoal.Y;
                        break;
                    case Opcode.GetPaneState: // x, y, result addr.
                        memory[(byte)operands[2]] = (sbyte)map[car.AbsolutePosition(new Position(operands[0], operands[1]))];
                        break;
                }
            }
            return (Decision)((byte)memory[0] % (byte)Decision.TotalDecisions);
        }

        internal void Resize(int newSize)
        {
            var oldNumInstructions = instructions.Length;
            var numInstructions = newSize / 4;
            Array.Resize(ref instructions, numInstructions);
            Array.Resize(ref bytes, numInstructions * 4);
            int i = 0;
            int j = oldNumInstructions;
            while (j < numInstructions)
            {
                var toCopy = Math.Min(oldNumInstructions, numInstructions - j);
                Array.Copy(instructions, i, instructions, j, toCopy);
                Array.Copy(bytes, i, bytes, j, toCopy * 4);
                i += toCopy;
                j += toCopy;
            }
        }

        public void WriteBytes(BinaryWriter writer)
        {
            writer.Write(bytes);
        }

        public List<byte> BytesList()
        {
            return bytes.ToList();
        }
    }
}
