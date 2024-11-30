using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class PythonServerHostedService: IHostedService {
    private Process _pythonProcess;

    public Task StartAsync(CancellationToken cancellationToken) {
        Console.WriteLine("Starting Python Server...");
        try {
			string currentDirectory = Directory.GetCurrentDirectory();

            // Move up one folder to the parent directory
            string parentDirectory = Directory.GetParent(currentDirectory)?.FullName;

			// Define the path to the virtual environment folder
			string virtualEnvPath = Path.Combine(parentDirectory, "AI_LLM_Analysis", "venv", "Scripts");

			// Set the working directory to the AI_LLM_Analysis folder
			string workingDirectory = Path.Combine(parentDirectory, "AI_LLM_Analysis");

			// Use the activation script for Windows
			string venvActivateCommand = Path.Combine(virtualEnvPath, "activate.bat");

			// Define the FastAPI command to run from the AI_LLM_Analysis folder
			string fastapiCommand = "uvicorn main:app --reload";

             // Define the Python commands
            string createVenvCommand = $"python -m venv venv";
            string installRequirementsCommand = $"pip install -r requirements.txt";
            string runServer = "fastapi dev main.py";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/K {"cd"} && {createVenvCommand} && {venvActivateCommand} && {installRequirementsCommand} && {runServer} && {fastapiCommand}",  //K to allow command prompt to stay open
                UseShellExecute = false,                 //used to make python server a subprocess of whole program
                CreateNoWindow = false,                  //show command prompt
                WorkingDirectory = workingDirectory
            };


            _pythonProcess = Process.Start(psi);

            if (_pythonProcess != null)
            {
                Console.WriteLine("Fast Api server started successfully with virtual environment");
            }

        } catch (Exception ex)
        {
            Console.WriteLine("Failed to start FastAPI server: " + ex.Message);
        }

        return Task.CompletedTask;
    }

        public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_pythonProcess != null && !_pythonProcess.HasExited)
        {
            _pythonProcess.Kill();
            Console.WriteLine("FastAPI server stopped.");
        }
        return Task.CompletedTask;
    }


}