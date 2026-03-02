using System;
using System.Diagnostics;
using SGNFW.Common;

// Token: 0x020000F8 RID: 248
public class PrjLog : Verbose<PrjLog>
{
	// Token: 0x06000BE0 RID: 3040 RVA: 0x0004617C File Offset: 0x0004437C
	[Conditional("SGNFW_DEBUG_LOG")]
	public static void OpenDebugErrorWindow(string str)
	{
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("！！！\u3000エラー\u3000！！！"), str, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		StackFrame stackFrame = new StackFrame(1);
		string name = stackFrame.GetMethod().Name;
		string fullName = stackFrame.GetMethod().ReflectedType.FullName;
		string text = str.Replace("\n", " ");
		Verbose<PrjLog>.LogError(string.Concat(new string[] { fullName, ".", name, "\n", text }), null);
	}
}
