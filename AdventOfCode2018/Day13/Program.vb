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
        Dim crashCart As Cart
        Dim prevlocations As New Dictionary(Of Tuple(Of Integer, Integer), List(Of Cart))
        While carts.Count > 1
            Dim locations As New Dictionary(Of Tuple(Of Integer, Integer), List(Of Cart))
            Dim remove As New HashSet(Of Tuple(Of Integer, Integer))
            Dim newcarts As New List(Of Cart)
            For Each crt1 In carts
                Dim crt As New Cart
                crt.id = crt1.id
                crt.x = crt1.x
                crt.y = crt1.y
                crt.direction = crt1.direction
                crt.nextAction = crt1.nextAction

                Dim curtrack As Char = map(crt.y)(crt.x)
                Select Case curtrack
                    Case "|"
                        If crt.direction = "v" Then
                            crt.y += 1
                        Else
                            crt.y -= 1
                        End If
                    Case "-"
                        If crt.direction = ">" Then
                            crt.x += 1
                        Else
                            crt.x -= 1
                        End If
                    Case "/"
                        If crt.direction = "^" Then
                            crt.x += 1
                            crt.direction = ">"
                        ElseIf crt.direction = "<" Then
                            crt.y += 1
                            crt.direction = "v"
                        ElseIf crt.direction = "v" Then
                            crt.x -= 1
                            crt.direction = "<"
                        ElseIf crt.direction = ">" Then
                            crt.y -= 1
                            crt.direction = "^"
                        End If
                    Case "\"
                        If crt.direction = "^" Then
                            crt.x -= 1
                            crt.direction = "<"
                        ElseIf crt.direction = "<" Then
                            crt.y -= 1
                            crt.direction = "^"
                        ElseIf crt.direction = "v" Then
                            crt.x += 1
                            crt.direction = ">"
                        ElseIf crt.direction = ">" Then
                            crt.y += 1
                            crt.direction = "v"
                        End If
                    Case "+"
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
                Dim tpl As New Tuple(Of Integer, Integer)(crt.x, crt.y)
                If locations.ContainsKey(tpl) Then
                    locations(tpl).Add(crt)
                    remove.Add(tpl)
                    If Not crashed Then crashCart = crt
                    crashed = True
                    remove.Add(tpl)
                ElseIf prevlocations.ContainsKey(tpl) Then
                    Dim prevcart = prevlocations(tpl)(0)
                    locations(tpl).Add(crt)
                    If Not crashed Then crashCart = crt
                    crashed = True
                    remove.Add(tpl)
                Else
                    locations.Add(tpl, New List(Of Cart)({crt}))
                    newcarts.Add(crt)
                End If
            Next
            Dim removeId As New HashSet(Of Integer)
            For Each tpl1 In remove
                Dim l1 = locations(tpl1)
                For Each crt2 In l1
                    removeId.Add(crt2.id)
                Next
                locations.Remove(tpl1)
                If prevlocations.ContainsKey(tpl1) Then removeId.Add(prevlocations(tpl1)(0).id)
            Next
            prevlocations = locations
            carts = New List(Of Cart)()
            For Each crt2 In newcarts
                If Not removeId.Contains(crt2.id) Then carts.Add(crt2)
            Next
            seconds += 1
        End While
        If Not crashCart Is Nothing Then
            Console.WriteLine("cart crashed after " & seconds & " seconds at location " & crashCart.x & "," & crashCart.y)
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

