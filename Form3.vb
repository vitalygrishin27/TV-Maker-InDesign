Public Class Form3
    Dim myFont As InDesign.Font
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            myFont = inDesignApp.Fonts.Item(TextBox1.Text)
            Dim reg
            reg = myFont.Registry
        Catch ex As Exception
            MessageBox.Show("Шрифт " + TextBox1.Text + " не зарегистрирован в системе", "Ошибка")
            Exit Sub
        End Try
        Try
            myFont = inDesignApp.Fonts.Item(TextBox2.Text)
            Dim reg
            reg = myFont.Registry
        Catch ex As Exception
            MessageBox.Show("Шрифт " + TextBox2.Text + " не зарегистрирован в системе", "Ошибка")
            Exit Sub
        End Try


        Dim f
        Try
            f = FSO.OpenTextFile("settings", 2, True) REM https://www.celitel.info/klad/wsh/filesystemobject.htm
        Catch ex As Exception
            MessageBox.Show("Ошибка чтения файла settings")
            Exit Sub
        End Try
        Try
            f.WriteLine("channelFontName=" + TextBox1.Text)
            f.WriteLine("channelFontSize=" + TextBox3.Text)
            f.WriteLine("сhannelSpaceBefore=" + TextBox4.Text)
            f.WriteLine("сhannelSpaceAfter=" + TextBox5.Text)
            f.WriteLine("channelUnderline=" + CheckBox1.Checked.ToString)
            f.WriteLine("textFontName=" + TextBox2.Text)
            f.WriteLine("sourceFilesFolder=" + TextBox6.Text)
            f.Close
            My.Forms.Form3.Hide()
            My.Forms.Form1.Enabled = True
            My.Forms.Form1.Activate()
        Catch ex As Exception
            MessageBox.Show("Ошибка сохранения настроек")
            Exit Sub
        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        My.Forms.Form3.Hide()
        My.Forms.Form1.Enabled = True
        My.Forms.Form1.Activate()
    End Sub

    Private Sub Form3_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        loadSettings()
    End Sub

    Private Sub loadSettings()
        Dim f
        Dim ts
        Dim str
        Try
            f = FSO.GetFile("settings")
        Catch ex As Exception
            FSO.CreateTextFile("settings", True)
        End Try
        f = FSO.GetFile("settings")
        ts = f.OpenAsTextStream(1)
        Try

            Do While Not ts.AtEndofStream
                str = ts.ReadLine
                If Mid(str, 1, 15) = "channelFontName" Then
                    TextBox1.Text = Mid(str, 17)
                ElseIf Mid(str, 1, 15) = "channelFontSize" Then
                    TextBox3.Text = Mid(str, 17)
                ElseIf Mid(str, 1, 12) = "textFontName" Then
                    TextBox2.Text = Mid(str, 14)
                ElseIf Mid(str, 1, 18) = "сhannelSpaceBefore" Then
                    TextBox4.Text = Mid(str, 20)
                ElseIf Mid(str, 1, 17) = "сhannelSpaceAfter" Then
                    TextBox5.Text = Mid(str, 19)
                ElseIf Mid(str, 1, 16) = "channelUnderline" Then
                    If Mid(str, 18) = "True" Then
                        CheckBox1.Checked = True
                    Else
                        CheckBox1.Checked = False
                    End If
                ElseIf Mid(str, 1, 17) = "sourceFilesFolder" Then
                    TextBox6.Text = Mid(str, 19)
                End If
            Loop

        Catch ex As Exception
            MessageBox.Show("Ошибка чтения настроек")
            Exit Sub
        End Try
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            TextBox6.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If (FontDialog1.ShowDialog() = DialogResult.OK) Then
            TextBox1.Text = FontDialog1.Font.Name
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If (FontDialog1.ShowDialog() = DialogResult.OK) Then
            TextBox2.Text = FontDialog1.Font.Name
        End If
    End Sub
End Class