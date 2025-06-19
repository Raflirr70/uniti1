using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Diagnostics;

public class compileCpp : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI outputText;

    public void OnSubmit()
    {
        // Dapatkan path Desktop user
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
        // Folder baru bernama "testcpp" di Desktop
        string tempFolder = Path.Combine(desktopPath, "testcpp");

        // Buat folder jika belum ada
        if (!Directory.Exists(tempFolder))
        {
            Directory.CreateDirectory(tempFolder);
            UnityEngine.Debug.Log("Folder dibuat di: " + tempFolder);
        }

        string cppPath = Path.Combine(tempFolder, "TempCode.cpp");
        string exePath = Path.Combine(tempFolder, "TempOutput.exe");

        string cppCode = inputField.text;

        File.WriteAllText(cppPath, cppCode);
        UnityEngine.Debug.Log("C++ code saved at: " + cppPath);
        UnityEngine.Debug.Log("Isi file cpp:\n" + File.ReadAllText(cppPath));

        Process compile = new Process();
        compile.StartInfo.FileName = @"C:\MinGW\bin\g++.exe";
        compile.StartInfo.Arguments = $"\"{cppPath}\" -o \"{exePath}\"";
        compile.StartInfo.CreateNoWindow = true;
        compile.StartInfo.UseShellExecute = false;
        compile.StartInfo.RedirectStandardOutput = true;
        compile.StartInfo.RedirectStandardError = true;

        compile.Start();
        string compileOutput = compile.StandardOutput.ReadToEnd();
        string compileError = compile.StandardError.ReadToEnd();
        compile.WaitForExit();

        UnityEngine.Debug.Log("Compile ExitCode: " + compile.ExitCode);
        UnityEngine.Debug.Log("Compile stdout:\n" + compileOutput);
        UnityEngine.Debug.LogError("Compile stderr:\n" + compileError);

        if (compile.ExitCode != 0)
        {
            outputText.text = "❌ Kompilasi Gagal:\n" + compileError;
            return;
        }

        UnityEngine.Debug.Log("Kompilasi berhasil: " + exePath);

        Process run = new Process();
        run.StartInfo.FileName = exePath;
        run.StartInfo.CreateNoWindow = true;
        run.StartInfo.UseShellExecute = false;
        run.StartInfo.RedirectStandardOutput = true;
        run.StartInfo.WorkingDirectory = tempFolder;

        run.Start();
        string result = run.StandardOutput.ReadToEnd();
        run.WaitForExit();

        UnityEngine.Debug.Log("Hasil Output:\n" + result);
        outputText.text = "Hasil Output:\n" + result;
    }
}
