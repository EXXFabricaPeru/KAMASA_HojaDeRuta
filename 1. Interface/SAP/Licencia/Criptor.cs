using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Licencia
{
    internal class Criptor
    {
        private const int version = 10;
        private const int shift = 5;

        public static string Desencriptar(string word)
        {
            string msj = "";
            if (word.Length > 2)
            {
                string version = word.Substring(0, 2);
                try
                {
                    int versionINT = Convert.ToInt32(version, 16);
                    string dword = word.Substring(2);

                    switch (versionINT)
                    {
                        case 10:
                            dword = System.Text.Encoding.UTF8.GetString((System.Convert.FromBase64String(dword)));
                            for (int i = 0; i < dword.Length; i++)
                            {
                                msj += new String((char)(((int)dword[i]) - shift), 1);
                            }
                            msj = System.Text.Encoding.UTF8.GetString((System.Convert.FromBase64String(msj)));
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return msj;
        }
    }
}
