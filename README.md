<h1 align="left">Github.Updater <img src="./Assets/GitHubUpdater.png" align="right" width="63px" height="63px"></h1> 

![.NET Core Desktop](https://github.com/LiorBanai/Github.Updater/workflows/.NET%20Core%20Desktop/badge.svg)
<a href="https://github.com/LiorBanai/Github.Updater/issues">
    <img src="https://img.shields.io/github/issues/LiorBanai/Github.Updater"  alt="Issues"/>
</a>
<a href="https://github.com/LiorBanai/Github.Updater/blob/master/LICENSE">
    <img src="https://img.shields.io/github/license/LiorBanai/Github.Updater"  alt="License"/>
</a>
<a href="https://github.com/LiorBanai/Github.Updater/releases"> 
    <img src="https://img.shields.io/github/v/release/LiorBanai/Github.Updater"  alt="Latest Release"/>
</a> 
 <a href="https://github.com/LiorBanai/Github.Updater/compare/V1.0.4...master">
    <img src="https://img.shields.io/github/commits-since/LiorBanai/Github.Updater/latest"  alt="Commits Since Latest Release"/>
</a>
       
       
a small utility to update your application when you have new release

very easy to use:

- Reference Github.Updater.Client [nuget](https://www.nuget.org/packages/Github.Updater.Client/).
- Create new Updater:
```csharp
       Updater = new Updater("Analogy Log Viewer", "Analogy-LogViewer/Analogy.LogViewer", updateFolder, "c:\", downloadSpeed,downloadProgress, downloadProgressValue);
           
```

with the following parameters: application name, repository name (both Organization name or user name andthe repository),folder to  download the updater and optional callbacksfor reporting progress.

- after that just pass your current version and call the following line (make sure you have releases with tag names; VX.X.X e.g V4.1.2)

```csharp

var currentVersion=new Version(4,2,9);
var newVersion = await Updater.CheckNewVersionExistsForApplication(currentVersion);
    if (newVersion)
       {
         await Updater.DownloadUpdaterIfNeeded((s) => UpdateText(tsslabel, s));
         await Updater.StartApplicationUpdateProcess(txtbApplicationFolder.Text,"net472.zip");
       }
          
```
