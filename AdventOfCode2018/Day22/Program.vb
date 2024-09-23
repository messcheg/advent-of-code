Imports System.Collections
Imports System.Data.SqlTypes
Imports System.Dynamic
Imports System.Formats
Imports System.IO
Imports System.Reflection.Emit
Imports System.Reflection.Metadata
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
        For y = 0 To target.y + target.x
            For x = 0 To target.x + target.y
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

                If x = target.x And y = target.y Then
                    c = "."
                ElseIf x <= target.x And y <= target.y Then
                    risklevel += rl
                End If

                geolevels.Add((x, y), (idx, lvl, c))
                '  Console.Write(c)

            Next
            ' Console.WriteLine()
        Next
        Console.WriteLine("Risklevel: " & risklevel)

        'rocky (.): torch or gear
        'wet   (=): gear or nothing
        'narrow(|): torch or nothing
        'move = 1 minute
        'switch = 7 minutes

        Dim work As New Dictionary(Of (loc As (x As Integer, y As Integer), currenttool As Char), Long)()
        Dim visited As New Dictionary(Of (loc As (x As Integer, y As Integer), tool As Char), Long)()
        Dim location = (x:=0, y:=0)
        work.Add(((0, 0), "T"), 0)
        Dim ready As Boolean = False
        While Not ready
            Dim workitem = work.First()
            For Each item In work
                If OrderWork(item, workitem, target) < 0 Then workitem = item
            Next
            visited.Add(workitem.Key, workitem.Value)
            work.Remove(workitem.Key)

            ready = target.x = workitem.Key.loc.x And target.y = workitem.Key.loc.y And workitem.Key.currenttool = "T"
            'find new work
            If Not ready Then
                Dim wx = workitem.Key.loc.x
                Dim wy = workitem.Key.loc.y
                Dim wct = workitem.Key.currenttool
                Dim time = workitem.Value
                Dim wmat = geolevels(workitem.Key.loc).material
                Dim wpos = tools(wmat)
                Dim otool = IIf(wpos(0) = wct, wpos(1), wpos(0))
                TryAddWork((workitem.Key.loc, otool), time + 7, work, visited)

                Dim loc = (wx + 1, wy)
                Dim newmat As Char
                getmaterial(loc, geolevels, depth, newmat)
                Dim possibilities = tools(newmat)
                If (possibilities(0) = wct Or possibilities(1) = wct) Then TryAddWork((loc, wct), time + 1, work, visited)

                loc = (wx, wy + 1)
                getmaterial(loc, geolevels, depth, newmat)
                possibilities = tools(newmat)
                If (possibilities(0) = wct Or possibilities(1) = wct) Then TryAddWork((loc, wct), time + 1, work, visited)

                If wx > 0 Then
                    loc = (wx - 1, wy)
                    getmaterial(loc, geolevels, depth, newmat)
                    possibilities = tools(newmat)
                    If (possibilities(0) = wct Or possibilities(1) = wct) Then TryAddWork((loc, wct), time + 1, work, visited)
                End If

                If wy > 0 Then
                    loc = (wx, wy - 1)
                    getmaterial(loc, geolevels, depth, newmat)
                    possibilities = tools(newmat)
                    If (possibilities(0) = wct Or possibilities(1) = wct) Then TryAddWork((loc, wct), time + 1, work, visited)
                End If
            End If
        End While

        Console.WriteLine("Minimum time : " & visited((target, "T")))
    End Sub

    Sub TryAddWork(key As (loc As (x As Integer, y As Integer), currenttool As Char),
                   newval As Long,
                   work As Dictionary(Of (loc As (x As Integer, y As Integer), currenttool As Char), Long),
                   visited As Dictionary(Of (loc As (x As Integer, y As Integer), currenttool As Char), Long))
        Dim val As Long
        If Not visited.ContainsKey(key) And (Not work.TryGetValue(key, val) Or val > newval) Then
            work(key) = newval
        End If
    End Sub

    Function OrderWork(x As KeyValuePair(Of (loc As (x As Integer, y As Integer), currenttool As Char), Long),
                       y As KeyValuePair(Of (loc As (x As Integer, y As Integer), currenttool As Char), Long),
                       target As (x As Integer, y As Integer)) As Integer
        If x.Value < y.Value Then Return -1
        If x.Value > y.Value Then Return 1
        'x.time=y.time
        If x.Key.currenttool = "T" And y.Key.currenttool <> "T" Then Return -1
        If y.Key.currenttool = "T" And x.Key.currenttool <> "T" Then Return 1
        If x.Key.currenttool = "G" And y.Key.currenttool = "N" Then Return -1
        If y.Key.currenttool = "G" And x.Key.currenttool = "N" Then Return 1
        'x.tool = y.tool
        Dim mandistX = Math.Abs(x.Key.loc.x - target.x) + Math.Abs(x.Key.loc.y - target.y)
        Dim mandistY = Math.Abs(y.Key.loc.x - target.x) + Math.Abs(y.Key.loc.y - target.y)
        Return mandistX.CompareTo(mandistY)
    End Function

    Function tools(field As Char) As String
        Select Case field
            Case "." : Return "TG"
            Case "|" : Return "TN"
            Case "=" : Return "GN"
        End Select
        Return ""
    End Function

    Sub getmaterial(location As (x As Integer, y As Integer),
                    geolevels As Dictionary(Of (x As Integer, y As Integer), (Index As Long, ErosionLevel As Long, material As Char)),
                    depth As Integer,
                    ByRef material As Char)
        If Not geolevels.ContainsKey(location) Then
            For y = 0 To location.y
                For x = 0 To location.x
                    If Not geolevels.ContainsKey((x, y)) Then
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
                    End If
                Next
            Next


        End If
        material = geolevels(location).material
    End Sub

End Module
