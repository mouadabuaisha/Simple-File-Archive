using System;
using System.IO;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading;

namespace FileArchive
{
    public class Watcher
    {
        public static void Main()
        {
            var flag = false;
            var watcherPath = "";
            while (!flag)
            {
                Console.WriteLine("Please Enter Your target folder path");
                var targetFolder = Console.ReadLine();
                watcherPath = targetFolder;

                if (!System.IO.Directory.Exists(watcherPath))
                {
                    Console.WriteLine("Your entered path is Incorrect");
                }
                else
                {
                    flag = true;
                }
            }

            Run(watcherPath);
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void Run(string watcherPath)
        {
            try
            {
                // Create a new FileSystemWatcher and set its properties.
                using (FileSystemWatcher watcher = new FileSystemWatcher())
                {

                    watcher.Path = watcherPath;

                    // Watch for changes in the renaming, creation of files or directories.
                    watcher.NotifyFilter = NotifyFilters.FileName
                                         | NotifyFilters.DirectoryName
                                         | NotifyFilters.CreationTime;

                    // Add event handlers.
                    watcher.Created += OnChanged;
                    watcher.Renamed += OnChanged;

                    // Begin watching.
                    watcher.EnableRaisingEvents = true;

                    // Wait for the user to quit the program.
                    Console.WriteLine("Press 'q' to quit the sample.");
                    while (Console.Read() != 'q') ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                var fileName = "\\" + e.Name;

                //Wait for any qued processes to be released
                Thread.Sleep(1000);

                //Ignore the temporay files
                if (GetExtension(e.Name) == "tmp" || GetExtension(e.Name) == "crdownload" || !System.IO.File.Exists(e.FullPath))
                {
                    return;
                }

                //Check the file type and if it has a duplicates
                if (Extensions.Image.Contains(GetExtension(e.Name)))
                {
                    //If the directory doesn't exist, Creat it
                    CheckDirectory(Folders.ImagesDestination);
                    //If the file name has a duplicate in its destination, add an auto-incremented index
                    fileName = CheckFile(fileName, Folders.ImagesDestination + fileName);
                    //Move the file with the new name
                    System.IO.File.Move(e.FullPath, Folders.ImagesDestination + fileName);
                    //Write logs to the user
                    Console.WriteLine("the file " + e.FullPath + " has been moved to " + Folders.ImagesDestination + fileName);
                }
                else if (Extensions.Video.Contains(GetExtension(e.Name)))
                {
                    CheckDirectory(Folders.VideosDestination);
                    fileName = CheckFile(fileName, Folders.VideosDestination + fileName);
                    System.IO.File.Move(e.FullPath, Folders.VideosDestination + fileName);
                    Console.WriteLine("the file " + e.FullPath + " has been moved to " + Folders.VideosDestination + fileName);
                }
                else if (Extensions.Document.Contains(GetExtension(e.Name)))
                {
                    CheckDirectory(Folders.DocumentsDestination);
                    fileName = CheckFile(fileName, Folders.DocumentsDestination + fileName);
                    System.IO.File.Move(e.FullPath, Folders.DocumentsDestination + fileName);
                    Console.WriteLine("the file " + e.FullPath + " has been moved to " + Folders.DocumentsDestination + fileName);
                }
                else if (Extensions.Sound.Contains(GetExtension(e.Name)))
                {
                    CheckDirectory(Folders.SoundDestination);
                    fileName = CheckFile(fileName, Folders.SoundDestination + fileName);
                    System.IO.File.Move(e.FullPath, Folders.SoundDestination + fileName);
                    Console.WriteLine("the file " + e.FullPath + " has been moved to " + Folders.SoundDestination + fileName);
                }
                else
                {
                    //If none of the extensions in the dictionary were matched, move it to Random folder
                    CheckDirectory(Folders.RandomDestination);
                    fileName = CheckFile(fileName, Folders.RandomDestination + fileName);
                    System.IO.File.Move(e.FullPath, Folders.RandomDestination + fileName);
                    Console.WriteLine("Couldn't recognize the file type, you'll find it in \"Random\" folder");
                    Console.WriteLine("the file " + e.FullPath + " has been moved to " + Folders.RandomDestination + fileName);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static string GetExtension(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf('.') + 1);
        }

        private static void CheckDirectory(string directoryPath)
        {
            bool exists = System.IO.Directory.Exists(directoryPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(directoryPath);
        }

        private static string CheckFile(string fileName, string destinationPath)
        {
            //Check file name duplication
            if (File.Exists(destinationPath))
            {
                try
                {
                    //if it's not the first duplication get the number
                    var fileNumString = fileName.Substring(fileName.LastIndexOf('('));
                    //Convert it to int and increment it
                    var fileNum = Convert.ToInt32(Regex.Match(fileName.Substring(fileName.LastIndexOf('(')), @"\d+").Value) + 1;
                    //Append it to the original name right before the extension
                    fileName = fileName.Substring(0, fileName.LastIndexOf('(')) + "(" + fileNum + ")." + GetExtension(fileName);
                    //Change destination path regardfully
                    destinationPath = destinationPath.Substring(0, destinationPath.LastIndexOf('\\')) + fileName;
                    //Call back the function till the name is not duplicated anumore
                    return CheckFile(fileName, destinationPath);
                }
                catch (Exception ex)
                {
                    //if threre weren't any parentheses most likely it's the first copy 
                    fileName = fileName.Substring(0, fileName.LastIndexOf('.')) + "(1)." + GetExtension(fileName);
                    destinationPath = destinationPath.Substring(0, destinationPath.LastIndexOf('\\')) + fileName;
                    return CheckFile(fileName, destinationPath);
                }
            }
            //Return the same name for non-duplicates
            return fileName;
        }
    }
}
