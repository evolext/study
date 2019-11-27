.386
.MODEL FLAT

.DATA
a DQ 0.55
b DQ 0.1
x0 DQ 0.8
x DQ ?
temp DQ ?
temp2 DQ ?

.CODE
; ���������� �������� ������� tg(0.55x+0.1) - x^2
FUNCTION PROC
	FST temp		; ST(0) = x
	FLD a			; ST(0) = 0.55
	FMULP			; ST(0) = 0.55x
	FLD b			; ST(0) = 0.1
	FADDP			; ST(0) = 0.55x + 0.1
	FPTAN			; ST(1) = tg(0.55x + 0.1)
	FMUL temp		; ST(0) = x
	FMUL temp		; ST(0) = x^2
	FSUB			; ST(0) = tg(0.55x + 0.1) - x^2
	RET	
FUNCTION ENDP

; ���������� ����� ��������
ITERATION PROC
	FST temp2		; temp2 = x
	CALL FUNCTION	; ST = f(x)
	FLD ST			; ST = f(x)   ST(1) = f(x)
	FMUL ST, ST(1)	; ST = (f(x))^2   ST(1) = f(x)
	FXCH ST(1)		; ST = f(x)   ST(1) = (f(x))^2
	FLD1			; ST = 1.0    ST(1) = f(x)   ST(2) = (f(x))^2
	FMUL ST, ST(1)	; ST = f(x)   
	FLD temp2		; ST = x   ST(1) = f(x)
	FADDP			; ST = x + f(x)    ST(1) = f(x)   ST(2) = (f(x))^2
	CALL FUNCTION	; ST = f(x + f(x))   ST(1) = f(x)   ST(2) = (f(x))^2
	FSUBRP			; ST = f(x + f(x)) - f(x)    ST(1) = (f(x))^2
	FDIVP ST(1), ST	; ST = (f(x))^2 / (f(x + f(x)) - f(x))
	FLD temp2		; ST = x	ST(1) = (f(x))^2 / (f(x + f(x)) - f(x))
	FSUBRP			; ST = ����� x
	RET
ITERATION ENDP

; �������� ���������
_SOLVE PROC
	PUSH EBP			; ��������� �������� ��������� ��������
	MOV EBP, ESP
	MOV EDX, [EBP] + 16 ; ������ ������, ���� ��������� ���������

	XOR EAX, EAX		; � EAX �������� ����� ���������� ��������
	INC EAX
	FINIT
	FLD qword ptr [EBP] + 8 ; ������ � ���� ��������� �������� ������
	FLD x0
	FST x ; x = x0

	; �������� ����
	cycle:
		CALL FUNCTION ; ��������� f(x)
		FABS		  ; |f(x)|
		FCOMIP ST, ST(1) ; ��������� �������� � �������� ������������
		JBE exit		 ; ���� ������������� �������, �� ������� �� �����
		FLD x			 ; ��������� �������� x
		CALL ITERATION   ; ������� x ����� ��������
		FST x       
		INC EAX
	JMP cycle

	exit:
	FLD x	; ���������� �������
	FSTP qword ptr [EDX] ; ��������� ������� �������� � �������� �� �������� ��������� ����������

	; ���������� �������� ��������� ��������
	POP EBP
	RET
_SOLVE ENDP

END