Imports System
Imports System.Runtime.InteropServices
Imports System.Collections
Imports System.IO
Imports System.Timers
Imports System.Reflection.Emit

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example_input.txt"
        Const fileName = "..\..\..\real_input.txt"

        Dim answer1 As Long = 0
        Dim answer2 As Long = 0
        Dim imported = File.ReadLines(fileName).ToArray()
        Dim gEvents(imported.Count - 1) As GuardEvent
        For i As Integer = 0 To imported.Count - 1
            Dim ge As New GuardEvent()
            Dim line = imported(i)
            ge.year = CInt(Mid(line, 2, 4))
            ge.month = CInt(Mid(line, 7, 2))
            ge.day = CInt(Mid(line, 10, 2))
            ge.hour = CInt(Mid(line, 13, 2))
            ge.minute = CInt(Mid(line, 16, 2))
            Dim l = Len(line)
            If l = 27 Then
                ge.action = "wake"
            ElseIf l = 31 Then
                ge.action = "sleep"
            Else
                ge.action = "shift"
                ge.guard = CInt(Mid(line, 27, Len(line) - 39))
            End If
            gEvents(i) = ge
        Next

        Dim ordeedEvents = gEvents.OrderBy(Function(ge) ge.datetime)
        Dim curGuard = 0
        Dim startSleep As GuardEvent = Nothing

        Dim sleepytime As New Dictionary(Of Integer, GuardSleep)

        For Each ge In ordeedEvents
            If ge.action = "shift" Then
                curGuard = ge.guard
            Else
                ge.guard = curGuard
                If ge.action = "sleep" Then startSleep = ge
            End If
            If ge.action <> "sleep" And startSleep IsNot Nothing Then
                Dim tl As GuardSleep = Nothing
                If Not sleepytime.TryGetValue(startSleep.guard, tl) Then
                    tl = New GuardSleep
                    tl.guard = startSleep.guard
                    sleepytime.Add(startSleep.guard, tl)
                End If

                Dim startY = startSleep.year
                Dim endY = ge.year
                For y As Integer = startY To endY
                    Dim startMo = startSleep.month
                    Dim startD = startSleep.day
                    Dim startH = startSleep.hour
                    Dim startMi = startSleep.minute
                    If y > startY Then
                        startMo = 1
                        startD = 1
                        startH = 0
                        startMi = 0
                    End If

                    Dim endMo = ge.month
                    Dim endD = ge.day
                    Dim endH = ge.hour
                    Dim endMi = ge.minute
                    If y < endY Then
                        endMo = 12
                        endD = 31
                        endH = 23
                        endMi = 59
                    End If

                    Dim DiyStart = DaysInYear(startY, startMo, startD)
                    Dim DiyEnd = DaysInYear(endY, endMo, endD)

                    Dim dH = 0
                    If startMi > endMi Then dH = 1
                    Dim dD = 0
                    If startH > endH + dH Then dD = 1

                    Dim fullHours = (DiyEnd - DiyStart) * 24 + (endH - startH - dH)
                    Dim minutes = fullHours * 60 + dH * 60 + endMi - startMi
                    tl.totalsleep += minutes
                    For m = 0 To 59
                        tl.sleepPerMinute(m) += fullHours
                        If m >= startMi And m < endMi Or (startMi > endMi And (m < endMi Or m >= startMi)) Then tl.sleepPerMinute(m) += 1
                    Next
                Next
                startSleep = Nothing
            End If
        Next

        Dim lazy = sleepytime.First().Value
        For Each lg In sleepytime.Values
            If lg.totalsleep > lazy.totalsleep Then lazy = lg
        Next

        Dim maxmin = 0
        For i = 0 To 59
            If lazy.sleepPerMinute(i) > lazy.sleepPerMinute(maxmin) Then maxmin = i
        Next

        answer1 = lazy.guard * maxmin

        Console.WriteLine("Answer1 = " & answer1)
        For Each lg In sleepytime.Values
            Dim maxmin1 = 0
            For i = 0 To 59
                If lg.sleepPerMinute(i) > lg.sleepPerMinute(maxmin1) Then maxmin1 = i
            Next
            If lg.sleepPerMinute(maxmin1) > lazy.sleepPerMinute(maxmin) Then
                maxmin = maxmin1
                lazy = lg
            End If
        Next

        answer2 = lazy.guard * maxmin


        Console.WriteLine("Answer1 = " & answer2)

    End Sub

    Private Function DaysInYear(year As Integer, month As Integer, day As Integer)
        Dim days = 0
        Dim schrikkel = 0
        If year Mod 1000 = 0 Or (year Mod 100 <> 0 And year Mod 4 = 0) Then
            schrikkel = 1
        End If
        Select Case month
            Case 1
                days = day
            Case 2
                days = day + 31
            Case 3
                days = day + 31 + 28 + schrikkel
            Case 4
                days = day + 31 + 28 + schrikkel + 31
            Case 5
                days = day + 31 + 28 + schrikkel + 31 + 30
            Case 6
                days = day + 31 + 28 + schrikkel + 31 + 30 + 31
            Case 7
                days = day + 31 + 28 + schrikkel + 31 + 30 + 31 + 30
            Case 8
                days = day + 31 + 28 + schrikkel + 31 + 30 + 31 + 30 + 31
            Case 9
                days = day + 31 + 28 + schrikkel + 31 + 30 + 31 + 30 + 31 + 31
            Case 10
                days = day + 31 + 28 + schrikkel + 31 + 30 + 31 + 30 + 31 + 31 + 30
            Case 11
                days = day + 31 + 28 + schrikkel + 31 + 30 + 31 + 30 + 31 + 31 + 30 + 31
            Case Else
                days = day + 31 + 28 + schrikkel + 31 + 30 + 31 + 30 + 31 + 31 + 30 + 31 + 30

        End Select
        Return days
    End Function

    Private Function MonthEnd(month As Integer, year As Integer) As Integer
        Dim days = 0
        Select Case month
            Case 1, 3, 5, 7, 8, 10, 12
                days = 31
            Case 4, 6, 9, 11
                days = 30
            Case Else
                If year Mod 1000 = 0 Or (year Mod 100 <> 0 And year Mod 4 = 0) Then
                    days = 29
                Else
                    days = 28
                End If
        End Select
        Return days
    End Function

    Private Class GuardSleep
        Public guard As Integer
        Public totalsleep As Long = 0
        Public sleepPerMinute(60) As Long
    End Class
    Private Class GuardEvent
        Public year As Integer
        Public month As Integer
        Public day As Integer
        Public hour As Integer
        Public minute As Integer
        Public guard As Integer
        Public action As String
        Public ReadOnly Property datetime As String
            Get
                Return year.ToString("0000") &
                month.ToString("00") &
                day.ToString("00") &
                hour.ToString("00") &
                minute.ToString("00")
            End Get
        End Property

    End Class
End Module
