using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gaia.Helpers.ExtensionMethods
{
    public static class CustomExtensions
    {
        public static string ToText(this int value)
        {
            string Num2Text = "";
            if (value < 0) return "menos " + Math.Abs(value).ToText();

            if (value == 0) Num2Text = "cero";
            else if (value == 1) Num2Text = "uno";
            else if (value == 2) Num2Text = "dos";
            else if (value == 3) Num2Text = "tres";
            else if (value == 4) Num2Text = "cuatro";
            else if (value == 5) Num2Text = "cinco";
            else if (value == 6) Num2Text = "seis";
            else if (value == 7) Num2Text = "siete";
            else if (value == 8) Num2Text = "ocho";
            else if (value == 9) Num2Text = "nueve";
            else if (value == 10) Num2Text = "diez";
            else if (value == 11) Num2Text = "once";
            else if (value == 12) Num2Text = "doce";
            else if (value == 13) Num2Text = "trece";
            else if (value == 14) Num2Text = "catorce";
            else if (value == 15) Num2Text = "quince";
            else if (value < 20) Num2Text = "dieci" + (value - 10).ToText();
            else if (value == 20) Num2Text = "veinte";
            else if (value < 30) Num2Text = "veinti" + (value - 20).ToText();
            else if (value == 30) Num2Text = "treinta";
            else if (value == 40) Num2Text = "cuarenta";
            else if (value == 50) Num2Text = "cincuenta";
            else if (value == 60) Num2Text = "sesenta";
            else if (value == 70) Num2Text = "setenta";
            else if (value == 80) Num2Text = "ochenta";
            else if (value == 90) Num2Text = "noventa";
            else if (value < 100)
            {
                int u = value % 10;
                Num2Text = string.Format("{0} y {1}", ((value / 10) * 10).ToText(), (u == 1 ? "uno" : (value % 10).ToText()));
            }
            else if (value == 100) Num2Text = "cien";
            else if (value < 200) Num2Text = "ciento " + (value - 100).ToText();
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800))
                Num2Text = ((value / 100)).ToText() + "cientos";
            else if (value == 500) Num2Text = "quinientos";
            else if (value == 700) Num2Text = "setecientos";
            else if (value == 900) Num2Text = "novecientos";
            else if (value < 1000) Num2Text = string.Format("{0} {1}", ((value / 100) * 100).ToText(), (value % 100).ToText());
            else if (value == 1000) Num2Text = "mil";
            else if (value < 2000) Num2Text = "mil " + (value % 1000).ToText();
            else if (value < 1000000)
            {
                Num2Text = ((value / 1000)).ToText() + " mil";
                if ((value % 1000) > 0) Num2Text += " " + (value % 1000).ToText();
            }
            else if (value == 1000000) Num2Text = "un millón";
            else if (value < 2000000) Num2Text = "un millón " + (value % 1000000).ToText();
            else if (value < int.MaxValue)
            {
                Num2Text = ((value / 1000000)).ToText() + " millones";
                if ((value - (value / 1000000) * 1000000) > 0) Num2Text += " " + (value - (value / 1000000) * 1000000).ToText();
            }
            return Num2Text;
        }

        public static string ToAudio(this int value)
        {
            string Num2Text = "";
            if (value < 0) return "menos " + Math.Abs(value).ToAudio();

            if (value == 0) Num2Text = "cero";
            else if (value == 1) Num2Text = "uno";
            else if (value == 2) Num2Text = "dos";
            else if (value == 3) Num2Text = "tres";
            else if (value == 4) Num2Text = "cuatro";
            else if (value == 5) Num2Text = "cinco";
            else if (value == 6) Num2Text = "seis";
            else if (value == 7) Num2Text = "siete";
            else if (value == 8) Num2Text = "ocho";
            else if (value == 9) Num2Text = "nueve";
            else if (value == 10) Num2Text = "diez";
            else if (value == 11) Num2Text = "once";
            else if (value == 12) Num2Text = "doce";
            else if (value == 13) Num2Text = "trece";
            else if (value == 14) Num2Text = "catorce";
            else if (value == 15) Num2Text = "quince";
            else if (value < 20) Num2Text = "dieci " + (value - 10).ToAudio();
            else if (value == 20) Num2Text = "veinte";
            else if (value < 30) Num2Text = "veinti " + (value - 20).ToAudio();
            else if (value == 30) Num2Text = "treinta";
            else if (value == 40) Num2Text = "cuarenta";
            else if (value == 50) Num2Text = "cincuenta";
            else if (value == 60) Num2Text = "sesenta";
            else if (value == 70) Num2Text = "setenta";
            else if (value == 80) Num2Text = "ochenta";
            else if (value == 90) Num2Text = "noventa";
            else if (value < 100)
            {
                int u = value % 10;
                Num2Text = string.Format("{0}y {1}", ((value / 10) * 10).ToAudio(), (u == 1 ? "uno" : (value % 10).ToAudio()));
            }
            else if (value == 100) Num2Text = "cien";
            else if (value < 200) Num2Text = "ciento " + (value - 100).ToAudio();
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800))
                Num2Text = ((value / 100)).ToAudio() + "cientos";
            else if (value == 500) Num2Text = "quinientos";
            else if (value == 700) Num2Text = "setecientos";
            else if (value == 900) Num2Text = "novecientos";
            else if (value < 1000) Num2Text = string.Format("{0} {1}", ((value / 100) * 100).ToAudio(), (value % 100).ToAudio());
            else if (value == 1000) Num2Text = "mil";
            else if (value < 2000) Num2Text = "mil " + (value % 1000).ToAudio();
            else if (value < 1000000)
            {
                Num2Text = ((value / 1000)).ToAudio() + " mil";
                if ((value % 1000) > 0) Num2Text += " " + (value % 1000).ToAudio();
            }
            else if (value == 1000000) Num2Text = "un millón";
            else if (value < 2000000) Num2Text = "un millón " + (value % 1000000).ToAudio();
            else if (value < int.MaxValue)
            {
                Num2Text = ((value / 1000000)).ToAudio() + " millones";
                if ((value - (value / 1000000) * 1000000) > 0) Num2Text += " " + (value - (value / 1000000) * 1000000).ToAudio();
            }
            return Num2Text;
        }

        public static string ToLetras(this DateTime value)
        {
            string dia = value.Day.ToText();
            string mes = (new CultureInfo("es-ES", false).DateTimeFormat).GetMonthName(value.Month);
            string anio = value.Year.ToText();

            var result = string.Format("{0} de {1} del año {2}", dia, mes, anio);

            return result;
        }

        public static string ToLetrasCompleta(this DateTime value)
        {
            int horaV = value.Hour;
            string dianoche = string.Empty;
            string dia = (value.Day == 1 ? "primero" : value.Day.ToText());
            string anio = value.Year.ToText();
            string minutos = (value.Minute == 00 ? "" : "y " + value.Minute.ToText() + " minutos");
            string ampm = (horaV > 12 ? "pm" : "am");
            string hora = ((horaV > 12 ? horaV - 12 : horaV) == 1 ? " una" : "s " + (horaV > 12 ? horaV - 12 : horaV).ToText());
            string mes = (new CultureInfo("es-ES", false).DateTimeFormat).GetMonthName(value.Month);
            
            if (horaV >= 1 && horaV <= 5){ dianoche = "madrugada"; }
            else if (horaV >= 6 && horaV <= 11){ dianoche = "mañana"; }
            else if(horaV >= 12 && horaV <= 17) { dianoche = "tarde"; }
            else if(horaV >= 18 && horaV <= 24) { dianoche = "noche"; }

            var result = string.Format("del día {0} del mes de {1} del año {2} a la{3} {4} de la {5}", dia, mes, anio, hora, minutos, dianoche);

            return result;
        }

        public static string ToFormatAMPM(this DateTime value)
        {
            string result = string.Empty;
            int hora = (value.Hour == 0 ? 12 : (value.Hour > 12 ? value.Hour - 12 : value.Hour));
            string minuto = (value.Minute.ToString().Length == 1 ? "0" + value.Minute.ToString() : value.Minute.ToString());
            string ampm = (value.Hour >= 12 ? "pm" : "am");

            if (value != null)
            {
                result = string.Format("{0}/{1}/{2} {3}:{4} {5}", value.Day, value.Month, value.Year, hora, minuto, ampm);
            }
            
            return result;
        }

        public static string ToLetrasHoraFecha(this DateTime value)
        {
            int horaV = value.Hour;
            string dianoche = string.Empty;
            string dia = (value.Day == 1 ? "primero" : value.Day.ToText());
            string anio = value.Year.ToText();
            string minutos = (value.Minute == 00 ? "" : "y " + value.Minute.ToText() + " minutos ");
            string ampm = (horaV > 12 ? "pm" : "am");
            string hora = ((horaV > 12 ? horaV - 12 : horaV) == 1 ? " una" : "s " + (horaV > 12 ? horaV - 12 : horaV).ToText());
            string mes = (new CultureInfo("es-ES", false).DateTimeFormat).GetMonthName(value.Month);

            if (horaV >= 1 && horaV <= 5) { dianoche = "madrugada"; }
            else if (horaV >= 6 && horaV <= 11) { dianoche = "mañana"; }
            else if (horaV >= 12 && horaV <= 17) { dianoche = "tarde"; }
            else if (horaV >= 18 && horaV <= 24) { dianoche = "noche"; }

            var result = string.Format("a la{0} {1}de la {2} del {3} de {4} del año {5}", hora, minutos, dianoche, dia, mes, anio);

            return result;
        }

        public static string ToFecha(this DateTime value)
        {
            if (value == null)
                return "";

            string dia = value.Day.ToString();
            string anio = value.Year.ToString();
            string mes = value.Month.ToString();
            
            var result = string.Format("{0}/{1}/{2}", (dia.Length > 1 ? dia : "0" + dia), (mes.Length > 1 ? mes : "0" + mes), anio);

            return result;
        }

        public static string ToFecha(this DateTime? value)
        {
            if (value == null)
                return "";

            string dia = value.Value.Day.ToString();
            string anio = value.Value.Year.ToString();
            string mes = value.Value.Month.ToString();

            var result = string.Format("{0}/{1}/{2}", (dia.Length > 1 ? dia : "0" + dia), (mes.Length > 1 ? mes : "0" + mes), anio);

            return result;
        }

        public static string ConvertCharacterSpecialToHtmlCharacter(this string html)
        {

            string[] CaractEspeciales = new string[] {
                "À","Á","Â","Ã","Ä","Å","à","á","â","ã","ä","å",
                "Ç","ç",
                "È","É","Ê","Ë","è","é","ê","ë",
                "Ì","Í","Î","Ï","ì","í","î","ï",
                "Ð",
                "Ñ","ñ",
                "Ò","Ó","Ô","Õ","Ö","ò","ó","ô","õ","ö",
                "Š","š",
                "Ù","Ú","Û","Ü","ù","ú","û","ü",
                "Ý","Ÿ","ý","ÿ",
                "°"
            };

            string[] RemplaceCarater = new string[] {
                "&#192;","&#193;","&#194;","&#195;","&#196;","&#197;","&#224;","&#225;","&#226;","&#227;","&#228;","&#229;",
                "&#199;","&#231;",
                "&#200;","&#201;","&#202;","&#203;","&#232;","&#233;","&#234;","&#235;",
                "&#204;","&#205;","&#206;","&#207;","&#236;","&#237;","&#238;","&#239;",
                "&#208;",
                "&#209;","&#241;",
                "&#210;","&#211;","&#212;","&#213;","&#214;","&#242;","&#243;","&#244;","&#245;","&#246;",
                "&#352;","&#353;",
                "&#217;","&#218;","&#219;","&#220;","&#249;","&#250;","&#251;","&#252;",
                "&#221;","&#376;","&#253;","&#255;",
                "&#176;"
            };

            for (int i = 0; i < CaractEspeciales.Count(); i++)
            {
                html = Regex.Replace(html, CaractEspeciales[i], RemplaceCarater[i]);
            }

            return html;
        }

        public static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

        public static bool isNumeroEntero(string Numero)
        {
            bool value = true;

            try
            {
                Int32.Parse(Numero);

                value = true;
            }  catch (Exception) { value = false; }

            return value;
        }
    }
}
