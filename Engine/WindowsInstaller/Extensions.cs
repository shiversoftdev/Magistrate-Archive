using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsInstaller
{
    internal static class Extensions
    {
        #region Process Async from https://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why/39872058#39872058

        public static async Task<int> StartProcess(
    string filename,
    string arguments,
    string workingDirectory = null,
    int? timeout = null,
    TextWriter outputTextWriter = null,
    TextWriter errorTextWriter = null)
        {
            using (var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    Arguments = arguments,
                    FileName = filename,
                    RedirectStandardOutput = outputTextWriter != null,
                    RedirectStandardError = errorTextWriter != null,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory
                }
            })
            {
                process.Start();
                var cancellationTokenSource = timeout.HasValue ?
                    new CancellationTokenSource(timeout.Value) :
                    new CancellationTokenSource();

                var tasks = new List<Task>(3) { process.WaitForExitAsync(cancellationTokenSource.Token) };
                if (outputTextWriter != null)
                {
                    tasks.Add(ReadAsync(
                        x =>
                        {
                            process.OutputDataReceived += x;
                            process.BeginOutputReadLine();
                        },
                        x => process.OutputDataReceived -= x,
                        outputTextWriter,
                        cancellationTokenSource.Token));
                }

                if (errorTextWriter != null)
                {
                    tasks.Add(ReadAsync(
                        x =>
                        {
                            process.ErrorDataReceived += x;
                            process.BeginErrorReadLine();
                        },
                        x => process.ErrorDataReceived -= x,
                        errorTextWriter,
                        cancellationTokenSource.Token));
                }

                await Task.WhenAll(tasks);
                return process.ExitCode;
            }
        }

        /// <summary>
        /// Waits asynchronously for the process to exit.
        /// </summary>
        /// <param name="process">The process to wait for cancellation.</param>
        /// <param name="cancellationToken">A cancellation token. If invoked, the task will return
        /// immediately as cancelled.</param>
        /// <returns>A Task representing waiting for the process to end.</returns>
        public static Task WaitForExitAsync(
            this Process process,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            process.EnableRaisingEvents = true;

            var taskCompletionSource = new TaskCompletionSource<object>();

            EventHandler handler = null;
            handler = (sender, args) =>
            {
                process.Exited -= handler;
                taskCompletionSource.TrySetResult(null);
            };
            process.Exited += handler;

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(
                    () =>
                    {
                        process.Exited -= handler;
                        taskCompletionSource.TrySetCanceled();
                    });
            }

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Reads the data from the specified data recieved event and writes it to the
        /// <paramref name="textWriter"/>.
        /// </summary>
        /// <param name="addHandler">Adds the event handler.</param>
        /// <param name="removeHandler">Removes the event handler.</param>
        /// <param name="textWriter">The text writer.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task ReadAsync(
            this Action<DataReceivedEventHandler> addHandler,
            Action<DataReceivedEventHandler> removeHandler,
            TextWriter textWriter,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var taskCompletionSource = new TaskCompletionSource<object>();

            DataReceivedEventHandler handler = null;
            handler = new DataReceivedEventHandler(
                (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        removeHandler(handler);
                        taskCompletionSource.TrySetResult(null);
                    }
                    else
                    {
                        textWriter.WriteLine(e.Data);
                    }
                });

            addHandler(handler);

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(
                    () =>
                    {
                        removeHandler(handler);
                        taskCompletionSource.TrySetCanceled();
                    });
            }

            return taskCompletionSource.Task;
        }

        #endregion

        /// <summary>
        /// Safely set bytes in a list
        /// </summary>
        /// <param name="RawData">The list to set bytes in</param>
        /// <param name="index">The index of the bytes</param>
        /// <param name="bytes">The byte array to write</param>
        public static void SetBytes(this List<byte> RawData, int index, byte[] bytes)
        {
            while (index + bytes.Length >= RawData.Count)
                RawData.Add(0x0);
            for (int i = 0; i < bytes.Length; i++)
            {
                RawData[i + index] = bytes[i];
            }
            return;
        }

        /// <summary>
        /// Safely get bytes from a list of bytes
        /// </summary>
        /// <param name="RawData">The list to read from</param>
        /// <param name="index">The index to read at</param>
        /// <param name="count">The amount of bytes to read</param>
        /// <returns></returns>
        public static byte[] GetBytes(this List<byte> RawData, int index, int count)
        {
            while (index + count > RawData.Count)
                RawData.Add(0x0);
            byte[] bytes = new byte[count];
            for (int i = 0; i < count; i++)
            {
                bytes[i] = RawData[i + index];
            }
            return bytes;
        }

        /// <summary>
        /// Read a null terminated string from a byte list
        /// </summary>
        /// <param name="RawData">The raw data to parse from</param>
        /// <param name="index">The index of the string. Gets modified to be past the null character</param>
        /// <returns></returns>
        public static string ReadString(this List<byte> RawData, ref int index)
        {
            string result = "";
            while (index < RawData.Count && RawData[index] != 0x0)
            {
                result += (char)RawData[index];
                index++;
            }
            index++;
            return result;
        }

    }
}
