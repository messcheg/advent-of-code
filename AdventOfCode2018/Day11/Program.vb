Imports System
Imports System.Reflection.Emit

Module Program
    Sub Main(args As String())
        Const serial = 7400
        'Const serial = 18
        Console.WriteLine("Fuel cell at 217, 196, grid serial number 39: power level  0. : " & Powerlevel(217, 196, 39))
        Console.WriteLine("Fuel cell at 101, 153, grid serial number 71: power level  4. : " & Powerlevel(101, 153, 71))
        Console.WriteLine("Fuel cell at  3,5, grid serial number 8: power level 4 : " & Powerlevel(3, 5, 8))
        Console.WriteLine("Fuel cell at  122,79, grid serial number 57: power level -5 : " & Powerlevel(122, 79, 57))
        Console.WriteLine("powerlevelsquare 33,45, 18 (29) : " & PowerlevelSquare(33, 45, 18))
        Console.WriteLine("powerlevelsquare 21,61, 42 (30) : " & PowerlevelSquare(21, 61, 42))
        Dim max As Integer = -100
        Dim xmax = 0
        Dim ymax = 0
        For i = 1 To 298
            For j = 1 To 298
                Dim pl = PowerlevelSquare(i, j, serial)
                If pl > max Then
                    max = pl
                    xmax = i
                    ymax = j
                End If
            Next
        Next
        Console.WriteLine("max powerlevel: " & max & " at (" & xmax & ", " & ymax & ")")
    End Sub

    Function Powerlevel(ByVal x As Long, ByVal y As Long, ByVal serial As Long) As Long
        Dim rackid = x + 10
        Dim powlev = y * rackid
        powlev += serial
        powlev *= rackid
        powlev = (powlev Mod 1000) \ 100
        powlev -= 5
        Return powlev
    End Function

    Function PowerlevelSquare(ByVal x As Long, ByVal y As Long, ByVal serial As Long) As Long
        Dim powlev = 0
        For i = 0 To 2
            For j = 0 To 2
                powlev += Powerlevel(x + i, y + j, serial)
            Next
        Next
        Return powlev
    End Function

End Module
