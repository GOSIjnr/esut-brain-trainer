using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class Logger
{
	private static readonly List<string> _bufferedLogEntries = [];
	private static bool _batchModeEnabled = false;
	private static int _maxBufferedLogCount = 50;

	private static LogLevel _minLogLevel = LogLevel.Info;
	private static LogLevel _errorThreshold = LogLevel.Error;
	private static LogLevel _fatalThreshold = LogLevel.Fatal;

	private const string LogSeparator = "==========================================================================";

	public static event Action OnFatalError;

	public enum LogLevel
	{
		Debug,
		Info,
		Warning,
		Error,
		Fatal,
	}

	public static bool IsBatchModeEnabled
	{
		get => _batchModeEnabled;
		set
		{
			bool previousState = _batchModeEnabled;
			_batchModeEnabled = value;

			if (previousState && !value)
			{
				FlushLogBuffer();
			}

			NotifyBatchModeChange(value);
		}
	}

	public static int MaxBatchSize
	{
		get => _maxBufferedLogCount;
		set
		{
			_maxBufferedLogCount = Mathf.Clamp(value, 2, int.MaxValue);
			Log("MaxBatchSize has been set to " + value, LogLevel.Debug);
		}
	}

	public static void SetLogLevelFilter(LogLevel value) => UpdateLogLevel(ref _minLogLevel, value, "CurrentLogLevel");
	public static void SetErrorLogLevelFilter(LogLevel value) => UpdateLogLevel(ref _errorThreshold, value, "ErrorLogLevel");
	public static void SetFatalLogLevelFilter(LogLevel value) => UpdateLogLevel(ref _fatalThreshold, value, "FatalLogLevel");

	private static void UpdateLogLevel(ref LogLevel targetLogLevel, LogLevel newLevel, string propertyName)
	{
		targetLogLevel = newLevel;
		Log(propertyName + " has been set to " + newLevel.ToString().ToUpper(), LogLevel.Debug);
	}

	private static void NotifyBatchModeChange(bool isActivated)
	{
		string status = isActivated ? "activated" : "deactivated";
		Log("BatchMode has been " + status, LogLevel.Debug);
	}

	public static void Log(string message = "", LogLevel level = LogLevel.Info, [CallerFilePath] string callerFile = "")
	{
		if (level < _minLogLevel) return;

		string timestamp = GetPreciseTimestamp();
		string callerScriptName = callerFile.GetFile().GetBaseName();
		string plainLogEntry = $"[{timestamp}] [{level.ToString().ToUpper()}] [{callerScriptName}] {message}";
		string logEntry = ColorizeLogEntry(plainLogEntry, level);

		bool needFlush = false;
		bool printedDirectly = false;

		if (_batchModeEnabled)
		{
			_bufferedLogEntries.Add(logEntry);

			if (_bufferedLogEntries.Count >= _maxBufferedLogCount)
			{
				needFlush = true;
			}
		}
		else
		{
			GD.PrintRich(logEntry);
			printedDirectly = true;
		}

		if (needFlush)
		{
			FlushLogBuffer();
			printedDirectly = true;
		}

		if (level >= _errorThreshold)
		{
			FlushLogBuffer();
			printedDirectly = true;
			ProcessError(message);
		}

		if (printedDirectly)
		{
			GD.PrintRich(LogSeparator);
		}

		if (level >= _fatalThreshold)
		{
			ProcessFatal();
		}
	}

	private static void FlushLogBuffer()
	{
		List<string> logsToFlush;

		if (_bufferedLogEntries.Count == 0) return;

		logsToFlush = [.. _bufferedLogEntries];
		_bufferedLogEntries.Clear();

		string combinedLogs = string.Join("\n", logsToFlush);
		GD.PrintRich(combinedLogs);
	}

	private static string ColorizeLogEntry(string logEntry, LogLevel level)
	{
		string color = level switch
		{
			LogLevel.Debug => "#B0B0B0", // Gray
			LogLevel.Info => "#FFFFFF",  // White
			LogLevel.Warning => "#FFFF00", // Yellow
			LogLevel.Error => "#FF0000", // Red
			LogLevel.Fatal => "#FF007F", // Pinkish Red
			_ => "#FFFFFF", // White
		};

		return $"[color={color}]{logEntry}[/color]";
	}

	private static void ProcessError(string message)
	{
		if (OS.HasFeature("editor"))
		{
			GD.PushError(message);
			return;
		}

		GD.PrintErr(System.Environment.StackTrace);
	}

	private static void ProcessFatal() => OnFatalError?.Invoke();

	private static string GetPreciseTimestamp()
	{
		string dateTime = Time.GetDatetimeStringFromSystem(true, true);
		ulong milliseconds = Time.GetTicksMsec() % 1000;
		return $"{dateTime}.{milliseconds:D3}";
	}
}
