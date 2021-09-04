Public Class Form1
    Private mov As Boolean = False REM For moving form
    Private downMouseX, downMouseY As Integer REM For moving form
    Dim document As InDesign.Document
    Dim page As InDesign.Page
    Dim myFont As InDesign.Font
    Dim myFontForChannel As InDesign.Font
    Dim myFontSizeForChannel As Integer
    Dim myFontChannelSpaceBefore As Integer
    Dim myFontChannelSpaceAfter As Integer
    Dim myFontChannelUnderline As Boolean
    Dim textFrame As InDesign.TextFrame
    Dim paragraph As InDesign.Paragraph
    Dim paragraphToApplyStyle As InDesign.Paragraph  REM temporary variable for apply style
    Dim insPointToApplyStyle As InDesign.InsertionPoint REM temporary variable for apply style
    Dim characterStyle As InDesign.CharacterStyle REM temporary variable for apply style
    Dim paragraphStyle As InDesign.ParagraphStyle
    Dim insPoint As InDesign.InsertionPoint REM point where in story will be inserted new text
    Dim myStory As InDesign.Story

    Dim daysKeyWords As Dictionary(Of String, String) = New Dictionary(Of String, String) REM SHOULD BE CONFIGURABLE
    Dim channelKeyWords As Dictionary(Of String, String) = New Dictionary(Of String, String) REM SHOULD BE CONFIGURABLE

    Dim boldKeyWords As List(Of String) = New List(Of String) REM These keywords will be bold. SHOULD BE CONFIGURABLE

    Dim paragraphStyleForText As InDesign.ParagraphStyle
    Dim paragraphStyleForChannelName As InDesign.ParagraphStyle

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If mov = True Then
            Dim curpos = New System.Drawing.Point
            curpos.X = Me.Location.X + (e.X - downMouseX)
            curpos.Y = Me.Location.Y + (e.Y - downMouseY)
            Me.Location = curpos
        End If
    End Sub
    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            mov = True
            downMouseX = e.X
            downMouseY = e.Y
        End If
    End Sub
    Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then mov = False
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        certificateCheck()
        inDesignApp = CreateObject("InDesign.Application")
        TransparencyKey = Me.BackColor
        daysKeyWords.Add("$$Понедельник", "MondayMainTextFrame")
        daysKeyWords.Add("$$Вторник", "TuesdayMainTextFrame")
        daysKeyWords.Add("$$Среда", "WednesdayMainTextFrame")
        daysKeyWords.Add("$$Четверг", "ThursdayMainFirstPartTextFrame")
        daysKeyWords.Add("$$Пятница", "FridayMainTextFrame")
        daysKeyWords.Add("$$Суббота", "SaturdayMainTextFrame")
        daysKeyWords.Add("$$Воскресенье", "SundayMainTextFrame")

        channelKeyWords.Add("Интер", "inter")
        channelKeyWords.Add("Перший", "ut-1")
        channelKeyWords.Add("1+1", "kanal1+1")
        channelKeyWords.Add("Новый канал", "n_kan")
        channelKeyWords.Add("СТБ", "stb")
        channelKeyWords.Add("ICTV", "ictv")
        channelKeyWords.Add("Украина", "ukr")
        channelKeyWords.Add("2+2", "2+2")
        channelKeyWords.Add("НТН", "ntn")
        channelKeyWords.Add("TET", "tet")

        boldKeyWords.AddRange(New String() {"М/ф", "Х/ф", "Т/с", "М/с", "Д/с"})

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        REM Загрузка параметров
        Try
            loadSettings()
        Catch ex As Exception
            MessageBox.Show("Ошибка чтения/применения настроек")
        End Try
        REM    Stop

        Try
            Button1.Enabled = False

            document = inDesignApp.Documents.Add
            document.Name = "TV"

            document.DocumentPreferences.PageHeight = "420mm"
            document.DocumentPreferences.PageWidth = "289mm"
            document.DocumentPreferences.PageOrientation = InDesign.idPageOrientation.idPortrait
            document.DocumentPreferences.FacingPages = True
            document.DocumentPreferences.StartPageNumber = 2
            document.DocumentPreferences.PagesPerDocument = 2

            page = document.Pages(1)
            page.MarginPreferences.Top = "15mm"
            page.MarginPreferences.Right = "15mm"
            page.MarginPreferences.Left = "10mm"
            page.MarginPreferences.Bottom = "12.5mm"
            page = document.Pages(2)
            page.MarginPreferences.Left = "10mm"
            page.MarginPreferences.Right = "15mm"
            page.MarginPreferences.Top = "15mm"
            page.MarginPreferences.Bottom = "12.5mm"

            page = document.Pages(1)
            textFrame = page.TextFrames.Add
            textFrame.GeometricBounds = New String() {"30mm", "15mm", "402.5mm", "87.5mm"}
            textFrame.TextFramePreferences.TextColumnCount = 2
            textFrame.Name = "MondayMainTextFrame"


            textFrame = page.TextFrames.Add
            textFrame.GeometricBounds = New String() {"30mm", "91.5mm", "402.5mm", "164mm"}
            textFrame.TextFramePreferences.TextColumnCount = 2
            textFrame.Name = "TuesdayMainTextFrame"

            textFrame = page.TextFrames.Add
            textFrame.GeometricBounds = New String() {"30mm", "168mm", "402.5mm", "240.5mm"}
            textFrame.TextFramePreferences.TextColumnCount = 2
            textFrame.Name = "WednesdayMainTextFrame"

            textFrame = page.TextFrames.Add
            textFrame.GeometricBounds = New String() {"30mm", "244.5mm", "402.5mm", "279mm"}
            textFrame.TextFramePreferences.TextColumnCount = 1
            textFrame.Name = "ThursdayMainFirstPartTextFrame"
            Dim firstPart = textFrame

            page = document.Pages(2)
            textFrame = page.TextFrames.Add
            textFrame.GeometricBounds = New String() {"30mm", "299mm", "402.5mm", "333.5mm"}
            textFrame.TextFramePreferences.TextColumnCount = 1
            textFrame.Name = "ThursdayMainSecondPartTextFrame"
            firstPart.NextTextFrame = textFrame

            textFrame = page.TextFrames.Add
            textFrame.GeometricBounds = New String() {"30mm", "337mm", "402.5mm", "410.5mm"}
            textFrame.TextFramePreferences.TextColumnCount = 2
            textFrame.Name = "FridayMainTextFrame"

            textFrame = page.TextFrames.Add
            textFrame.GeometricBounds = New String() {"30mm", "413.5mm", "402.5mm", "487mm"}
            textFrame.TextFramePreferences.TextColumnCount = 2
            textFrame.Name = "SaturdayMainTextFrame"

            textFrame = page.TextFrames.Add
            textFrame.GeometricBounds = New String() {"30mm", "490.5mm", "402.5mm", "563mm"}
            textFrame.TextFramePreferences.TextColumnCount = 2
            textFrame.Name = "SundayMainTextFrame"

            page = document.Pages(1)
            textFrame = page.TextFrames.Add
            textFrame.GeometricBounds = New String() {"30mm", "-175.5mm", "402.5mm", "-14mm"}
            textFrame.TextFramePreferences.TextColumnCount = 2
            textFrame.Name = "TempTextFrame"

            REM Плашки
            REM textFrame = page.TextFrames.Add
            REM textFrame.GeometricBounds = New String() {"23mm", "15mm", "29mm", "87.5mm"}
            REM  textFrame.TextFramePreferences.TextColumnCount = 1
            REM  textFrame.Name = "MondayTitle"
            REM  MessageBox.Show(textFrame.FillColor)



            REM  myFont = inDesignApp.Fonts.Item("Arial")
            fileProcessing()
            If CheckBox1.Checked = True Then
                removeDuplicateRows()
            End If
            If CheckBox2.Checked = True Then
                finishTextFrames()
            End If
            MessageBox.Show("Выполнено успешно!", "TV")
        Catch ex As Exception
            MessageBox.Show("Внутренняя ошибка. " + vbCr + ex.Message, "Ошибка")
        End Try

    End Sub

    Private Sub fileProcessing()
        Dim tempTextFrame As InDesign.TextFrame
        Dim targetTextFrame As InDesign.TextFrame = Nothing
        Dim parCont As String
        Dim day As KeyValuePair(Of String, String)
        For Each channel As KeyValuePair(Of String, String) In channelKeyWords
            tempTextFrame = findTextFrameByName("TempTextFrame")
            tempTextFrame.Contents = ""
            insPoint = textFrame.InsertionPoints.Item(-1)
            insPoint.Place(sourceFilesFolder + "\" + channel.Value) REM All files need to be UTF-8 encodings
            adjustOverflows(tempTextFrame)
            Dim dayCount = 0
            Dim moveToNextTextFrame = False
            day = daysKeyWords.ElementAt(dayCount)
            Dim totalParagraphCount = tempTextFrame.Paragraphs.Count
            Dim currentParagraphIndex = 0
            For Each paragr As InDesign.Paragraph In tempTextFrame.Paragraphs
                currentParagraphIndex += 1
                parCont = paragr.Contents
                If parCont.Contains(day.Key) Then
                    targetTextFrame = findTextFrameByName(day.Value)
                    If dayCount < 6 Then dayCount += 1
                    day = daysKeyWords.ElementAt(dayCount)
                    moveToNextTextFrame = True
                End If
                If targetTextFrame IsNot Nothing Then
                    If targetTextFrame.Name.Equals("ThursdayMainFirstPartTextFrame") And Not findTextFrameByName("ThursdayMainSecondPartTextFrame").Contents = "" Then
                        targetTextFrame = findTextFrameByName("ThursdayMainSecondPartTextFrame")
                    End If
                End If
                If targetTextFrame IsNot Nothing And moveToNextTextFrame = False Then
                    insertNewParagraphToTextFrame(targetTextFrame, parCont, findOrCreateParagraphStyleForText())
                ElseIf moveToNextTextFrame = True Then
                    insertNewParagraphToTextFrame(targetTextFrame, channel.Key + vbCr, findOrCreateParagraphStyleForChannelName())
                    moveToNextTextFrame = False
                End If

                If currentParagraphIndex = totalParagraphCount Then
                    Exit For
                End If
            Next
            targetTextFrame = Nothing
        Next
    End Sub

    Private Sub insertNewParagraphToTextFrame(textFrame As InDesign.TextFrame, text As String, paragraphStyle As InDesign.ParagraphStyle)
        insPointToApplyStyle = textFrame.InsertionPoints.Item(-1)
        insPointToApplyStyle.Contents = text
        paragraphToApplyStyle = textFrame.Paragraphs.LastItem()
        paragraphToApplyStyle.ApplyParagraphStyle(paragraphStyle)
        For Each k As String In boldKeyWords
            If text.Contains(k) Then
                Try
                    paragraphToApplyStyle.FontStyle = "Bold"
                Catch ex As Exception

                End Try

                Exit For
            End If
        Next
        If paragraphToApplyStyle.Contents.Equals(vbCr) Then
            paragraphToApplyStyle.Delete()
            Return
        End If
        adjustOverflows(textFrame)
    End Sub

    Private Function findTextFrameByName(textName As String) As InDesign.TextFrame
        For Each txf As InDesign.TextFrame In document.AllPageItems
            If txf.Name.Equals(textName) Then
                Return txf
            End If
        Next
        Return Nothing
    End Function

    Private Function findOrCreateParagraphStyleForChannelName()
        If (paragraphStyleForChannelName IsNot Nothing) Then Return paragraphStyleForChannelName
        For Each ps As InDesign.ParagraphStyle In document.AllParagraphStyles
            If ps.Name.Equals("TV-ChannelName") Then
                paragraphStyleForChannelName = ps
                Return ps
            End If
        Next
        paragraphStyle = document.ParagraphStyles.Add
        paragraphStyle.Name = "TV-ChannelName"
        paragraphStyle.AppliedFont = myFontForChannel
        Try
            paragraphStyle.FontStyle = "Bold"
        Catch ex As Exception

        End Try
        Try
            paragraphStyle.Underline = myFontChannelUnderline
        Catch ex As Exception

        End Try
        paragraphStyle.PointSize = myFontSizeForChannel
        paragraphStyle.SpaceAfter = myFontChannelSpaceAfter
        paragraphStyle.SpaceBefore = myFontChannelSpaceBefore
        paragraphStyle.Justification = InDesign.idJustification.idCenterAlign
        paragraphStyle.Leading = 7
        paragraphStyleForChannelName = paragraphStyle
        Return paragraphStyle
    End Function

    Private Function findOrCreateParagraphStyleForText()
        If (paragraphStyleForText IsNot Nothing) Then Return paragraphStyleForText
        For Each ps As InDesign.ParagraphStyle In document.AllParagraphStyles
            If ps.Name.Equals("TV-simpleText") Then
                paragraphStyleForText = ps
                Return ps
            End If
        Next
        paragraphStyle = document.ParagraphStyles.Add
        paragraphStyle.Name = "TV-simpleText"
        paragraphStyle.AppliedFont = myFont
        Try
            paragraphStyle.FontStyle = "Regular"
        Catch ex As Exception

        End Try
        paragraphStyle.PointSize = 6
        paragraphStyle.Leading = 7
        paragraphStyle.SpaceAfter = 0
        paragraphStyle.SpaceBefore = 0
        paragraphStyle.Justification = InDesign.idJustification.idLeftAlign
        paragraphStyle.LeftIndent = 5
        paragraphStyle.FirstLineIndent = -5
        paragraphStyleForText = paragraphStyle
        Return paragraphStyle
    End Function

    Private Sub adjustOverflows(myTextFrame As InDesign.TextFrame)
        myStory = myTextFrame.ParentStory
        If myStory.Leading > myStory.PointSize + 1 Then myStory.Leading = myStory.PointSize + 1
        Do While myTextFrame.Overflows
            If myStory.PointSize + 1 >= myStory.Leading Then
                myStory.PointSize -= 0.5
            Else
                myStory.Leading -= 0.5
            End If
        Loop
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form1.ActiveForm.Close()
    End Sub

    Private Sub removeDuplicateRows()
        For Each day As KeyValuePair(Of String, String) In daysKeyWords
            textFrame = findTextFrameByName(day.Value)
            myStory = textFrame.ParentStory
            Dim totalCountParagraph = myStory.Paragraphs.Count
            Dim textToCompare = ""
            Dim paragraphIndexOfSourceText = 1
            Dim paragraphNeedsToRemove As List(Of Integer) = New List(Of Integer)
            For currentParagraphIndex = 1 To totalCountParagraph - 1
                If currentParagraphIndex < totalCountParagraph Then
                    paragraph = myStory.Paragraphs.Item(currentParagraphIndex)
                End If
                If (Not paragraph.AppliedParagraphStyle.Name.Equals("TV-ChannelName") And currentParagraphIndex < totalCountParagraph) Then

                    If textToCompare = "" Then
                        textToCompare = paragraph.Contents.Substring(paragraph.Contents.IndexOf(".") + 3)
                        paragraphIndexOfSourceText = currentParagraphIndex
                    End If
                    If (textToCompare.Equals(paragraph.Contents.Substring(paragraph.Contents.IndexOf(".") + 3)) And paragraphIndexOfSourceText <> currentParagraphIndex) Then
                        paragraphNeedsToRemove.Add(currentParagraphIndex)
                    End If

                Else
                    If paragraphNeedsToRemove.Count > 0 Then
                        Dim previousText = myStory.Paragraphs.Item(paragraphIndexOfSourceText).Contents.Substring(myStory.Paragraphs.Item(paragraphIndexOfSourceText).Contents.IndexOf(".") + 3)
                        Dim originalTime = myStory.Paragraphs.Item(paragraphIndexOfSourceText).Contents.Substring(0, myStory.Paragraphs.Item(paragraphIndexOfSourceText).Contents.Length - previousText.Length)
                        Dim newParagraph = originalTime
                        For Each newTime As Integer In paragraphNeedsToRemove
                            newParagraph += ", "
                            newParagraph += myStory.Paragraphs.Item(newTime).Contents.Substring(0, myStory.Paragraphs.Item(newTime).Contents.IndexOf(".") + 3)
                        Next

                        newParagraph += " "
                        newParagraph += previousText
                        myStory.Paragraphs.Item(paragraphIndexOfSourceText).Contents = newParagraph
                        For i = paragraphNeedsToRemove.Count - 1 To 0 Step -1
                            Dim newTime As Integer = paragraphNeedsToRemove(i)
                            myStory.Paragraphs.Item(newTime).Contents = ""
                        Next
                        totalCountParagraph = myStory.Paragraphs.Count
                        paragraphNeedsToRemove.Clear()
                    End If
                    If textToCompare = "" Then
                        currentParagraphIndex = paragraphIndexOfSourceText + 1
                    Else
                        currentParagraphIndex = paragraphIndexOfSourceText
                    End If
                    If paragraphIndexOfSourceText = totalCountParagraph - 1 Then
                        Exit For
                    End If
                    textToCompare = ""
                End If
            Next
        Next
    End Sub

    Private Sub finishTextFrames()
        For Each day As KeyValuePair(Of String, String) In daysKeyWords
            textFrame = findTextFrameByName(day.Value)
            myStory = textFrame.ParentStory
            Dim totalCountParagraph = myStory.Paragraphs.Count
            Dim changeLeading = True

            If myStory.Overflows Then
                For currentParagraphIndex = 1 To totalCountParagraph
                    paragraph = myStory.Paragraphs.Item(currentParagraphIndex)
                    If (Not paragraph.AppliedParagraphStyle.Name.Equals("TV-ChannelName")) Then
                        If paragraph.PointSize + 1 < paragraph.Leading Then
                            paragraph.PointSize -= 0.5
                        Else
                            paragraph.Leading -= 0.5
                        End If
                    End If
                    If Not myStory.Overflows Then
                        Exit For
                    End If
                    If currentParagraphIndex = totalCountParagraph Then
                        currentParagraphIndex = 1
                    End If
                Next

            Else
                For currentParagraphIndex = 1 To totalCountParagraph
                    paragraph = myStory.Paragraphs.Item(currentParagraphIndex)
                    If (Not paragraph.AppliedParagraphStyle.Name.Equals("TV-ChannelName")) Then
                        If paragraph.PointSize + 1 < paragraph.Leading Then
                            paragraph.PointSize += 0.5
                            changeLeading = False
                        Else
                            paragraph.Leading += 0.5
                            changeLeading = True
                        End If
                    End If
                    If myStory.Overflows Then
                        Exit For
                    End If
                    If currentParagraphIndex = totalCountParagraph Then
                        currentParagraphIndex = 1
                    End If
                Next
                If changeLeading = True Then
                    paragraph.Leading -= 0.5
                Else
                    paragraph.PointSize -= 0.5
                End If

            End If


            For currentParagraphIndex = totalCountParagraph To 1 Step -1

                paragraph = myStory.Paragraphs.Item(currentParagraphIndex)
                If (Not paragraph.AppliedParagraphStyle.Name.Equals("TV-ChannelName")) Then
                    paragraph.Leading += 0.5
                End If
                If myStory.Overflows Then
                    Exit For
                End If
                If currentParagraphIndex = 1 Then
                    currentParagraphIndex = totalCountParagraph
                End If
            Next
            paragraph.Leading -= 0.5
        Next

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        My.Forms.Form1.Enabled = False
        My.Forms.Form2.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        My.Forms.Form1.Enabled = False
        My.Forms.Form3.Show(Form1.ActiveForm)
    End Sub

    Private Sub certificateCheck()
        Dim f
        Dim ts
        Dim str
        Try
            f = FSO.GetFile("certificate")
        Catch ex As Exception
            MessageBox.Show("Ошибка чтения файла certificate")
            Exit Sub
        End Try
        Try
            ts = f.OpenAsTextStream(1)
            Do While Not ts.AtEndofStream
                str = ts.ReadAll
            Loop
            Dim index
            index = str.Substring(0, 1)
            If (str.Substring(index - 1, 1) <> str.Substring(1, 1)) Then
                MessageBox.Show("Сертификат не подлинный")
            End If
        Catch ex As Exception
            MessageBox.Show("Ошибка чтения сертификата")
            Exit Sub
        End Try
        Dim day
        Dim month
        Dim year
        Dim secret
        Try
            Dim firstIndex = 2
            day = str.Substring(firstIndex + 1, str.Substring(firstIndex, 1))
            firstIndex += str.Substring(firstIndex, 1)
            Dim count = str.Substring(firstIndex + 1, 1)
            month = str.Substring(firstIndex + 2, count)
            firstIndex += count + 1
            count = str.Substring(firstIndex + 1, 1)
            year = str.Substring(firstIndex + 2, count)
            firstIndex += count + 1
            count = str.Substring(firstIndex + 1, 1)
            secret = str.Substring(firstIndex + 2, count)

            Dim dateInCertificateString = (day / secret) & "." & (month / secret) & "." & (year / secret)
            Dim dateInCertificate = Date.Parse(dateInCertificateString)
            If dateInCertificate.CompareTo(Date.Now.Date) = -1 Then
                REM   MessageBox.Show("Действие сертификата закончилось", "Ошибка")
                Label5.Text = dateInCertificate
                Label5.BackColor = Color.Red

                Exit Sub
            Else
                Label5.Text = dateInCertificate
                Button1.Enabled = True
            End If

        Catch ex As Exception
            MessageBox.Show("Ошибка во время обработки сертификата", "Ошибка")
        End Try

    End Sub

    Private Sub loadSettings()
        Dim f
        Dim ts
        Dim str
        myFont = inDesignApp.Fonts.Item("Arial")
        myFontForChannel = inDesignApp.Fonts.Item("Arial")
        myFontSizeForChannel = 11
        myFontChannelSpaceBefore = 1
        myFontChannelSpaceAfter = 1
        myFontChannelUnderline = True
        sourceFilesFolder = ""

        Try
            f = FSO.GetFile("settings")
        Catch ex As Exception
            Exit Sub
        End Try
        Try
            ts = f.OpenAsTextStream(1)
            Do While Not ts.AtEndofStream
                str = ts.ReadLine
                If Mid(str, 1, 15) = "channelFontName" Then
                    myFontForChannel = inDesignApp.Fonts.Item(Mid(str, 17))
                ElseIf Mid(str, 1, 15) = "channelFontSize" Then
                    myFontSizeForChannel = Integer.Parse(Mid(str, 17))
                ElseIf Mid(str, 1, 12) = "textFontName" Then
                    myFont = inDesignApp.Fonts.Item(Mid(str, 14))
                ElseIf Mid(str, 1, 18) = "сhannelSpaceBefore" Then
                    myFontChannelSpaceBefore = Integer.Parse(Mid(str, 20))
                ElseIf Mid(str, 1, 17) = "сhannelSpaceAfter" Then
                    myFontChannelSpaceAfter = Integer.Parse(Mid(str, 19))
                ElseIf Mid(str, 1, 16) = "channelUnderline" Then
                    If Mid(str, 18) = True Then
                        myFontChannelUnderline = True
                    Else
                        myFontChannelUnderline = False
                    End If
                ElseIf Mid(str, 1, 17) = "sourceFilesFolder" Then
                    sourceFilesFolder = Mid(str, 19)
                End If
            Loop
        Catch ex As Exception
            MessageBox.Show("Ошибка во время применения настроек. Будут использованы настройки по умолчанию", "Ошибка")
        End Try



    End Sub

End Class
