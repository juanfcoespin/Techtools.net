using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using System.Security.Permissions;
using TechTools.Utils;


namespace TechTools.Utils
{
    public class FileUtils
    {
        public delegate void dPercentageProgress(int progress);
        public event dPercentageProgress PercentageCopyProgressEvent;
        public FileUtils() { }
        
        public static void EscribirEnArchivo(string fileName, string texto)
        {
                          
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
                // esto inserta texto en un archivo existente, si el archivo no existe lo crea
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(texto);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void AddDataToFile(string fileName, string text)
        {

            try
            {
                if(HasFolders(fileName) && !Directory.Exists(fileName))
                    CreateFileDirectory(fileName);
                //File.SetAttributes(fileName, FileAttributes.Normal);
                using (StreamWriter stream = System.IO.File.AppendText(fileName))
                {
                    stream.WriteLine(text);
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void CreateFileDirectory(string fileName)
        {
            var matrix = fileName.Split(new char[] { '\\'});
            //se extre solo la parte del archivo
            var index = 0;
            var folderPath = "";
            foreach (var item in matrix)
            {
                if (index <= matrix.Length - 2)
                {
                    if (index > 0)
                        folderPath += "\\";
                    folderPath += item;
                }
                index++;
            }
            Directory.CreateDirectory(folderPath);
        }
        /// <summary>
        /// test.txt => test_20190516_15h30_25s21
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string AddTimeToFileName(string fileName)
        {
            var extension = GetFileNameExtension(fileName);
            fileName = fileName.Replace("."+extension, null);
            var today = DateTime.Now;
            return string.Format("{0}_{1}{2}{3}_{4}h{5}_{6}s{7}.{8}",
                fileName,
                today.Year,
                StringUtils.getTwoDigitNumber(today.Month),
                StringUtils.getTwoDigitNumber(today.Day),
                StringUtils.getTwoDigitNumber(today.Hour),
                StringUtils.getTwoDigitNumber(today.Minute),
                StringUtils.getTwoDigitNumber(today.Second),
                today.Millisecond,
                extension
                );
        }
        private static string GetFileNameExtension(string fileName)
        {
            var matrix = fileName.Split(new char[] { '.'});
            if (matrix != null)
                return matrix[matrix.Length - 1];
            return fileName;
        }
        public static bool HasFolders(string fileName)
        {
            var matrix = fileName.Split(new char[] { '\\','/'});
            return matrix.Length > 0;
        }
        public static void Ejecutar(string filePath)
        {
            if (System.IO.File.Exists(filePath))
                Process.Start(filePath);
            else
                throw new Exception(String.Format("The file {0} not exist!",filePath));
        }
        public static void EjecutarEnDirectorioTemporal(string pathArchivo)
        {
            pathArchivo = Path.GetTempPath() + pathArchivo;
            Ejecutar(pathArchivo);
        }
        public static void BorrarDelDirectorioTemporal(string fileName)
        {
            try
            {
                var filePath = string.Format("{0}{1}", Path.GetTempPath(), fileName);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            catch {}// da un error cuando se trata de borrar un archivo que está abierto

            
        }
        /// <summary>
        /// Esta funcion hace un pool de espera a la creacion de un archivo
        /// </summary>
        /// <param name="pathArchivo"></param>
        public static byte[] GetByteArrayFromPathArchivo(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                using (FileStream fsSource = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {

                    // Read the source file into a byte array. 
                    byte[] bytes = new byte[fsSource.Length];
                    int numBytesToRead = (int)fsSource.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead. 
                        int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                        // Break when the end of the file is reached. 
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    return bytes;
                }
            }
            return null;
        }
        
        public  bool CreateFile_FromByteArrayWithProgress(string filePath, byte[] bFile)
        {
            return CreateFile_FromByteArray(filePath, bFile, PercentageCopyProgressEvent);
        }
        public static bool CreateFile_FromByteArray(string filePath, byte[] bFile, dPercentageProgress percentageProgressEvent=null)
        {
            try
            {
                FileStream fs = System.IO.File.Create(filePath);
                long totalBytes = bFile.Length;
                long i = 0;
                long lastProgress = 0;
                foreach (byte currentByte in bFile)
                {
                    if (percentageProgressEvent != null)
                    {
                        long currentProgress = (i++ * 100) / totalBytes;
                        if (currentProgress != lastProgress)//para que solo se dispare cuando hay avances enteros y no decimales
                        {
                            percentageProgressEvent.Invoke((int)currentProgress);
                            lastProgress = currentProgress;
                        }
                    }
                    
                    fs.WriteByte(currentByte);
                }
                //se indica que se envie los datos al archivo fisico
                fs.Flush();
                fs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CreateFile_FromByteArrayEnDirectorioTemporal(string fileName, byte[] bFile)
        {
            var path = string.Format("{0}{1}",Path.GetTempPath(),fileName);
            return CreateFile_FromByteArrayWithProgress(path, bFile);
        }
        public static void EjecutarEnDirectorioTemporal_FromByteArray(string fileName, byte[] file)
        {
            var filePath = System.IO.Path.GetTempPath() + fileName;
            if (FileUtils.CreateFile_FromByteArray(filePath, file,null))
            {
                Ejecutar(filePath);
            }
        }
        public static string GetStrigText(string filePath)
        {
            if (System.IO.File.Exists(filePath)) {
                var reader = new StreamReader(filePath);
                var ms = reader.ReadToEnd();
                reader.Close();
                return ms;
            }
            return null;
        }
		public static Image GetImageFromByteArray(byte[] bImage)
		{
			try
			{
				Image img = null;
				if (bImage != null)
				{
					MemoryStream memory = new MemoryStream();
					memory.Write(bImage, 0, bImage.Length);
					img = Image.FromStream(memory);
				}
				return img;
			}
			catch (Exception e)
			{
				throw new Exception("getImage:" + e.ToString(), e);
			}
		}
    }
}
