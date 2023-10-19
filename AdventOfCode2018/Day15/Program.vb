Imports System.Collections
Imports System.Dynamic
Imports System.Formats
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        Const fileName = "..\..\..\example.txt"
        'Const fileName = "E:\develop\advent-of-code-input\2018\Day15.txt"

        Dim answer1 As Long = 0
        Dim answer2 As Long = 0


        Dim input = File.ReadLines(fileName).ToList()
    End Sub
End Module
