using UnityEngine;
using System.Collections;
using System.IO;

namespace ET.Codes.Hotfix.GameLogic.Common
{
    /// <summary>
    /// 文件读取系统
    /// </summary>
    public class FileSystem
    {

        private static FileSystem instance;
        public static FileSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FileSystem();
                }
                return instance;
            }
        }
        private FileSystem()
        {

        }

        private string m_DataPath;
        protected string m_strDownloadPath;
#if UNITY_EDITOR

        private string m_SecondDataPath;
#endif
        public void Init(string dataPath)
        {
            m_DataPath = dataPath;

#if UNITY_EDITOR
            m_SecondDataPath = string.Empty;
            m_strDownloadPath = "Assets/DownloadResource/";
#else
            m_strDownloadPath =  Application.persistentDataPath + "/" + FuncUtility.GetPlatformName(); 
#endif

        }
        void CreateDirectory(string path)
        {
            if (Directory.Exists(path) == true)
                return;
            int iLastIndex = path.LastIndexOf('/');
            if (iLastIndex > 0)
            {
                string parentPath = path.Substring(0, iLastIndex);
                if (Directory.Exists(parentPath) == false)
                {
                    CreateDirectory(parentPath);
                }
            }
            Directory.CreateDirectory(path);

        }
        public TextWriter CreateTextWriter(string fileName)
        {
            string strFullPath = m_DataPath + fileName;
            int iLastIndex = strFullPath.LastIndexOf('/');
            if (iLastIndex > 0)
            {
                string parentPath = strFullPath.Substring(0, iLastIndex);
                if (File.Exists(parentPath) == false)
                {
                    CreateDirectory(parentPath);
                }
            }

            return File.CreateText(strFullPath);

        }

        public bool IsExists(string fileName)
        {
            string fullPath = m_DataPath + fileName;
            return IsExists(fullPath, fileName);
        }

        bool IsExists(string fullPath, string fileName)
        {
            bool bExists = File.Exists(fullPath);
            if (bExists == true)
                return true;
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(m_SecondDataPath) == true)
                return false;

            string srcFilePath = m_SecondDataPath + fileName;
            if (File.Exists(srcFilePath) == true)
            {

                int iLastIndex = fullPath.LastIndexOf('/');
                if (iLastIndex > 0)
                {
                    string parentPath = fullPath.Substring(0, iLastIndex);
                    if (File.Exists(parentPath) == false)
                    {
                        CreateDirectory(parentPath);
                    }
                }
                File.Copy(srcFilePath, fullPath);
                return true;
            }
#endif
            return false;
        }
        public FileStream OpenFile(string fileName)
        {

            string strFullPath = m_DataPath + fileName;
            if (File.Exists(strFullPath) == true)
            {
                return new FileStream(strFullPath, FileMode.Open);
            }
            else
            {
#if !UNITY_ANDROID
                Debug.Log(" file " + strFullPath + " not exists !");
#endif
            }
            return null;
        }
        /// <summary>
        /// 创建2进制的文件
        /// </summary>
        public BinaryWriter CreateBinaryFile(string fileName)
        {
            FileStream fs = CreateWriter(fileName);
            if (fs != null)
            {
                return new BinaryWriter(fs);
            }
            return null;
        }
        /// <summary>
        /// 打开二进制文件XML
        /// </summary>
        public BinaryReader OpenBinaryFile(string fileName)
        {
            if (fileName == null)
                return null;
            BinaryReader reader = null;
            FileStream stream = null;
#if UNITY_EDITOR
            //string m_strDownloadPath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
            //string strDownload = m_strDownloadPath + fileName;
            //if (File.Exists(strDownload))//判断文件是否存在
            //{

            //    stream = new FileStream(strDownload, FileMode.Open);
            //    reader = new BinaryReader(stream);
            //    return reader;
            //}
#endif

            string xmlFile = m_strDownloadPath + fileName;
            //Debug.LogError ( xmlFile );
            if (IsExists(xmlFile, fileName))//判断文件是否存在
            {

                stream = new FileStream(xmlFile, FileMode.Open);
                reader = new BinaryReader(stream);
                return reader;
            }
            else
            {
                TextAsset text = LoadTextAsset(fileName);
                if (text != null)
                    return reader = new BinaryReader(new MemoryStream(text.bytes));
                else
                    return null;
            }
        }
        public FileStream CreateWriter(string fileName)
        {
            string strFullPath = m_DataPath + fileName;
            int iLastIndex = strFullPath.LastIndexOf('/');
            if (iLastIndex > 0)
            {
                string parentPath = strFullPath.Substring(0, iLastIndex);
                if (File.Exists(parentPath) == false)
                {
                    CreateDirectory(parentPath);
                }
            }

            return File.Create(strFullPath);

        }
        public string LoadText(string fileName)
        {
            if (fileName == null)
                return null;
            TextReader reader = null;
            TextAsset text = null;
            string xmlFile = m_DataPath + fileName;

            if (IsExists(xmlFile, fileName))
            {
                reader = new StreamReader(xmlFile);
            }
            if (reader == null)
            {
                text = LoadTextAsset(fileName);
                if (text != null)
                {
                    string strText = text.text;
                    Resources.UnloadAsset(text);
                    return strText;
                }
                else
                    return null;
            }
            else
            {
                string strText = reader.ReadToEnd();
                reader.Close();
                return strText;
            }

        }
        /// <summary>
        /// 加载文本数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public TextAsset LoadTextAsset(string fileName)
        {
            TextAsset textAsset = null;
            int iLen = fileName.Length;
            int iDotIndex = 0;
            for (int idot = iLen - 1; idot > 0; idot--)
            {
                if (fileName[idot] == '.')
                {
                    iDotIndex = idot;
                }
            }

            if (iDotIndex > 0)
            {
                string subFileName = fileName.Substring(0, iDotIndex);//返回一个从0，到iDotIndex
                //不包含iDotIndex的子字符串
                textAsset = Resources.Load(subFileName) as TextAsset;
            }

            else
            {
                textAsset = Resources.Load(fileName) as TextAsset;
            }
            return textAsset;
        }
        public void WriteBinaryData(string fileName, byte[] bData)
        {
            using (FileStream fs = CreateWriter(fileName))
            {
                fs.Write(bData, 0, bData.Length);
            }
        }
        public void WriteText(string fileName, string strText)
        {
            using (TextWriter fs = CreateTextWriter(fileName))
            {
                fs.Write(strText);
            }
        }
        public string RemoveFileExtent(string fileName)
        {
            int iEnd = -1;
            for (int i = fileName.Length - 1; i > 0; i--)
            {
                if (fileName[i] == '.')
                {
                    iEnd = i;
                    break;
                }
            }
            if (iEnd == -1)
                return "";
            return fileName.Substring(0, iEnd);
        }
        public byte[] LoadBinaryData(string fileName)
        {
            if (fileName == null)
                return null;
            BinaryReader reader = null;

            FileStream stream = null;
            TextAsset text = null;
            string xmlFile = m_DataPath + fileName;
            //Debug.LogError ( xmlFile );
            if (IsExists(xmlFile, fileName))
            {

                stream = new FileStream(xmlFile, FileMode.Open);
                reader = new BinaryReader(stream);
            }
            if (reader == null)
            {
                text = LoadTextAsset(fileName);
                if (text != null)
                    return text.bytes;
                else
                    return null;
            }
            else
            {
                byte[] bytes = reader.ReadBytes((int)stream.Length);
                stream.Close();
                reader.Close();
                return bytes;
            }
        }
    }
}