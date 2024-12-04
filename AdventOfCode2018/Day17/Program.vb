Imports System.Collections
Imports System.Dynamic
Imports System.Formats
Imports System.IO
Imports System.Reflection.Emit
Imports System.Runtime.Intrinsics.X86
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example.txt"
        Const fileName = "E:\develop\advent-of-code-input\2018\Day17.txt"

        Dim voortgang = False


        Dim filelist = File.ReadLines(fileName).ToList()
        Dim fields As New Dictionary(Of Tuple(Of Integer, Integer), Char)


        For Each line In filelist
            Dim coord = line.Split(", ")
            Dim xval = IIf(coord(0)(0) = "x", 0, 1)
            Dim xpart = GetParts(coord(xval))
            Dim ypart = GetParts(coord(1 - xval))
            For i = xpart.Item1 To xpart.Item2
                For j = ypart.Item1 To ypart.Item2
                    fields(New Tuple(Of Integer, Integer)(i, j)) = "#"
                Next
            Next
        Next

        Dim xmin = fields.Keys.Select(Function(t) t.Item1).Min()
        Dim xmax = fields.Keys.Select(Function(t) t.Item1).Max()
        Dim ymin = fields.Keys.Select(Function(t) t.Item2).Min()
        Dim ymax = fields.Keys.Select(Function(t) t.Item2).Max()

        fields(New Tuple(Of Integer, Integer)(500, 0)) = "+"
        Dim labour As New Queue(Of Tuple(Of Integer, Integer))()
        labour.Enqueue(New Tuple(Of Integer, Integer)(500, 0))
        If voortgang Then
            For j = 0 To ymax + 1
                For i = xmin - 1 To xmax + 1
                    Dim c As Char = "."
                    If fields.TryGetValue(New Tuple(Of Integer, Integer)(i, j), c) Then
                        Console.Write(c)
                    Else
                        Console.Write(".")
                    End If
                Next
                Console.WriteLine()
            Next
            Console.ReadLine()
        End If

        While labour.Count > 0
            Dim nextjob = labour.Dequeue()
            Dim c = fields(nextjob)
            Dim i = nextjob.Item1
            Dim j = nextjob.Item2
            Dim down = New Tuple(Of Integer, Integer)(i, j + 1)
                Dim c1 As Char = "."
            If Not fields.TryGetValue(down, c1) Then
                If j < ymax Then
                    fields(down) = "|"
                    labour.Enqueue(down)
                End If
            ElseIf c1 = "~" Or c1 = "#" Then
                'naar de zijkanten proberen te vloeien
                Dim li = i
                Dim leftfall As Boolean = False
                Dim leftfound As Boolean = False
                While Not leftfound
                    Dim c2 As Char = "."
                    Dim c3 As Char = "."
                    Dim cur = New Tuple(Of Integer, Integer)(li - 1, j)
                    Dim bot = New Tuple(Of Integer, Integer)(li - 1, j + 1)
                    If Not fields.TryGetValue(cur, c2) OrElse c2 = "|" Then
                        If Not fields.TryGetValue(bot, c3) OrElse c3 = "|" Then
                            leftfound = True
                            leftfall = True
                        End If
                        li -= 1
                    Else
                        If c2 = "#" Or c2 = "~" Then
                            leftfound = True
                        End If
                    End If
                End While
                Dim ri = i
                Dim rightfall As Boolean = False
                Dim rightfound As Boolean = False
                While Not rightfound
                    Dim c2 As Char = "."
                    Dim c3 As Char = "."
                    Dim cur = New Tuple(Of Integer, Integer)(ri + 1, j)
                    Dim bot = New Tuple(Of Integer, Integer)(ri + 1, j + 1)
                    If Not fields.TryGetValue(cur, c2) OrElse c2 = "|" Then
                        If Not fields.TryGetValue(bot, c3) OrElse c3 = "|" Then
                            rightfound = True
                            rightfall = True
                        End If
                        ri += 1
                    Else
                        If c2 = "#" Or c2 = "~" Then
                            rightfound = True
                        End If
                    End If
                End While
                If rightfall Or leftfall Then
                    If li < i Or ri > i Then
                        For k = li To ri
                            Dim c4 As Char = "."
                            Dim field = New Tuple(Of Integer, Integer)(k, j)
                            If Not fields.TryGetValue(field, c4) Then
                                fields(field) = "|"
                            End If
                        Next
                        If rightfall Then labour.Enqueue(New Tuple(Of Integer, Integer)(ri, j))
                        If leftfall Then labour.Enqueue(New Tuple(Of Integer, Integer)(li, j))
                    End If
                Else
                    For k = li To ri
                        Dim c4 As Char = "."
                        Dim field = New Tuple(Of Integer, Integer)(k, j)
                        Dim above = New Tuple(Of Integer, Integer)(k, j - 1)
                        If Not fields.TryGetValue(field, c4) OrElse c4 = "|" Then
                            fields(field) = "~"
                        End If
                        If fields.TryGetValue(above, c4) AndAlso c4 = "|" Then
                            labour.Enqueue(above)
                        End If
                    Next
                End If
            End If


            If voortgang Then
                Console.SetCursorPosition(0, 0)
                For j = 0 To ymax + 1
                    For i = xmin - 1 To xmax + 1
                        Dim c5 As Char = "."
                        If fields.TryGetValue(New Tuple(Of Integer, Integer)(i, j), c5) Then
                            Console.Write(c5)
                        Else
                            Console.Write(".")
                        End If
                    Next
                    Console.WriteLine()
                Next
                System.Threading.Thread.Sleep(100)
            End If
        End While

        Console.SetCursorPosition(0, 0)
        For j = 0 To ymax + 2
            For i = xmin - 2 To xmax + 2
                Dim c As Char = "."
                If fields.TryGetValue(New Tuple(Of Integer, Integer)(i, j), c) Then
                    Console.Write(c)
                Else
                    Console.Write(".")
                End If
            Next
            Console.WriteLine()
        Next

        Dim answer1 = fields.Where(Function(t) t.Key.Item2 >= ymin And t.Key.Item2 <= ymax And (t.Value = "~" Or t.Value = "|")).Count
        Console.WriteLine("Amount of water: " & answer1)

        Dim answer2 = fields.Where(Function(t) t.Key.Item2 >= ymin And t.Key.Item2 <= ymax And (t.Value = "~")).Count
        Console.WriteLine("Amount of water: " & answer2)

    End Sub

    Function GetParts(part As String) As Tuple(Of Integer, Integer)
        Dim parts = part.Substring(2).Split("..")
        Dim x1 = Integer.Parse(parts(0))
        Dim x2 = x1
        If parts.Count = 2 Then x2 = Integer.Parse(parts(1))
        Return New Tuple(Of Integer, Integer)(x1, x2)
    End Function


End Module
