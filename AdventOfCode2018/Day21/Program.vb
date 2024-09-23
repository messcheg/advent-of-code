Imports System.Collections
Imports System.Data.SqlTypes
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
        Const fileName = "E:\develop\advent-of-code-input\2018\Day21.txt"

        Dim filelist = File.ReadLines(fileName).ToList()
        Dim programOp = filelist.Skip(1).Select(Function(t) [Enum].Parse(Of Instructions)(t.Substring(0, 4))).ToList()
        Dim programArg = filelist.Skip(1).Select(Function(t) t.Substring(5).Split(" ").Select(Function(x) Long.Parse(x)).ToList()).ToList()


        'execute program
        Dim registers As New List(Of Long)({12446070, 0, 0, 0, 0, 0})
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

        ' PART II

        'translate program to VB
        For pc = 0 To programOp.Count - 1
            Console.WriteLine(translate(programOp(pc), programArg(pc)(0), programArg(pc)(1), programArg(pc)(2), ipreg, pc))
        Next


        Dim R0 As Long = 12446070
        Dim R1 As Long = 0
        Dim R2 As Long = 0
        Dim R3 As Long = 0
        Dim R5 As Long = 0
        Dim R6 As Long = 0
        Dim ready = False
        Dim repeating As New HashSet(Of Long)()

        R5 = 0
        Do
            repeating.Add(R5)
            R0 = R5
            R1 = R5 Or 65536
            R5 = 4591209
            Do
                R3 = R1 And 255
                R5 = R5 + R3
                R5 = R5 And 16777215
                R5 = R5 * 65899
                R5 = R5 And 16777215
                ready = True
                If 256 <= R1 Then
                    R1 = (R1 \ 256)
                    ready = False
                End If
            Loop Until ready
        Loop Until repeating.Contains(R5)

        Console.WriteLine("answer2: " & R0)
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

    Function translate(instruction As Instructions, A As Long, B As Long, C As Long, ipreg As Long, pc As Integer) As String

        Dim pre As String
        If C = ipreg Then pre = pc & " GOTO 1 + " Else pre = pc & " R" & C & " = "
        Dim ra As String
        If A = ipreg Then ra = pc Else ra = "R" & A
        Dim rb As String
        If B = ipreg Then rb = pc Else rb = "R" & B

        Select Case instruction
            Case Instructions.addr : Return pre & ra & " + " & rb
            Case Instructions.addi : Return pre & ra & " + " & B

            Case Instructions.mulr : Return pre & ra & " * " & rb
            Case Instructions.muli : Return pre & ra & " * " & B

            Case Instructions.banr : Return pre & ra & " AND " & rb
            Case Instructions.bani : Return pre & ra & " AND " & B

            Case Instructions.borr : Return pre & ra & " OR " & rb
            Case Instructions.bori : Return pre & ra & " OR " & B

            Case Instructions.setr : Return pre & ra
            Case Instructions.seti : Return pre & A

            Case Instructions.gtir : Return pre & "IIf(" & A & " > " & rb & ", 1, 0)"
            Case Instructions.gtri : Return pre & "IIf(" & ra & " > " & B & ", 1, 0)"
            Case Instructions.gtrr : Return pre & "IIf(" & ra & " > " & rb & ", 1, 0)"

            Case Instructions.eqir : Return pre & "IIf(" & A & " = " & rb & ", 1, 0)"
            Case Instructions.eqri : Return pre & "IIf(" & ra & " = " & B & ", 1, 0)"
            Case Instructions.eqrr : Return pre & "IIf(" & ra & " = " & rb & ", 1, 0)"
        End Select
        Return ""
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
