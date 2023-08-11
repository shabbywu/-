using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MoonSharp.Interpreter.Platforms;

public class StandardPlatformAccessor : PlatformAccessorBase
{
	public static FileAccess ParseFileAccess(string mode)
	{
		mode = mode.Replace("b", "");
		switch (mode)
		{
		case "r":
			return FileAccess.Read;
		case "r+":
			return FileAccess.ReadWrite;
		case "w":
			return FileAccess.Write;
		default:
			_ = mode == "w+";
			return FileAccess.ReadWrite;
		}
	}

	public static FileMode ParseFileMode(string mode)
	{
		mode = mode.Replace("b", "");
		return mode switch
		{
			"r" => FileMode.Open, 
			"r+" => FileMode.OpenOrCreate, 
			"w" => FileMode.Create, 
			"w+" => FileMode.Truncate, 
			_ => FileMode.Append, 
		};
	}

	public override Stream IO_OpenFile(Script script, string filename, Encoding encoding, string mode)
	{
		return new FileStream(filename, ParseFileMode(mode), ParseFileAccess(mode), FileShare.ReadWrite | FileShare.Delete);
	}

	public override string GetEnvironmentVariable(string envvarname)
	{
		return Environment.GetEnvironmentVariable(envvarname);
	}

	public override Stream IO_GetStandardStream(StandardFileType type)
	{
		return type switch
		{
			StandardFileType.StdIn => Console.OpenStandardInput(), 
			StandardFileType.StdOut => Console.OpenStandardOutput(), 
			StandardFileType.StdErr => Console.OpenStandardError(), 
			_ => throw new ArgumentException("type"), 
		};
	}

	public override void DefaultPrint(string content)
	{
		Console.WriteLine(content);
	}

	public override string IO_OS_GetTempFilename()
	{
		return Path.GetTempFileName();
	}

	public override void OS_ExitFast(int exitCode)
	{
		Environment.Exit(exitCode);
	}

	public override bool OS_FileExists(string file)
	{
		return File.Exists(file);
	}

	public override void OS_FileDelete(string file)
	{
		File.Delete(file);
	}

	public override void OS_FileMove(string src, string dst)
	{
		File.Move(src, dst);
	}

	public override int OS_Execute(string cmdline)
	{
		Process? process = Process.Start(new ProcessStartInfo("cmd.exe", $"/C {cmdline}")
		{
			ErrorDialog = false
		});
		process.WaitForExit();
		return process.ExitCode;
	}

	public override CoreModules FilterSupportedCoreModules(CoreModules module)
	{
		return module;
	}

	public override string GetPlatformNamePrefix()
	{
		return "std";
	}
}
