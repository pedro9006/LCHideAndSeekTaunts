using System;
using System.IO;
using System.Reflection;
using BepInEx;

[BepInPlugin("com.SpLinTeR.Taunt_FATCOOP", "Taunt_FATCOOP", "1.0.3")]
public class FolderMoverMod : BaseUnityPlugin
{
    private void Awake()
    {
        CopyFolder();
    }

    public void CopyFolder()
    {
        // Obtenir le répertoire de la DLL (où le mod est situé)
        string dllDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        // Remonter au dossier parent de la DLL, soit "plugins"
        string pluginsDirectory = Directory.GetParent(dllDirectory)?.FullName;

        if (pluginsDirectory == null)
        {
            Logger.LogError("There aint a folder for the plugins dude.");
            return;
        }

        // Chemins des dossiers SpLinTeRTV-Hide_And_Seek_SoundsFR/Sounds et Gogozooom-Hide_And_Seek/Sounds dans le répertoire "plugins"
        string soundsDirectory = Path.Combine(pluginsDirectory, "FatCoopHideAndSeekTaunts", "Sounds");
        string targetDirectory = Path.Combine(pluginsDirectory, "Gogozooom-Hide_And_Seek", "Sounds");

        // Vérifier que le dossier "Sounds" existe dans SpLinTeRTV-Hide_And_Seek_SoundsFR
        if (!Directory.Exists(soundsDirectory))
        {
            Logger.LogError($"Sounds sure exist in FatCoopHideAndSeekTaunts : {soundsDirectory}");
            return;
        }

        try
        {
            // Si le dossier cible "Sounds" dans Gogozooom-Hide_And_Seek existe, on le supprime avec son contenu
            if (Directory.Exists(targetDirectory))
            {
                Directory.Delete(targetDirectory, true); // true pour supprimer le contenu aussi
                Logger.LogInfo($"Sounds no longer exist in Gogozooom-Hide_And_Seek : {targetDirectory}");
            }

            // Copier le dossier "Sounds" vers Gogozooom-Hide_And_Seek
            CopyDirectory(soundsDirectory, targetDirectory);
            Logger.LogInfo($"Le dossier {soundsDirectory} a été copié vers {targetDirectory}.");
        }
        catch (Exception ex)
        {
            Logger.LogError($"yikes an error happened : {ex.Message}");
        }
    }

    // Méthode pour copier un dossier et son contenu récursivement
    public static void CopyDirectory(string sourceDir, string destDir)
    {
        // Créer le répertoire de destination s'il n'existe pas
        Directory.CreateDirectory(destDir);

        // Copier les fichiers dans le répertoire de destination
        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string destFile = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFile, true); // true pour écraser les fichiers existants
        }

        // Copier les sous-dossiers récursivement
        foreach (string subDir in Directory.GetDirectories(sourceDir))
        {
            string destSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
            CopyDirectory(subDir, destSubDir); // Appel récursif pour copier les sous-dossiers
        }
    }
}
