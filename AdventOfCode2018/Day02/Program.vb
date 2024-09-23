Imports System
Imports System.Runtime.InteropServices
Imports System.Collections
Imports System.IO

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example_input.txt"
        'Const fileName = "..\..\..\example_input2.txt"
        Const fileName = "..\..\..\real_input.txt"

        Dim answer1 As Long = 0

        Dim cnt2 As Long
        Dim cnt3 As Long
        For Each line In File.ReadLines(fileName)
            Dim h As New Dictionary(Of Char, Integer)
            For Each c In line
                If h.ContainsKey(c) Then
                    h(c) = h(c) + 1
                Else
                    h(c) = 1
                End If
            Next
            If h.ContainsValue(3) Then cnt3 += 1
            If h.ContainsValue(2) Then cnt2 += 1
        Next
        answer1 = cnt2 * cnt3

        Console.WriteLine("Answer1 = " & answer1)

        Dim answer2 As String = 0
        Dim lines = File.ReadLines(fileName)
        Dim placeInStr = 0
        Dim placeInArr = 0
        For cntr = 0 To lines.Count - 2
            For cntr1 = cntr + 1 To lines.Count - 1
                Dim diff As Integer = 0
                Dim s1 = lines(cntr)
                Dim s2 = lines(cntr1)
                For ccnt = 0 To Len(s1) - 1
                    If s1(ccnt) <> s2(ccnt) Then
                        diff += 1
                        placeInStr = ccnt
                    End If
                    If diff > 1 Then Exit For
                Next
                If diff = 1 Then
                    placeInArr = cntr
                    Exit For
                End If
            Next
            If placeInArr > 0 Then Exit For
        Next

        answer2 = Left(lines(placeInArr), placeInStr) & Mid(lines(placeInArr), placeInStr + 2)

        Console.WriteLine("Answer1 = " & answer2)

    End Sub
End Module
