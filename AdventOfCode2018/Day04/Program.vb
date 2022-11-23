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

        For Each line In File.ReadLines(fileName)
            answer1 += CLng(line)
        Next
        Console.WriteLine("Answer1 = " & answer1)

        Console.WriteLine("Answer1 = " & answer2)

    End Sub
End Module
