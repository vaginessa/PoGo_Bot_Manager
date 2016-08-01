Public Class EditSettings

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            'Read the default configuration file
            Dim config As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\Haxton\user.xml")
            System.IO.File.Delete(Application.StartupPath & "\Haxton_" & Form1.RemoveWhitespace(TextBox1.Text) & "\user.xml")

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
            config_file = My.Computer.FileSystem.OpenTextFileWriter(Application.StartupPath & Form1.RemoveWhitespace("\Haxton_" & TextBox1.Text) & "\user.xml", True)
            config_file.WriteLine(config.ToString)
            config_file.Close()
            Form1.ListBox1.Items(Form1.TabControl1.SelectedIndex) = Form1.RemoveWhitespace(TextBox1.Text)
            Form1.TabControl1.SelectedTab.Text = Form1.RemoveWhitespace(TextBox1.Text)

        Catch ex As Exception
            If ex.ToString.Contains("Could not find") Then
                MsgBox("You cannot edit the current username! If you want to use a different username, please remove the current bot and add a new one :)")
            Else
                MsgBox(ex.ToString)
            End If

        End Try
    End Sub

    Private Sub EditSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        TextBox1.Text = Form1.TabControl1.SelectedTab.Text
    End Sub
End Class