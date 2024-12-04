Imports System.Collections
Imports System.Dynamic
Imports System.Formats
Imports System.IO
Imports System.Runtime.Intrinsics.X86
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example.txt"
        'Const fileName = "..\..\..\example1.txt"
        'Const fileName = "..\..\..\example2.txt"
        'Const fileName = "..\..\..\example3.txt"
        Const fileName = "E:\develop\advent-of-code-input\2018\Day15.txt"

        Dim attackpower = 3
        Dim outcome = PlayBattle(fileName, attackpower)

        Console.WriteLine("Total score1: " & outcome.Item1 & ", rounds: " & outcome.item4)

        Console.ReadKey()

        While outcome.Item2 > 0
            attackpower += 1
            outcome = PlayBattle(fileName, attackpower)
        End While
        Console.WriteLine("")

        Console.WriteLine("Total score1: " & outcome.Item1 & ", rounds: " & outcome.Item4)


    End Sub

    Function PlayBattle(ByVal fileName As String, ByVal elfAttackPower As Integer) As Tuple(Of Integer, Integer, Integer, Integer)
        Dim field = File.ReadLines(fileName).Select(Function(t) t.ToList()).ToList()
        Dim totalG = 0
        Dim totalE = 0

        Dim hitpoints As New Dictionary(Of Tuple(Of Integer, Integer), Integer)()
        For py = 0 To field.Count - 1
            For px = 0 To field(py).Count - 1
                'check if the field contains a player
                If field(py)(px) = "G" Or field(py)(px) = "E" Then
                    hitpoints.Add(New Tuple(Of Integer, Integer)(px, py), 200)
                    If field(py)(px) = "G" Then totalG += 1 Else totalE += 1
                End If
            Next
        Next

        Dim startE = totalE
        Dim startG = totalG

        Dim round = 0
        Dim lastroundbroken = False
        While totalG > 0 And totalE > 0
            round += 1
            lastroundbroken = False
            'play a roud
            Dim players As New List(Of Tuple(Of Integer, Integer))()
            For py = 0 To field.Count - 1
                For px = 0 To field(py).Count - 1
                    'check if the field contains a player
                    If field(py)(px) = "G" Or field(py)(px) = "E" Then
                        players.Add(New Tuple(Of Integer, Integer)(px, py))
                    End If
                Next
            Next

            For Each player In players
                Dim x = player.Item1
                Dim y = player.Item2
                'check if the field contains a player
                If field(y)(x) = "G" Or field(y)(x) = "E" Then
                    Dim newposition = player
                    'check if it already touches another player
                    Dim enemy As Char = IIf(field(y)(x) = "G", "E", "G")
                    Dim touch As Boolean = (x > 0 And field(y)(x - 1) = enemy) OrElse
                        (x < field(y).Count - 1 And field(y)(x + 1) = enemy) OrElse
                        (y > 0 And field(y - 1)(x) = enemy) OrElse
                        (y < field.Count - 1 And field(y + 1)(x) = enemy)
                    'çheck if there are enemies left
                    If totalG = 0 Or totalE = 0 Then lastroundbroken = True
                    If Not touch Then
                        ' find  shortest path to nearest enemy
                        Dim path_to_here As List(Of List(Of Integer)) = Nothing
                        FindShortestPath(field, player, path_to_here) 'find reachable squres next to target
                        Dim locations As New HashSet(Of Tuple(Of Integer, Integer, Integer))()
                        For y2 = 0 To field.Count - 1
                            For x2 = 0 To field(0).Count - 1
                                If field(y2)(x2) = enemy Then
                                    If y2 > 0 AndAlso path_to_here(y2 - 1)(x2) > 0 Then locations.Add(New Tuple(Of Integer, Integer, Integer)(x2, y2 - 1, path_to_here(y2 - 1)(x2)))
                                    If x2 > 0 AndAlso path_to_here(y2)(x2 - 1) > 0 Then locations.Add(New Tuple(Of Integer, Integer, Integer)(x2 - 1, y2, path_to_here(y2)(x2 - 1)))
                                    If x2 < field(0).Count - 1 AndAlso path_to_here(y2)(x2 + 1) > 0 Then locations.Add(New Tuple(Of Integer, Integer, Integer)(x2 + 1, y2, path_to_here(y2)(x2 + 1)))
                                    If y2 < field.Count - 1 AndAlso path_to_here(y2 + 1)(x2) > 0 Then locations.Add(New Tuple(Of Integer, Integer, Integer)(x2, y2 + 1, path_to_here(y2 + 1)(x2)))
                                End If
                            Next
                        Next
                        If locations.Count > 0 Then
                            Dim min = locations.Min(Function(t) t.Item3)
                            Dim candidates = locations.Where(Function(t) t.Item3 = min).ToList()
                            Dim finalcandidate = candidates.First()
                            For Each locat In candidates
                                If (locat.Item2 < finalcandidate.Item2) Or (locat.Item2 = finalcandidate.Item2 And locat.Item1 < finalcandidate.Item1) Then finalcandidate = locat
                            Next

                            Dim path_to_here1 As List(Of List(Of Integer)) = Nothing
                            FindShortestPath(field, New Tuple(Of Integer, Integer)(finalcandidate.Item1, finalcandidate.Item2), path_to_here1) 'find reachable squres next to target
                            If y > 0 AndAlso path_to_here1(y - 1)(x) = min - 1 Then
                                newposition = New Tuple(Of Integer, Integer)(x, y - 1)
                            ElseIf x > 0 AndAlso path_to_here1(y)(x - 1) = min - 1 Then
                                newposition = New Tuple(Of Integer, Integer)(x - 1, y)
                            ElseIf x < field(0).Count - 1 AndAlso path_to_here1(y)(x + 1) = min - 1 Then
                                newposition = New Tuple(Of Integer, Integer)(x + 1, y)
                            ElseIf y < field.Count - 1 AndAlso path_to_here1(y + 1)(x) = min - 1 Then
                                newposition = New Tuple(Of Integer, Integer)(x, y + 1)
                            End If

                            'move
                            field(newposition.Item2)(newposition.Item1) = field(y)(x)
                            field(y)(x) = "."
                            hitpoints(newposition) = hitpoints(player)
                            hitpoints.Remove(player)
                        End If
                    End If
                    'check if we need to attack
                    x = newposition.Item1
                    y = newposition.Item2
                    Dim target As Tuple(Of Integer, Integer) = Nothing
                    If (y > 0 And field(y - 1)(x) = enemy) Then target = New Tuple(Of Integer, Integer)(x, y - 1)
                    If (x > 0 And field(y)(x - 1) = enemy) Then
                        Dim newT = New Tuple(Of Integer, Integer)(x - 1, y)
                        If target Is Nothing OrElse hitpoints(target) > hitpoints(newT) Then target = newT
                    End If
                    If (x < field(y).Count - 1 And field(y)(x + 1) = enemy) Then
                        Dim newT = New Tuple(Of Integer, Integer)(x + 1, y)
                        If target Is Nothing OrElse hitpoints(target) > hitpoints(newT) Then target = newT
                    End If
                    If (y < field.Count - 1 And field(y + 1)(x) = enemy) Then
                        Dim newT = New Tuple(Of Integer, Integer)(x, y + 1)
                        If target Is Nothing OrElse hitpoints(target) > hitpoints(newT) Then target = newT
                    End If

                    If Not target Is Nothing Then
                        'attack!!
                        If enemy = "G" Then
                            hitpoints(target) = hitpoints(target) - elfAttackPower
                        Else
                            hitpoints(target) = hitpoints(target) - 3
                        End If
                        If hitpoints(target) <= 0 Then
                            'defeated
                            field(target.Item2)(target.Item1) = "."
                            hitpoints.Remove(target)
                            If enemy = "G" Then totalG -= 1 Else totalE -= 1
                        End If
                    End If
                End If

            Next


            'print the field
            Console.SetCursorPosition(0, 0)
            Console.WriteLine("------------- round " & round & "power " & elfAttackPower & " -----------------")
            For j = 0 To field.Count - 1
                Dim disphit As String = ""
                For i = 0 To field(0).Count - 1
                    Console.Write(field(j)(i))
                    If field(j)(i) = "E" Or field(j)(i) = "G" Then
                        disphit &= " " & field(j)(i) & ":" & hitpoints(New Tuple(Of Integer, Integer)(i, j))
                    End If
                Next
                Console.WriteLine(disphit & "                   ")
            Next
            'Threading.Thread.Sleep(200)
            'Console.ReadKey()
        End While
        If lastroundbroken Then round -= 1
        Dim outcome = round * hitpoints.Values.Sum()
        Return New Tuple(Of Integer, Integer, Integer, Integer)(outcome, startE - totalE, startG - totalG, round)
    End Function

    Private Sub FindShortestPath(field As List(Of List(Of Char)), player As Tuple(Of Integer, Integer), ByRef path_to_here As List(Of List(Of Integer)))
        Dim x = player.Item1
        Dim y = player.Item2

        path_to_here = New List(Of List(Of Integer))()
        For i = 0 To field.Count - 1
            Dim line = New List(Of Integer)(field(0).Count)
            For j = 0 To field(0).Count - 1
                line.Add(-1) 'not visited
            Next
            path_to_here.Add(line)
        Next
        Dim work As New Queue(Of Tuple(Of Integer, Integer))()
        path_to_here(y)(x) = 0
        work.Enqueue(player)
        While work.Count > 0
            Dim cur = work.Dequeue()
            Dim x1 = cur.Item1
            Dim y1 = cur.Item2
            ' left, right, down, up
            If x1 > 0 AndAlso field(y1)(x1 - 1) = "." AndAlso path_to_here(y1)(x1 - 1) = -1 Then
                path_to_here(y1)(x1 - 1) = path_to_here(y1)(x1) + 1
                Dim tpl As New Tuple(Of Integer, Integer)(x1 - 1, y1)
                work.Enqueue(tpl)
            End If
            If x1 < field(0).Count - 1 AndAlso field(y1)(x1 + 1) = "." AndAlso path_to_here(y1)(x1 + 1) = -1 Then
                path_to_here(y1)(x1 + 1) = path_to_here(y1)(x1) + 1
                Dim tpl As New Tuple(Of Integer, Integer)(x1 + 1, y1)
                work.Enqueue(tpl)
            End If
            If y1 > 0 AndAlso field(y1 - 1)(x1) = "." AndAlso path_to_here(y1 - 1)(x1) = -1 Then
                path_to_here(y1 - 1)(x1) = path_to_here(y1)(x1) + 1
                Dim tpl As New Tuple(Of Integer, Integer)(x1, y1 - 1)
                work.Enqueue(tpl)
            End If
            If y1 < field(0).Count - 1 AndAlso field(y1 + 1)(x1) = "." AndAlso path_to_here(y1 + 1)(x1) = -1 Then
                path_to_here(y1 + 1)(x1) = path_to_here(y1)(x1) + 1
                Dim tpl As New Tuple(Of Integer, Integer)(x1, y1 + 1)
                work.Enqueue(tpl)
            End If
        End While
    End Sub
End Module
