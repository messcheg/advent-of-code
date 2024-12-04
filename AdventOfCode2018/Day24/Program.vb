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
        Const fileName = "E:\develop\advent-of-code-input\2018\Day24.txt"

        Dim filelist = File.ReadLines(fileName).ToList()
        Dim infection As New Dictionary(Of Integer, (units As Long, hitpooint As Long, immunities As ULong, weaknesses As ULong, damage As Long, damagetype As ULong, initiative As Integer))
        Dim imunesystem As New Dictionary(Of Integer, (units As Long, hitpooint As Long, immunities As ULong, weaknesses As ULong, damage As Long, damagetype As ULong, initiative As Integer))
        Dim characteristics As New Dictionary(Of String, Integer)()
        Dim i As Integer = 1
        Dim col = 0
        While i < filelist.Count
            If (filelist(i).StartsWith("Infection")) Then
                col = 1
            ElseIf filelist(i) <> "" Then
                Dim words = filelist(i).Split(" units each with ")
                Dim units = Long.Parse(words(0))
                words = words(1).Split(" hit points ")
                Dim hitpoints = Long.Parse(words(0))
                Dim immunities As ULong = 0
                Dim weaknesses As ULong = 0
                If words(1).StartsWith("(") Then
                    words = words(1).Split(") ")
                    Dim health = words(0).Substring(1).Split("; ")
                    For Each h In health
                        Dim check = h.Split(" to ")
                        Dim result As ULong = 0
                        For Each p In check(1).Split(", ")
                            Dim val = 0
                            If Not characteristics.ContainsKey(p) Then
                                val = characteristics.Count
                                characteristics(p) = val
                            Else
                                val = characteristics(p)
                            End If
                            result = result Or (1 << val)
                        Next
                        If check(0) = "immune" Then immunities = result Else weaknesses = result
                    Next
                End If
                words = words(1).Split(" ")
                Dim damage = Long.Parse(words(5))
                Dim dt = words(6)
                Dim dtval = 0
                If Not characteristics.ContainsKey(dt) Then
                    dtval = characteristics.Count
                    characteristics(dt) = dtval
                Else
                    dtval = characteristics(dt)
                End If
                Dim damagetype As ULong = 1 << dtval
                Dim initiative As Integer = Integer.Parse(words(10))

                Dim unitrec = (units, hitpoints, immunities, weaknesses, damage, damagetype, initiative)
                If col = 1 Then
                    infection(initiative) = unitrec
                Else
                    imunesystem(initiative) = unitrec
                End If
            End If

            i += 1
        End While


        Dim infection1 As New Dictionary(Of Integer, (units As Long, hitpooint As Long, immunities As ULong, weaknesses As ULong, damage As Long, damagetype As ULong, initiative As Integer))
        Dim imunesystem1 As New Dictionary(Of Integer, (units As Long, hitpooint As Long, immunities As ULong, weaknesses As ULong, damage As Long, damagetype As ULong, initiative As Integer))
        For Each inf In infection : infection1.Add(inf.Key, inf.Value) : Next
        For Each imu In imunesystem : imunesystem1.Add(imu.Key, imu.Value) : Next


        PlayGame(infection1, imunesystem1)


        If imunesystem1.Count > 0 Then
            Console.WriteLine("Imunesystem won: " & imunesystem1.Select(Function(f) f.Value.units).Sum())
        Else
            Console.WriteLine("Infection won: " & infection1.Select(Function(f) f.Value.units).Sum())
        End If

        Dim powerboostUp = 4096
        Dim powerboostDown = 0
        Dim upcount = 0
        While powerboostUp > powerboostDown + 1
            Dim powerboost = (powerboostUp + powerboostDown) \ 2

            infection1 = New Dictionary(Of Integer, (units As Long, hitpooint As Long, immunities As ULong, weaknesses As ULong, damage As Long, damagetype As ULong, initiative As Integer))
            imunesystem1 = New Dictionary(Of Integer, (units As Long, hitpooint As Long, immunities As ULong, weaknesses As ULong, damage As Long, damagetype As ULong, initiative As Integer))
            For Each inf In infection : infection1.Add(inf.Key, inf.Value) : Next
            For Each imu In imunesystem
                Dim v = imu.Value
                v.damage += powerboost
                imunesystem1.Add(imu.Key, v)
            Next

            Dim result = PlayGame(infection1, imunesystem1)

            If result < 2 Then
                powerboostUp = powerboost
                upcount = imunesystem1.Select(Function(f) f.Value.units).Sum()
            Else
                powerboostDown = powerboost
            End If
        End While

        Console.WriteLine("Imunesystem won: " & upcount & " with powerboost: " & powerboostUp)


    End Sub

    Private Function PlayGame(infection As Dictionary(Of Integer, (units As Long, hitpooint As Long, immunities As ULong, weaknesses As ULong, damage As Long, damagetype As ULong, initiative As Integer)), imunesystem As Dictionary(Of Integer, (units As Long, hitpooint As Long, immunities As ULong, weaknesses As ULong, damage As Long, damagetype As ULong, initiative As Integer))) As Integer
        While infection.Count > 0 And imunesystem.Count > 0
            'target selection
            Dim targets As New Dictionary(Of Integer, Integer)
            targets = Selecttargets(infection, imunesystem, targets)
            targets = Selecttargets(imunesystem, infection, targets)

            If targets.Count = 0 Then Return 0
            Dim changed = False
            'actual fight
            For Each t In targets.OrderByDescending(Function(f) f.Key)
                Dim isAtcImu = imunesystem.ContainsKey(t.Key)
                Dim isAtcInf = Not isAtcImu AndAlso infection.ContainsKey(t.Key)
                Dim isDefImu = Not isAtcImu AndAlso imunesystem.ContainsKey(t.Value)
                Dim isDefInf = Not isAtcInf AndAlso infection.ContainsKey(t.Value)

                Dim fight = isAtcImu And isDefInf Or isAtcInf And isDefImu
                If fight Then
                    Dim atc = infection
                    Dim def = imunesystem
                    If isAtcImu Then
                        atc = imunesystem
                        def = infection
                    End If

                    Dim atcUnit = atc(t.Key)
                    Dim defUnit = def(t.Value)
                    Dim effPower = atcUnit.damage * atcUnit.units
                    Dim power = IIf((atcUnit.damagetype And defUnit.weaknesses) > 0, effPower + effPower, effPower)

                    Dim loss = power \ defUnit.hitpooint
                    defUnit.units -= loss

                    If defUnit.units <= 0 Then
                        def.Remove(defUnit.initiative)
                        changed = True
                    ElseIf loss > 0 Then
                        def(defUnit.initiative) = defUnit
                        changed = True
                    End If
                End If
            Next
            If Not changed Then Return 3
        End While
        If infection.Count > 0 Then Return 2 Else Return 1

    End Function

    Private Function Selecttargets(attackers As Dictionary(Of Integer, (units As Long, hitpooint As Long, immunities As ULong, weaknesses As ULong, damage As Long, damagetype As ULong, initiative As Integer)), defenders As Dictionary(Of Integer, (units As Long, hitpooint As Long, immunities As ULong, weaknesses As ULong, damage As Long, damagetype As ULong, initiative As Integer)), targets As Dictionary(Of Integer, Integer)) As Dictionary(Of Integer, Integer)
        Dim avalable = defenders.Keys.ToHashSet()
        For Each attack In attackers.Values.OrderByDescending(Function(f) f.units * f.damage * 100 + f.initiative)
            Dim target = defenders.Values.First()
            Dim maxdamage = 0
            Dim effectivePower = attack.damage * attack.units
            If avalable.Count > 0 Then

                For Each av In avalable
                    Dim defend = defenders(av)
                    Dim curdamage = effectivePower
                    If (attack.damagetype And defend.weaknesses) > 0 Then
                        curdamage += effectivePower
                    ElseIf (attack.damagetype And defend.immunities) > 0 Then
                        curdamage = 0
                    End If
                    If curdamage > maxdamage Or (curdamage = maxdamage And
                        (defend.damage * defend.units > target.damage * target.units Or
                          (defend.damage * defend.units = target.damage * target.units And defend.initiative > target.initiative))) Then
                        maxdamage = curdamage
                        target = defend
                    End If
                Next
                If maxdamage > 0 Then
                    targets(attack.initiative) = target.initiative
                    avalable.Remove(target.initiative)
                End If
            End If
        Next

        Return targets
    End Function
End Module
