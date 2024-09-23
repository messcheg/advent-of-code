Imports System.Collections
Imports System.Dynamic
Imports System.Formats
Imports System.Globalization
Imports System.IO
Imports System.Reflection.Emit
Imports System.Runtime.Intrinsics.X86
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example.txt"
        Const fileName = "E:\develop\advent-of-code-input\2018\Day18.txt"

        Dim voortgang = False


        Dim filelist = File.ReadLines(fileName).ToList()
        Dim field = filelist.Select(Function(f) f.ToList()).ToList()

        Dim totLumb = 0
        Dim totTree = 0

        Dim repeat As New Dictionary(Of String, Long)()
        Dim key As New StringBuilder(2500)
        Dim repeating As Boolean = False
        Const totalrounds = 1000000000
        Dim m As Long = 1
        While m <= totalrounds
            totLumb = 0
            totTree = 0
            key.Clear()
            Dim field_new As New List(Of List(Of Char))()
            If m <= 10 Then Console.WriteLine("--------------[Round " & m & " ]--------------")
            For j = 0 To field.Count - 1
                Dim newline As New List(Of Char)()
                field_new.Add(newline)
                For i = 0 To field(0).Count - 1
                    Dim j1 = IIf(j > 0, j - 1, j)
                    Dim j2 = IIf(j < field.Count - 1, j + 1, j)
                    Dim i1 = IIf(i > 0, i - 1, i)
                    Dim i2 = IIf(i < field(0).Count - 1, i + 1, i)
                    Dim trees = 0
                    Dim lumbers = 0
                    For jx = j1 To j2
                        For ix = i1 To i2
                            If field(jx)(ix) = "#" Then lumbers += 1
                            If field(jx)(ix) = "|" Then trees += 1
                        Next
                    Next
                    If field(j)(i) = "." Then
                        If trees >= 3 Then
                            newline.Add("|")
                            totTree += 1
                        Else
                            newline.Add(".")
                        End If
                    ElseIf field(j)(i) = "|" Then
                        If lumbers >= 3 Then
                            newline.Add("#")
                            totLumb += 1
                        Else
                            newline.Add("|")
                            totTree += 1
                        End If

                    ElseIf field(j)(i) = "#" Then
                        If trees >= 1 And lumbers >= 2 Then
                            newline.Add("#")
                            totLumb += 1
                        Else
                            newline.Add(".")
                        End If

                    End If
                    key.Append(field_new(j)(i))
                    If m <= 10 Then Console.Write(field_new(j)(i))
                Next
                If m <= 10 Then Console.WriteLine()
            Next
            field = field_new
            Dim xx = key.ToString()
            If Not repeating Then
                If repeat.ContainsKey(xx) Then

                    'we found a repetition
                    repeating = True
                    Dim dif = m - repeat(xx)
                    Dim gap = (totalrounds - m) \ dif
                    m += gap * dif
                Else
                    repeat.Add(xx, m)
                End If
            End If

            If m = 10 Then
                Console.WriteLine("-------[Answer 1]---------")
                Console.WriteLine("trees (" & totTree & ") * lumbers (" & totLumb & ") = " & (totLumb * totTree))
            End If
            m += 1

        End While

        Console.WriteLine("-------[Answer 2]---------")
        Console.WriteLine("trees (" & totTree & ") * lumbers (" & totLumb & ") = " & (totLumb * totTree))


    End Sub
End Module
