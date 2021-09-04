Public Class Form2
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Forms.Form2.Hide()
        My.Forms.Form1.Enabled = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim f
        Try
            f = FSO.OpenTextFile("certificate", 2, True)
        Catch ex As Exception
            MessageBox.Show("Ошибка чтения файла certificate")
            Exit Sub
        End Try
        Try
            f.WriteLine(TextBox1.Text)
            MessageBox.Show("Сертификат загружен. Перезапустите приложение")
            My.Forms.Form2.Hide()
            My.Forms.Form1.Enabled = True
            My.Forms.Form1.Close()
        Catch ex As Exception
            MessageBox.Show("Ошибка ввода сертификата")
            Exit Sub
        End Try
    End Sub
End Class