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
        Const fileName = "E:\develop\advent-of-code-input\2018\Day19.txt"

        Dim filelist = File.ReadLines(fileName).ToList()
        Dim programOp = filelist.Skip(1).Select(Function(t) [Enum].Parse(Of Instructions)(t.Substring(0, 4))).ToList()
        Dim programArg = filelist.Skip(1).Select(Function(t) t.Substring(5).Split(" ").Select(Function(x) Long.Parse(x)).ToList()).ToList()

        'execute program
        Dim registers As New List(Of Long)({0, 0, 0, 0, 0, 0})
        Dim ipreg As Integer = Integer.Parse(filelist(0).Split(" ").ToList()(1))
        Dim ip = 0
        Dim steps As Long = 0
        While ip < programOp.Count
            registers(ipreg) = ip
            registers = Alu(programOp(ip), programArg(ip)(0), programArg(ip)(1), programArg(ip)(2), registers)
            ip = registers(ipreg) + 1
            steps += 1

        End While

        Console.WriteLine("number of steps needed: " & steps)
        Console.WriteLine("registervalue 0 after code execution: " & registers(0))


        Dim r5 As Long = 10551355
        Dim r0 As Long = 0
        For r1 = 1 To r5
            If r5 Mod r1 = 0 Then
                r0 += r1
            End If
        Next
        Console.WriteLine("r0 after code execution: " & r0)

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
