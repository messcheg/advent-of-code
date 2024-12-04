Imports System.Collections
Imports System.Dynamic
Imports System.Formats
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        Dim recipes As New List(Of Long)({3, 7})
        Dim elf1 As Long = 0
        Dim elf2 As Long = 1
        'Const perfect = 9
        'Const perfect = 18
        'Const perfect = 2018
        Const perfect = 580741
        Dim range As New List(Of Long)()
        Dim xsearchnum As Long = perfect
        While xsearchnum > 0
            range.Add(xsearchnum Mod 10)
            xsearchnum = xsearchnum \ 10
        End While

        Dim rangefound As Long = -1

        While recipes.Count < perfect + 10 Or rangefound < 0
            Dim x = recipes(elf1) + recipes(elf2)
            Dim a = x \ 10
            Dim b = x Mod 10
            If a > 0 Then
                recipes.Add(a)
                If rangefound < 0 Then
                    Dim equal As Boolean = recipes.Count >= range.Count
                    Dim r As Long = 0
                    While equal And r < range.Count
                        equal = range(r) = recipes(recipes.Count - r - 1)
                        r += 1
                    End While
                    If equal Then rangefound = recipes.Count - r
                End If

            End If

            recipes.Add(b)
            If rangefound < 0 Then
                Dim equal As Boolean = recipes.Count >= range.Count
                Dim r As Long = 0
                While equal And r < range.Count
                    equal = range(r) = recipes(recipes.Count - r - 1)
                    r += 1
                End While
                If equal Then rangefound = recipes.Count - r
            End If


            elf1 = (elf1 + recipes(elf1) + 1) Mod recipes.Count
            elf2 = (elf2 + recipes(elf2) + 1) Mod recipes.Count
        End While

        Console.Write("Answer1: ")
        For i = perfect To perfect + 9
            Console.Write(recipes(i))
        Next
        Console.WriteLine()


        Console.Write("Answer2: " & rangefound)
        Console.WriteLine()


    End Sub
End Module
