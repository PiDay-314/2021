using System;
using System.IO;

namespace AsciiArtDeployer
{
    public sealed class WorkspaceDirectory : IDisposable
    {
        private readonly DirectoryInfo directoryInfo;

        public WorkspaceDirectory()
        {
            Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Pi AsciiArtDeployer", Guid.NewGuid().ToString());
            directoryInfo = Directory.CreateDirectory(Path);
        }

        public string Path { get; set; }

        public string GetFilePath(string fileName)
        {
            return System.IO.Path.Combine(Path, fileName);
        }

        public void Dispose()
        {
            SetAttributesNormal(directoryInfo);
            directoryInfo.Delete(recursive: true);
        }

        private void SetAttributesNormal(DirectoryInfo directoryInfo)
        {
            foreach (var subdirectory in directoryInfo.GetDirectories())
            {
                SetAttributesNormal(subdirectory);
            }
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }
        }
    }
}
