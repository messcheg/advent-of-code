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
        Dim rules As New Dictionary(Of Char, HashSet(Of Char))
        For Each line In lines
            'Step P must be finished before step Y can begin.
            Dim first = line(5)
            Dim second = line(36)

            If (rules.ContainsKey(second)) Then
                rules(second).Add(first)
            Else
                Dim S As New HashSet(Of Char)
                S.Add(first)
                rules(second) = S
            End If
            If Not rules.ContainsKey(first) Then rules(first) = New HashSet(Of Char)

        Next

        Dim placed As New HashSet(Of Char)
        Dim result As String = ""
        For i = 0 To rules.Count - 1
            Dim nxt As Char = vbNullChar
            For Each r In rules
                If Not placed.Contains(r.Key) AndAlso r.Value.Count <= placed.Count Then
                    Dim independent = True
                    For Each v In r.Value
                        If Not placed.Contains(v) Then independent = False
                    Next
                    If independent Then
                        If nxt = vbNullChar OrElse nxt > r.Key Then nxt = r.Key
                    End If
                End If
            Next
            placed.Add(nxt)
            result = result & nxt
        Next

        Console.WriteLine("Answer1 = " & result)

        Const numWorkers As Integer = 5
        Const delay As Integer = 60

        Dim workers(numWorkers - 1) As Integer
        Dim workerTask(numWorkers - 1) As Char
        Dim busy(numWorkers - 1) As Boolean
        Dim currtime = 0
        placed = New HashSet(Of Char)
        Dim released As New HashSet(Of Char)
        For i = 0 To rules.Count - 1
            Dim nxt As Char = vbNullChar
            While nxt = vbNullChar
                For Each r In rules
                    If Not placed.Contains(r.Key) AndAlso r.Value.Count <= placed.Count Then
                        Dim independent = True
                        For Each v In r.Value
                            If Not released.Contains(v) Then independent = False
                        Next
                        If independent Then
                            If nxt = vbNullChar OrElse nxt > r.Key Then nxt = r.Key
                        End If
                    End If
                Next

                Dim release As Boolean = (nxt = vbNullChar)
                If Not release Then
                    'schedule for a worker
                    Dim useWorker = -1
                    For w = 0 To numWorkers - 1
                        If Not busy(w) Then
                            useWorker = w
                            Exit For
                        End If
                    Next
                    If useWorker <> -1 Then
                        workerTask(useWorker) = nxt
                        workers(useWorker) = currtime + delay + Asc(nxt) - Asc("A"c) + 1
                        busy(useWorker) = True
                        placed.Add(nxt)
                    Else
                        release = True
                        nxt = vbNullChar
                    End If
                End If
                If release Then
                    'release the first (ones) to be ready
                    Dim mintime As Integer = Integer.MaxValue
                    For w = 0 To numWorkers - 1
                        If busy(w) AndAlso workers(w) < mintime Then
                            mintime = workers(w)
                        End If
                    Next
                    currtime = mintime
                    For w = 0 To numWorkers - 1
                        If busy(w) AndAlso workers(w) = mintime Then
                            released.Add(workerTask(w))
                            busy(w) = False
                        End If
                    Next
                End If
            End While
        Next

        answer2 = currtime
        For w = 0 To numWorkers - 1
            If busy(w) AndAlso workers(w) > answer2 Then
                answer2 = workers(w)
            End If
        Next

        Console.WriteLine("Answer2 = " & answer2)

    End Sub


End Module
