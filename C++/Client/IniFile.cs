using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Scroller
{
   public class IniFile 
    {
       public string path;
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public IniFile(string inipath)
        {
           path = inipath;
        }
     public void IniWriteValue(string Section,string Key,string Value)
    {
         
      WritePrivateProfileString(Section,Key,Value,this.path);
    }
   
    public string IniReadValue(string Section,string Key)
    { 
      StringBuilder temp = new StringBuilder(255);
      int i = GetPrivateProfileString(Section,Key,"",temp,255, this.path);
      string strval = temp.ToString();
      if (strval == "")
      {
          return (-1).ToString();
      }
      else
      {
          return strval;
      }
     // return temp.ToString();
    }
  }
    }



    

