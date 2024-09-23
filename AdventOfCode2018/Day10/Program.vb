Imports System
Imports System.Collections
Imports System.Formats
Imports System.IO
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example.txt"
        Const fileName = "E:\develop\advent-of-code-input\2018\Day10.txt"
        Console.SetBufferSize(120, 1000)
        Console.SetWindowSize(120, 40)

        Dim answer1 As Long = 0
        Dim answer2 As Long = 0

        Dim input = File.ReadLines(fileName)
        Dim speed As New List(Of Tuple(Of Long, Long))(input.Count)
        Dim position As New List(Of Tuple(Of Long, Long))(input.Count)
        ' position=< 9,  1> velocity=< 0,  2>
        Dim pattern = "^position=<\s*([\s,\-]*\d+)\,\s*([\s,\-]*\d+)> velocity=<\s*([\s,\-]*\d+)\,\s*([\s,\-]*\d+)>"
        Dim rex As New Regex(pattern)

        For Each line In input
            Dim match = rex.Match(line)
            Dim posx As Long = Long.Parse(match.Groups(1).Value)
            Dim posy As Long = Long.Parse(match.Groups(2).Value)
            Dim velx As Long = Long.Parse(match.Groups(3).Value)
            Dim vely As Long = Long.Parse(match.Groups(4).Value)
            position.Add(New Tuple(Of Long, Long)(posx, posy))
            speed.Add(New Tuple(Of Long, Long)(velx, vely))
        Next

        Dim seconds As Integer = 0
        Dim timelap As Integer = 1
        While True
            Dim minx = position.Select(Function(t) t.Item1).Min()
            Dim maxx = position.Select(Function(t) t.Item1).Max()
            Dim miny = position.Select(Function(t) t.Item2).Min()
            Dim maxy = position.Select(Function(t) t.Item2).Max()
            Dim minspeedx = speed.Select(Function(t) t.Item1).Min()

            If maxx - minx < 100 And maxy - miny < 30 Then
                ' print de output
                Dim bars As New List(Of BitArray)(30)
                For i = 0 To 29
                    bars.Add(New BitArray(100, False))
                Next
                For Each pos In position
                    bars(pos.Item2 - miny)(pos.Item1 - minx) = True
                Next
                Console.WriteLine("Second: " & seconds.ToString())
                For Each bar In bars
                    For Each b In bar
                        If b Then
                            Console.Write("#")
                        Else
                            Console.Write(".")
                        End If
                    Next
                    Console.WriteLine()
                Next
                timelap = 1
            Else
                timelap = ((maxx - minx) - 99) / (-minspeedx * 2)
                If timelap < 1 Then timelap = 1
            End If

            For i = 0 To position.Count - 1
                Dim p = position(i)
                Dim v = speed(i)
                position(i) = New Tuple(Of Long, Long)(p.Item1 + timelap * v.Item1, p.Item2 + timelap * v.Item2)
            Next

            seconds += timelap
            Console.WriteLine("Press s Key")
            Dim result = Console.ReadKey
        End While
    End Sub
End Module
