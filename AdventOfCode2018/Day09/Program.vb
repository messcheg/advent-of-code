Imports System

Module Program
    Sub Main(args As String())
        Const players = 9
        Const lastMarble = 25

        Dim scores() = New ULong(players) {}
        Dim marbles As New LinkedList(Of Long)()

        Dim currentplayer = 1
        marbles.AddFirst(0)
        Dim current = marbles.First
        For i = 1 To lastMarble
            If i Mod 23 = 0 Then
                Dim last = current
                For k = 1 To 7
                    last = current
                    If current Is marbles.First Then
                        current = marbles.Last
                    Else
                        current = current.Previous
                    End If
                Next
                scores(currentplayer) += i + current.Value
                marbles.Remove(current)
                current = last
            Else
                If current Is marbles.Last Then
                    current = marbles.First
                Else
                    current = current.Next
                End If
                current = marbles.AddAfter(current, i)
            End If
            'Console.Write("[" & currentplayer & "] ")
            'Dim mrb = marbles.First
            'While Not mrb Is Nothing
            '    If mrb Is current Then
            '        Console.Write(" (" & mrb.Value & ") ")
            '    Else
            '        Console.Write(" " & mrb.Value & " ")
            '    End If
            '    mrb = mrb.Next
            'End While
            'Console.WriteLine()

            currentplayer += 1
            If currentplayer > players Then currentplayer = 1
        Next

        Dim answer1 = scores.Max()

        Console.WriteLine("Answer1 = " & answer1)

    End Sub
End Module
