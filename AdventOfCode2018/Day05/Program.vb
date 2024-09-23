Imports System
Imports System.Runtime.InteropServices
Imports System.Collections
Imports System.IO
Imports System.Security.Cryptography
Imports System.Runtime.InteropServices.JavaScript.JSType

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example_input.txt"
        Const fileName = "..\..\..\real_input.txt"

        Dim answer1 As Long = 0
        Dim answer2 As Long = 0

        Dim reaction = File.ReadLines(fileName)(0).ToArray()
        answer1 = React(reaction)
        Console.WriteLine("Answer1 = " & answer1)

        Dim reaction1 = File.ReadLines(fileName)(0).ToArray()
        Dim unique As New HashSet(Of Char)
        For Each c In reaction1
            unique.Add(Char.ToLower(c))
        Next

        answer2 = reaction1.Length
        Dim morf(reaction1.Length - 1) As Char

        For Each c In unique
            For i = 0 To reaction1.Length - 1
                Dim r = reaction1(i)
                If Char.ToLower(r) = c Then
                    morf(i) = " "c
                Else
                    morf(i) = r
                End If
            Next
            Dim a2 = React(morf)
            If a2 < answer2 Then answer2 = a2
        Next

        Console.WriteLine("Answer2 = " & answer2)

    End Sub

    Private Function React(reaction() As Char) As Integer
        Dim i = 0
        While i < reaction.Length
            Dim j = i + 1
            While j < reaction.Length AndAlso reaction(j) = " "c
                j += 1
            End While
            If j = reaction.Length Then
                i = j
            Else
                Dim c1 = reaction(i)
                Dim c2 = reaction(j)
                If (c1 <> c2 And Char.ToLower(c1) = Char.ToLower(c2)) Then
                    reaction(i) = " "c
                    reaction(j) = " "c
                    While i > 0 And reaction(i) = " "c
                        i -= 1
                    End While
                Else
                    i = j
                End If
            End If
        End While

        Dim answer = 0
        For Each c In reaction
            If c <> " "c Then answer += 1
        Next

        Return answer

    End Function
End Module
