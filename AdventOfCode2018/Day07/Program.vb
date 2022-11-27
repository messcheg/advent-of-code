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

        Dim lines = File.ReadLines(fileName)
        Dim N = lines.Count - 1
        Dim rules As New Dictionary(Of String, HashSet(Of String))
        Dim current As Integer = 0
        For i = 0 To N
            Dim splits = lines(i).Split(" ")
            'Step P must be finished before step Y can begin.
            Dim first = splits(1)
            Dim second = splits(7)
            If (rules.ContainsKey(second)) Then
                rules(second).Add(first)
            Else
                Dim S As New HashSet(Of String)
                S.Add(first)
                rules(second) = S
            End If
            If Not rules.ContainsKey(first) Then rules(first) = New HashSet(Of String)

        Next

        Dim placed As New HashSet(Of String)
        Dim result As String = ""
        For i = 0 To rules.Count - 1
            Dim nxt As String = ""
            For Each r In rules
                If Not placed.Contains(r.Key) AndAlso r.Value.Count <= placed.Count Then
                    Dim independent = True
                    For Each v In r.Value
                        If Not placed.Contains(v) Then independent = False
                    Next
                    If independent Then
                        If nxt = "" Or nxt > r.Key Then nxt = r.Key
                    End If
                End If
            Next
            placed.Add(nxt)
            result = result & nxt
        Next


        Console.WriteLine("Answer1 = " & result)

        Console.WriteLine("Answer2 = " & answer2)

    End Sub


End Module
