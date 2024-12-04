Imports System.Collections
Imports System.Dynamic
Imports System.Formats
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example.txt"
        Const fileName = "E:\develop\advent-of-code-input\2018\Day13.txt"

        Dim answer1 As Long = 0
        Dim answer2 As Long = 0

        Dim input = File.ReadLines(fileName).ToList()
        Dim carts As New List(Of Cart)

        'find the carts and create an emptyy map
        Dim map As New List(Of String)
        Dim y As Integer = 0
        For Each line In input
            Dim newLine As String = ""
            For x = 0 To line.Length - 1
                Dim c As Char = line(x)
                If c = "v" Or c = "^" Or c = "<" Or c = ">" Then
                    Dim crt = New Cart
                    crt.id = carts.Count
                    crt.x = x
                    crt.y = y
                    crt.direction = c
                    carts.Add(crt)
                    newLine &= IIf(c = "v" Or c = "^", "|", "-")
                Else
                    newLine &= c
                End If
            Next
            map.Add(newLine)
            y += 1
        Next

        Dim seconds As Integer = 0
        Dim crashed As Boolean = False
        Dim crashCart As Cart = Nothing
        While carts.Count > 1
            Dim from_to As New List(Of Tuple(Of Integer, Tuple(Of Integer, Integer), Tuple(Of Integer, Integer)))
            For Each crt In carts
                Dim from As New Tuple(Of Integer, Integer)(crt.x, crt.y)
                Dim curtrack As Char = map(crt.y)(crt.x)
                Select Case curtrack & crt.direction
                    Case "|v"
                        crt.y += 1
                    Case "|^"
                        crt.y -= 1
                    Case "->"
                        crt.x += 1
                    Case "-<"
                        crt.x -= 1
                    Case "/^"
                        crt.x += 1
                        crt.direction = ">"
                    Case "/<"
                        crt.y += 1
                            crt.direction = "v"
                    Case "/v"
                        crt.x -= 1
                            crt.direction = "<"
                    Case "/>"
                        crt.y -= 1
                            crt.direction = "^"
                    Case "\^"
                        crt.x -= 1
                            crt.direction = "<"
                    Case "\<"
                        crt.y -= 1
                            crt.direction = "^"
                    Case "\v"
                        crt.x += 1
                            crt.direction = ">"
                    Case "\>"
                        crt.y += 1
                            crt.direction = "v"
                    Case "+v", "+^", "+<", "+>"
                        Select Case crt.nextAction
                            Case "L"
                                crt.nextAction = "S"
                                Select Case crt.direction
                                    Case "^"
                                        crt.direction = "<"
                                        crt.x -= 1
                                    Case "v"
                                        crt.direction = ">"
                                        crt.x += 1
                                    Case "<"
                                        crt.direction = "v"
                                        crt.y += 1
                                    Case ">"
                                        crt.direction = "^"
                                        crt.y -= 1
                                End Select
                            Case "R"
                                crt.nextAction = "L"
                                Select Case crt.direction
                                    Case "v"
                                        crt.direction = "<"
                                        crt.x -= 1
                                    Case "^"
                                        crt.direction = ">"
                                        crt.x += 1
                                    Case ">"
                                        crt.direction = "v"
                                        crt.y += 1
                                    Case "<"
                                        crt.direction = "^"
                                        crt.y -= 1
                                End Select
                            Case "S"
                                crt.nextAction = "R"
                                Select Case crt.direction
                                    Case "^"
                                        crt.y -= 1
                                    Case "v"
                                        crt.y += 1
                                    Case "<"
                                        crt.x -= 1
                                    Case ">"
                                        crt.x += 1
                                End Select
                        End Select
                End Select
                Dim tpl_to As New Tuple(Of Integer, Integer)(crt.x, crt.y)
                from_to.Add(New Tuple(Of Integer, Tuple(Of Integer, Integer), Tuple(Of Integer, Integer))(crt.id, from, tpl_to))
            Next
            'all same 'to'-fields need to be removed
            Dim removeloc As New HashSet(Of Tuple(Of Integer, Integer))
            Dim occupied As New HashSet(Of Tuple(Of Integer, Integer))
            For Each ft In from_to
                If occupied.Contains(ft.Item3) Then
                    removeloc.Add(ft.Item3)
                Else
                    occupied.Add(ft.Item3)
                End If
            Next

            'all fields where from-to is to-from
            Dim removeid As New HashSet(Of Integer)
            For Each ft In from_to
                If occupied.Contains(ft.Item2) Then
                    For Each ft1 In from_to
                        If ft.Item2.Equals(ft1.Item3) And ft.Item3.Equals(ft1.Item2) Then
                            removeid.Add(ft.Item1)
                            removeid.Add(ft1.Item1)
                        End If
                    Next
                End If
            Next


            Dim newcarts As New List(Of Cart)
            For Each crt In carts
                If removeid.Contains(crt.id) Or removeloc.Contains(New Tuple(Of Integer, Integer)(crt.x, crt.y)) Then
                    If Not crashed Then
                        crashCart = crt
                        crashed = True
                    End If
                Else
                    newcarts.Add(crt)
                End If
            Next

            carts = newcarts
            seconds += 1
        End While
        If Not crashCart Is Nothing Then
            Console.WriteLine("cart crashed after " & seconds & " seconds at location " & crashCart.x & "," & crashCart.y)
        End If
        If carts.Count > 0 Then
            Console.WriteLine("cart survived after " & seconds & " seconds at location " & carts(0).x & "," & carts(0).y)
        End If
    End Sub

    Class Cart
        Public id As Integer
        Public x As Integer
        Public y As Integer
        Public direction As Char
        Public nextAction As Char = "L"
    End Class

End Module

