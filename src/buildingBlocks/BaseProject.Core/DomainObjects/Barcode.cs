using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BaseProject.Core.DomainObjects
{
    public class BarcodeEAN13
    {
        private const int Lentgh = 13;

        public string Code { get; private set; }
        public BarcodeEAN13() { }
        public BarcodeEAN13(string code)
        {
            if (code.Length != Lentgh || string.IsNullOrEmpty(code) || !IsValid(code))
                throw new DomainException("O código informado não é um EAN13");
            Code = code;
        }

        public static bool IsValid(string code)
        {
            if (!Regex.IsMatch(code, "^[0-9]{13}$"))
            {
                return false;
            }
            var numeros = code.ToCharArray().Select(a => (int)a).ToArray();
            int somaPares = numeros[1] + numeros[3] + numeros[5] + numeros[7] + numeros[9] + numeros[11];
            int somaImpares = numeros[0] + numeros[2] + numeros[4] + numeros[6] + numeros[8] + numeros[10];
            int resultado = somaImpares + somaPares * 3;
            int digitoVerificador = 10 - resultado % 10;
            return digitoVerificador == numeros[12];
        }

    }
}
