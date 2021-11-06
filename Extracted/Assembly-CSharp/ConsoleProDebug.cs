using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200007F RID: 127
public static class ConsoleProDebug
{
	// Token: 0x060003DF RID: 991 RVA: 0x0002219C File Offset: 0x0002039C
	public static void Clear()
	{
		if (ConsoleProDebug.ConsoleClearMethod != null)
		{
			ConsoleProDebug.ConsoleClearMethod.Invoke(null, null);
		}
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x000221B8 File Offset: 0x000203B8
	public static void LogToFilter(string inLog, string inFilterName)
	{
		Debug.Log(inLog + "\nCPAPI:{\"cmd\":\"Filter\" \"name\":\"" + inFilterName + "\"}");
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x000221D0 File Offset: 0x000203D0
	public static void Watch(string inName, string inValue)
	{
		Debug.Log(string.Concat(new string[]
		{
			inName,
			" : ",
			inValue,
			"\nCPAPI:{\"cmd\":\"Watch\" \"name\":\"",
			inName,
			"\"}"
		}));
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060003E2 RID: 994 RVA: 0x00022214 File Offset: 0x00020414
	private static MethodInfo ConsoleClearMethod
	{
		get
		{
			if (ConsoleProDebug._consoleClearMethod == null || !ConsoleProDebug._checkedConsoleClearMethod)
			{
				ConsoleProDebug._checkedConsoleClearMethod = true;
				if (ConsoleProDebug.ConsoleWindowType == null)
				{
					return null;
				}
				ConsoleProDebug._consoleClearMethod = ConsoleProDebug.ConsoleWindowType.GetMethod("ClearEntries", BindingFlags.Static | BindingFlags.Public);
			}
			return ConsoleProDebug._consoleClearMethod;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060003E3 RID: 995 RVA: 0x00022264 File Offset: 0x00020464
	private static Type ConsoleWindowType
	{
		get
		{
			if (ConsoleProDebug._consoleWindowType == null || !ConsoleProDebug._checkedConsoleWindowType)
			{
				ConsoleProDebug._checkedConsoleWindowType = true;
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					Type[] types = assemblies[i].GetTypes();
					for (int j = 0; j < types.Length; j++)
					{
						if (types[j].Name == "ConsolePro3Window")
						{
							ConsoleProDebug._consoleWindowType = types[j];
						}
					}
				}
			}
			return ConsoleProDebug._consoleWindowType;
		}
	}

	// Token: 0x0400046E RID: 1134
	private static bool _checkedConsoleClearMethod;

	// Token: 0x0400046F RID: 1135
	private static MethodInfo _consoleClearMethod;

	// Token: 0x04000470 RID: 1136
	private static bool _checkedConsoleWindowType;

	// Token: 0x04000471 RID: 1137
	private static Type _consoleWindowType;
}
