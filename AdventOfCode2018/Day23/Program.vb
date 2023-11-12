Imports System.Collections
Imports System.Dynamic
Imports System.Formats
Imports System.Globalization
Imports System.IO
Imports System.Reflection.Emit
Imports System.Runtime.Intrinsics.X86
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example.txt"
        'Const fileName = "..\..\..\example1.txt"
        Const fileName = "E:\develop\advent-of-code-input\2018\Day23.txt"


        Dim filelist = File.ReadLines(fileName).ToList()
        Dim pods As New Dictionary(Of (x As Long, y As Long, z As Long), Long)
        Dim maxR As Long = 0
        For Each line In filelist
            Dim spl = line.Split(">, r=")
            Dim pos = spl(0).Substring(5).Split(",").Select(Function(f) Long.Parse(f))
            Dim r = Long.Parse(spl(1))
            pods((pos(0), pos(1), pos(2))) = r
            If r > maxR Then maxR = r
        Next

        Dim largest = pods.Where(Function(f) f.Value = maxR).First()
        Dim inreach = pods.Where(Function(f) Math.Abs(f.Key.x - largest.Key.x) + Math.Abs(f.Key.y - largest.Key.y) + Math.Abs(f.Key.z - largest.Key.z) <= maxR)

        Console.WriteLine("Number of bots in reach: " & inreach.Count)

        Dim maxovl As Integer = Nothing
        Dim candidates As List(Of HashSet(Of Integer)) = Nothing
        GetBestSpot2(pods, maxovl)
        Console.WriteLine("Number of candidates: " & candidates.Count & " with " & maxovl & " pods")

    End Sub

    Private Sub GetBestSpot1(pods As Dictionary(Of (x As Long, y As Long, z As Long), Long), ByRef maxovl As Integer, ByRef candidates As List(Of HashSet(Of Integer)))
        Dim podlist = pods.Select(Function(f) (pos:=f.Key, r:=f.Value)).ToList()
        Dim overlaps As New List(Of HashSet(Of Integer))
        For i = 0 To podlist.Count - 1
            overlaps.Add(New HashSet(Of Integer)({i}))
        Next
        Dim ready = False
        While (Not ready)
            Dim newovl = New List(Of HashSet(Of Integer))
            For i = 0 To podlist.Count - 1
                Dim cur = podlist(i)
                For Each li In overlaps
                    If Not li.Contains(i) Then
                        Dim allOverlap = True
                        For Each other In li
                            Dim oth = podlist(other)
                            allOverlap = allOverlap And Math.Abs(cur.pos.x - oth.pos.x) + Math.Abs(cur.pos.y - oth.pos.y) + Math.Abs(cur.pos.z - oth.pos.z) <= oth.r + cur.r
                        Next
                        If allOverlap Then
                            Dim newli = li.ToList()
                            newli.Add(i)
                            newovl.Add(New HashSet(Of Integer)(newli))
                        End If
                    End If
                Next
            Next
            If newovl.Count = 0 Then
                ready = True
            Else
                overlaps = newovl
            End If

        End While
        Dim maxovl1 = overlaps.Select(Function(f) f.Count).Max()
        maxovl = maxovl1
        candidates = overlaps.Where(Function(f) f.Count = maxovl1).ToList()
    End Sub

    Private Sub GetBestSpot2(pods As Dictionary(Of (x As Long, y As Long, z As Long), Long), ByRef maxovl As Integer)
        Dim podlist = pods.Select(Function(f) (pos:=f.Key, r:=f.Value)).ToList()

        Dim podsSortedByXaxis = podlist.OrderBy(Function(x) x.pos.x - x.r).ToList()
        Dim currentpods As New List(Of (pos As (x As Long, y As Long, z As Long), r As Long))()
        Dim maxxount = 0
        Dim maxpods As List(Of (pos As (x As Long, y As Long, z As Long), r As Long))
        Dim xfrom As Long = 0
        Dim xuntil As Long = 0
        For Each pod In podsSortedByXaxis
            Dim ended = currentpods.Where(Function(f) f.pos.x + f.r < pod.pos.x - pod.r).Count
            currentpods.Add(pod)
            If currentpods.Count > maxxount Then
                maxxount = currentpods.Count
                maxpods = currentpods
                xfrom = currentpods.Select(Function(f) f.pos.x - f.r).Max()
                xuntil = currentpods.Select(Function(f) f.pos.x + f.r).Min()

                Dim podsSortedByYaxis = currentpods.OrderBy(Function(y) y.pos.y - y.r).ToList()
                Dim currentypods As New List(Of (pos As (x As Long, y As Long, z As Long), r As Long))()
                For Each pody In podsSortedByYaxis
                    Dim endedy = currentypods.Where(Function(f) f.pos.y + f.r < pody.pos.y - pody.r).Count
                    If endedy > 0 Then
                        currentypods = currentypods.Where(Function(f) f.pos.y + f.r >= pody.pos.y - pody.r).ToList()
                        'hier was ik gebleven

                    End If

                Next
            End If
            If ended > 0 Then
                currentpods = currentpods.Where(Function(f) f.pos.x + f.r >= pod.pos.x - pod.r).ToList()
            End If

        Next


        maxovl = maxxount

    End Sub





End Module
