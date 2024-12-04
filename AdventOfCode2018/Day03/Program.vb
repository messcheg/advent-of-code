Imports System
Imports System.Runtime.InteropServices
Imports System.Collections
Imports System.IO

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example_input.txt"
        Const fileName = "..\..\..\real_input.txt"

        Dim answer1 As Long = 0
        Dim answer2 As Long = 0

        Dim fabric As New Dictionary(Of Tuple(Of Integer, Integer), List(Of Integer))

        For Each line In File.ReadLines(fileName)
            Dim split1 = line.Split(" @ ")
            Dim split2 = split1(1).Split(": ")
            Dim split3 = split2(0).Split(",")
            Dim split4 = split2(1).Split("x")
            Dim claim = CInt(Mid(split1(0), 2))
            Dim inchLeft = CInt(split3(0))
            Dim inchTop = CInt(split3(1))
            Dim inchWidth = CInt(split4(0))
            Dim inchHeight = CInt(split4(1))
            For w = inchLeft To inchLeft + inchWidth - 1
                For h = inchTop To inchTop + inchHeight - 1
                    Dim t As New Tuple(Of Integer, Integer)(w, h)
                    If fabric.ContainsKey(t) Then
                        fabric(t).Add(claim)
                    Else
                        Dim l As New List(Of Integer)
                        l.Add(claim)
                        fabric.Add(t, l)
                    End If
                Next
            Next
        Next

        For Each l In fabric.Values
            If l.Count > 1 Then answer1 += 1
        Next

        Console.WriteLine("Answer1 = " & answer1)

        For Each line In File.ReadLines(fileName)
            Dim split1 = line.Split(" @ ")
            Dim split2 = split1(1).Split(": ")
            Dim split3 = split2(0).Split(",")
            Dim split4 = split2(1).Split("x")
            Dim claim = CInt(Mid(split1(0), 2))
            Dim inchLeft = CInt(split3(0))
            Dim inchTop = CInt(split3(1))
            Dim inchWidth = CInt(split4(0))
            Dim inchHeight = CInt(split4(1))
            Dim clean As Boolean = True
            For w = inchLeft To inchLeft + inchWidth - 1
                For h = inchTop To inchTop + inchHeight - 1
                    Dim t As New Tuple(Of Integer, Integer)(w, h)
                    clean = fabric(t).Count = 1
                    If Not clean Then Exit For
                Next
                If Not clean Then Exit For
            Next
            If clean Then
                answer2 = claim
                Exit For
            End If
        Next


        Console.WriteLine("Answer2 = " & answer2)

    End Sub
End Module
