using System;
using System.IO;
using System.Reflection;
using BepInEx;

[BepInPlugin("com.SpLinTeR.TauntFR", "TauntFR", "1.0.0")]
public class FolderMoverMod : BaseUnityPlugin
{
    private void Awake()
    {
        MergeFolderToParent("Gogozooom-Hide_And_Seek");
    }

    public void MergeFolderToParent(string folderName)
    {
        // Obtenir le répertoire de la DLL (où le mod est situé)
        string dllDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        // Chemin du dossier à fusionner dans le répertoire de la DLL
        string sourcePath = Path.Combine(dllDirectory, folderName);

        // Chemin du dossier parent du répertoire de la DLL
        string parentDirectory = Directory.GetParent(dllDirectory)?.FullName;

        if (parentDirectory == null)
        {
            Logger.LogError("Impossible de trouver le dossier parent.");
            return;
        }

        // Chemin de destination dans le dossier parent de la DLL
        string destinationPath = Path.Combine(parentDirectory, folderName);

        try
        {
            // Fusionner le contenu des dossiers
            MergeDirectories(sourcePath, destinationPath);
            Logger.LogInfo($"Dossier fusionné de {sourcePath} vers {destinationPath}");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Erreur lors de la fusion des dossiers : {ex.Message}");
        }
    }

    private void MergeDirectories(string sourceDir, string targetDir)
    {
        // Crée le dossier de destination s'il n'existe pas
        Directory.CreateDirectory(targetDir);

        // Copier les fichiers
        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string fileName = Path.GetFileName(file);
            string destFile = Path.Combine(targetDir, fileName);

            // Copier le fichier si le fichier de destination n'existe pas ou écraser s'il est plus récent
            if (!File.Exists(destFile) || File.GetLastWriteTime(file) > File.GetLastWriteTime(destFile))
            {
                File.Copy(file, destFile, true);
            }
        }

        // Copier les sous-dossiers de façon récursive
        foreach (string directory in Directory.GetDirectories(sourceDir))
        {
            string dirName = Path.GetFileName(directory);
            string destDir = Path.Combine(targetDir, dirName);

            // Appel récursif pour fusionner les sous-dossiers
            MergeDirectories(directory, destDir);
        }
    }
}