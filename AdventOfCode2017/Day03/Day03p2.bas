#RetroDevStudio.MetaData.BASIC:2049,BASIC V2
5 SYS58692
10 PRINT"AOC 2017 - DAY 03 - PART 2"
20 READR
21 GOSUB220
22 IFR=-1THENEND
25 NU=1:X=0:Y=0:L=0:D=1:S=0
30 IFNU>RGOTO100
40 A=NU:X1=X:Y1=Y:GOSUB200
50 IFD=1ANDX=LANDY=LTHENL=L+1:X=X+1:D=2:GOTO60
51 IFD=1ANDX<LTHENX=X+1:GOTO60
52 IFD=2ANDY=-LTHENX=X-1:D=3:GOTO60
53 IFD=2ANDY>-LTHENY=Y-1:GOTO60
54 IFD=3ANDX=-LTHENY=Y+1:D=4:GOTO60
55 IFD=3ANDX>-LTHENX=X-1:GOTO60
56 IFD=4ANDY=LTHENX=X+1:D=1:GOTO60
57 IFD=4ANDY<LTHENY=Y+1
60 X1=X-1:Y1=Y:GOSUB210
61 NU=A:X1=X+1:GOSUB210
62 NU=NU+A:Y1=Y-1:GOSUB210
63 NU=NU+A:X1=X:GOSUB210
64 NU=NU+A:X1=X-1:GOSUB210
65 NU=NU+A:Y1=Y+1:GOSUB210
66 NU=NU+A:X1=X:GOSUB210
67 NU=NU+A:X1=X+1:GOSUB210
68 NU=NU+A:S=S+1
70 PRINT"LEVEL:";L;"SEQUENCE:";S;"VALUE:"NU
80 GOTO30
100 A=NU:X1=X:Y1=Y:GOSUB200
102 GOSUB321
105 PRINT"NEXT NUMBER AFTER:";R;"IS:";NU
110 GOTO20
199 REM STORE VALUES IN AN ARRAYOF 11X11
200 GR=(Y1*11+X1)*3+51200
201 H2=INT(A/65536):L2=A-H2*65536
202 H1=INT(L2/256):H0=L2-H1*256
203 POKEGR,H2:POKEGR+1,H1:POKEGR+2,H0
205 RETURN
210 GR=(Y1*11+X1)*3+51200
211 A=(PEEK(GR)*256+PEEK(GR+1))*256+PEEK(GR+2)
212 RETURN
219 REM INITIALIZE
220 A=0
221 FORY1=-5TO5
222 FORX1=-5TO5
223 GOSUB200
224 NEXT
225 NEXT
226 RETURN
321 FORX1=-5TO5
322 FORY1=-5TO5
323 GOSUB210
326 L$=L$+STR$(A)
327 NEXT
328 PRINTL$
329 L$="":NEXT
330 RETURN
900 DATA 950
901 REM DATA 368078
902 DATA -1