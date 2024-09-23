Imports System
Imports System.Collections
Imports System.Dynamic
Imports System.Formats
Imports System.IO
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example.txt"
        Const fileName = "E:\develop\advent-of-code-input\2018\Day12.txt"
        Console.SetBufferSize(120, 1000)
        Console.SetWindowSize(120, 40)

        Dim answer1 As Long = 0
        Dim answer2 As Long = 0

        Dim input = File.ReadLines(fileName).ToList()
        Dim startpods As String = input(0).Substring(15)
        Dim rules As New HashSet(Of String)()
        For p = 2 To input.Count - 1
            If input(p).Last = "#" Then rules.Add(input(p).Substring(0, 5))
        Next

        Dim Seenbefore As New Dictionary(Of String, Tuple(Of Long, Long))()

        Dim A1_pods As String = ""
        Dim A1offset As Long = 0

        Dim offset As Long = 5
        Dim pods = "....." & startpods & "....."
        Seenbefore.Add(pods, New Tuple(Of Long, Long)(0, 5))
        Dim jumped = False
        Dim i As Long = 1
        Dim total As Long = 50000000000
        While i <= total
            Dim newpods As New List(Of Char)(pods.Length + 4)
            newpods.Add(".")
            newpods.Add(".")
            offset += 2
            newpods.Add(".")
            newpods.Add(".")
            For j = 0 To pods.Length - 5
                If (rules.Contains(pods.Substring(j, 5))) Then
                    newpods.Add("#")
                Else
                    newpods.Add(".")
                End If
            Next
            newpods.Add(".")
            newpods.Add(".")
            newpods.Add(".")
            newpods.Add(".")
            Dim cntdots = 0
            While (cntdots < newpods.Count) And (newpods(cntdots) = ".")
                cntdots += 1
            End While
            Dim startdots = cntdots - 5
            cntdots = 0
            While (cntdots < newpods.Count) And (newpods(newpods.Count - cntdots - 1) = ".")
                cntdots += 1
            End While
            Dim enddots = cntdots - 5
            offset -= startdots
            pods = newpods.GetRange(startdots, newpods.Count - startdots - enddots).ToArray()
            'Console.WriteLine("[" & i & "] " & offset & ": " & pods)
            If i = 20 Then
                A1offset = offset
                A1_pods = pods
            End If
            If Not Seenbefore.ContainsKey(pods) Then
                Seenbefore.Add(pods, New Tuple(Of Long, Long)(i, offset))
            ElseIf Not jumped Then
                jumped = True
                Dim lasttime = Seenbefore(pods)
                Dim diffnum = i - lasttime.Item1
                Dim diffofffset = offset - lasttime.Item2
                Dim leap = (total - i) \ diffnum
                i += leap * diffnum
                offset += leap * diffofffset
            End If

            i += 1
        End While

        Dim podcount = 0
        Dim curent = -A1offset
        For Each pod In A1_pods
            If pod = "#" Then podcount += curent
            curent += 1
        Next

        Console.WriteLine("Answer1 = " & podcount)

        Dim podcount1 As Long = 0
        Dim curent1 = -offset
        For Each pod In pods
            If pod = "#" Then podcount1 += curent1
            curent1 += 1
        Next

        Console.WriteLine("Answer2 = " & podcount1)
    End Sub
End Module
