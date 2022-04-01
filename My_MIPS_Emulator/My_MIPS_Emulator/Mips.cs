using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;

namespace My_MIPS_Emulator
{
    class Mips
    {
        int PC_Counter = 1000;
        public int[] Rejesters = new int[32];
        public string IF_ID;
        public string[] ID_EX = new string[9];
        public string[] EX_MEM = new string[8];
        public string[] MEM_WB = new string[6];
        //controls
        string PCSrc, RegWrite, ALUSrc, ALUOp, RegDst, Branch, MemWrite, MemRead, MemToReg;
        public Dictionary<int, string> Memory_address = new Dictionary<int, string>();
        Dictionary<int, string> instructuin_input = new Dictionary<int, string>();

        int WriteRegister = 0, Writedata = 0;
        int ReadRegister1 = 0, ReadRegister2 = 0, insr_0_15 = 0, insr_20_16 = 0, insr_15_11 = 0,
            readdata1 = 0, Readdata2 = 0;
        int Adder2output = 0, Aluoutput = 0, Aluzero = 0, read_data_forword = 0;
        int andoutput = 0, address = 0, MEMwritedata = 0, MEMreaddata = 0;
        public void Get_Input(Dictionary<int, string> s, int last_address)
        {
            instructuin_input = s;

            Rejesters[0] = 0;
            for (int i = 1; i < 32; i++)
            {
                Rejesters[i] = 100 + i;

            }

            IF_ID = "000000000000000000000000000000000000";
            ID_EX[0] = "000000000";
            EX_MEM[0] = "00000";
            MEM_WB[0] = "00";

            for (int i = 1; i < 9; i++)
            {
                ID_EX[i] = "0";
            }
            for (int i = 1; i < 8; i++)
            {
                EX_MEM[i] = "0";
            }
            for (int i = 1; i < 6; i++)
            {
                MEM_WB[i] = "0";
            }


            instructuin_input.Add(last_address + 4, "00000000000000000000000000000000");
            instructuin_input.Add(last_address + 8, "00000000000000000000000000000000");
            instructuin_input.Add(last_address + 12, "00000000000000000000000000000000");
            instructuin_input.Add(last_address + 16, "00000000000000000000000000000000");

            /*1000 : 00000000011110110001000000100100
            1004 : 00000000101000000010000000100000
            1008 : 10001111101001110000000000000100
            1012 : 00000000010000110100000000100010
            1016 : 0000000010000101 0011000000100101*/

            /*1000 : 000000  00010 00011 00101 00000 100000
              1004 : 000000  00110 00001 00100 00000 100010
              1008 : 000000  01000 01001 00111 00000 100100
              1012 : 000000  00100 00111 01010 00000 100101
              1016 : 100011 10100 01011 0000000000001000*/


            /*for (int i = 0; i <= 134; i++)
            {
                Memory_address.Add(i, "99");
            }*/


            PCSrc = "0"; RegWrite = "0"; ALUSrc = "0";
            ALUOp = "00"; RegDst = "0"; Branch = "0"; MemWrite = "0";
            MemRead = "0"; MemToReg = "0";


        }



        public int Adder1(int input1, int input2)
        {
            int Adder1_output = input1 + input2;

            return Adder1_output;
        }




        void Register_File(int opcode)
        {
            if (opcode == 0)
            {
                ReadRegister1 = Convert.ToInt32(IF_ID.Substring(6, 5), 2);
                ReadRegister2 = Convert.ToInt32(IF_ID.Substring(11, 5), 2);

                readdata1 = Rejesters[ReadRegister1];
                Readdata2 = Rejesters[ReadRegister2];
                if (ReadRegister1 == 0)
                {
                    readdata1 = 0;
                }
                if (ReadRegister2 == 0)
                {
                    Readdata2 = 0;
                }
                insr_15_11 = Convert.ToInt32(IF_ID.Substring(16, 5), 2);

            }

            if (opcode == 35)
            {
                ReadRegister1 = Convert.ToInt32(IF_ID.Substring(6, 5), 2);
                readdata1 = ReadRegister1 + 100;
                if (ReadRegister1 == 0)
                {
                    readdata1 = 0;
                }
                if(IF_ID.Substring(16, 1)=="0")
                {
                    insr_0_15 =  Convert.ToInt32("0000000000000000" + IF_ID.Substring(16, 16), 2);
                }
                if (IF_ID.Substring(16, 1) == "1")
                {
                    insr_0_15 = Convert.ToInt32("1111111111111111" + IF_ID.Substring(16, 16), 2);
                }
                
                insr_20_16 = Convert.ToInt32(IF_ID.Substring(11, 5), 2);
            }


        }


        void Adder2()
        {
            Adder2output = Convert.ToInt32(EX_MEM[1]) + (Convert.ToInt32(EX_MEM[4]) << 2);
            EX_MEM[1] = Adder2output.ToString();
        }

        void PCSrc_MUX()
        {
            andoutput = Convert.ToInt32(EX_MEM[0].Substring(2, 1)) & Convert.ToInt32(EX_MEM[2]);
            PCSrc = andoutput.ToString();
        }
        void RegDst_MUX()
        {
            if (ID_EX[0].Substring(3, 1) == "0")
            {
                EX_MEM[5] = ID_EX[5];
            }
            if (ID_EX[0].Substring(3, 1) == "1")
            {
                EX_MEM[5] = ID_EX[6];
            }
        }
        void MemToReg_MUX()
        {
            WriteRegister = Convert.ToInt32(MEM_WB[3]);
            if (MEM_WB[0] == "11" & Convert.ToInt32(MEM_WB[5]) == 35)
            {

                Writedata = Convert.ToInt32(MEM_WB[1]);
                Rejesters[WriteRegister] = Writedata;
            }
            if (MEM_WB[0] == "01" & Convert.ToInt32(MEM_WB[5]) == 0)
            {
                Writedata = Convert.ToInt32(MEM_WB[2]);
                Rejesters[WriteRegister] = Writedata;
            }
        }


        void Memory_Access()
        {
            address = Convert.ToInt32(EX_MEM[3]);


            if ((Convert.ToInt32(EX_MEM[7]) == 35) & (Convert.ToInt32(EX_MEM[0].Substring(1, 1)) == 1))
            {
                Memory_address.Add(address,"99");
                MEM_WB[1] = Memory_address[address].ToString();
            }
            if (Convert.ToInt32(EX_MEM[0].Substring(0, 1)) == 1)
            {
                MEMwritedata = Convert.ToInt32(EX_MEM[4]);
                Memory_address.Add(address, MEMwritedata.ToString());
            }

        }






        public void Fetsh_Instruction()
        {



            IF_ID = instructuin_input[PC_Counter];

            int Adder1_output = Adder1(4, PC_Counter);
            if (PCSrc == "0")
            {
                PC_Counter = Adder1_output;
            }
            if (PCSrc == "1")
            {
                PC_Counter = Convert.ToInt32(EX_MEM[1]);
            }

            IF_ID += PC_Counter.ToString();
        }
        public void Decode_Instructionion()
        {


            int func = Convert.ToInt32(IF_ID.Substring(26, 6), 2);
            int opcode = Convert.ToInt32(IF_ID.Substring(0, 6), 2);

            if ((opcode == 0) & (func == 36))
            {
                RegWrite = "1";
                ALUOp = "10";
                ALUSrc = "0";
                RegDst = "1";
                MemWrite = "0";
                MemRead = "0";
                MemToReg = "0";
                Branch = "0";
            }
            if ((opcode == 0) & (func == 32))
            {
                RegWrite = "1";
                ALUOp = "10";
                ALUSrc = "0";
                RegDst = "1";
                MemWrite = "0";
                MemRead = "0";
                MemToReg = "0";
                Branch = "0";


            }
            if (opcode == 35)
            {
                RegWrite = "1";
                ALUOp = "00";
                ALUSrc = "1";
                RegDst = "0";
                MemWrite = "0";
                MemRead = "1";
                MemToReg = "1";
                Branch = "0";
            }
            if ((opcode == 0) & (func == 34))
            {
                RegWrite = "1";
                ALUOp = "10";
                ALUSrc = "0";
                RegDst = "1";
                MemWrite = "0";
                MemRead = "0";
                MemToReg = "0";
                Branch = "0";
            }
            if ((opcode == 0) & (func == 37))
            {
                RegWrite = "1";
                ALUOp = "10";
                ALUSrc = "0";
                RegDst = "1";
                MemWrite = "0";
                MemRead = "0";
                MemToReg = "0";
                Branch = "0";
            }


            //Register File
            Register_File(opcode);




            ID_EX[0] = ALUOp + ALUSrc + RegDst + MemWrite + MemRead + Branch + MemToReg + RegWrite;
            ID_EX[1] = IF_ID.Substring(32, 4);
            ID_EX[2] = readdata1.ToString();
            ID_EX[3] = Readdata2.ToString();
            ID_EX[4] = insr_0_15.ToString();
            ID_EX[5] = insr_20_16.ToString();
            ID_EX[6] = insr_15_11.ToString();
            ID_EX[7] = func.ToString();
            ID_EX[8] = opcode.ToString();


        }
        public void Excute_Stage()
        {


            EX_MEM[0] = ID_EX[0].Substring(4, 5);

            //Adder 2
            Adder2();

            if (ID_EX[0].Substring(2, 1) == "0")
            {
                if (Convert.ToInt32(ID_EX[7]) == 36)
                {
                    Aluoutput = Convert.ToInt32(ID_EX[2]) & Convert.ToInt32(ID_EX[3]);

                }
                if (Convert.ToInt32(ID_EX[7]) == 32)
                {
                    Aluoutput = Convert.ToInt32(ID_EX[2]) + Convert.ToInt32(ID_EX[3]);

                }
                if (Convert.ToInt32(ID_EX[7]) == 34)
                {
                    Aluoutput = Convert.ToInt32(ID_EX[2]) - Convert.ToInt32(ID_EX[3]);
                    Aluzero = 1;

                }
                if (Convert.ToInt32(ID_EX[7]) == 37)
                {
                    Aluoutput = Convert.ToInt32(ID_EX[2]) | Convert.ToInt32(ID_EX[3]);

                }

            }
            if (ID_EX[0].Substring(2, 1) == "1")
            {
                Aluoutput = Convert.ToInt32(ID_EX[2]) + Convert.ToInt32(ID_EX[4]);

            }
            EX_MEM[2] = Aluzero.ToString();
            EX_MEM[3] = Aluoutput.ToString();
            EX_MEM[4] = ID_EX[3];

            //RegDst MUX
            RegDst_MUX();


            EX_MEM[6] = ID_EX[7];
            EX_MEM[7] = ID_EX[8];

        }
        public void Memory_Addressing()
        {


            MEM_WB[0] = EX_MEM[0].Substring(3, 2);

            //PCSrc MUX
            PCSrc_MUX();

            //Memory Access

            Memory_Access();

            MEM_WB[2] = EX_MEM[3];
            MEM_WB[3] = EX_MEM[5];

            MEM_WB[4] = EX_MEM[6];
            MEM_WB[5] = EX_MEM[7];
        }
        public void Write_Back()
        {

            //Mem To Reg MUX
            MemToReg_MUX();

            Writedata = 0;
            WriteRegister = 0;

        }
        /*public void Display()
        {
            Console.WriteLine(PC_Counter);
            for (int i = 0; i < 32; i++)
            {
                Console.Write(Rejesters[i] + "   ");
            }
            Console.WriteLine();
            for (int i = 129; i < 134; i++)
            {
                Console.Write(Memory_address[i] + "    ");
            }
            Console.WriteLine();
            Console.Write(IF_ID);
            Console.WriteLine();
            for (int i = 0; i < 9; i++)
            {
                Console.Write(ID_EX[i] + "   ");
            }
            Console.WriteLine();
            for (int i = 0; i < 8; i++)
            {
                Console.Write(EX_MEM[i] + "   ");
            }
            Console.WriteLine();
            for (int i = 0; i < 6; i++)
            {
                Console.Write(MEM_WB[i] + "   ");
            }
        }*/

    }
}
