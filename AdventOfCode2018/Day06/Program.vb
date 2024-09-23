Imports System
Imports System.Runtime.InteropServices
Imports System.Collections
Imports System.IO

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example_input.txt"
        Const fileName = "..\..\..\real_input.txt"
        Dim maxSum = 10000
        Dim answer1 As Long = 0
        Dim answer2 As Long = 0
        Dim lines = File.ReadLines(fileName)
        Dim Xs(lines.Count - 1) As Integer
        Dim Ys(lines.Count - 1) As Integer
        Dim i = 0
        For Each line In lines
            Dim xy = line.Split(", ")
            Xs(i) = xy(0)
            Ys(i) = xy(1)
            i = i + 1
        Next
        Dim xMax = Xs.Max
        Dim yMax = Ys.Max
        'Dim XYs(xMax, yMax) As Integer
        Dim fieldsize(lines.Count - 1) As Integer
        Dim sumOnBorder = False
        Dim infinite As New HashSet(Of Integer)
        For x = 0 To xMax

            For y = 0 To yMax
                Dim dist(lines.Count - 1) As Integer
                For i = 0 To Xs.Length - 1
                    Dim d = Math.Abs(x - Xs(i)) + Math.Abs(y - Ys(i))
                    dist(i) = d
                Next
                If dist.Sum < maxSum Then
                    answer2 += 1
                    If x = 0 Or x = xMax Or y = 0 Or y = yMax Then sumOnBorder = True
                End If
                Dim mindist = dist.Min
                Dim closest = -1
                Dim count = 0
                For j = 0 To dist.Count - 1
                    If dist(j) = mindist Then
                        If closest = -1 Then closest = j
                        count = count + 1
                    End If
                Next
                If count = 1 Then
                    'XYs(x, y) = closest
                    fieldsize(closest) += 1
                    If x = 0 OrElse x = xMax OrElse y = 0 OrElse y = yMax Then infinite.Add(closest)
                    'Else
                    'XYs(x, y) = -1
                End If
            Next
        Next
        'For x = 0 To xMax
        '    infinite.Add(XYs(x, 0))
        '    infinite.Add(XYs(x, yMax))
        'Next
        'For y = 0 To yMax
        '    infinite.Add(XYs(0, y))
        '    infinite.Add(XYs(xMax, y))
        'Next
        Dim largest As Integer = 0
        For i = 0 To fieldsize.Length - 1
            If Not infinite.Contains(i) AndAlso fieldsize(i) > largest Then largest = fieldsize(i)
        Next
        answer1 = largest

        Console.WriteLine("Answer1 = " & answer1)

        Console.WriteLine("Answer2 = " & answer2)

    End Sub

End Module
