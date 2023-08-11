namespace ICSharpCode.SharpZipLib.Core;

public class DirectoryEventArgs : ScanEventArgs
{
	private readonly bool hasMatchingFiles_;

	public bool HasMatchingFiles => hasMatchingFiles_;

	public DirectoryEventArgs(string name, bool hasMatchingFiles)
		: base(name)
	{
		hasMatchingFiles_ = hasMatchingFiles;
	}
}
