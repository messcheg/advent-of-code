Imports System.Collections
Imports System.Data.SqlTypes
Imports System.Dynamic
Imports System.Formats
Imports System.IO
Imports System.Reflection.Emit
Imports System.Runtime.Intrinsics.X86
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        'Dim depth As Integer = 510
        'Dim target = (x:=10, y:=10)

        Dim depth As Integer = 4848
        Dim target = (x:=15, y:=700)

        Dim risklevel As Long = 0
        Dim geolevels As New Dictionary(Of (x As Integer, y As Integer), (Index As Long, ErosionLevel As Long, material As Char))
        For y = 0 To target.y
            For x = 0 To target.x
                Dim idx As Long = 0
                If x = 0 Then
                    idx = 48271 * y
                ElseIf y = 0 Then
                    idx = 16807 * x
                Else
                    idx = geolevels((x - 1, y)).ErosionLevel * geolevels((x, y - 1)).ErosionLevel
                End If
                Dim lvl As Long = (idx + depth) Mod 20183
                Dim c As Char = " "
                Dim rl = lvl Mod 3
                Select Case rl
                    Case 0 : c = "."
                    Case 1 : c = "="
                    Case 2 : c = "|"
                End Select

                geolevels.Add((x, y), (idx, lvl, c))
                '  Console.Write(c)

                If x <> target.x Or y <> target.y Then risklevel += rl
                'risklevel += rl
            Next
            ' Console.WriteLine()
        Next
        Console.WriteLine("Risklevel: " & risklevel)
    End Sub



End Module
