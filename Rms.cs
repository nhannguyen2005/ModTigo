using System;
using System.IO;
using System.Threading;
using UnityEngine;

public class Rms
{
	public static int status;

	public static sbyte[] data;

	public static string filename;

	private const int INTERVAL = 5;

	private const int MAXTIME = 500;

	public static string RMS_ResVersion = "ResVersion";

	public static string RMS_clienttype = "clienttype";

	public static string RMS_lastZoomlevel = "lastZoomlevel";

	public static string RMS_userAo = "userAo";

	public static string RMS_acc = "acc";

	public static string RMS_pass = "pass";

	public static string RMS_check = "check";

	private static readonly object rmsLock = new object();

	private static string documentPath = null;

	public static void saveRMS(string filename, sbyte[] data)
	{
		lock (rmsLock)
		{
			__saveRMS(filename, data);
		}
	}

	public static sbyte[] loadRMS(string filename)
	{
		lock (rmsLock)
		{
			return __loadRMS(filename);
		}
	}

	public static string loadRMSString(string fileName)
	{
		sbyte[] array = loadRMS(fileName);
		if (array == null)
		{
			return null;
		}
		DataInputStream dataInputStream = new DataInputStream(array);
		try
		{
			string result = dataInputStream.readUTF();
			dataInputStream.close();
			return result;
		}
		catch (Exception ex)
		{
			Cout.println(ex.StackTrace);
		}
		return null;
	}

	public static byte[] convertSbyteToByte(sbyte[] var)
	{
		byte[] array = new byte[var.Length];
		for (int i = 0; i < var.Length; i++)
		{
			if (var[i] > 0)
			{
				array[i] = (byte)var[i];
			}
			else
			{
				array[i] = (byte)(var[i] + 256);
			}
		}
		return array;
	}

	public static void saveRMSString(string filename, string data)
	{
		DataOutputStream dataOutputStream = new DataOutputStream();
		try
		{
			dataOutputStream.writeUTF(data);
			saveRMS(filename, dataOutputStream.toByteArray());
			dataOutputStream.close();
		}
		catch (Exception ex)
		{
			Cout.println(ex.StackTrace);
		}
	}

	private static void _saveRMS(string filename, sbyte[] data)
	{
		saveRMS(filename, data);
	}

	private static sbyte[] _loadRMS(string filename)
	{
		return loadRMS(filename);
	}

	public static void update()
	{
	}

	public static int loadRMSInt(string file)
	{
		sbyte[] array = loadRMS(file);
		return (array != null) ? array[0] : (-1);
	}

	public static void saveRMSInt(string file, int x)
	{
		try
		{
			saveRMS(file, new sbyte[1] { (sbyte)x });
		}
		catch (Exception)
		{
		}
	}

	public static string GetiPhoneDocumentsPath()
	{
		if (documentPath == null)
		{
			documentPath = Application.persistentDataPath;
		}
		return documentPath;
	}

	private static void __saveRMS(string filename, sbyte[] data)
	{
		try
		{
			string text = GetiPhoneDocumentsPath() + "/" + filename;
			File.WriteAllBytes(text, ArrayCast.cast(data));
			Main.setBackupIcloud(text);
		}
		catch (Exception)
		{
		}
	}

	private static sbyte[] __loadRMS(string filename)
	{
		try
		{
			string text = GetiPhoneDocumentsPath() + "/" + filename;
			if (!File.Exists(text))
			{
				return null;
			}
			byte[] array = File.ReadAllBytes(text);
			return ArrayCast.cast(array);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static void clearAll()
	{
		Cout.LogError3("clean rms");
		FileInfo[] files = new DirectoryInfo(GetiPhoneDocumentsPath() + "/").GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			if (!(fileInfo.Name == GameMidlet.VERSION) && !(fileInfo.Name == ServerListScreen.RMS_svselect) && !(fileInfo.Name == RMS_acc) && !(fileInfo.Name == RMS_pass) && !(fileInfo.Name == RMS_check))
			{
				fileInfo.Delete();
			}
		}
	}

	public static void DeleteStorage(string path)
	{
		try
		{
			File.Delete(GetiPhoneDocumentsPath() + "/" + path);
		}
		catch (Exception)
		{
		}
	}

	public static string ByteArrayToString(byte[] ba)
	{
		string text = BitConverter.ToString(ba);
		return text.Replace("-", string.Empty);
	}

	public static byte[] StringToByteArray(string hex)
	{
		int length = hex.Length;
		byte[] array = new byte[length / 2];
		for (int i = 0; i < length; i += 2)
		{
			array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return array;
	}

	public static void deleteRecord(string name)
	{
		try
		{
			PlayerPrefs.DeleteKey(name);
		}
		catch (Exception ex)
		{
			Cout.println("loi xoa RMS --------------------------" + ex.ToString());
		}
	}

	public static void clearRMS()
	{
		deleteRecord("data");
		deleteRecord("dataVersion");
		deleteRecord("map");
		deleteRecord("mapVersion");
		deleteRecord("skill");
		deleteRecord("killVersion");
		deleteRecord("item");
		deleteRecord("itemVersion");
	}

	public static void saveIP(string strID)
	{
		saveRMSString("NRIPlink", strID);
	}

	public static string loadIP()
	{
		string text = loadRMSString("NRIPlink");
		if (text == null)
		{
			return null;
		}
		return text;
	}
}
