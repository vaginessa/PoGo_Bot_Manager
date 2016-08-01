Imports System.IO

Public Class Settings

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            'Check if the Haxton folder exists
            If (System.IO.Directory.Exists(Application.StartupPath & "\Haxton") = False) Then
                MessageBox.Show("The Haxton folder wasn't found at the location : " & Application.StartupPath & "\Haxton")
            Else
                'Create a separate folder containing a duplicate of the Haxton bot with the desired configuration
                My.Computer.FileSystem.CopyDirectory(Application.StartupPath & "\Haxton", Application.StartupPath & "\Haxton_" & TextBox1.Text, True)
                System.IO.File.Delete(Application.StartupPath & "\Haxton_" & TextBox1.Text & "\user.xml")
                'Read the default configuration file
                Dim config As String

                config = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\Haxton\user.xml")
                'Building the new configuration file
                If ComboBox1.SelectedIndex = 0 Then
                    'If the account type is Google, replace only the Google login data
                    config = config.Replace("<GoogleUsername>email@gmail.com</GoogleUsername>", "<GoogleUsername>" & TextBox1.Text & "</GoogleUsername>")
                    config = config.Replace("<GooglePassword>password</GooglePassword>", "<GooglePassword>" & TextBox2.Text & "</GooglePassword>")
                Else
                    'If the account type is PTC, replace only the PTC login data
                    config = config.Replace("<AuthType>Google</AuthType>", "<AuthType>Ptc</AuthType>")
                    config = config.Replace("<PtcUsername>username</PtcUsername>", "<PtcUsername>" & TextBox1.Text & "</PtcUsername>")
                    config = config.Replace("<PtcPassword>password</PtcPassword>", "<PtcPassword>" & TextBox2.Text & "</PtcPassword>")
                End If
                config = config.Replace("<DefaultLatitude>35.6329007</DefaultLatitude>", "<DefaultLatitude>" & TextBox3.Text & "</DefaultLatitude>")
                config = config.Replace("<DefaultLongitude>139.8782056</DefaultLongitude>", "<DefaultLongitude>" & TextBox4.Text & "</DefaultLongitude>")
                config = config.Replace("<KeepMinIvPercentage>90</KeepMinIvPercentage>", "<KeepMinIvPercentage>" & TextBox5.Text & "</KeepMinIvPercentage>")
                config = config.Replace("<KeepMinCp>100</KeepMinCp>", "<KeepMinCp>" & TextBox6.Text & "</KeepMinCp>")
                config = config.Replace("<WalkingSpeedInKilometerPerHour>180</WalkingSpeedInKilometerPerHour>", "<WalkingSpeedInKilometerPerHour>" & TextBox7.Text & "</WalkingSpeedInKilometerPerHour>")
                If CheckBox1.Checked = False Then
                    config = config.Replace("<EvolveAllPokemonWithEnoughCandy>true</EvolveAllPokemonWithEnoughCandy>", "<EvolveAllPokemonWithEnoughCandy>false</EvolveAllPokemonWithEnoughCandy>")
                End If
                If CheckBox2.Checked Then
                    config = config.Replace("<KeepPokemonsThatCanEvolve>false</KeepPokemonsThatCanEvolve>", "<KeepPokemonsThatCanEvolve>true</KeepPokemonsThatCanEvolve>")
                End If
                If CheckBox3.Checked = False Then
                    config = config.Replace("<TransferDuplicatePokemon>true</TransferDuplicatePokemon>", "<TransferDuplicatePokemon>false</TransferDuplicatePokemon>")
                End If

                config = config.Replace("<DelayBetweenPokemonCatch>50</DelayBetweenPokemonCatch>", "<DelayBetweenPokemonCatch>" & TextBox8.Text & "</DelayBetweenPokemonCatch>")
                If CheckBox4.Checked Then
                    config = config.Replace("<UsePokemonToNotCatchFilter>false</UsePokemonToNotCatchFilter>", "<UsePokemonToNotCatchFilter>true</UsePokemonToNotCatchFilter>")
                End If
                config = config.Replace("<KeepMinDuplicatePokemon>1</KeepMinDuplicatePokemon>", "<KeepMinDuplicatePokemon>" & TextBox9.Text & "</KeepMinDuplicatePokemon>")

                If CheckBox5.Checked = False Then
                    config = config.Replace("<PrioritizeIvOverCp>true</PrioritizeIvOverCp>", "<PrioritizeIvOverCp>false</PrioritizeIvOverCp>")
                End If
                config = config.Replace("<MaxTravelDistanceInMeters>4500</MaxTravelDistanceInMeters>", "<MaxTravelDistanceInMeters>" & TextBox10.Text & "</MaxTravelDistanceInMeters>")

                'Write the new configuration file
                Dim config_file As System.IO.StreamWriter
                config_file = My.Computer.FileSystem.OpenTextFileWriter(Application.StartupPath & "\Haxton_" & TextBox1.Text & "\user.xml", True)
                config_file.WriteLine(config.ToString)
                config_file.Close()
                Form1.ListBox1.Items.Add(TextBox1.Text)
                'Rename the exe and it's config
                Try
                    My.Computer.FileSystem.RenameFile(Application.StartupPath & "\Haxton_" & TextBox1.Text & "\PokemonGo.Haxton.Console.exe", TextBox1.Text & ".exe")
                    My.Computer.FileSystem.RenameFile(Application.StartupPath & "\Haxton_" & TextBox1.Text & "\PokemonGo.Haxton.Console.exe.config", TextBox1.Text & ".exe.config")
                Catch ex As Exception

                End Try
               'Creating a new tab in Form1
                Form1.TabControl1.TabPages.Add(TextBox1.Text)
                Form1.Button1.Enabled = True
                Form1.Button3.Enabled = True
                Form1.Button4.Enabled = True
                Form1.Button5.Enabled = True
                Form1.Button6.Enabled = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
    End Sub

    End Class