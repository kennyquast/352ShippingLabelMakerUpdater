Imports System
Imports System.IO
Module Module1
    Public HomeFolder As String = ""
    Public DownloadFolder As String = ""
    Public VersionInfo As String = ""
    Public AppPath As String = My.Application.Info.DirectoryPath
    Sub Main(ByVal args() As String)
        Dim versionNumber As Version

        versionNumber = My.Application.Info.Version
        Console.Write("352ShippingLabelMaker Updater file version : ")
        Console.WriteLine(versionNumber)
        Console.WriteLine(vbLf)
        If args.Length = 0 Then
            Console.WriteLine("Error: No home folder specified")
            Console.WriteLine("This program must be run from the main application Or use the following format.")
            Console.WriteLine("352ShipingLabelMakerUpdater.exe homefolderpath   - eg: C:\homefolder")
            Console.WriteLine("Press Any Key to Exit...")
            Console.ReadLine()
            End
        Else
            Dim data As String = args(0)
            HomeFolder = data
        End If



        ' Console.WriteLine("Argument: {0}", data)
        ' Console.Write("Press Enter to exit")
        ' Console.Read()

        'uncomment all below for full functioality
        Kill352ShippingLabelMaker()
        Console.WriteLine("Pausing for 3 seconds to close Label Maker Software Please Wait...")
        '3 second delay should be enough to kill this 
        'process as the application SHOULD be closed before this is launched anyway
        Threading.Thread.Sleep(3000)
        BackupFiles()
        GetCurrentVersion()        '
        'Download Files from server
        CopyDirectory(HomeFolder & "\updates\" & VersionInfo, AppPath, True)
        'restart label scanner
        Shell("352ShippingLabelMaker.exe")
        End
    End Sub
    Public Sub Kill352ShippingLabelMaker()

        For Each prog As Process In Process.GetProcesses
            If prog.ProcessName = "352ShippingLabelMaker" Then
                prog.Kill()
            End If
        Next
        Exit Sub

    End Sub
    Public Sub BackupFiles()
        'Delete the oldest backup from last time
        Dim bk As String = "working\352ShippingLabelMaker.exe"
        File.Delete(bk)
        'copy the current version into a workign folder
        Dim file1 As String = "352ShippingLabelMaker.exe"
        Dim file2 As String = "working\352shippinglabelmaker.exe"
        File.Copy(file1, file2)




    End Sub
    Private Sub GetCurrentVersion()

        'read a file that contains the most current version info dfrom the server
        Dim lines() As String = IO.File.ReadAllLines(HomeFolder & "\currentversion.txt")
        VersionInfo = lines(0)

    End Sub

    Sub CopyDirectory(ByVal SourcePath As String, ByVal DestPath As String, Optional ByVal Overwrite As Boolean = False)

        Dim SourceDir As DirectoryInfo = New DirectoryInfo(SourcePath)

        Dim DestDir As DirectoryInfo = New DirectoryInfo(DestPath)



        ' the source directory must exist, otherwise throw an exception

        If SourceDir.Exists Then

            ' if destination SubDir's parent SubDir does not exist throw an exception

            If Not DestDir.Parent.Exists Then

                Throw New DirectoryNotFoundException("Destination directory does not exist: " + DestDir.Parent.FullName)

            End If



            If Not DestDir.Exists Then

                DestDir.Create()

            End If



            ' copy all the files of the current directory

            Dim ChildFile As FileInfo

            For Each ChildFile In SourceDir.GetFiles()

                If Overwrite Then

                    ChildFile.CopyTo(Path.Combine(DestDir.FullName, ChildFile.Name), True)

                Else

                    ' if Overwrite = false, copy the file only if it does not exist

                    ' this is done to avoid an IOException if a file already exists

                    ' this way the other files can be copied anyway...

                    If Not File.Exists(Path.Combine(DestDir.FullName, ChildFile.Name)) Then

                        ChildFile.CopyTo(Path.Combine(DestDir.FullName, ChildFile.Name), False)

                    End If

                End If

            Next



            ' copy all the sub-directories by recursively calling this same routine

            Dim SubDir As DirectoryInfo

            For Each SubDir In SourceDir.GetDirectories()

                CopyDirectory(SubDir.FullName, Path.Combine(DestDir.FullName, SubDir.Name), Overwrite)

            Next

        Else

            Throw New DirectoryNotFoundException("Source directory does not exist: " + SourceDir.FullName)

        End If

    End Sub
    Public Sub MakeFolder()
        'Make Folder not needed, but code left for educational purposes
        ' Specify the directories you want to manipulate.
        Dim di As DirectoryInfo = New DirectoryInfo("test")
        Try
            ' Determine whether the directory exists.
            If di.Exists Then
                ' Indicate that it already exists.
                Console.WriteLine("That path exists already.")
                Return
            End If

            ' Try to create the directory.
            di.Create()
            Console.WriteLine("The directory was created successfully.")

            '' Delete the directory.
            'di.Delete()
            'Console.WriteLine("The directory was deleted successfully.")

        Catch e As Exception
            Console.WriteLine("The process failed: {0}", e.ToString())
        End Try
    End Sub

End Module
