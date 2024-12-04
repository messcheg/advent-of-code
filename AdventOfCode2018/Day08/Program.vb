Imports System
Imports System.Runtime.InteropServices
Imports System.Collections
Imports System.IO

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example_input.txt"
        Const fileName = "E:\develop\advent-of-code-input\2018\Day08.txt"

        Dim answer1 As Long = 0
        Dim answer2 As Long = 0

        Dim line As String = File.ReadLines(fileName)(0)
        Dim numbers = line.Split(" ").Select(Function(t) Int32.Parse(t)).ToArray()
        Dim ctr As Int32 = 0
        Dim root As New NavigationNode()
        BuildTree(numbers, ctr, root, answer1)

        Console.WriteLine("Answer1 = " & answer1)

        Console.WriteLine("Answer2 = " & root.Value)

    End Sub

    Class NavigationNode
        Public childs As New List(Of NavigationNode)
        Public metadata As New List(Of Int32)
        Private val As Long = -1

        Public Function Value() As Long
            If val < 0 Then
                If childs.Count = 0 Then
                    val = metadata.Sum()
                Else
                    val = 0
                    For Each num In metadata
                        If num > 0 And num <= childs.Count Then
                            val += childs(num - 1).Value()
                        End If
                    Next
                End If
            End If
            Return val
        End Function

    End Class

    Sub BuildTree(ByVal list As Int32(), ByRef ctr As Int32, curnode As NavigationNode, ByRef totalMeta As Long)
        Dim numchilds = list(ctr)
        ctr = ctr + 1
        Dim numMeta = list(ctr)
        ctr = ctr + 1
        For i = 1 To numchilds
            Dim newNode As New NavigationNode()
            curnode.childs.Add(newNode)
            BuildTree(list, ctr, newNode, totalMeta)
        Next
        For i = 1 To numMeta
            Dim curMeta = list(ctr)
            ctr += 1
            totalMeta += curMeta
            curnode.metadata.Add(curMeta)
        Next

    End Sub

End Module
