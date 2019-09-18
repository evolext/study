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
TEMP DD ? ; переменная для временного хранения первого операнда и ответа

.CODE
MAIN PROC


; Получаем дескриптор ввода
PUSH -10;
CALL GetStdHandle@4
MOV DIN, EAX
; Ввод строки
PUSH 0
PUSH OFFSET LENS
PUSH 255
PUSH OFFSET BUF
PUSH DIN
CALL ReadConsoleA@20


CALL CONVERTING
MOV TEMP, EAX ; сохраняем первый операнд


; Получаем дескриптор ввода
PUSH -10;
CALL GetStdHandle@4
MOV DIN, EAX
; Ввод строки
PUSH 0
PUSH OFFSET LENS
PUSH 255
PUSH OFFSET BUF
PUSH DIN
CALL ReadConsoleA@20

CALL CONVERTING
SUB TEMP, EAX ; само вычитание



MOV EAX, TEMP 	; Младшая часть делимого
XOR ECX, ECX	; В ECX будем хранить длину получившегося числа

cycle:
	CMP EAX, 10
	JL next

	MOV EDX, 0 		;Старшая часть делимого
	MOV EBX, 10 	;Делитель
	DIV EBX ;Частное в EAX
			;Остаток в EDX
    ADD EDX, '0'
	PUSH EDX
	INC ECX
	JMP cycle



next:
	INC ECX
    ADD EAX, '0'
	PUSH EAX ; в этот момент готовое число в десятичной СС хранится в стеке в обратном порядке
			 ; ECX = количество разрядов полученного числа

    MOV ESI, OFFSET STRN
    MOV LENS, ECX

add_to_str:
    pop [ESI]
    INC ESI
    LOOP add_to_str

    PUSH -11  ; Получим дескриптор вывода
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
    MOV DI, 16      ; система счисления
    MOV ECX, LENS   ; счетчик цикла
    MOV ESI, OFFSET BUF	; помещаем в ESI указатель на начало введенной строки

    XOR BX, BX	; обнуляем BX
    XOR AX, AX	; обнуляем AX

    CONVERT:        ; начало цикла перевода 
	    MOV BL, [ESI]	; помещаем символ из строки в BL

	    CMP BL, 48 
	    JL is_letter
	    CMP BL, 57
	    JG is_letter
	    ; символ строки - цифра
	    SUB BL, '0'
	    MUL DI
	    ADD AX, BX
	    JMP next

	    is_letter:
		    ; проверяем на вхождение в другой диапазон
		    CMP BL, 65 ; нужно проерить, останется ли bl тем же значением, что и при первой проверке
		    JL cycle_exit
		    CMP BL, 70
		    JG cycle_exit
		    ; символ строки - буква
		    SUB BL, 'A'
		    MUL DI
		    ADD AX, BX
		    ADD AX, 10
    
	    next:
		    INC ESI ; переход к следующему сиволу
		    LOOP CONVERT

        cycle_exit:
        RET
CONVERTING ENDP

END MAIN