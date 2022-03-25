﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment;

public record struct PingResult(int ExitCode, string? StdOutput);

public class PingProcess
{
    // not working on Windows
    //Limiting the ping command ping -c 2 8.8.8.8
    //Format - ping Address -c #times we want to ping the IP

    private ProcessStartInfo StartInfo { get; } = new("ping");

    public PingResult Run(string hostNameOrAddress)
    {
        StartInfo.Arguments = hostNameOrAddress;
        StringBuilder? stringBuilder = null;
        void UpdateStdOutput(string? line) =>
            (stringBuilder ??= new StringBuilder()).AppendLine(line);
        Process process = RunProcessInternal(StartInfo, UpdateStdOutput, default, default);
        return new PingResult(process.ExitCode, stringBuilder?.ToString());
    }

    public Task<PingResult> RunTaskAsync(string hostNameOrAddress)
    {
        return Task.Run(
            () => Run(hostNameOrAddress)
            );
    }

    async public Task<PingResult> RunAsync(
        string hostNameOrAddress, CancellationToken cancellationToken = default)
    {
        StartInfo.Arguments = hostNameOrAddress;
        StringBuilder? stringBuilder = null;
        void UpdateStdOutput(string? line) =>
            (stringBuilder ??= new StringBuilder()).AppendLine(line);

        Task<PingResult> task = Task.Run(
            () =>
            {
                Process process = RunProcessInternal(StartInfo, UpdateStdOutput, default, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return new PingResult(process.ExitCode, stringBuilder?.ToString());
            }, cancellationToken);

        return await task;
    }

    async public Task<PingResult> RunAsync(IEnumerable<string> hostNameOrAddresses,
        CancellationToken cancellationToken = default)
    {
        StringBuilder? stringBuilder = new();
        ParallelQuery<Task<PingResult>>? all = hostNameOrAddresses.AsParallel().Select(async item =>
        {
            Task<PingResult> task = Task.Run(() => Run(item), cancellationToken);

            await task.WaitAsync(default(CancellationToken));
            return task.Result;
        });

        await Task.WhenAll(all);
        int total = all.Aggregate(0, (total, item)
            => total + item.Result.ExitCode); // 0
        // update stringBuilder: "The order of the items in the stdOutput is irrelevent and expected to be intermingled"
        // thank you for doing aggregate above as an example/remider
        stringBuilder.Append(all.Aggregate("", (compiledString, currentString)
            => compiledString.Trim() + currentString.Result.StdOutput).Trim());

        // int? lineCount = stringBuilder?.ToString().Split(Environment.NewLine).Length; // format stringBuilder to 40 lines
        return new PingResult(total, stringBuilder?.ToString());
    }

    //not passing tests idk if it's me or my OS
    async public Task<PingResult> RunLongRunningAsync(string hostNameOrAddress, CancellationToken cancellationToken = default)
    {
        StartInfo.Arguments = hostNameOrAddress;
        StringBuilder? stringBuilder = null;
        void UpdateStdOutput(string? line) =>
            (stringBuilder ??= new StringBuilder()).AppendLine(line);
        
        Task<PingResult> task = Task.Factory.StartNew(() =>
        {
            Process process = RunProcessInternal(StartInfo, UpdateStdOutput, default, cancellationToken); 
            return new PingResult(process.ExitCode, stringBuilder?.ToString());
        }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        
        return await task;
    }

    private Process RunProcessInternal(
        ProcessStartInfo startInfo,
        Action<string?>? progressOutput,
        Action<string?>? progressError,
        CancellationToken token)
    {
        var process = new Process
        {
            StartInfo = UpdateProcessStartInfo(startInfo)
        };
        return RunProcessInternal(process, progressOutput, progressError, token);
    }

    private Process RunProcessInternal(
        Process process,
        Action<string?>? progressOutput,
        Action<string?>? progressError,
        CancellationToken token)
    {
        process.EnableRaisingEvents = true;
        process.OutputDataReceived += OutputHandler;
        process.ErrorDataReceived += ErrorHandler;

        try
        {
            if (!process.Start())
            {
                return process;
            }

            token.Register(obj =>
            {
                if (obj is Process p && !p.HasExited)
                {
                    try
                    {
                        p.Kill();
                    }
                    catch (Win32Exception ex)
                    {
                        throw new InvalidOperationException($"Error cancelling process{Environment.NewLine}{ex}");
                    }
                }
            }, process);


            if (process.StartInfo.RedirectStandardOutput)
            {
                process.BeginOutputReadLine();
            }
            if (process.StartInfo.RedirectStandardError)
            {
                process.BeginErrorReadLine();
            }

            if (process.HasExited)
            {
                return process;
            }
            process.WaitForExit();
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Error running '{process.StartInfo.FileName} {process.StartInfo.Arguments}'{Environment.NewLine}{e}");
        }
        finally
        {
            if (process.StartInfo.RedirectStandardError)
            {
                process.CancelErrorRead();
            }
            if (process.StartInfo.RedirectStandardOutput)
            {
                process.CancelOutputRead();
            }
            process.OutputDataReceived -= OutputHandler;
            process.ErrorDataReceived -= ErrorHandler;

            if (!process.HasExited)
            {
                process.Kill();
            }

        }
        return process;

        void OutputHandler(object s, DataReceivedEventArgs e)
        {
            progressOutput?.Invoke(e.Data);
        }

        void ErrorHandler(object s, DataReceivedEventArgs e)
        {
            progressError?.Invoke(e.Data);
        }
    }

    private static ProcessStartInfo UpdateProcessStartInfo(ProcessStartInfo startInfo)
    {
        startInfo.CreateNoWindow = true;
        startInfo.RedirectStandardError = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false;
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;

        return startInfo;
    }
}