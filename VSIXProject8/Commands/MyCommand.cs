using System.Diagnostics;
using System.IO;

namespace VSIXProject8;

[Command(PackageIds.MyCommand)]
internal sealed class MyCommand : BaseCommand<MyCommand>
{
    protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
    {
        var project = await VS.Solutions.GetActiveProjectAsync();
        var projectPath = project.FullPath;

        string gitPath;
        FindGitFolder(projectPath, out gitPath);

        var repo = new LibGit2Sharp.Repository(gitPath);
        Debug.WriteLine(repo.Network.Remotes["origin"]);
        Debug.WriteLine(repo.Network.Remotes["origin"].Url);
        
    }


    private void FindGitFolder(string path, out string foundPath)
    {
        foundPath = null;
        // Check if the current directory contains a .git folder
        if (Directory.Exists(Path.Combine(path, ".git")))
        {
            foundPath = path;
            return;
        }
        else
        {
            // Climb up to the parent directory
            string parentPath = Directory.GetParent(path)?.FullName;
            if (!string.IsNullOrEmpty(parentPath))
            {
                FindGitFolder(parentPath, out foundPath); // Recursively search the parent directory
            }
        }

        return;
    }
}
