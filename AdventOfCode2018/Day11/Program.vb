Imports System
Imports System.Reflection.Emit
Imports System.Security.Cryptography
Imports System.Math
Module Program
    Sub Main(args As String())
        Const serial = 7400
        'Const serial = 18

        Dim cache(300, 300) As Long
        clearcache(cache)
        Console.WriteLine("Fuel cell at 217, 196, grid serial number 39: power level  0. : " & Powerlevel(217, 196, 39, cache))
        clearcache(cache)
        Console.WriteLine("Fuel cell at 101, 153, grid serial number 71: power level  4. : " & Powerlevel(101, 153, 71, cache))
        clearcache(cache)
        Console.WriteLine("Fuel cell at  3,5, grid serial number 8: power level 4 : " & Powerlevel(3, 5, 8, cache))
        clearcache(cache)
        Console.WriteLine("Fuel cell at  122,79, grid serial number 57: power level -5 : " & Powerlevel(122, 79, 57, cache))
        clearcache(cache)
        Console.WriteLine("powerlevelsquare 33,45, 18 (29) : " & PowerlevelSquare(33, 45, 18, cache))
        clearcache(cache)
        Console.WriteLine("powerlevelsquare 21,61, 42 (30) : " & PowerlevelSquare(21, 61, 42, cache))
        clearcache(cache)
        Console.WriteLine("powerleveledge 3, 5, 1, 8 (4) : " & PowerlevelEdge(3, 5, 1, 8, cache))
        clearcache(cache)
        Dim max As Integer = -100
        Dim maxall As Long = -100
        Dim xmax = 0
        Dim ymax = 0
        Dim xmaxall = 0
        Dim ymaxall = 0
        Dim smaxall = 0
        For i = 1 To 300
            For j = 1 To 300
                Dim pl As Long = 0
                For s = 1 To Math.Min(301 - i, 301 - j)
                    pl += PowerlevelEdge(i, j, s, serial, cache)
                    If (pl > max) And (s = 3) Then
                        max = pl
                        xmax = i
                        ymax = j
                    End If
                    If pl > maxall Then
                        maxall = pl
                        xmaxall = i
                        ymaxall = j
                        smaxall = s
                    End If
                Next
            Next
        Next

        Console.WriteLine("max powerlevel 3 : " & max & " at (" & xmax & ", " & ymax & ")")
        Console.WriteLine("max powerlevel all : " & maxall & " at (" & xmaxall & ", " & ymaxall & ", " & smaxall & ")")
    End Sub

    Sub clearcache(ByRef cache(,) As Long)
        For i = 1 To 300
            For j = 1 To 300
                cache(i, j) = -20
            Next
        Next
    End Sub
    Function Powerlevel(ByVal x As Long, ByVal y As Long, ByVal serial As Long, ByRef cache(,) As Long) As Long
        Dim powlev = cache(x, y)
        If powlev = -20 Then
            Dim rackid = x + 10
            powlev = y * rackid
            powlev += serial
            powlev *= rackid
            powlev = (powlev Mod 1000) \ 100
            powlev -= 5
            cache(x, y) = powlev
        End If
        Return powlev
    End Function

    Function PowerlevelSquare(ByVal x As Long, ByVal y As Long, ByVal serial As Long, ByRef cache(,) As Long) As Long
        Dim powlev = 0
        For i = 0 To 2
            For j = 0 To 2
                powlev += Powerlevel(x + i, y + j, serial, cache)
            Next
        Next
        Return powlev
    End Function

    Function PowerlevelEdge(ByVal x As Long, ByVal y As Long, ByVal size As Long, ByVal serial As Long, ByRef cache(,) As Long) As Long
        Dim powlev = Powerlevel(x + size - 1, y + size - 1, serial, cache)
        For i = 0 To size - 2
            powlev += Powerlevel(x + i, y + size - 1, serial, cache) + Powerlevel(x + size - 1, y + i, serial, cache)
        Next

        Return powlev
    End Function

End Module
