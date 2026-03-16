using System;
using System.Diagnostics;
using SGNFW.Common;

public class PrjLog : Verbose<PrjLog>
{
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
