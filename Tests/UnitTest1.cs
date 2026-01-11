using NUnit.Framework;
using System.Text;
using System.Threading.Tasks;
using HTTPRequestsLibrary;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System;
using System.IO;

namespace APITests
{
    public class Tests
    {
        private SendRequest client;
        private Process apiProcess;

        [OneTimeSetUp]
        public async Task Setup()
        {
            // Resolve absolute path to GestioneDb project
            var workDir = TestContext.CurrentContext.WorkDirectory;
            var projectPath = Path.GetFullPath(Path.Combine(workDir, "..", "..", "..", "..", "GestioneDb", "GestioneDb.csproj"));
            var runArgs = projectPath != null && File.Exists(projectPath)
                ? $"run --project \"{projectPath}\" --urls http://localhost:5211"
                : "run --project ../GestioneDb --urls http://localhost:5211";

            var start = new ProcessStartInfo("dotnet", runArgs)
            {
                WorkingDirectory = workDir,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var stdout = new StringBuilder();
            var stderr = new StringBuilder();

            apiProcess = Process.Start(start);
            if (apiProcess == null)
                throw new Exception("Impossibile avviare il processo 'dotnet'. Verifica che 'dotnet' sia nel PATH.");

            apiProcess.OutputDataReceived += (s, e) => { if (e.Data != null) stdout.AppendLine(e.Data); };
            apiProcess.BeginOutputReadLine();
            apiProcess.ErrorDataReceived += (s, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };
            apiProcess.BeginErrorReadLine();

            // wait for the port to be open (timeout 60s)
            var sw = Stopwatch.StartNew();
            var portOpen = false;
            var timeout = TimeSpan.FromSeconds(60);
            while (sw.Elapsed < timeout)
            {
                // If the process exited early capture logs and fail
                if (apiProcess.HasExited)
                {
                    var outText = stdout.ToString();
                    var errText = stderr.ToString();
                    throw new Exception($"API process exited prematurely. ExitCode={apiProcess.ExitCode}. Stdout:\n{outText}\nStderr:\n{errText}");
                }

                try
                {
                    using (var tcp = new TcpClient())
                    {
                        var t = tcp.ConnectAsync("127.0.0.1", 5211);
                        if (t.Wait(500) && tcp.Connected)
                        {
                            portOpen = true;
                            break;
                        }
                    }
                }
                catch { }

                Thread.Sleep(200);
            }

            if (!portOpen)
            {
                try { if (apiProcess != null && !apiProcess.HasExited) apiProcess.Kill(); } catch { }
                var outText = apiProcess != null ? stdout.ToString() : string.Empty;
                var errText = apiProcess != null ? stderr.ToString() : string.Empty;
                throw new Exception($"API non partita entro timeout. Stdout:\n{outText}\nStderr:\n{errText}");
            }

            client = new SendRequest();

            // Ensure previous test data is removed to avoid BadRequest on duplicate App
            try
            {
                await client.DeletePasswordByApp("TestApp");
            }
            catch { }
            try
            {
                await client.DeletePasswordByApp("TestApp2");
            }
            catch { }
        }

        [Category("API")]
        [Order(1)]
        [TestCase("TestApp", "ok")]
        [TestCase("TestApp2", "ok2")]
        public async Task AddPasswordTest(string app, string StringPassword)
        {
            var NewPassword = new Password
            {
                App = app,
                EncryptedPassword = Encoding.UTF8.GetBytes(StringPassword)
            };

            var Response = await client.AddPassword(NewPassword);

            if (!Response.IsSuccessStatusCode)
            {
                var body = await Response.Content.ReadAsStringAsync();
                Assert.Fail($"AddPassword returned {(int)Response.StatusCode} {Response.ReasonPhrase}. Body: {body}");
            }

            Assert.That(Response.IsSuccessStatusCode);
        }

        [Test]
        [Category("API")]
        [Order(2)]
        [Repeat(5)]
        public async Task GetListOfPasswordsTest()
        {
            var Response = await client.GetPasswords();
            Assert.That(Response, Is.Not.Null);
            Assert.That(Response.Count, Is.GreaterThan(0));
        }

        [Category("API")]
        [Order(3)]
        [TestCase("TestApp")]
        [TestCase("TestApp2")]
        public async Task GetPasswordByAppTest(string app)
        {
            var Response = await client.GetPasswordByApp(app);
            Assert.That(Response, Is.Not.Null);
        }

        [Category("API")]
        [Order(4)]
        [TestCase("TestApp", "si")]
        [TestCase("TestApp2", "si2")]
        public async Task UpdatePasswordTest(string app, string StringPassword)
        {
            var ResponseGet1 = await client.GetPasswordByApp(app);

            var UpdatedPassword = new Password
            {
                App = app,
                EncryptedPassword = Encoding.UTF8.GetBytes(StringPassword)
            };

            var ResponsePut = await client.UpdatePasswordByApp(UpdatedPassword);

            var ResponseGet2 = await client.GetPasswordByApp(app);

            Assert.That(ResponsePut.IsSuccessStatusCode);
            Assert.That(ResponseGet1, Is.Not.EqualTo(ResponseGet2));
        }

        [Category("API")]
        [Order(5)]
        [TestCase("TestApp")]
        [TestCase("TestApp2")]
        public async Task DeletePasswordByApp(string app)
        {
            var Response = await client.DeletePasswordByApp(app);
            Assert.That(Response.IsSuccessStatusCode);

            var Check = await client.GetPasswordByApp(app);
            Assert.That(Check, Is.Null);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            try { client?.Dispose(); } catch { }

            try
            {
                if (apiProcess != null && !apiProcess.HasExited)
                {
                    apiProcess.Kill();
                    apiProcess.WaitForExit(2000);
                }
            }
            catch { }
            finally { apiProcess?.Dispose(); }
        }
    }
}