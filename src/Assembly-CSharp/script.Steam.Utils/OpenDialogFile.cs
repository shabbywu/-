using System;
using System.Runtime.InteropServices;

namespace script.Steam.Utils;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenDialogFile
{
	public int structSize;

	public IntPtr dlgOwner = IntPtr.Zero;

	public IntPtr instance = IntPtr.Zero;

	public string filter;

	public string customFilter;

	public int maxCustFilter;

	public int filterIndex;

	public string file;

	public int maxFile;

	public string fileTitle;

	public int maxFileTitle;

	public string initialDir;

	public string title;

	public int flags;

	public short fileOffset;

	public short fileExtension;

	public string defExt;

	public IntPtr custData = IntPtr.Zero;

	public IntPtr hook = IntPtr.Zero;

	public string templateName;

	public IntPtr reservedPtr = IntPtr.Zero;

	public int reservedInt;

	public int flagsEx;
}
