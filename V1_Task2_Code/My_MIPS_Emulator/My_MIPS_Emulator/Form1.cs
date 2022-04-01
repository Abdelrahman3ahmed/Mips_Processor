using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections;

namespace My_MIPS_Emulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void inializeBtn_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> input = new Dictionary<int, string>();
            string[] instr = new string[2];

            for (int i = 0; i < UserCodetxt.Lines.Count(); i++)
            {
                instr = UserCodetxt.Lines[i].Split(':');
                instr[0] = String.Concat(instr[0].Where(c => !Char.IsWhiteSpace(c)));
                instr[1] = String.Concat(instr[1].Where(c => !Char.IsWhiteSpace(c)));
                input.Add(Convert.ToInt32(instr[0]), instr[1]);

            }
            m.Get_Input(input, Convert.ToInt32(instr[0]));
            numofcycles = (UserCodetxt.Lines.Count() * 2) - 1;
        }
        Mips m = new Mips();
        int cycle = 1;
        int numofcycles;
        private void runCycleBtn_Click(object sender, EventArgs e)
        {
            MipsRegisterGrid.Rows.Clear();
            MemoryGrid.Rows.Clear();
            PiplineGrid.Rows.Clear();

            if (cycle <= numofcycles)
            {
                m.Write_Back();
                m.Memory_Addressing();
                m.Excute_Stage();
                m.Decode_Instructionion();
                m.Fetsh_Instruction();
                /*if (cycle == 1)
                {
                    m.Fetsh_Instruction();
                }
                if (cycle == 2)
                {
                    m.Decode_Instructionion();
                    m.Fetsh_Instruction();
                }
                if (cycle == 3)
                {
                    m.Excute_Stage();
                    m.Decode_Instructionion();
                    m.Fetsh_Instruction();
                }
                if (cycle == 4)
                {
                    m.Memory_Addressing();
                    m.Excute_Stage();
                    m.Decode_Instructionion();
                    m.Fetsh_Instruction();
                }
                
                if (cycle > 4 & cycle < numofcycles - 3)
                {
                    m.Write_Back();
                    m.Memory_Addressing();
                    m.Excute_Stage();
                    m.Decode_Instructionion();
                    m.Fetsh_Instruction();
                }
                if (cycle == numofcycles - 3)
                {
                    m.Write_Back();
                    m.Memory_Addressing();
                    m.Excute_Stage();
                    m.Decode_Instructionion();
                    m.Fetsh_Instruction();
                }
                if (cycle == numofcycles - 2)
                {
                    m.Write_Back();
                    m.Memory_Addressing();
                    m.Excute_Stage();
                    m.Fetsh_Instruction();
                }
                if (cycle == numofcycles - 1)
                {
                    m.Write_Back();
                    m.Memory_Addressing();
                    m.Fetsh_Instruction();
                }
                if (cycle == numofcycles)
                {
                    m.Write_Back();
                    m.Fetsh_Instruction();
                }*/


                for (int i = 0; i < 32; i++)
                {
                    MipsRegisterGrid.Rows.Add("$" + i.ToString(), m.Rejesters[i].ToString());

                }
                
                if(m.Memory_address.Count() != 0)
                {
                    foreach (int key in m.Memory_address.Keys)
                    {
                        MemoryGrid.Rows.Add(key.ToString(), m.Memory_address[key]);
                    }
                }
                

                

                PiplineGrid.Rows.Add("IF_ID Instruction", m.IF_ID.Substring(0, 32));
                PiplineGrid.Rows.Add("IF_ID Adder_1 Output", m.IF_ID.Substring(32, 4));
                PiplineGrid.Rows.Add("ID_EX ALUOp", m.ID_EX[0].Substring(0, 2));
                PiplineGrid.Rows.Add("ID_EX ALUSrc", m.ID_EX[0].Substring(2, 1));
                PiplineGrid.Rows.Add("ID_EX RegDst", m.ID_EX[0].Substring(3, 1));
                PiplineGrid.Rows.Add("ID_EX MemWrite", m.ID_EX[0].Substring(4, 1));
                PiplineGrid.Rows.Add("ID_EX MemRead", m.ID_EX[0].Substring(5, 1));
                PiplineGrid.Rows.Add("ID_EX Branch", m.ID_EX[0].Substring(6, 1));
                PiplineGrid.Rows.Add("ID_EX MemToReg", m.ID_EX[0].Substring(7, 1));
                PiplineGrid.Rows.Add("ID_EX RegWrite", m.ID_EX[0].Substring(8, 1));
                PiplineGrid.Rows.Add("ID_EX Adder_1 Output", m.ID_EX[1]);
                PiplineGrid.Rows.Add("ID_EX Read data_1", m.ID_EX[2]);
                PiplineGrid.Rows.Add("ID_EX Read data_2", m.ID_EX[3]);
                PiplineGrid.Rows.Add("ID_EX Insr_0_15", m.ID_EX[4]);
                PiplineGrid.Rows.Add("ID_EX Insr_20_16", m.ID_EX[5]);
                PiplineGrid.Rows.Add("ID_EX Insr_15_11", m.ID_EX[6]);
                



                PiplineGrid.Rows.Add("EX_MEM MemWrite", m.EX_MEM[0].Substring(0, 1));
                PiplineGrid.Rows.Add("EX_MEM MemRead", m.EX_MEM[0].Substring(1, 1));
                PiplineGrid.Rows.Add("EX_MEM Branch", m.EX_MEM[0].Substring(2, 1));
                PiplineGrid.Rows.Add("EX_MEM MemToReg", m.EX_MEM[0].Substring(3, 1));
                PiplineGrid.Rows.Add("EX_MEM RegWrite", m.EX_MEM[0].Substring(4, 1));
                PiplineGrid.Rows.Add("EX_MEM Adder_2 Output", m.EX_MEM[1]);
                PiplineGrid.Rows.Add("EX_MEM ALU Zero", m.EX_MEM[2]);
                PiplineGrid.Rows.Add("EX_MEM ALU Result", m.EX_MEM[3]);
                PiplineGrid.Rows.Add("EX_MEM Read data_2", m.EX_MEM[4]);
                PiplineGrid.Rows.Add("EX_MEM MUX_RegDst Output", m.EX_MEM[5]);
                




                PiplineGrid.Rows.Add("MEM_WB MemToReg", m.MEM_WB[0].Substring(0, 1));
                PiplineGrid.Rows.Add("MEM_WB RegWrite", m.MEM_WB[0].Substring(1, 1));
                PiplineGrid.Rows.Add("MEM_WB Memory Read data", m.MEM_WB[1]);
                PiplineGrid.Rows.Add("MEM_WB ALU Result", m.MEM_WB[2]);
                PiplineGrid.Rows.Add("MEM_WB MUX_RegDst Output", m.MEM_WB[3]);
                

                cycle++;
            }
        }

        private void MemoryGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
