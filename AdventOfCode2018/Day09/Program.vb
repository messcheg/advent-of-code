Imports System

Module Program
    Sub Main(args As String())
        ' 479 players; l10st marble is worth 71035 points
        Const players = 479
        Const lastMarble = 7103500

        Dim scores() = New ULong(players) {}
        Dim marbles As New List(Of Long)()

        Dim currentMarble = 0
        Dim currentplayer = 1
        marbles.Add(0)
        For i = 1 To lastMarble
            If i Mod 23 = 0 Then
                scores(currentplayer) += i
                Dim marbleToTake = currentMarble - 7
                While marbleToTake < 0
                    marbleToTake += marbles.Count
                End While
                scores(currentplayer) += marbles(marbleToTake)
                marbles.RemoveAt(marbleToTake)
                currentMarble = marbleToTake
                If currentMarble < 0 Then currentMarble += marbles.Count
            Else
                Dim insertAfter = currentMarble + 1
                If insertAfter = marbles.Count Then insertAfter = 0
                marbles.Insert(insertAfter + 1, i)
                currentMarble = insertAfter + 1
            End If
            'Console.Write("[" & currentplayer & "] ")
            'For j = 0 To marbles.Count - 1
            '    If j = currentMarble Then
            '        Console.Write(" (" & marbles(j) & ") ")
            '    Else
            '        Console.Write(" " & marbles(j) & " ")
            '    End If
            'Next
            'Console.WriteLine()
            currentplayer += 1
            If currentplayer > players Then currentplayer = 1
        Next

        Dim answer1 = scores.Max()

        Console.WriteLine("Answer1 = " & answer1)

    End Sub
End Module
