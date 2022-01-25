using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tikz_Fix
{
    class StringOperations
    {
        public static string GenerateOutput(BindingList<TikzCode> tikzCode)
        {
            string output = "";
            output += "\\begin{tikzpicture}[scale=0.03]\n";
            foreach (var element in tikzCode)
            {
                output += "\\definecolor{strokeColor}" + element.strokeColor + " \\definecolor{fillColor}" + element.fillColor + " \\draw [color=strokeColor, fill=fillColor, fill opacity=" + element.opacity + ", line width=" + element.thickness + "] " + element.shape + ";\n";
            }
            output += "\\end{tikzpicture}";
            
            return output;
        }

        public static BindingList<TikzCode> DivideElements(string text) 
        {
            BindingList<TikzCode> elements = new BindingList<TikzCode>();

            TikzCode _tikzCode = new TikzCode();
            _tikzCode.strokeColor = "";
            _tikzCode.fillColor = "";
            _tikzCode.opacity = 1;
            _tikzCode.thickness = 0;
            _tikzCode.shape = "";
            int index = 0;
            
            while (text.Substring(index).Contains(";"))
            {
                index += text.Substring(index).IndexOf(@"\definecolor{strokeColor}{RGB}") + @"\definecolor{strokeColor}{RGB}".Length;
                _tikzCode.strokeColor += "{RGB}" + text.Substring(index, text.Substring(index).IndexOf("}")) + "}";
                index += text.Substring(index).IndexOf("}") + 1;

                index += text.Substring(index).IndexOf(@"\definecolor{fillColor}{RGB}") + @"\definecolor{fillColor}{RGB}".Length;
                _tikzCode.fillColor += "{RGB}" + text.Substring(index, text.Substring(index).IndexOf("}")) + "}";
                index += text.Substring(index).IndexOf("}") + 1;

                index += text.Substring(index).IndexOf("fill opacity=") + "fill opacity=".Length;
                _tikzCode.opacity = Int32.Parse(text.Substring(index, text.Substring(index).IndexOf(",")));
                index += text.Substring(index).IndexOf(",");

                index += text.Substring(index).IndexOf("line width=") + "line width=".Length;
                _tikzCode.thickness = Int32.Parse(text.Substring(index, text.Substring(index).IndexOf("]")));
                index += text.Substring(index).IndexOf("]");

                index += text.Substring(index).IndexOf("(") + "(".Length;
                _tikzCode.shape += "(" + text.Substring(index, text.Substring(index).IndexOf(")")) + ")";
                index += text.Substring(index).IndexOf(")") + 1;
                _tikzCode.shape += text.Substring(index, text.Substring(index).IndexOf(")")) + ")";
                index += text.Substring(index).IndexOf(")") + 1;


                if (string.Equals(text[index].ToString(), ";"))
                {
                    elements.Add(_tikzCode);
                    _tikzCode = new TikzCode();
                    _tikzCode.strokeColor = "";
                    _tikzCode.fillColor = "";
                    _tikzCode.opacity = 1;
                    _tikzCode.thickness = 0;
                    _tikzCode.shape = "";
                }

                index++;
            }
            
            return elements;
        }

        public static string StrokeRGBToHex(TikzCode element)
        {
            byte[] sRGB = { 0, 0, 0 };
            sRGB[0] = byte.Parse(element.strokeColor.Substring(element.strokeColor.IndexOf("}") + 2, Math.Abs(element.strokeColor.IndexOf("}") + 2 - element.strokeColor.IndexOf(","))));
            string scolor2 = element.strokeColor.Substring(element.strokeColor.IndexOf(",") + 1);
            sRGB[1] = byte.Parse(scolor2.Substring(0, scolor2.IndexOf(",")));
            string scolor3 = scolor2.Substring(scolor2.IndexOf(",") + 1);
            sRGB[2] = byte.Parse(scolor3.Substring(0, scolor3.IndexOf("}")));
            Color myColor = Color.FromRgb(sRGB[0], sRGB[1], sRGB[2]);
            string strokeHex = "#FF" + myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");

            return strokeHex;
        }

        public static string FillRGBToHex(TikzCode element)
        {
            byte[] fRGB = { 0, 0, 0 };
            fRGB[0] = byte.Parse(element.fillColor.Substring(element.fillColor.IndexOf("}") + 2, Math.Abs(element.fillColor.IndexOf("}") + 2 - element.fillColor.IndexOf(","))));
            string fcolor2 = element.fillColor.Substring(element.fillColor.IndexOf(",") + 1);
            fRGB[1] = byte.Parse(fcolor2.Substring(0, fcolor2.IndexOf(",")));
            string fcolor3 = fcolor2.Substring(fcolor2.IndexOf(",") + 1);
            fRGB[2] = byte.Parse(fcolor3.Substring(0, fcolor3.IndexOf("}")));
            Color myColor = Color.FromRgb(fRGB[0], fRGB[1], fRGB[2]);
            string fillHex = "#FF" + myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");

            return fillHex;
        }
    }
}
