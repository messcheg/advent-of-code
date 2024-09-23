Imports System.Collections
Imports System.ComponentModel
Imports System.Dynamic
Imports System.Formats
Imports System.Globalization
Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Reflection.Emit
Imports System.Runtime.CompilerServices
Imports System.Runtime.Intrinsics.X86
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example.txt"
        'Const fileName = "..\..\..\example1.txt"
        Const fileName = "E:\develop\advent-of-code-input\2018\Day25.txt"

        Dim filelist = File.ReadLines(fileName).Select(Function(f) f.Split(",").Select(Function(x) Integer.Parse(x))).Select(Function(f) (w:=f(0), x:=f(1), y:=f(2), z:=f(3))).ToList()

        Dim cohorts = 0
        While filelist.Count > 0
            cohorts += 1
            Dim worklist As New Queue(Of (w As Integer, x As Integer, y As Integer, z As Integer))()
            worklist.Enqueue(filelist(0))
            filelist = filelist.Skip(1).ToList()
            While worklist.Count > 0
                Dim newlist As New List(Of (w As Integer, x As Integer, y As Integer, z As Integer))()
                Dim cur = worklist.Dequeue()
                For Each item In filelist
                    If Math.Abs(item.w - cur.w) + Math.Abs(item.x - cur.x) + Math.Abs(item.y - cur.y) + Math.Abs(item.z - cur.z) <= 3 Then
                        worklist.Enqueue(item)
                    Else
                        newlist.Add(item)
                    End If
                Next
                filelist = newlist
            End While
        End While
        Console.WriteLine("Cohorts: " & cohorts)
    End Sub
End Module
