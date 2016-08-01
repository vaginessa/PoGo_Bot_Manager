Imports System.Runtime.InteropServices

Public Class Test
    Dim mywindow As Integer
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function ShowWindowAsync(ByVal hwnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim processRunning As Process() = Process.GetProcesses()
        For Each pr As Process In processRunning
            If pr.ProcessName = "notepad" Then
                mywindow = pr.MainWindowHandle.ToInt32()
                ShowWindowAsync(mywindow, 0)
            End If
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim processRunning As Process() = Process.GetProcesses()
        For Each pr As Process In processRunning
            If pr.ProcessName = "notepad" Then

                ShowWindowAsync(mywindow, 1)
            End If
        Next
        Button1.Text = "Hide"
    End Sub
End Class