Imports System.Collections
Imports System.Dynamic
Imports System.Formats
Imports System.IO
Imports System.Reflection.Emit
Imports System.Runtime.Intrinsics.X86
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Module Program
    Sub Main(args As String())
        'Const fileName = "..\..\..\example.txt"
        Const fileName = "E:\develop\advent-of-code-input\2018\Day16.txt"

        Dim filelist = File.ReadLines(fileName).ToList()

        Dim opcodemap As New List(Of HashSet(Of Instructions))()
        For i = 0 To 15
            Dim hs As New HashSet(Of Instructions)()
            For j = Instructions.addr To Instructions.eqrr
                hs.Add(j)
            Next
            opcodemap.Add(hs)
        Next

        Dim ambcnt As Integer = 0
        Dim line As Integer = 0
        While filelist(line).StartsWith("Before")
            Dim before = filelist(line).Split("[")(1).Split("]")(0).Split(",").Select(Function(t) Long.Parse(t)).ToList()
            Dim instr = filelist(line + 1).Split(" ").Select(Function(t) Integer.Parse(t)).ToList()
            Dim after = filelist(line + 2).Split("[")(1).Split("]")(0).Split(",").Select(Function(t) Long.Parse(t)).ToList()

            Dim hs As New HashSet(Of Instructions)()
            For xx = Instructions.addr To Instructions.eqrr
                Dim res = Alu(xx, instr(1), instr(2), instr(3), before)
                If res(0) = after(0) AndAlso res(1) = after(1) AndAlso res(2) = after(2) AndAlso res(3) = after(3) Then
                    hs.Add(xx)
                End If
            Next
            If (hs.Count >= 3) Then ambcnt += 1
            opcodemap(instr(0)).IntersectWith(hs)

            If opcodemap(instr(0)).Count = 1 Then
                Dim ins = opcodemap(instr(0)).First()
                For i = 0 To 15
                    If i <> instr(0) Then opcodemap(i).Remove(ins)
                Next
            End If

            line += 4
        End While

        Dim opcode As New List(Of Instructions)(16)
        For i = 0 To 15
            opcode.Add(opcodemap(i).First())
        Next

        Console.WriteLine("Number of ambigious opcodes: " & ambcnt)

        Console.WriteLine("Found the following opcodes:")
        For i = 0 To 15
            Console.WriteLine(" " & i & " - " & opcode(i))
        Next

        'execute program
        line += 2
        Dim registers As New List(Of Long)({0, 0, 0, 0})
        While line < filelist.Count
            Dim instr = filelist(line).Split(" ").Select(Function(t) Integer.Parse(t)).ToList()
            registers = Alu(opcode(instr(0)), instr(1), instr(2), instr(3), registers)
            line += 1
        End While

        Console.WriteLine("registervalue 0 after code execution: " & registers(0))

    End Sub

    Function Alu(instruction As Instructions, A As Long, B As Long, C As Long, regs As List(Of Long)) As List(Of Long)
        Dim regs1 = regs.ToList()
        Select Case instruction
            Case Instructions.addr : regs1(C) = regs1(A) + regs1(B)
            Case Instructions.addi : regs1(C) = regs1(A) + B

            Case Instructions.mulr : regs1(C) = regs1(A) * regs1(B)
            Case Instructions.muli : regs1(C) = regs1(A) * B

            Case Instructions.banr : regs1(C) = regs1(A) And regs1(B)
            Case Instructions.bani : regs1(C) = regs1(A) And B

            Case Instructions.borr : regs1(C) = regs1(A) Or regs1(B)
            Case Instructions.bori : regs1(C) = regs1(A) Or B

            Case Instructions.setr : regs1(C) = regs1(A)
            Case Instructions.seti : regs1(C) = A

            Case Instructions.gtir : regs1(C) = IIf(A > regs1(B), 1, 0)
            Case Instructions.gtri : regs1(C) = IIf(regs1(A) > B, 1, 0)
            Case Instructions.gtrr : regs1(C) = IIf(regs1(A) > regs1(B), 1, 0)

            Case Instructions.eqir : regs1(C) = IIf(A = regs1(B), 1, 0)
            Case Instructions.eqri : regs1(C) = IIf(regs1(A) = B, 1, 0)
            Case Instructions.eqrr : regs1(C) = IIf(regs1(A) = regs1(B), 1, 0)
        End Select

        Return regs1
    End Function

    Public Enum Instructions As Integer
        addr = 0
        addi
        mulr
        muli
        banr
        bani
        borr
        bori
        setr
        seti
        gtir
        gtri
        gtrr
        eqir
        eqri
        eqrr
    End Enum



End Module
