Public Class Statistics
    Public username As String
    Public level As String
    Public current_exp As Integer
    Public max_exp As Integer
    Public exp_per_hour As Integer
    Public pokemon_per_hour As Integer
    Public stardust As String
    Public pokemons_caught As Integer
    Public pokestops_looted As Integer
    Public pokemons_evolved As Integer
    Public pokemons_transferred As Integer

    Private Sub Statistics_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.TopMost = True
        Label1.Text = username
        Label2.Text = "Level: " & level
        Label3.Text = "EXP: " & current_exp & "/" & max_exp
        Label4.Text = "EXP/h: " & exp_per_hour
        Label5.Text = "Next level: " & Int((max_exp - current_exp) / exp_per_hour * 1.0) & " hours " & Math.Round((((max_exp - current_exp) / exp_per_hour) * 100 * 60) / 100 - Int((max_exp - current_exp) / exp_per_hour * 1.0) * 60, 2, MidpointRounding.ToEven) & " minutes"
        Label6.Text = "Pokemon/h: " & pokemon_per_hour
        Label7.Text = "Stardust: " & stardust
        Label8.Text = "Pokemons caught: " & pokemons_caught
        Label9.Text = "Pokestops looted: " & pokestops_looted
        Label10.Text = "Pokemons evolved: " & pokemons_evolved
        Label11.Text = "Pokemons transferred: " & pokemons_transferred
    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click

    End Sub

    Private Sub Statistics_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class