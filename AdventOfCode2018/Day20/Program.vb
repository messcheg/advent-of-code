Imports System.Collections
Imports System.Dynamic
Imports System.Formats
Imports System.IO
Imports System.Linq.Expressions
Imports System.Reflection.Emit
Imports System.Runtime.Intrinsics.X86
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example.txt"
        Const fileName = "E:\develop\advent-of-code-input\2018\Day20.txt"

        Dim instruction = File.ReadLines(fileName).ToList()(0)

        Dim field As New Dictionary(Of (x As Integer, y As Integer), Char)()
        Dim leveling As New Stack(Of (startx As Integer, starty As Integer))()
        Dim pos As (x As Integer, y As Integer) = (0, 0)
        For Each c In instruction
            Select Case c
                Case "^"
                    MakeBox(pos, field, "X")
                Case "("
                    leveling.Push(pos)
                Case ")"
                    pos = leveling.Pop()
                Case "|"
                    pos = leveling.Peek()
                Case "N"
                    field((pos.x, pos.y - 1)) = "-"
                    pos = (pos.x, pos.y - 2)
                    MakeBox(pos, field, ".")
                Case "S"
                    field((pos.x, pos.y + 1)) = "-"
                    pos = (pos.x, pos.y + 2)
                    MakeBox(pos, field, ".")
                Case "E"
                    field((pos.x + 1, pos.y)) = "|"
                    pos = (pos.x + 2, pos.y)
                    MakeBox(pos, field, ".")
                Case "W"
                    field((pos.x - 1, pos.y)) = "|"
                    pos = (pos.x - 2, pos.y)
                    MakeBox(pos, field, ".")
            End Select
        Next

        For Each kvp In field.ToArray()
            If kvp.Value = "?" Then field(kvp.Key) = "#"
        Next

        Dim miny = field.Keys.Select(Function(p) p.y).Min()
        Dim minx = field.Keys.Select(Function(p) p.x).Min()
        Dim maxy = field.Keys.Select(Function(p) p.y).Max()
        Dim maxx = field.Keys.Select(Function(p) p.x).Max()

        For j = miny To maxy
            For i = minx To maxx
                If field.ContainsKey((i, j)) Then
                    Console.Write(field((i, j)))
                Else
                    Console.Write(" ")
                End If
            Next
            Console.WriteLine()
        Next

        Dim state As New Dictionary(Of (x As Integer, y As Integer), (visited As Boolean, doors As Integer, Path As String))()
        Dim worklist As New Queue(Of (x As Integer, y As Integer))()
        state((0, 0)) = (False, 0, "")
        worklist.Enqueue((0, 0))
        While worklist.Count > 0
            Dim work = worklist.Dequeue()
            Dim workstat = state(work)
            workstat.visited = True
            If field((work.x, work.y - 1)) = "-" Then
                Dim newwork = (work.x, work.y - 2)
                If Not state.ContainsKey(newwork) Then
                    'add work and state
                    state(newwork) = (False, workstat.doors + 1, workstat.Path & "N")
                    worklist.Enqueue(newwork)
                End If
            End If
            If field((work.x, work.y + 1)) = "-" Then
                Dim newwork = (work.x, work.y + 2)
                If Not state.ContainsKey(newwork) Then
                    'add work and state
                    state(newwork) = (False, workstat.doors + 1, workstat.Path & "S")
                    worklist.Enqueue(newwork)
                End If
            End If
            If field((work.x - 1, work.y)) = "|" Then
                Dim newwork = (work.x - 2, work.y)
                If Not state.ContainsKey(newwork) Then
                    'add work and state
                    state(newwork) = (False, workstat.doors + 1, workstat.Path & "W")
                    worklist.Enqueue(newwork)
                End If
            End If
            If field((work.x + 1, work.y)) = "|" Then
                Dim newwork = (work.x + 2, work.y)
                If Not state.ContainsKey(newwork) Then
                    'add work and state
                    state(newwork) = (False, workstat.doors + 1, workstat.Path & "E")
                    worklist.Enqueue(newwork)
                End If
            End If
        End While

        Console.WriteLine("number of doors:" & state.Max(Function(f) f.Value.doors))
        Console.WriteLine("number of rooms mmore than 1000 doors away: " & state.Where(Function(f) f.Value.doors >= 1000).Count)
    End Sub
    Sub MakeBox(pos As (x As Integer, y As Integer), field As Dictionary(Of (x As Integer, y As Integer), Char), ByVal c As Char)
        field(pos) = c
        field((pos.x + 1, pos.y + 1)) = "#"
        field((pos.x - 1, pos.y + 1)) = "#"
        field((pos.x + 1, pos.y - 1)) = "#"
        field((pos.x - 1, pos.y - 1)) = "#"
        If Not field.ContainsKey((pos.x, pos.y - 1)) Then field((pos.x, pos.y - 1)) = "?"
        If Not field.ContainsKey((pos.x, pos.y + 1)) Then field((pos.x, pos.y + 1)) = "?"
        If Not field.ContainsKey((pos.x + 1, pos.y)) Then field((pos.x + 1, pos.y)) = "?"
        If Not field.ContainsKey((pos.x - 1, pos.y)) Then field((pos.x - 1, pos.y)) = "?"
    End Sub
End Module
