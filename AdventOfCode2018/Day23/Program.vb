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
        ' Const fileName = "..\..\..\example1.txt"
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
        Dim dist = GetBestSpot(pods)

        Console.WriteLine(" Distance to best spot " & dist)

    End Sub


    Private Function GetBestSpot(pods As Dictionary(Of (x As Long, y As Long, z As Long), Long)) As Long
        Dim podlist = pods.Select(Function(f) (pos:=f.Key, r:=f.Value)).ToList()

        Dim podsSortedByXaxis = podlist.OrderBy(Function(x) x.pos.x - x.r).ToList()
        Dim podsSortedByEndXaxis = podlist.OrderBy(Function(x) x.pos.x + x.r).ToList()
        Dim notabovex As Integer = 1001
        Dim topcount = 0
        Dim toppods As New List(Of (pos As (x As Long, y As Long, z As Long), r As Long))

        While notabovex > topcount
            Dim maxcount = 0
            Dim maxpods As List(Of (pos As (x As Long, y As Long, z As Long), r As Long))

            Dim currentpods As New List(Of (pos As (x As Long, y As Long, z As Long), r As Long))()
            Dim xstarted = 0
            Dim xended = 0
            Dim bestpos As (x As Long, y As Long, z As Long) = (0, 0, 0)
            Dim bestposdist As Long = 0
            While xended < podlist.Count
                If xstarted < podlist.Count AndAlso podsSortedByXaxis(xstarted).pos.x - podsSortedByXaxis(xstarted).r <= podsSortedByEndXaxis(xended).pos.x + podsSortedByEndXaxis(xended).r Then
                    currentpods.Add(podsSortedByXaxis(xstarted))
                    xstarted += 1
                Else
                    currentpods.Remove(podsSortedByEndXaxis(xended))
                    xended += 1
                End If

                If currentpods.Count > maxcount And currentpods.Count > topcount And currentpods.Count < notabovex Then
                    maxcount = currentpods.Count
                    maxpods = currentpods.ToList()
                End If

            End While
            currentpods = maxpods
            notabovex = maxcount

            Dim podsSortedByYaxis = currentpods.OrderBy(Function(y) y.pos.y - y.r).ToList()
            Dim podsSortedByEndYaxis = currentpods.OrderBy(Function(y) y.pos.y + y.r).ToList()

            Dim currentypods As New List(Of (pos As (x As Long, y As Long, z As Long), r As Long))()
            Dim ystarted = 0
            Dim yended = 0
            maxcount = 0
            While yended < currentpods.Count
                If ystarted < currentpods.Count AndAlso podsSortedByYaxis(ystarted).pos.y - podsSortedByYaxis(ystarted).r <= podsSortedByEndYaxis(yended).pos.y + podsSortedByEndYaxis(yended).r Then
                    currentypods.Add(podsSortedByYaxis(ystarted))
                    ystarted += 1
                Else
                    currentypods.Remove(podsSortedByEndYaxis(yended))
                    yended += 1
                End If

                If currentypods.Count > maxcount Then
                    maxcount = currentypods.Count
                    maxpods = currentypods.ToList()
                End If
            End While

            currentypods = maxpods

            Dim podsSortedByZaxis = currentypods.OrderBy(Function(z) z.pos.z - z.r).ToList()
            Dim podsSortedByEndZaxis = currentypods.OrderBy(Function(z) z.pos.z + z.r).ToList()

            Dim currentzpods As New List(Of (pos As (x As Long, y As Long, z As Long), r As Long))()
            Dim zstarted = 0
            Dim zended = 0
            maxcount = 0
            While zended < currentypods.Count
                If zstarted < currentypods.Count AndAlso podsSortedByZaxis(zstarted).pos.z - podsSortedByZaxis(zstarted).r <= podsSortedByEndZaxis(zended).pos.z + podsSortedByEndZaxis(zended).r Then
                    currentzpods.Add(podsSortedByZaxis(zstarted))
                    zstarted += 1
                Else
                    currentzpods.Remove(podsSortedByEndZaxis(zended))
                    zended += 1
                End If
                If currentzpods.Count > maxcount Then
                    maxcount = currentzpods.Count
                    maxpods = currentzpods.ToList()
                End If

            End While

            If maxcount > topcount Then
                topcount = maxcount
                toppods = maxpods
            End If
        End While



        Dim dist As Long = 0
        Dim finalcnt = 0
        Dim finalmxcnt = 0
        Dim cnta = 0
        Dim cntr = 0
        Dim add = toppods.Select(Function(p) Math.Max(0, Math.Abs(p.pos.x) + Math.Abs(p.pos.y) + Math.Abs(p.pos.z) - p.r)).Order().ToList()
        Dim remo = toppods.Select(Function(p) Math.Max(0, Math.Abs(p.pos.x) + Math.Abs(p.pos.y) + Math.Abs(p.pos.z) + p.r)).Order().ToList()
        While cnta < add.Count
            If add(cnta) <= remo(cntr) Then
                cnta += 1
                finalcnt += 1
                If finalcnt > finalmxcnt Then
                    dist = add(cnta - 1)
                    finalmxcnt = finalcnt
                End If
            Else
                cntr += 1
                finalcnt -= 1
            End If
        End While

        Return dist
    End Function





End Module
