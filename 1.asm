.386
.MODEL FLAT, STDCALL

EXTERN GetStdHandle@4 : PROC
EXTERN ReadConsoleA@20 : PROC
EXTERN ExitProcess@4: PROC
EXTERN WriteConsoleA@20: PROC
EXTERN CharToOemA@8: PROC
EXTERN lstrlenA@4: PROC

.DATA
STR1 DB "������� ������ �����: ", 13,10,0
STR2 DB "������� ������ �����: ", 13,10,0
LENS DD ?   ; ���-�� ��������� ��������
BUF DB 255 dup(?)
SIGN DD ?   ; ���������� ��� �������� �����
STRN DB "-", 255 dup(?) ; ������ ��� ������
DIN DD ?    ; ���������� ��� ����������� �����
DOUT DD ?   ; ���������� ��� ����������� ������
TEMP DD ?   ; ���������� ��� ���������� �������� ������� �������� � ������

.CODE
MAIN PROC

; ������������ ������ ��������� ������
MOV EAX, OFFSET STR1
PUSH EAX
PUSH EAX
CALL CharToOemA@8
; ������������ ������ ��������� ������
MOV EAX, OFFSET STR2
PUSH EAX
PUSH EAX
CALL CharToOemA@8

; ������� ���������� ������
PUSH -11
CALL GetStdHandle@4
MOV DOUT, EAX

; �������� ���������� �����
PUSH -10;
CALL GetStdHandle@4
MOV DIN, EAX

; ����� ������ ������
PUSH OFFSET STR1    ; � ���� ���������� ����� ������
CALL lstrlenA@4     ; ����� ������ ������ � EAX
; ��� �����
PUSH 0
PUSH OFFSET LENS
PUSH EAX
PUSH OFFSET STR1
PUSH DOUT
CALL WriteConsoleA@20


; ���� ������� �����
PUSH 0
PUSH OFFSET LENS
PUSH 255
PUSH OFFSET BUF
PUSH DIN
CALL ReadConsoleA@20


CALL CONVERTING   ; �������������� ������ � �����
MOV TEMP, EAX     ; ��������� ������ �������


; ����� ������ ������
PUSH OFFSET STR2    ; � ���� ���������� ����� ������
CALL lstrlenA@4     ; ����� ������ ������ � EAX
; ��� �����
PUSH 0
PUSH OFFSET LENS
PUSH EAX
PUSH OFFSET STR2
PUSH DOUT
CALL WriteConsoleA@20

; ���� ������� �����
PUSH 0
PUSH OFFSET LENS
PUSH 255
PUSH OFFSET BUF
PUSH DIN
CALL ReadConsoleA@20

CALL CONVERTING     ; �������������� ������ ������ � �����
SUB TEMP, EAX       ; ��������� ���� �����




; ����� ������
MOV ESI, OFFSET STRN ; ��������� �� ������ ������
XOR ECX, ECX	     ; � ECX ����� ������� ����� ������������� �����

CMP TEMP, 0 ; ���� ����� �������������
JGE output
; ������� �� �������������� ���� � ������
SUB TEMP, 1
XOR TEMP, -1
INC ESI 
INC ECX     ; � ������ ����� ������� ���� "�����"



output:
MOV EAX, TEMP 	; ��������� ��������� �������� � �������


; ������� �������� � 10�� ����� ���������� ������� �� �������
hex_to_dec:
	CMP EAX, 10
	JL next         ; ���� ����� < 10, �� ������� �� �����

	MOV EDX, 0 		;������� ����� ��������
	MOV EBX, 10 	;��������
	DIV EBX         ;������� � EAX
			        ;������� � EDX
    ADD EDX, '0'
	PUSH EDX
	INC ECX
	JMP hex_to_dec



next:
	INC ECX         ; ������� ��������� ����� �����
    ADD EAX, '0'
	PUSH EAX ; � ���� ������ ������� ����� � ���������� �� �������� � ����� � �������� �������
			 ; ECX = ���������� �������� ����������� �����


   
 
MOV LENS, ECX ; ����� ��������� ������, ��� �� ������� �����
; ����������� ����� �� ����� � ������ ������
add_to_str:
    pop [ESI]   ; ��������� �� ����� � ������
    INC ESI
    LOOP add_to_str

; ����� ������ ������
PUSH 0
PUSH OFFSET LENS
PUSH LENS
PUSH OFFSET STRN
PUSH DOUT
CALL WriteConsoleA@20

; ����� �� ���������
PUSH 0
CALL ExitProcess@4
MAIN ENDP

;---------��������� ����������� ������ � �����-----------------------------------------------------------

CONVERTING PROC
    MOV SIGN, 1     ; ���������� �������, ��� ����� �������������

    MOV EDI, 16         ; ������� ���������
    MOV ECX, LENS       ; ������� ����� = ����� ������
    MOV ESI, OFFSET BUF	; �������� � ESI ��������� �� ������ ��������� ������

    XOR EBX, EBX	; �������� BX
    XOR EAX, EAX	; �������� AX

    ; �������� �� �����
    MOV BL, [ESI]
    CMP BL, 45
    JNZ cycle_body ; ���� ������������� �����
    MOV SIGN, -1   ; ������ ���� ����� �� "�����"
    INC ESI        ; ����� ������ ��-�� ����� �����������

    ; ���� ��������
    CONVERT:             
	    MOV BL, [ESI]	; �������� ������ �� ������ � BL
 
    cycle_body:         ; ���� ������ - �����
	    CMP BL, 48 
	    JL cycle_exit   ; BL < 48 => ������� �� �����, ��� ��� ������ ��������� ��� ���������
	    CMP BL, 57
	    JG is_uppercase_letter    ; BL > 57 => ��� �� �����, � �����
	    ; ������ ������ - �����
	    SUB BL, '0'
	    MUL DI
	    ADD EAX, EBX
	    JMP next

	    is_uppercase_letter:    ; ���� ������ - ��������� �����
		    CMP BL, 65 
		    JL cycle_exit   ; BL < 65 => � ��������� [57-65] , �.�. �� �����
		    CMP BL, 70
		    JG is_lowercase_letter   ; BL > 70 => ���, ��������, �������� �����
		    ; ������ ������ - ��������� �����
		    SUB BL, 'A'
		    MUL EDI
		    ADD EAX, EBX
		    ADD EAX, 10
            JMP next

        is_lowercase_letter:    ; ���� ������ - �������� �����
            CMP BL, 97
            JL cycle_exit   ; BL < 97 => � ��������� [65,97], �.�. �� �����
            CMP BL, 102
            JG cycle_exit   ; �� �����
            ; ������ ������ - �������� �����
            SUB BL, 'a'
            MUL EDI
            ADD EAX, EBX
            ADD EAX, 10
    
	    next:
		    INC ESI      ; ������� � ���������� �������
		    LOOP CONVERT ; ������� � ������ �����

        cycle_exit:
            IMUL SIGN     ; ������������� ����
            RET           ; �����
CONVERTING ENDP

;-------------------------------------------------------------------------------------------------------------

END MAIN