using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace deneme
{
    public class Data
    {
        private DateTime time;
     
        public double timespan;
        public int f;
        int c;
        int count;
        int flag;
        public string str_form;
        string[] str_pil = new string[8] { "", "", "", "", "", "", "", "" };
        string[] str_mppt = new string[4] { "", "", "", "" };
        List<byte> f_data = new List<byte>();
        List<byte[]> p_data = new List<byte[]>();
        List<string> k_data = new List<string>() { "0", "0", "0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0 0 0 0 0 0 0 0", "0", "0", "0", "0", "0", "0", "0 0 0", "0", "0","0" };
        Encoding enc = Encoding.GetEncoding(28591); //objeye giderek getencodinge giderek sayı bulunabilir google
        public enum infos
        {
            SOC = 51,
            İvmeX = 119,
            İvmeY = 118,
            BGrup = 0,
            İGrup = 16,
            ÜGrup = 17,
            DGrup = 18,
            BaraG = 1,
            BaraA = 2,
            Hız = 4,
            MotorS = 146,
            Gidilen = 43, // Hex=2b,Dec=43
            HarcananE = 5,
            MpptA = 84,
            MpptS = 128,
            Eğim = 120,
        }


        public Data()
        {

        }

        public void encode() 
        {
            f_data = enc.GetBytes(str_form).ToList<byte>();
        }


        public void parse_it()
        {    // 4 lü ayırma yeri çünkü 4 byte gelior
            count = f_data[0];
            if (count != f_data.Count() - 2) return;
            flag = f_data[1];
            if (count == 13) count = 12;
            
            for (int i = 0; i < count / 4; i++)
            {
                byte[] myarr = new byte[4] { 0, 0, 0, 0 };
                for (int k = 0; k < 4; k++)
                {

                    myarr[k] = f_data[2 + i * 4 + k];
                }
                p_data.Add(myarr);
            }
        }

        public List<string> write_collect()
        {
            switch (flag)
            {
                case (int)infos.SOC:
                    f = 1;
                    k_data[0] = Math.Round(BitConverter.ToSingle(p_data[0], 0), 2).ToString();
                    p_data.Clear();
                    break;
                case (int)infos.İvmeX:
                    f = 2;
                    k_data[1]=Math.Round(BitConverter.ToSingle(p_data[0], 0), 2).ToString();
                    p_data.Clear();
                    break;
                case (int)infos.İvmeY:
                    f = 3;
                    k_data[2]= Math.Round(BitConverter.ToSingle(p_data[0], 0),2).ToString();
                    p_data.Clear();
                    break;
                case (int)infos.BGrup:
                    f = 4;
                    c = 0;
                    foreach (byte[] b in p_data)
                    {
                        str_pil[c] = Math.Round(BitConverter.ToSingle(b, 0), 2).ToString();
                        c++;
                    }
                    k_data[3] = string.Join("  ", str_pil);
                    p_data.Clear();
                    break;
                case (int)infos.İGrup:
                    f = 5;
                    c = 0;
                    foreach (byte[] b in p_data)
                    {
                        str_pil[c] = Math.Round(BitConverter.ToSingle(b, 0), 2).ToString();
                        c++;
                    }
                    k_data[4] = string.Join("  ", str_pil);
                    p_data.Clear();
                    break;
                case (int)infos.ÜGrup:
                    f = 6;
                    c = 0;
                    foreach (byte[] b in p_data)
                    {
                        str_pil[c] = Math.Round(BitConverter.ToSingle(b, 0), 2).ToString();
                        c++;
                    }
                    k_data[5] = string.Join("  ", str_pil);
                    p_data.Clear();
                    break;
                case (int)infos.DGrup:
                    f = 7;
                    c = 0;
                    foreach (byte[] b in p_data)
                    {
                        str_pil[c] = Math.Round(BitConverter.ToSingle(b, 0), 2).ToString();
                        c++;
                    }
                    k_data[6] = string.Join("  ", str_pil);
                    p_data.Clear();
                    break;
                case (int)infos.BaraG:
                    f = 8;
                    k_data[7] = Math.Round(BitConverter.ToSingle(p_data[0], 0),2).ToString();
                    if (time != null)
                    {
                        timespan=(double)Math.Abs((DateTime.Now.Ticks-time.Ticks));
                    }
                    time = DateTime.Now;
                    p_data.Clear();
                    break;
                case (int)infos.BaraA:
                    f = 9;
                    k_data[8] = Math.Round(BitConverter.ToSingle(p_data[0], 0),2).ToString();
                    if (time != null)
                    {
                        timespan = (double)Math.Abs((DateTime.Now.Ticks - time.Ticks));
                    }
                    time = DateTime.Now;
                    p_data.Clear();
                    break;
                case (int)infos.Hız:
                    f = 10;
                    k_data[9] = Math.Round(BitConverter.ToSingle(p_data[0], 0)*3.6, 2).ToString();
                    p_data.Clear();
                    break;
                case (int)infos.MotorS:
                    f = 11;
                    k_data[10] = Math.Round(BitConverter.ToSingle(p_data[0], 0), 2).ToString();
                    p_data.Clear();
                    break;
                case (int)infos.Gidilen:
                    f = 12;
                    k_data[11]= Math.Round(BitConverter.ToSingle(p_data[0], 0)/1000, 4).ToString();
                    p_data.Clear();
                    break;
                case (int)infos.HarcananE:
                    f = 13;
                    k_data[12] = Math.Round(BitConverter.ToSingle(p_data[0], 0), 2).ToString();
                    k_data[16] = Math.Round(BitConverter.ToSingle(p_data[1], 0), 2).ToString();
                    p_data.Clear();
                    break;
                case (int)infos.MpptA:
                    f = 14;
                    c = 0;
                    foreach (byte[] b in p_data)
                    {
                        str_mppt[c] = Math.Round(BitConverter.ToSingle(b, 0),2).ToString();
                        c++;
                    }
                    k_data[13] = string.Join("  ", str_mppt);
                    p_data.Clear();
                    break;
                case (int)infos.MpptS:
                    f = 15;
                    k_data[14]= Math.Round(BitConverter.ToSingle(p_data[0], 0), 2).ToString();
                    p_data.Clear();
                    break;
                case (int)infos.Eğim:
                    f = 16;
                    k_data[15] = Math.Round(BitConverter.ToSingle(p_data[0], 0)*-1, 2).ToString();
                    p_data.Clear();
                    break;


            

            }

            return k_data;

        }
    }

    
}
