.386
.MODEL FLAT, STDCALL

EXTERN GetStdHandle@4 : PROC
EXTERN ReadConsoleA@20 : PROC
EXTERN ExitProcess@4: PROC
EXTERN  WriteConsoleA@20: PROC

.DATA
LENS DD ?
BUF DB 255 dup(?)
STRN DB 255 dup(?)
DIN DD ?
DOUT DD ?
TEMP DD ? ; ���������� ��� ���������� �������� ������� �������� � ������

.CODE
MAIN PROC


; �������� ���������� �����
PUSH -10;
CALL GetStdHandle@4
MOV DIN, EAX
; ���� ������
PUSH 0
PUSH OFFSET LENS
PUSH 255
PUSH OFFSET BUF
PUSH DIN
CALL ReadConsoleA@20


CALL CONVERTING
MOV TEMP, EAX ; ��������� ������ �������


; �������� ���������� �����
PUSH -10;
CALL GetStdHandle@4
MOV DIN, EAX
; ���� ������
PUSH 0
PUSH OFFSET LENS
PUSH 255
PUSH OFFSET BUF
PUSH DIN
CALL ReadConsoleA@20

CALL CONVERTING
SUB TEMP, EAX ; ���� ���������



MOV EAX, TEMP 	; ������� ����� ��������
XOR ECX, ECX	; � ECX ����� ������� ����� ������������� �����

cycle:
	CMP EAX, 10
	JL next

	MOV EDX, 0 		;������� ����� ��������
	MOV EBX, 10 	;��������
	DIV EBX ;������� � EAX
			;������� � EDX
    ADD EDX, '0'
	PUSH EDX
	INC ECX
	JMP cycle



next:
	INC ECX
    ADD EAX, '0'
	PUSH EAX ; � ���� ������ ������� ����� � ���������� �� �������� � ����� � �������� �������
			 ; ECX = ���������� �������� ����������� �����

    MOV ESI, OFFSET STRN
    MOV LENS, ECX

add_to_str:
    pop [ESI]
    INC ESI
    LOOP add_to_str

    PUSH -11  ; ������� ���������� ������
    CALL GetStdHandle@4
    MOV DOUT, EAX
    PUSH 0
    PUSH OFFSET LENS
    PUSH LENS
    PUSH OFFSET STRN
    PUSH DOUT
    CALL WriteConsoleA@20




PUSH 0
CALL ExitProcess@4
MAIN ENDP


;-----------------------------------------------------------------------------------------------

CONVERTING PROC
    MOV DI, 16      ; ������� ���������
    MOV ECX, LENS   ; ������� �����
    MOV ESI, OFFSET BUF	; �������� � ESI ��������� �� ������ ��������� ������

    XOR BX, BX	; �������� BX
    XOR AX, AX	; �������� AX

    CONVERT:        ; ������ ����� �������� 
	    MOV BL, [ESI]	; �������� ������ �� ������ � BL

	    CMP BL, 48 
	    JL is_letter
	    CMP BL, 57
	    JG is_letter
	    ; ������ ������ - �����
	    SUB BL, '0'
	    MUL DI
	    ADD AX, BX
	    JMP next

	    is_letter:
		    ; ��������� �� ��������� � ������ ��������
		    CMP BL, 65 ; ����� ��������, ��������� �� bl ��� �� ���������, ��� � ��� ������ ��������
		    JL cycle_exit
		    CMP BL, 70
		    JG cycle_exit
		    ; ������ ������ - �����
		    SUB BL, 'A'
		    MUL DI
		    ADD AX, BX
		    ADD AX, 10
    
	    next:
		    INC ESI ; ������� � ���������� ������
		    LOOP CONVERT

        cycle_exit:
        RET
CONVERTING ENDP

END MAIN