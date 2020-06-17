using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using DealFile;
using System.IO;

namespace DealMedia
{
    public class ParVoice
    {
        #region 静态实例
        public static ParVoice P_I = new ParVoice();
        #endregion 静态实例

        #region 定义
        public string PathRootVoice = ParPathRoot.PathRoot+"Store\\Voice\\";
        public string PathVoiceIni = ParPathRoot.PathRoot + "Store\\Voice\\ParVoice.ini";

        public List<BasicParVoice> BasicParVoice_L = new List<BasicParVoice>();
        #endregion 定义

        public BasicParVoice this[string name]
        {
            get
            {
                try
                {
                    name = name.Replace(".mp3","");
                    foreach (BasicParVoice item in BasicParVoice_L)
                    {
                        if (item.NameVoice == name+".mp3")
                        {
                            return item;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public void ReadParIni()
        {
            try
            {
                BasicParVoice_L.Clear();
                DirectoryInfo theFolder = new DirectoryInfo(PathRootVoice);
                foreach (FileInfo item in theFolder.GetFiles())
                {
                    string name = item.Name;
                    BasicParVoice_L.Add(new BasicParVoice()
                    {
                        NameVoice = name,                       
                    });
                }
                for (int i = 0; i < BasicParVoice_L.Count; i++)
                {
                    bool blExceute = IniFile.I_I.ReadIniBl(BasicParVoice_L[i].NameVoice, "BlExceute", true, PathVoiceIni);
                    int NumPlay = IniFile.I_I.ReadIniInt(BasicParVoice_L[i].NameVoice, "NumPlay", 2, PathVoiceIni);

                    BasicParVoice_L[i].No = i + 1;
                    BasicParVoice_L[i].BlExceute = blExceute;
                    BasicParVoice_L[i].NumPlay = NumPlay;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void WriteParIni()
        {
            try
            {                
                for (int i = 0; i < BasicParVoice_L.Count; i++)
                {
                    bool blExceute = IniFile.I_I.ReadIniBl(BasicParVoice_L[i].NameVoice, "BlExceute", PathVoiceIni);
                    int NumPlay = IniFile.I_I.ReadIniInt(BasicParVoice_L[i].NameVoice, "NumPlay", PathVoiceIni);

                    BasicParVoice_L[i].BlExceute = blExceute;
                    BasicParVoice_L[i].NumPlay = NumPlay;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class BasicParVoice:BaseClass
    {
        public string NameVoice { get; set; }
        public bool BlExceute { get; set; }//是否播放

        int numPlay = 0;
        public int NumPlay//播放次数
        {
            get
            {
                if (numPlay==0)
                {
                    numPlay = 1;
                }
                if (numPlay>5)
                {
                    numPlay = 5;
                }
                return numPlay;
            }
            set
            {
                numPlay = value;
            }
        }
    }
}
